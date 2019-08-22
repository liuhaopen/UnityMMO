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

    protected override void OnCreate()
    {
        base.OnCreate();
        group = GetEntityQuery(typeof(LooksInfo), typeof(LocomotionState));
    }

    protected override void OnUpdate()
    {
        var looksInfos = group.ToComponentDataArray<LooksInfo>(Allocator.TempJob);
        var locoStates = group.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
        // var directors = group.ToComponentArray<PlayableDirector>();
        for (int i=0; i<looksInfos.Length; i++)
        {
            var looksInfo = looksInfos[i];
            // var director = directors[i];
            // Debug.Log("director.state : "+director.state.ToString());
            if (looksInfo.CurState!=LooksInfo.State.Loaded)
                continue;
            var looksEntity = looksInfo.LooksEntity;
            var animator = EntityManager.GetComponentObject<Animator>(looksEntity);
            if (animator!=null)
                UpdateAnimator(animator, locoStates[i]);
        }
        looksInfos.Dispose();
        locoStates.Dispose();
    }

    void UpdateAnimator(Animator animator, LocomotionState locoData)
    {
        LocomotionState.State locoState = locoData.LocoState;
        string aniName = "";
        if (locoState == LocomotionState.State.Idle && !animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            aniName = "idle";
        }
        else if (locoState == LocomotionState.State.Run && !animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            aniName = "run";
        }
        else if (locoState == LocomotionState.State.BeHit && !animator.GetCurrentAnimatorStateInfo(0).IsName("behit"))
        {
            aniName = "behit";
        }
        else if (locoState == LocomotionState.State.Jump && !animator.GetCurrentAnimatorStateInfo(0).IsName("jump"))
        {
            aniName = "jump";
        }
        else if (locoState == LocomotionState.State.DoubleJump && !animator.GetCurrentAnimatorStateInfo(0).IsName("jump2"))
        {
            aniName = "jump2";
        }
        else if (locoState == LocomotionState.State.TrebleJump && !animator.GetCurrentAnimatorStateInfo(0).IsName("jump3"))
        {
            aniName = "jump3";
        }
        else if (locoState == LocomotionState.State.Dead && !animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
        {
            aniName = "death";
            // Debug.Log("TimeEx.ServerTime:"+TimeEx.ServerTime+" startTime:"+locoData.StartTime+" "+((Time.time - locoData.StartTime)));
            // if (Time.time - locoData.StartTime <= 1)
            // {
                // ------animator.Play("death");
            // }
            // else
            // {
            //     //have been dead for a long time
            //     animator.Play("death", 0, 1.0f);
            // }
        }
        if (aniName != "")
        {
            animator.Play(aniName);
            if (aniName == "idle" || aniName == "run")
            {
                var headTrans = animator.transform.Find("head");
                Animator headAnimator = null;
                if (headTrans != null)
                {
                    headAnimator = headTrans.GetComponent<Animator>();
                    animator.Play(aniName);
                }
                // Debug.Log("locoState : "+locoState+" headTrans:"+(headTrans!=null));
            }
        }
    }
}