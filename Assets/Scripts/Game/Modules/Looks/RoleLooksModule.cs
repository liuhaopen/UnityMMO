using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Profiling;
using UnityMMO;

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
    
    public static void Create(EntityCommandBuffer commandBuffer, int career, Vector3 position, Quaternion rotation, Entity ownerEntity, int body, int hair)
    {
        var data = new RoleLooksSpawnRequest(career, position, rotation, ownerEntity, body, hair);
        commandBuffer.CreateEntity();
        commandBuffer.AddComponent(data);
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
        commandBuffer.CreateEntity();
        commandBuffer.AddComponent(data);
    }
}

[DisableAutoCreation]
public class HandleRoleLooksNetRequest : BaseComponentSystem
{
    ComponentGroup RequestGroup;
    public HandleRoleLooksNetRequest(GameWorld world) : base(world)
    {
    }
    protected override void OnCreateManager()
    {
        Debug.Log("on OnCreateManager HandleRoleLooksNetRequest");
        base.OnCreateManager();
        RequestGroup = GetComponentGroup(typeof(RoleLooksNetRequest));
    }

    protected override void OnUpdate()
    {
        //TODO:控制刷新或请求频率
        var requestArray = RequestGroup.GetComponentDataArray<RoleLooksNetRequest>();
        if (requestArray.Length == 0)
            return;

        var requestEntityArray = RequestGroup.GetEntityArray();
        
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
            UnityMMO.NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_get_role_look_info>(req, (_) =>
            {
                SprotoType.scene_get_role_look_info.response rsp = _ as SprotoType.scene_get_role_look_info.response;
                Debug.Log("rsp.result : "+rsp.result.ToString()+" owner:"+owner.ToString());
                if (rsp.result == UnityMMO.GameConst.NetResultOk)
                {
                    bool hasRoleState = m_world.GetEntityManager().HasComponent<RoleState>(owner);
                    if (hasRoleState)
                    {
                        RoleState roleState = m_world.GetEntityManager().GetComponentObject<RoleState>(owner); 
                        Debug.Log("roleState.position : "+roleState.position.ToString());
                        RoleLooksSpawnRequest.Create(m_world.GetEntityManager(), (int)rsp.role_looks_info.career, roleState.position, Quaternion.identity, owner, (int)rsp.role_looks_info.body, (int)rsp.role_looks_info.hair);
                    }
                }
            });
        }
    }
}

//当有玩家离主角较近时，且未加载过模型的话就给它加个RoleLooks
[DisableAutoCreation]
public class HandleRoleLooks : BaseComponentSystem
{
    ComponentGroup RoleGroup;
    public HandleRoleLooks(GameWorld world) : base(world)
    {
    }
    protected override void OnCreateManager()
    {
        Debug.Log("on OnCreateManager HandleRoleLooks");
        base.OnCreateManager();
        RoleGroup = GetComponentGroup(typeof(RoleState), typeof(Position));
    }

    protected override void OnUpdate()
    {
        EntityArray entities = RoleGroup.GetEntityArray();
        var roleStateArray = RoleGroup.GetComponentArray<RoleState>();
        var mainRoleGOE = RoleMgr.GetInstance().GetMainRole();
        float3 mainRolePos = float3.zero;
        if (mainRoleGOE!=null)
            mainRolePos = m_world.GetEntityManager().GetComponentData<Position>(mainRoleGOE.Entity).Value;
        for (int i=0; i<roleStateArray.Length; i++)
        {
            var roleState = roleStateArray[i];
            var entity = entities[i];
            bool isNeedReqLooksInfo = false;
            bool isNeedHideLooks = false;

            //其它玩家离主角近时就要请求该玩家的角色外观信息
            var curPos = m_world.GetEntityManager().GetComponentData<Position>(entity).Value;
            float distance = Vector3.Distance(curPos, mainRolePos);
            Debug.Log("distance : "+distance);
            if (!roleState.hasLooks)
            {
                isNeedReqLooksInfo = distance <= 400;
                // bool isMainRole = m_world.GetEntityManager().HasComponent(entity, typeof(UnityMMO.MainRoleTag));
                // if (isMainRole)
                // {
                //     isNeedReqLooksInfo = true;
                // }
                // else
                // {
                    // //其它玩家离主角近时就要请求该玩家的角色外观信息
                    // var curPos = m_world.GetEntityManager().GetComponentData<Position>(entity).Value;
                    // float distance = Vector3.Distance(curPos, mainRolePos);
                    // Debug.Log("distance : "+distance);
                    // isNeedReqLooksInfo = distance <= 200;
                // }
            }
            else
            {
                isNeedHideLooks = distance >= 300;
            }
            if (isNeedReqLooksInfo)
            {
                roleState.hasLooks = true;
                RoleLooksNetRequest.Create(PostUpdateCommands, roleState.roleUid, entity);
            }
        }
    }
}

//当存在RoleLooksSpawnRequest组件时就给它加载一个RoleLooks外观
[DisableAutoCreation]
public class HandleRoleLooksSpawnRequests : BaseComponentSystem
{
    ComponentGroup SpawnGroup;

    public HandleRoleLooksSpawnRequests(GameWorld world) : base(world)
    {
    }

    protected override void OnCreateManager()
    {
        Debug.Log("on OnCreateManager role looks system");
        base.OnCreateManager();
        SpawnGroup = GetComponentGroup(typeof(RoleLooksSpawnRequest));
    }

    protected override void OnUpdate()
    {
        // Debug.Log("on OnUpdate role looks system");
        var requestArray = SpawnGroup.GetComponentDataArray<RoleLooksSpawnRequest>();
        if (requestArray.Length == 0)
            return;

        var requestEntityArray = SpawnGroup.GetEntityArray();
        
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
            var playerState = EntityManager.GetComponentObject<RoleState>(request.ownerEntity);
            int career = request.career;
            int body = request.body;
            int hair = request.hair;
            int bodyID = 1000+career*100+body;
            int hairID = 1000+career*100+hair;
            string careerPath = UnityMMO.GameConst.GetRoleCareerResPath(career);
            string bodyPath = careerPath+"/body/body_"+bodyID+"/model_body_"+bodyID+".prefab";
            string hairPath = careerPath+"/hair/hair_"+hairID+"/model_hair_"+hairID+".prefab";
            Debug.Log("SpawnRoleLooks bodyPath : "+bodyPath);
            XLuaFramework.ResourceManager.GetInstance().LoadAsset<GameObject>(bodyPath, delegate(UnityEngine.Object[] objs) {
                if (objs!=null && objs.Length>0)
                {
                    GameObject bodyObj = objs[0] as GameObject;
                    GameObjectEntity bodyOE = m_world.Spawn<GameObjectEntity>(bodyObj);
                    var parentTrans = EntityManager.GetComponentObject<Transform>(request.ownerEntity);
                    bodyOE.transform.SetParent(parentTrans);
                    // bodyOE.transform.SetParent(UnityMMO.RoleMgr.GetInstance().RoleContainer);
                    bodyOE.transform.localPosition = request.position;
                    bodyOE.GetComponent<Rigidbody>().isKinematic = true;
                    playerState.looksEntity = bodyOE.Entity;
                    LoadHair(hairPath, bodyOE.transform.Find("head"));
                }
                else
                {
                    Debug.LogError("cannot fine file "+bodyPath);
                }
            });
        }
    }

    void LoadHair(string hairPath, Transform parentNode)
    {
        XLuaFramework.ResourceManager.GetInstance().LoadPrefabGameObjectWithAction(hairPath, delegate(UnityEngine.Object obj) {
            var hairObj = obj as GameObject;
            hairObj.transform.SetParent(parentNode);
            hairObj.transform.localPosition = Vector3.zero;
        });
    }

}

[DisableAutoCreation]
public class HandleLooksFollowLogicTransform : BaseComponentSystem
{
    ComponentGroup Group;

    public HandleLooksFollowLogicTransform(GameWorld world) : base(world)
    {}

    protected override void OnCreateManager()
    {
        Debug.Log("on OnCreateManager role looks system");
        base.OnCreateManager();
        Group = GetComponentGroup(typeof(RoleState), typeof(Position), typeof(Rotation));
    }

    protected override void OnUpdate()
    {
        var states = Group.GetComponentArray<RoleState>();
        var pos = Group.GetComponentDataArray<Position>();
        var rotations = Group.GetComponentDataArray<Rotation>();
        for (int i = 0; i < states.Length; i++)
        {
            var looksEntity = states[i].looksEntity;
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
    }
}