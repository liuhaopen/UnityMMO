using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO.Component;

namespace UnityMMO
{
public class RoleMgr
{
	static RoleMgr Instance;
    GameWorld m_GameWorld;
    private Transform container;
    GameObjectEntity mainRoleGOE;
    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
    Dictionary<long, string> names = new Dictionary<long, string>();
    Dictionary<long, RoleLooksInfo> looksInfos = new Dictionary<long, RoleLooksInfo>();
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

    public Entity AddMainRole(long uid, long typeID, string name, int career, Vector3 pos, float curHp, float maxHp)
	{
        GameObjectEntity roleGameOE = m_GameWorld.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("MainRole"));
        // Debug.Log("add main role : "+uid+" "+new System.Diagnostics.StackTrace().ToString());
        roleGameOE.name = "MainRole_"+uid;
        roleGameOE.transform.SetParent(container);
        roleGameOE.transform.localPosition = pos;
        Entity role = roleGameOE.Entity;
        RoleMgr.GetInstance().SetName(uid, name);
        InitRole(role, uid, typeID, pos, pos, curHp, maxHp, false);
        roleGameOE.GetComponent<UIDProxy>().Value = new UID{Value=uid};
        EntityManager.AddComponentData(role, new PosSynchInfo {LastUploadPos = float3.zero});
        EntityManager.AddComponent(role, ComponentType.ReadWrite<UserCommand>());
        var nameboardData = EntityManager.GetComponentObject<NameboardData>(role);
        nameboardData.SetName(name);
        var roleInfo = roleGameOE.GetComponent<RoleInfo>();
        roleInfo.Name = name;
        roleInfo.Career = career;
        mainRoleGOE = roleGameOE;
        SceneMgr.Instance.ApplyMainRole(roleGameOE);
        return role;
	}

    public void UpdateMainRoleNavAgent()
    {
        if (mainRoleGOE != null)
        {
            Debug.Log("role mgr reset nav agent");
            var moveQuery = mainRoleGOE.GetComponent<MoveQuery>();
            moveQuery.UpdateNavAgent();
        }
    }

    public GameObjectEntity GetMainRole()
    {
        return mainRoleGOE;
    }

    public long GetMainRoleUID()
    {
        if (mainRoleGOE != null)
        {
            return EntityManager.GetComponentData<UID>(mainRoleGOE.Entity).Value;
        }
        return 0;
    }

    public bool IsMainRoleEntity(Entity entity)
    {
        if (mainRoleGOE==null || entity==Entity.Null)
            return false;
        return mainRoleGOE.Entity == entity;
    }

    public bool IsRoleEntity(Entity entity)
    {
        var sceneObjectTypeData = EntityManager.GetComponentData<SceneObjectTypeData>(entity);
        return sceneObjectTypeData.Value == SceneObjectType.Role;
    }

    public Entity AddRole(long uid, long typeID, Vector3 pos, Vector3 targetPos, float curHp, float maxHp)
	{
        GameObjectEntity roleGameOE = m_GameWorld.Spawn<GameObjectEntity>(ResMgr.GetInstance().GetPrefab("Role"));
        roleGameOE.name = "Role_"+uid;
        roleGameOE.transform.SetParent(container);
        roleGameOE.transform.localPosition = pos;
        Entity role = roleGameOE.Entity;
        roleGameOE.GetComponent<UIDProxy>().Value = new UID{Value=uid};
        InitRole(role, uid, typeID, pos, targetPos, curHp, maxHp);
        return role;
	}

    private void InitRole(Entity role, long uid, long typeID, Vector3 pos, Vector3 targetPos, float curHp, float maxHp, bool isNeedNavAgent=false)
    {
        EntityManager.AddComponentData(role, new MoveSpeed {Value = 1200, BaseValue = 1200});
        EntityManager.AddComponentData(role, new TargetPosition {Value = targetPos});
        EntityManager.AddComponentData(role, new LocomotionState {LocoState = curHp>0?LocomotionState.State.Idle:LocomotionState.State.Dead});
        EntityManager.AddComponentData(role, new LooksInfo {CurState=LooksInfo.State.None, LooksEntity=Entity.Null});
        EntityManager.AddComponentData(role, new SceneObjectTypeData {Value=SceneObjectType.Role});
        // EntityManager.AddComponentData(role, new NameboardData {UIResState=NameboardData.ResState.WaitLoad});
        EntityManager.AddComponentData(role, new TypeID {Value=typeID});
        EntityManager.AddComponentData(role, new GroundInfo {GroundNormal=Vector3.zero, Altitude=0});
        // EntityManager.AddComponentData(role, new JumpState {JumpStatus=JumpState.State.None, JumpCount=0, OriginYPos=0, AscentHeight=0});
        EntityManager.AddComponentData(role, new JumpData{JumpCount=0});
        EntityManager.AddComponentData(role, ActionData.Empty);
        EntityManager.AddComponentData(role, new PosOffset {Value = float3.zero});
        EntityManager.AddComponentData(role, new HealthStateData {CurHp=curHp, MaxHp=maxHp});
        EntityManager.AddComponentData(role, new TimelineState {NewStatus=TimelineState.NewState.Allow, InterruptStatus=TimelineState.InterruptState.Allow});
        
        MoveQuery rmq = EntityManager.GetComponentObject<MoveQuery>(role);
        rmq.Initialize(isNeedNavAgent);
    }

    public string GetName(long uid)
    {
        string name = "";
        names.TryGetValue(uid, out name);
        return name;
    }

    public void SetName(long uid, string name)
    {
        names[uid] = name;
    }

    public void UpdateLooksInfo(long uid, RoleLooksInfo info)
    {
        //Cat_TODO:增加时间字段，如果取值时超过一定时间就向服务器拿最新的
        looksInfos[uid] = info;
    }

    public RoleLooksInfo GetLooksInfo(long uid)
    {
        RoleLooksInfo info = new RoleLooksInfo();
        looksInfos.TryGetValue(uid, out info);
        return info;
    }

    public RoleLooksInfo GetMainRoleLooksInfo()
    {
        long uid = GetMainRoleUID();
        return GetLooksInfo(uid);
    }

    public void StopMainRoleRunning()
    {
        if (mainRoleGOE == null)
            return;
        var query = mainRoleGOE.GetComponent<MoveQuery>();
        query.StopFindWay();
        var auto = mainRoleGOE.GetComponent<AutoFight>();
        auto.enabled = false;
    }
}

    public struct RoleLooksInfo
    {
        public Int64 uid;
        public int career;
        public int body;
        public int hair;
        public int weapon;
        public int wing;
        public int horse;
        public Int64 hp;
        public Int64 maxHp;
        public string name;
        // public RoleLooksInfo() => this.name = "NoOne";
    }
}