using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO.Component;

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
        container = GameObject.Find("SceneObjContainer/MonsterContainer").transform;
	}

    public void OnDestroy()
	{
		Instance = null;
	}

    public Entity AddMonster(long uid, long typeID, Vector3 pos, Vector3 targetPos, float curHp, float maxHp)
	{
        GameObjectEntity monsterGameOE = m_world.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("Monster"));
        monsterGameOE.name = ConfigMonster.GetInstance().GetName(typeID);
        monsterGameOE.transform.SetParent(container);
        // Debug.Log("pos : "+pos.x+" "+pos.y+" "+pos.z);
        monsterGameOE.transform.localPosition = pos;
        Entity monster = monsterGameOE.Entity;
        monsterGameOE.GetComponent<UIDProxy>().Value = new UID{Value=uid};
        InitMonster(monsterGameOE, uid, typeID, pos, targetPos, curHp, maxHp);
        return monster;
	}

    private void InitMonster(GameObjectEntity monsterGameOE, long uid, long typeID, Vector3 pos, Vector3 targetPos, float curHp, float maxHp)
    {
        Entity monster = monsterGameOE.Entity;
        var speed = ConfigMonster.GetInstance().GetMoveSpeed(typeID);
        EntityManager.AddComponentData(monster, new TargetPosition {Value = targetPos});
        var locoStateData = new LocomotionState {LocoState = curHp>0?LocomotionState.State.Idle:LocomotionState.State.Dead};
        //It should have been dead for a long time
        if (curHp==0)
            locoStateData.StartTime = 0;
        EntityManager.AddComponentData(monster, locoStateData);
        EntityManager.AddComponentData(monster, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        EntityManager.AddComponentData(monster, new TypeID {Value=typeID});
        EntityManager.AddComponentData(monster, ActionData.Empty);
        EntityManager.AddComponentData(monster, new SceneObjectTypeData {Value=SceneObjectType.Monster});
        EntityManager.AddComponentData(monster, new PosOffset {Value = float3.zero});
        EntityManager.AddComponentData(monster, new HealthStateData {CurHp=curHp, MaxHp=maxHp});
        EntityManager.AddComponentData(monster, new TimelineState {NewStatus=TimelineState.NewState.Allow, InterruptStatus=TimelineState.InterruptState.Allow});
        monsterGameOE.gameObject.AddComponent<BeHitEffect>();
        monsterGameOE.gameObject.AddComponent<SuckHPEffect>();
        monsterGameOE.gameObject.AddComponent<LocomotionStateStack>();
        EntityManager.AddComponentObject(monster, monsterGameOE.gameObject.GetComponent<BeHitEffect>());
        EntityManager.AddComponentObject(monster, monsterGameOE.gameObject.GetComponent<SuckHPEffect>());
        EntityManager.AddComponentObject(monster, monsterGameOE.gameObject.GetComponent<LocomotionStateStack>());
        monsterGameOE.gameObject.AddComponent<SpeedData>();
        var speedData = monsterGameOE.gameObject.GetComponent<SpeedData>();
        speedData.InitSpeed(speed/GameConst.SpeedFactor);
        EntityManager.AddComponentObject(monster, speedData);
        monsterGameOE.gameObject.AddComponent<ParticleEffects>();
        var particleEffects = monsterGameOE.gameObject.GetComponent<ParticleEffects>();
        EntityManager.AddComponentObject(monster, particleEffects);
        MoveQuery rmq = EntityManager.GetComponentObject<MoveQuery>(monster);
        rmq.Initialize();

        CreateLooks(monster, typeID);
    }

    private void CreateLooks(Entity ownerEntity, long typeID)
    {
        string resID = "MonsterRes_"+typeID;
        if (ResMgr.GetInstance().HasLoadedPrefab(resID))
        {
            var obj = ResMgr.GetInstance().GetGameObject(resID);
            InitLooksObj(obj, ownerEntity, typeID);
        }
        else
        {
            string bodyPath = ResPath.GetMonsterBodyResPath(typeID);
            ResMgr.GetInstance().LoadPrefab(bodyPath, resID, delegate(GameObject prefab)
            {
                var obj = ResMgr.GetInstance().GetGameObject(resID);
                InitLooksObj(obj, ownerEntity, typeID);
            });
        }
    }

    private void InitLooksObj(GameObject obj, Entity ownerEntity, long typeID)
    {
        GameObjectEntity bodyOE = m_world.SpawnByGameObject<GameObjectEntity>(obj);
        var parentTrans = EntityManager.GetComponentObject<Transform>(ownerEntity);
        bodyOE.transform.SetParent(parentTrans);
        bodyOE.transform.localPosition = Vector3.zero;
        bodyOE.transform.localRotation = Quaternion.identity;
        var looksInfo = EntityManager.GetComponentData<LooksInfo>(ownerEntity);
        looksInfo.CurState = LooksInfo.State.Loaded;
        looksInfo.LooksEntity = bodyOE.Entity;
        EntityManager.SetComponentData<LooksInfo>(ownerEntity, looksInfo);
        ECSHelper.UpdateNameboardHeight(ownerEntity, bodyOE.transform);
    }

    public string GetName(Entity entity)
    {
        var typeIDData = EntityManager.GetComponentData<TypeID>(entity);
        return ConfigMonster.GetInstance().GetName(typeIDData.Value);
    }
}

}