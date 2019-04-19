using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO
{
public class MonsterMgr
{
	static MonsterMgr Instance;
    GameWorld m_world;
    private Transform container;
    public EntityManager EntityManager { get => m_world.GetEntityManager();}
    public Transform MonsterContainer { get => container; set => container = value; }

    public static MonsterMgr GetInstance()
    {
        if (Instance!=null)
            return Instance;
        Instance = new MonsterMgr();
        return Instance;
    }
    private MonsterMgr(){}

    public void Init(GameWorld world)
	{
        m_world = world;
        container = GameObject.Find("SceneObjContainer").transform;
	}

    public void OnDestroy()
	{
		Instance = null;
	}

    public Entity AddMonster(long uid, long typeID, Vector3 pos)
	{
        GameObjectEntity monsterGameOE = m_world.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("Monster"));
        monsterGameOE.name = "Monster_"+uid;
        monsterGameOE.transform.SetParent(container);
        monsterGameOE.transform.localPosition = pos;
        Entity monster = monsterGameOE.Entity;
        InitMonster(monster, uid, typeID, pos);
        return monster;
	}

    private void InitMonster(Entity monster, long uid, long typeID, Vector3 pos)
    {
        EntityManager.AddComponentData(monster, new MoveSpeed {Value = 1000});
        EntityManager.AddComponentData(monster, new TargetPosition {Value = new float3(pos.x, pos.y, pos.z)});
        EntityManager.AddComponentData(monster, new LocomotionState {Value = LocomotionState.State.Idle});
        EntityManager.AddComponentData(monster, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        EntityManager.AddComponentData(monster, new UID {Value=uid});
        EntityManager.AddComponentData(monster, new TypeID {Value=typeID});
        EntityManager.AddComponentData(monster, new SceneObjectTypeData {Value=SceneObjectType.Monster});
        EntityManager.AddComponentData(monster, new NameboardData {UIResState=NameboardData.ResState.WaitLoad});
        // EntityManager.AddComponentData(monster, new JumpState {JumpStatus=JumpState.State.None, JumpCount=0, OriginYPos=0, AscentHeight=0});
        EntityManager.AddComponentData(monster, new PosOffset {Value = float3.zero});
        EntityManager.AddComponentData(monster, new TimelineState {NewStatus=TimelineState.NewState.Allow, InterruptStatus=TimelineState.InterruptState.Allow});
        
        MoveQuery rmq = EntityManager.GetComponentObject<MoveQuery>(monster);
        rmq.Initialize();

        CreateLooks(monster, typeID);
    }

    private void CreateLooks(Entity ownerEntity, long typeID)
    {
        var resPath = GameConst.GetMonsterResPath(typeID);
        string bodyPath = resPath+"/model_clothe_"+typeID+".prefab";
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(bodyPath, delegate(UnityEngine.Object[] objs) {
            if (objs!=null && objs.Length>0)
            {
                GameObject bodyObj = objs[0] as GameObject;
                GameObjectEntity bodyOE = m_world.Spawn<GameObjectEntity>(bodyObj);
                var parentTrans = EntityManager.GetComponentObject<Transform>(ownerEntity);
                bodyOE.transform.SetParent(parentTrans);
                bodyOE.transform.localPosition = Vector3.zero;
                bodyOE.transform.localRotation = Quaternion.identity;
                var looksInfo = EntityManager.GetComponentData<LooksInfo>(ownerEntity);
                looksInfo.CurState = LooksInfo.State.Loaded;
                looksInfo.LooksEntity = bodyOE.Entity;
                EntityManager.SetComponentData<LooksInfo>(ownerEntity, looksInfo);
            }
            else
            {
                Debug.LogError("cannot fine file "+bodyPath);
            }
        });
    }

    public string GetName(Entity entity)
    {
        var typeIDData = EntityManager.GetComponentData<TypeID>(entity);
        return "monster"+typeIDData.Value;
    }
}

}