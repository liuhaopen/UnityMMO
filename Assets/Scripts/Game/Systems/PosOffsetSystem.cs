using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;
using UnityMMO.Component;

[DisableAutoCreation]
public class ResetPosOffsetSystem : BaseComponentSystem
{
    public ResetPosOffsetSystem(GameWorld world) : base(world) {}

    EntityQuery group;

    protected override void OnCreate()
    {
        base.OnCreate();
        group = GetEntityQuery(typeof(PosOffset));
    }

    protected override void OnUpdate()
    {
        var entities = group.ToEntityArray(Allocator.TempJob);
        var posOffsets = group.ToComponentDataArray<PosOffset>(Allocator.TempJob);
        for (int i=0; i<posOffsets.Length; i++)
        {
            var posOffset = posOffsets[i];
            posOffset.Value = float3.zero;
            // posOffsets[i] = posOffset;
            EntityManager.SetComponentData<PosOffset>(entities[i], posOffset);
        }
        entities.Dispose();
        posOffsets.Dispose();
    }
}