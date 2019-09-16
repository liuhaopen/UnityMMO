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
        InitNPC(NPCGameOE, uid, typeID, pos, targetPos);
        return NPC;
	}

    private void InitNPC(GameObjectEntity NPCGameOE, long uid, long typeID, Vector3 pos, Vector3 targetPos)
    {
        Entity NPC = NPCGameOE.Entity;
        EntityManager.AddComponentData(NPC, new LocomotionState {LocoState = LocomotionState.State.Idle});
        EntityManager.AddComponentData(NPC, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        EntityManager.AddComponentData(NPC, new TypeID {Value=typeID});
        EntityManager.AddComponentData(NPC, ActionData.Empty);
        EntityManager.AddComponentData(NPC, new SceneObjectTypeData {Value=SceneObjectType.NPC});
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
                ECSHelper.UpdateNameboardHeight(ownerEntity, bodyOE.transform);
            }
            else
            {
                Debug.LogError("cannot fine file "+resPath);
            }
        });
    }

    public string GetName(Entity entity)
    {
        if (EntityManager.HasComponent<TypeID>(entity))
        {
            var typeIDData = EntityManager.GetComponentData<TypeID>(entity);
            return ConfigNPC.GetInstance().GetName(typeIDData.Value);
        }
        return "神秘人";
    }
}

}