using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class FightSpawnSystem : BaseComponentSystem
{
    public FightSpawnSystem(GameWorld world) : base(world) {}

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
            //如果不是主角参与的战斗，不在视野里的战斗协议需要过滤，在视野里的话就根据场景的人数每人分配一个时间间隔，人越多就间隔越大，间隔内收到的战斗协议也要过滤掉
            if (IsIgnore())
                continue;
        }
    }

    bool IsIgnore()
    {
        return false;
    }
}