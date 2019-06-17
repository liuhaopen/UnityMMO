using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;
using UnityMMO.Component;

[DisableAutoCreation]
public class UpdateAnimatorSystem : BaseComponentSystem
{
    public UpdateAnimatorSystem(GameWorld world) : base(world) {}

    EntityQuery group;

    protected override void OnCreateManager()
    {
        base.OnCreateManager();
        group = GetEntityQuery(typeof(LooksInfo), typeof(LocomotionState), typeof(PlayableDirector));
    }

    protected override void OnUpdate()
    {
        var looksInfos = group.ToComponentDataArray<LooksInfo>(Allocator.TempJob);
        var locoStates = group.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
        var directors = group.ToComponentArray<PlayableDirector>();
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
                UpdateAnimator(animator, locoStates[i]);
        }
        looksInfos.Dispose();
        locoStates.Dispose();
    }

    void UpdateAnimator(Animator animator, LocomotionState locoData)
    {
        LocomotionState.State locoState = locoData.LocoState;
        // Debug.Log("locoState : "+locoState);
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
        else if (locoState == LocomotionState.State.Dead && !animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
        {
            // Debug.Log("TimeEx.ServerTime:"+TimeEx.ServerTime+" startTime:"+locoData.StartTime+" "+((Time.time - locoData.StartTime)));
            // if (Time.time - locoData.StartTime <= 1)
            // {
                animator.Play("death");
            // }
            // else
            // {
            //     //have been dead for a long time
            //     animator.Play("death", 0, 1.0f);
            // }
        }
        
    }
}