using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO.Component;

namespace UnityMMO
{
public class NPCMgr
{
	static NPCMgr Instance;
    GameWorld m_world;
    private Transform container;
    public EntityManager EntityManager { get => m_world.GetEntityManager();}
    public Transform NPCContainer { get => container; set => container = value; }

    public static NPCMgr GetInstance()
    {
        if (Instance!=null)
            return Instance;
        Instance = new NPCMgr();
        return Instance;
    }
    private NPCMgr(){}

    public void Init(GameWorld world)
	{
        m_world = world;
        container = GameObject.Find("SceneObjContainer/NPCContainer").transform;
	}

    public void OnDestroy()
	{
		Instance = null;
	}

    public Entity AddNPC(long uid, long typeID, Vector3 pos, Vector3 targetPos)
	{
        GameObject prefab = ResMgr.GetInstance().GetPrefab("NPC");
        GameObjectEntity NPCGameOE = m_world.Spawn<GameObjectEntity>(prefab);
        NPCGameOE.name = ConfigNPC.GetInstance().GetName(typeID);
        NPCGameOE.transform.SetParent(container);
        NPCGameOE.transform.localPosition = pos;
        Entity NPC = NPCGameOE.Entity;
        NPCGameOE.GetComponent<UIDProxy>().Value = new UID{Value=uid};
        InitNPC(NPC, uid, typeID, pos, targetPos);
        return NPC;
	}

    private void InitNPC(Entity NPC, long uid, long typeID, Vector3 pos, Vector3 targetPos)
    {
        // EntityManager.AddComponentData(NPC, new MoveSpeed {Value = ConfigNPC.GetInstance().GetMoveSpeed(typeID)});
        // EntityManager.AddComponentData(NPC, new TargetPosition {Value = targetPos});
        EntityManager.AddComponentData(NPC, new LocomotionState {LocoState = LocomotionState.State.Idle});
        EntityManager.AddComponentData(NPC, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        // EntityManager.SetComponentData(NPC, new UID {Value=uid});
        EntityManager.AddComponentData(NPC, new TypeID {Value=typeID});
        EntityManager.AddComponentData(NPC, ActionData.Empty);
        EntityManager.AddComponentData(NPC, new SceneObjectTypeData {Value=SceneObjectType.NPC});
        EntityManager.AddComponentData(NPC, new NameboardData {UIResState=NameboardData.ResState.WaitLoad});
        // EntityManager.AddComponentData(NPC, new PosOffset {Value = float3.zero});
        // EntityManager.AddComponentData(NPC, new TimelineState {NewStatus=TimelineState.NewState.Allow, InterruptStatus=TimelineState.InterruptState.Allow});
        // MoveQuery rmq = EntityManager.GetComponentObject<MoveQuery>(NPC);
        // rmq.Initialize();
        CreateLooks(NPC, typeID);
    }

    private void CreateLooks(Entity ownerEntity, long typeID)
    {
        var resPath = ResPath.GetNPCLooksPath(typeID);
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(resPath, delegate(UnityEngine.Object[] objs) {
            if (objs!=null && objs.Length>0 && EntityManager.Exists(ownerEntity))
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
                Debug.LogError("cannot fine file "+resPath);
            }
        });
    }

    public string GetName(Entity entity)
    {
        var typeIDData = EntityManager.GetComponentData<TypeID>(entity);
        return ConfigNPC.GetInstance().GetName(typeIDData.Value);
    }
}

}