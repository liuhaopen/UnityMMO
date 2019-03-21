using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

public struct TimelineSpawnRequest : IComponentData
{
    public Entity Owner;
    // public string TimelinePath;

    public static void Create(EntityManager entityMgr, Entity Owner, string TimelinePath)
    {
        var data = new TimelineSpawnRequest();
        data.Owner = Owner;
        // data.TimelinePath = TimelinePath;
        Entity entity = entityMgr.CreateEntity(typeof(RoleLooksSpawnRequest));
        entityMgr.SetComponentData(entity, data);
    }
}


[DisableAutoCreation]
public class TimelineSpawnSystem : BaseComponentSystem
{
    public TimelineSpawnSystem(GameWorld world) : base(world) {}

    ComponentGroup RequestGroup;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        RequestGroup = GetComponentGroup(typeof(TimelineSpawnRequest));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var requestArray = RequestGroup.GetComponentDataArray<TimelineSpawnRequest>();
        if (requestArray.Length == 0)
            return;

        var requestEntityArray = RequestGroup.GetEntityArray();
        
        // Copy requests as spawning will invalidate Group
        var requests = new TimelineSpawnRequest[requestArray.Length];
        for (var i = 0; i < requestArray.Length; i++)
        {
            requests[i] = requestArray[i];
            PostUpdateCommands.DestroyEntity(requestEntityArray[i]);
        }

        for(var i = 0; i < requests.Length; i++)
        {
            
        }
    }
}