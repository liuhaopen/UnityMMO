using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Profiling;
using UnityMMO;
using UnityMMO.Component;

public struct RoleLooksSpawnRequest : IComponentData
{
    public int career;
    public Vector3 position;
    public Quaternion rotation;
    public Entity ownerEntity;
    public int body;
    public int hair;

    private RoleLooksSpawnRequest(int career, Vector3 position, Quaternion rotation, Entity ownerEntity, int body, int hair)
    {
        this.career = career;
        this.position = position;
        this.rotation = rotation;
        this.ownerEntity = ownerEntity;
        this.body = body;
        this.hair = hair;
    }

    public static void Create(EntityManager entityMgr, int career, Vector3 position, Quaternion rotation, Entity ownerEntity, int body, int hair)
    {
        var data = new RoleLooksSpawnRequest(career, position, rotation, ownerEntity, body, hair);
        Entity entity = entityMgr.CreateEntity(typeof(RoleLooksSpawnRequest));
        entityMgr.SetComponentData(entity, data);
    }
}

public struct RoleLooksNetRequest : IComponentData
{
    public long roleUid;
    public Entity owner;

    public static void Create(EntityCommandBuffer commandBuffer, long roleUid, Entity owner)
    {
        var data = new RoleLooksNetRequest();
        data.roleUid = roleUid;
        data.owner = owner;
        var entity = commandBuffer.CreateEntity();
        commandBuffer.AddComponent(entity, data);
    }
}

[DisableAutoCreation]
public class HandleRoleLooksNetRequest : BaseComponentSystem
{
    EntityQuery RequestGroup;
    public HandleRoleLooksNetRequest(GameWorld world) : base(world)
    {
    }
    protected override void OnCreate()
    {
        Debug.Log("on OnCreate HandleRoleLooksNetRequest");
        base.OnCreate();
        RequestGroup = GetEntityQuery(typeof(RoleLooksNetRequest));
    }

    protected override void OnUpdate()
    {
        //TODO:控制刷新或请求频率
        var requestArray = RequestGroup.ToComponentDataArray<RoleLooksNetRequest>(Allocator.TempJob);
        if (requestArray.Length == 0)
        {
            requestArray.Dispose();
            return;
        }

        var requestEntityArray = RequestGroup.ToEntityArray(Allocator.TempJob);
        
        // Copy requests as spawning will invalidate Group
        var requests = new RoleLooksNetRequest[requestArray.Length];
        for (var i = 0; i < requestArray.Length; i++)
        {
            requests[i] = requestArray[i];
            PostUpdateCommands.DestroyEntity(requestEntityArray[i]);
        }

        for(var i = 0; i < requests.Length; i++)
        {
            SprotoType.scene_get_role_look_info.request req = new SprotoType.scene_get_role_look_info.request();
            req.uid = requests[i].roleUid;
            Entity owner = requests[i].owner;
            if (!EntityManager.Exists(owner))
                continue;
            UnityMMO.NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_get_role_look_info>(req, (_) =>
            {
                SprotoType.scene_get_role_look_info.response rsp = _ as SprotoType.scene_get_role_look_info.response;
                // Debug.Log("rsp.result : "+rsp.result.ToString()+" owner:"+owner.ToString());
                if (rsp.result == UnityMMO.GameConst.NetResultOk)
                {
                    var name = rsp.role_looks_info.name;
                    RoleMgr.GetInstance().SetName(req.uid, name);
                    RoleMgr.GetInstance().UpdateLooksInfo(req.uid, new RoleLooksInfo{
                        uid=req.uid, career=(int)rsp.role_looks_info.career,
                        body=(int)rsp.role_looks_info.body,
                        hair=(int)rsp.role_looks_info.hair,
                        weapon=(int)rsp.role_looks_info.weapon,
                        wing=(int)rsp.role_looks_info.wing,
                        horse=(int)rsp.role_looks_info.horse,
                        hp=(int)rsp.role_looks_info.hp,
                        maxHp=(int)rsp.role_looks_info.max_hp,
                        name=name,
                    });
                    if (m_world.GetEntityManager().HasComponent<HealthStateData>(owner))
                    {
                        var hpData = m_world.GetEntityManager().GetComponentData<HealthStateData>(owner);
                        hpData.CurHp = rsp.role_looks_info.hp;
                        hpData.MaxHp = rsp.role_looks_info.max_hp;
                        m_world.GetEntityManager().SetComponentData<HealthStateData>(owner, hpData);
                    }
                    if (m_world.GetEntityManager().HasComponent<NameboardData>(owner))
                    {
                        var nameboardData = m_world.GetEntityManager().GetComponentObject<NameboardData>(owner);
                        nameboardData.SetName(name);
                    }
                    bool hasTrans = m_world.GetEntityManager().HasComponent<Transform>(owner);
                    if (hasTrans)
                    {
                        var transform = m_world.GetEntityManager().GetComponentObject<Transform>(owner); 
                        //因为是异步操作，等后端发送信息过来时PostUpdateCommands已经失效了，所以不能这么用
                        RoleLooksSpawnRequest.Create(m_world.GetEntityManager(), (int)rsp.role_looks_info.career, transform.localPosition, transform.localRotation, owner, (int)rsp.role_looks_info.body, (int)rsp.role_looks_info.hair);
                    }
                }
            });
        }
        requestEntityArray.Dispose();
        requestArray.Dispose();
    }
}

//当有玩家离主角较近时，且未加载过模型的话就给它加个RoleLooks
[DisableAutoCreation]
public class HandleRoleLooks : BaseComponentSystem
{
    EntityQuery RoleGroup;
    public HandleRoleLooks(GameWorld world) : base(world)
    {
    }
    protected override void OnCreate()
    {
        Debug.Log("on OnCreate HandleRoleLooks");
        base.OnCreate();
        RoleGroup = GetEntityQuery(typeof(UID), typeof(LooksInfo), typeof(RoleInfo));
    }

    protected override void OnUpdate()
    {
        var entities = RoleGroup.ToEntityArray(Allocator.TempJob);
        var uidArray = RoleGroup.ToComponentDataArray<UID>(Allocator.TempJob);
        var looksInfoArray = RoleGroup.ToComponentDataArray<LooksInfo>(Allocator.TempJob);
        var mainRoleGOE = RoleMgr.GetInstance().GetMainRole();
        float3 mainRolePos = float3.zero;
        if (mainRoleGOE!=null)
            mainRolePos = m_world.GetEntityManager().GetComponentObject<Transform>(mainRoleGOE.Entity).localPosition;
        for (int i=0; i<looksInfoArray.Length; i++)
        {
            var uid = uidArray[i];
            var looksInfo = looksInfoArray[i];
            var entity = entities[i];
            bool isNeedReqLooksInfo = false;
            bool isNeedHideLooks = false;

            //其它玩家离主角近时就要请求该玩家的角色外观信息
            var curPos = m_world.GetEntityManager().GetComponentObject<Transform>(entity).localPosition;
            float distance = Vector3.Distance(curPos, mainRolePos);
            if (looksInfo.CurState == LooksInfo.State.None)
            {
                isNeedReqLooksInfo = distance <= 400;
            }
            else
            {
                isNeedHideLooks = distance >= 300;
            }
            // Debug.Log("isNeedReqLooksInfo : "+isNeedReqLooksInfo.ToString()+" looksInfo.CurState:"+looksInfo.CurState.ToString());
            if (isNeedReqLooksInfo)
            {
                looksInfo.CurState = LooksInfo.State.Loading;
                // looksInfoArray[i] = looksInfo;
                EntityManager.SetComponentData<LooksInfo>(entities[i], looksInfo);
                RoleLooksNetRequest.Create(PostUpdateCommands, uid.Value, entity);
            }
        }
        entities.Dispose();
        looksInfoArray.Dispose();
        uidArray.Dispose();
    }
}

//当存在RoleLooksSpawnRequest组件时就给它加载一个RoleLooks外观
[DisableAutoCreation]
public class HandleRoleLooksSpawnRequests : BaseComponentSystem
{
    EntityQuery SpawnGroup;

    public HandleRoleLooksSpawnRequests(GameWorld world) : base(world)
    {
    }

    protected override void OnCreate()
    {
        Debug.Log("on OnCreate role looks system");
        base.OnCreate();
        SpawnGroup = GetEntityQuery(typeof(RoleLooksSpawnRequest));
    }

    protected override void OnUpdate()
    {
        // Debug.Log("on OnUpdate role looks system");
        var requestArray = SpawnGroup.ToComponentDataArray<RoleLooksSpawnRequest>(Allocator.TempJob);
        if (requestArray.Length == 0)
        {
            requestArray.Dispose();
            return;
        }

        var requestEntityArray = SpawnGroup.ToEntityArray(Allocator.TempJob);
        
        // Copy requests as spawning will invalidate Group
        var spawnRequests = new RoleLooksSpawnRequest[requestArray.Length];
        for (var i = 0; i < requestArray.Length; i++)
        {
            spawnRequests[i] = requestArray[i];
            PostUpdateCommands.DestroyEntity(requestEntityArray[i]);
        }

        for(var i =0;i<spawnRequests.Length;i++)
        {
            var request = spawnRequests[i];
            // var playerState = EntityManager.GetComponentObject<RoleState>(request.ownerEntity);
            var looksInfo = EntityManager.GetComponentData<LooksInfo>(request.ownerEntity);
            int career = request.career;
            int body = request.body;
            int hair = request.hair;
            // Debug.Log("body : "+body+" hair:"+hair);
            string bodyPath = ResPath.GetRoleBodyResPath(career, body);
            string hairPath = ResPath.GetRoleHairResPath(career, hair);
            // Debug.Log("SpawnRoleLooks bodyPath : "+bodyPath);
            XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(bodyPath, delegate(UnityEngine.Object[] objs) {
                if (objs!=null && objs.Length>0)
                {
                    GameObject bodyObj = objs[0] as GameObject;
                    GameObjectEntity bodyOE = m_world.Spawn<GameObjectEntity>(bodyObj);
                    var parentTrans = EntityManager.GetComponentObject<Transform>(request.ownerEntity);
                    parentTrans.localPosition = request.position;
                    bodyOE.transform.SetParent(parentTrans);
                    bodyOE.transform.localPosition = Vector3.zero;
                    bodyOE.transform.localRotation = Quaternion.identity;
                    ECSHelper.UpdateNameboardHeight(request.ownerEntity, bodyOE.transform);
                    LoadHair(hairPath, bodyOE.transform.Find("head"));
                    // Debug.Log("load ok role model");
                    looksInfo.CurState = LooksInfo.State.Loaded;
                    looksInfo.LooksEntity = bodyOE.Entity;
                    EntityManager.SetComponentData<LooksInfo>(request.ownerEntity, looksInfo);
                }
                else
                {
                    Debug.LogError("cannot fine file "+bodyPath);
                }
            });
        }
        requestEntityArray.Dispose();
        requestArray.Dispose();
    }

    void LoadHair(string hairPath, Transform parentNode)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadPrefabGameObjectWithAction(hairPath, delegate(UnityEngine.Object obj) {
            var hairObj = obj as GameObject;
            hairObj.transform.SetParent(parentNode);
            hairObj.transform.localPosition = Vector3.zero;
            hairObj.transform.localRotation = Quaternion.identity;
        });
    }

}

[DisableAutoCreation]
public class HandleLooksFollowLogicTransform : BaseComponentSystem
{
    EntityQuery Group;

    public HandleLooksFollowLogicTransform(GameWorld world) : base(world)
    {}

    protected override void OnCreate()
    {
        Debug.Log("on OnCreate role looks system");
        base.OnCreate();
        Group = GetEntityQuery(typeof(LooksInfo), typeof(Translation), typeof(Rotation));
    }

    protected override void OnUpdate()
    {
        var states = Group.ToComponentDataArray<LooksInfo>(Allocator.TempJob);
        var pos = Group.ToComponentDataArray<Translation>(Allocator.TempJob);
        var rotations = Group.ToComponentDataArray<Rotation>(Allocator.TempJob);
        for (int i = 0; i < states.Length; i++)
        {
            var looksEntity = states[i].LooksEntity;
            if (looksEntity != Entity.Null)
            {
                var transform = EntityManager.GetComponentObject<Transform>(looksEntity);
                if (transform != null)
                {
                    transform.localPosition = pos[i].Value;
                    transform.rotation = rotations[i].Value;
                }
            }
        }
        states.Dispose();
        pos.Dispose();
        rotations.Dispose();
    }
}