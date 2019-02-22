using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class TargetPosSystem : ComponentSystem
{
    public UpdateRoleTransformFromLooks(GameWorld world) : base(world) {}

    ComponentGroup group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(TargetPosition), typeof(Position), typeof(MoveSpeed));
    }

    protected override void OnUpdate()
    {
        var targetPos = group.GetComponentDataArray<TargetPosition>();
        var sppeds = group.GetComponentDataArray<MoveSpeed>();
        var pos = group.GetComponentDataArray<Position>();
        for (int i=0; i<targetPos.Length; i++)
        {

        }
    }
}