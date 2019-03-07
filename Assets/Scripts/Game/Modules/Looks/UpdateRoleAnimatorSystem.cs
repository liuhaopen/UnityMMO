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
        group = GetComponentGroup(typeof(RoleState), typeof(GroundInfo));
    }

    protected override void OnUpdate()
    {
        var states = group.GetComponentArray<RoleState>();
        for (int i=0; i<states.Length; i++)
        {
            var looksEntity = states.looksEntity;
            var animator = m_World.GetEntityManager().GetComponentObject<Animator>(looksEntity);
            UpdateAnimator(states[i], animator);
        }
    }

    void UpdateAnimator(RoleState state, Animator animator)
    {
        
    }
}