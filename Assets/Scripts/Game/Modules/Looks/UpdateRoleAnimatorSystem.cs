using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;

[DisableAutoCreation]
public class UpdateRoleAnimatorSystem : BaseComponentSystem
{
    public UpdateRoleAnimatorSystem(GameWorld world) : base(world) {}

    ComponentGroup group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(RoleState));
    }

    protected override void OnUpdate()
    {
        var states = group.GetComponentArray<RoleState>();
        for (int i=0; i<states.Length; i++)
        {
            // EntityManager
        }
    }
}