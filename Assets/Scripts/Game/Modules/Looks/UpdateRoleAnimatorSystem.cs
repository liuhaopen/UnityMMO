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
        var grounds = group.GetComponentDataArray<GroundInfo>();
        for (int i=0; i<states.Length; i++)
        {
            var looksEntity = states[i].looksEntity;
            var animator = m_world.GetEntityManager().GetComponentObject<Animator>(looksEntity);
            Debug.Log("animator : "+(animator!=null).ToString());
            if (animator!=null)
                UpdateAnimator(animator, states[i], grounds[i]);
        }
    }

    void UpdateAnimator(Animator animator, RoleState state, GroundInfo groundInfo)
    {
        // animator.SetBool("IsGrounded", groundInfo.groundNormal);
        
    }
}