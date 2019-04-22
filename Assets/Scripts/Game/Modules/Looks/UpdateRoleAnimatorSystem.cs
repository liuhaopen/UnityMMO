using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

[DisableAutoCreation]
public class UpdateRoleAnimatorSystem : BaseComponentSystem
{
    public UpdateRoleAnimatorSystem(GameWorld world) : base(world) {}

    ComponentGroup group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetComponentGroup(typeof(LooksInfo), typeof(LocomotionState), typeof(PlayableDirector));
    }

    protected override void OnUpdate()
    {
        var looksInfos = group.GetComponentDataArray<LooksInfo>();
        var locoStates = group.GetComponentDataArray<LocomotionState>();
        var directors = group.GetComponentArray<PlayableDirector>();
        for (int i=0; i<looksInfos.Length; i++)
        {
            var looksInfo = looksInfos[i];
            var director = directors[i];
            // Debug.Log("director.state : "+director.state.ToString());
            if (looksInfo.CurState!=LooksInfo.State.Loaded)
                continue;
            var looksEntity = looksInfo.LooksEntity;
            var animator = m_world.GetEntityManager().GetComponentObject<Animator>(looksEntity);
            if (animator!=null)
                UpdateAnimator(animator, locoStates[i].LocoState);
        }
    }

    void UpdateAnimator(Animator animator, LocomotionState.State locoState)
    {
        // Debug.Log("locoState : "+locoState.ToString());
        if (locoState == LocomotionState.State.Idle && !animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            // animator.CrossFade("idle", 0.2f, 0, Time.deltaTime);
            animator.Play("idle");
        }
        else if (locoState == LocomotionState.State.Run && !animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            // animator.CrossFade("run", 0.2f, 0, Time.deltaTime);
            animator.Play("run");
        }
        else if (locoState == LocomotionState.State.BeHit && !animator.GetCurrentAnimatorStateInfo(0).IsName("behit"))
        {
            animator.Play("behit");
        }
        else if (locoState == LocomotionState.State.Jump && !animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            animator.Play("jump");
        }
        else if (locoState == LocomotionState.State.DoubleJump && !animator.GetCurrentAnimatorStateInfo(0).IsName("jump2"))
        {
            animator.Play("jump2");
        }
        else if (locoState == LocomotionState.State.TrebleJump && !animator.GetCurrentAnimatorStateInfo(0).IsName("jump3"))
        {
            animator.Play("jump3");
        }
    }
}