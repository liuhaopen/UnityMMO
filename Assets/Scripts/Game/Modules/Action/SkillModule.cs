using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

public struct SkillSpawnRequest : IComponentData
{
    public long UID;
    public int SkillID;
    private SkillSpawnRequest(long UID, int SkillID)
    {
        this.UID = UID;
        this.SkillID = SkillID;
    }

    public static void Create(EntityCommandBuffer commandBuffer, long UID, int SkillID)
    {
        var data = new SkillSpawnRequest(UID, SkillID);
        commandBuffer.CreateEntity();
        commandBuffer.AddComponent(data);
    }
}


[DisableAutoCreation]
public class SkillSpawnSystem : BaseComponentSystem
{
    public SkillSpawnSystem(GameWorld world) : base(world) {}

    ComponentGroup RequestGroup;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        RequestGroup = GetComponentGroup(typeof(SkillSpawnRequest));
    }

    protected override void OnUpdate()
    {
        float dt = Time.deltaTime;
        var requestArray = RequestGroup.GetComponentDataArray<SkillSpawnRequest>();
        if (requestArray.Length == 0)
            return;

        var requestEntityArray = RequestGroup.GetEntityArray();
        
        // Copy requests as spawning will invalidate Group
        var requests = new SkillSpawnRequest[requestArray.Length];
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