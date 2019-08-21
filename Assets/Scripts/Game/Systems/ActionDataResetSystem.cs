using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;
using UnityMMO.Component;

[DisableAutoCreation]
public class ActionDataResetSystem : BaseComponentSystem
{
    public ActionDataResetSystem(GameWorld world) : base(world) {}

    EntityQuery group;

    protected override void OnCreate()
    {
        base.OnCreate();
        group = GetEntityQuery(typeof(ActionData));
    }

    protected override void OnUpdate()
    {
        var entities = group.ToEntityArray(Allocator.TempJob);
        var datas = group.ToComponentDataArray<ActionData>(Allocator.TempJob);
        for (int i=0; i<datas.Length; i++)
        {
            // datas[i] = ActionData.Empty;
            EntityManager.SetComponentData<ActionData>(entities[i], ActionData.Empty);
        }
        entities.Dispose();
        datas.Dispose();
    }
}