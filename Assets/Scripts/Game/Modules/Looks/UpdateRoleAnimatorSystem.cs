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
        group = GetComponentGroup(typeof(LooksInfo), typeof(LocomotionState));
    }

    protected override void OnUpdate()
    {
        // var states = group.GetComponentArray<RoleState>();
        // var grounds = group.GetComponentDataArray<GroundInfo>();
        var looksInfos = group.GetComponentDataArray<LooksInfo>();
        var locoStates = group.GetComponentDataArray<LocomotionState>();
        // Debug.Log("states.Length : "+states.Length);
        for (int i=0; i<looksInfos.Length; i++)
        {
            var looksInfo = looksInfos[i];
            if (looksInfo.CurState!=LooksInfo.State.Loaded)
                continue;
            var looksEntity = looksInfo.LooksEntity;
            var animator = m_world.GetEntityManager().GetComponentObject<Animator>(looksEntity);
            // Debug.Log("animator : "+(animator!=null).ToString());
            if (animator!=null)
                UpdateAnimator(animator, locoStates[i].Value);
        }
    }

    void UpdateAnimator(Animator animator, LocomotionState.State locoState)
    {
        // Debug.Log("locoState : "+locoState.ToString());
        if (locoState == LocomotionState.State.Idle)
        {
            // animator.CrossFade("idle", 0.2f, 0, Time.deltaTime);
            animator.Play("idle");
        }
        else if (locoState == LocomotionState.State.Run)
        {
            // animator.CrossFade("run", 0.2f, 0, Time.deltaTime);
            animator.Play("run");
        }
    }
}