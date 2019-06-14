using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class ActionDataResetSystem : BaseComponentSystem
{
    public ActionDataResetSystem(GameWorld world) : base(world) {}

    EntityQuery group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetEntityQuery(typeof(ActionData));
    }

    protected override void OnUpdate()
    {
        var datas = group.ToComponentDataArray<ActionData>(Allocator.TempJob);
        for (int i=0; i<datas.Length; i++)
        {
            datas[i] = ActionData.Empty;
        }
        datas.Dispose();
    }
}