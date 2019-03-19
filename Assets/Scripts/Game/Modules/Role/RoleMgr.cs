using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO
{
public class RoleMgr
{
	public static RoleMgr Instance;
    GameWorld m_GameWorld;
    private Transform container;
    Dictionary<long, Entity> entityDic = new Dictionary<long, Entity>();
    GameObjectEntity mainRoleGOE;
    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    public EntityManager EntityManager { get => m_GameWorld.GetEntityManager();}
    public Transform RoleContainer { get => container; set => container = value; }

    public static RoleMgr GetInstance()
    {
        if (Instance!=null)
            return Instance;
        Instance = new RoleMgr();
        return Instance;
    }

    public void Init(GameWorld world)
	{
        m_GameWorld = world;

        container = GameObject.Find("SceneObjContainer/RoleContainer").transform;

        LoadPrefab("Assets/AssetBundleRes/role/prefab/MainRole.prefab", "MainRole");
        LoadPrefab("Assets/AssetBundleRes/role/prefab/Role.prefab", "Role");
	}

    void LoadPrefab(string path, string storePrefabName)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(path, delegate(UnityEngine.Object[] objs) {
            if (objs.Length > 0 && (objs[0] as GameObject)!=null)
            {
                GameObject prefab = objs[0] as GameObject;
                if (prefab != null) 
                {
                    this.prefabDic[storePrefabName] = prefab;
                    return;
                }
            }
            Debug.LogError("cannot find prefab in "+path);
        });
    }

    public void OnDestroy()
	{
		Instance = null;
	}

    public Entity AddMainRole(long uid, Vector3 pos)
	{
        GameObjectEntity roleGameOE = m_GameWorld.Spawn<GameObjectEntity>(prefabDic["MainRole"]);
        roleGameOE.name = "MainRole_"+uid;
        roleGameOE.transform.SetParent(RoleContainer);
        Debug.Log("role mgr pos : "+pos.x+" "+pos.z);
        roleGameOE.transform.localPosition = pos;
        Entity role = roleGameOE.Entity;
        InitRole(role, uid, pos);
        // EntityManager.AddComponent(role, ComponentType.Create<MainRoleTag>());
        // EntityManager.AddComponent(role, ComponentType.Create<PlayerInput>());
        EntityManager.AddComponentData(role, new PosSynchInfo {LastUploadPos = float3.zero});
        EntityManager.AddComponent(role, ComponentType.Create<UserCommand>());
        
        entityDic.Add(uid, role);
        mainRoleGOE = roleGameOE;

        // SceneMgr.Instance.ApplyDetector(roleGameOE.GetComponent<SceneDetectorBase>());
        SceneMgr.Instance.ApplyMainRole(roleGameOE);
        return role;
	}

    public GameObjectEntity GetMainRole()
    {
        return mainRoleGOE;
    }

    public Entity AddRole(long uid)
	{
        GameObjectEntity roleGameOE = m_GameWorld.Spawn<GameObjectEntity>(prefabDic["Role"]);
        // Debug.Log("add role : "+(roleGameOE!=null).ToString());
        roleGameOE.name = "Role_"+uid;
        roleGameOE.transform.SetParent(RoleContainer);
        Entity role = roleGameOE.Entity;
        InitRole(role, uid, Vector3.zero);
        // EntityManager.AddSharedComponentData(role, GetLookFromPrototype("Prototype/MainRoleRenderPrototype"));
        entityDic.Add(uid, role);
        return role;
	}

    private void InitRole(Entity role, long uid, Vector3 pos)
    {
        // EntityManager.AddComponentData(role, new Position {Value = new float3(pos.x, pos.y, pos.z)});
        EntityManager.AddComponentData(role, new MoveSpeed {Value = 200});
        EntityManager.AddComponentData(role, new TargetPosition {Value = new float3(pos.x, pos.y, pos.z)});
        EntityManager.AddComponentData(role, new LocomotionState {Value = LocomotionState.State.Idle});
        EntityManager.AddComponentData(role, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        // EntityManager.AddComponentData(role, new Rotation {Value = quaternion.identity});
        // EntityManager.AddComponentData(role, new GroundInfo());
        RoleState roleState = EntityManager.GetComponentObject<RoleState>(role);
        // roleState.position = pos;
        roleState.roleUid = uid;

        MoveQuery rmq = EntityManager.GetComponentObject<MoveQuery>(role);
        rmq.Initialize();
    }
}

}