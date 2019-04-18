using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace UnityMMO
{
public class RoleMgr
{
	static RoleMgr Instance;
    GameWorld m_GameWorld;
    private Transform container;
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
    private RoleMgr(){}

    public void Init(GameWorld world)
	{
        m_GameWorld = world;
        container = GameObject.Find("SceneObjContainer/RoleContainer").transform;
	}

    public void OnDestroy()
	{
		Instance = null;
	}

    public Entity AddMainRole(long uid, long typeID, string name, int career, Vector3 pos)
	{
        GameObjectEntity roleGameOE = m_GameWorld.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("MainRole"));
        roleGameOE.name = "MainRole_"+uid;
        roleGameOE.transform.SetParent(container);
        roleGameOE.transform.localPosition = pos;
        Entity role = roleGameOE.Entity;
        InitRole(role, uid, typeID, pos);
        EntityManager.AddComponentData(role, new PosSynchInfo {LastUploadPos = float3.zero});
        EntityManager.AddComponent(role, ComponentType.Create<UserCommand>());
        
        var roleInfo = roleGameOE.GetComponent<RoleInfo>();
        roleInfo.Name = name;
        roleInfo.Career = career;
        mainRoleGOE = roleGameOE;
        SceneMgr.Instance.ApplyMainRole(roleGameOE);
        return role;
	}

    public GameObjectEntity GetMainRole()
    {
        return mainRoleGOE;
    }

    public bool IsMainRoleEntity(Entity entity)
    {
        if (mainRoleGOE==null || entity==Entity.Null)
            return false;
        return mainRoleGOE.Entity == entity;
    }

    public Entity AddRole(long uid, long type_id, Vector3 pos)
	{
        GameObjectEntity roleGameOE = m_GameWorld.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("Role"));
        roleGameOE.name = "Role_"+uid;
        roleGameOE.transform.SetParent(container);
        roleGameOE.transform.localPosition = pos;
        Entity role = roleGameOE.Entity;
        InitRole(role, uid, type_id, pos);
        return role;
	}

    private void InitRole(Entity role, long uid, long typeID, Vector3 pos)
    {
        EntityManager.AddComponentData(role, new MoveSpeed {Value = 1200});
        EntityManager.AddComponentData(role, new TargetPosition {Value = new float3(pos.x, pos.y, pos.z)});
        EntityManager.AddComponentData(role, new LocomotionState {Value = LocomotionState.State.Idle});
        EntityManager.AddComponentData(role, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        EntityManager.AddComponentData(role, new UID {Value=uid});
        EntityManager.AddComponentData(role, new SceneObjectTypeData {Value=SceneObjectType.Role});
        EntityManager.AddComponentData(role, new NameboardData {UIResState=NameboardData.ResState.WaitLoad});
        EntityManager.AddComponentData(role, new TypeID {Value=typeID});
        EntityManager.AddComponentData(role, new GroundInfo {GroundNormal=Vector3.zero, Altitude=0});
        EntityManager.AddComponentData(role, new JumpState {JumpStatus=JumpState.State.None, JumpCount=0, OriginYPos=0, AscentHeight=0});
        EntityManager.AddComponentData(role, new PosOffset {Value = float3.zero});
        EntityManager.AddComponentData(role, new TimelineState {NewStatus=TimelineState.NewState.Allow, InterruptStatus=TimelineState.InterruptState.Allow});
        
        MoveQuery rmq = EntityManager.GetComponentObject<MoveQuery>(role);
        rmq.Initialize();
    }
}

}