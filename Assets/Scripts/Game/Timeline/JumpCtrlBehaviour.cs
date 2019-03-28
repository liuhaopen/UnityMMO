using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

// A behaviour that is attached to a playable
public class JumpCtrlBehaviour : PlayableBehaviour
{
    public AnimationCurve Curve;
    public Entity Owner;
    public EntityManager EntityMgr;
    private float curTime;
    private float maxTime;
    private int jumpCount;
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        if (EntityMgr.HasComponent<JumpState>(Owner))
        {
            var jumpState = EntityMgr.GetComponentData<JumpState>(Owner);
            jumpState.JumpCount = 0;
            jumpState.JumpStatus = JumpState.State.None;
            EntityMgr.SetComponentData<JumpState>(Owner, jumpState);
        }
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        maxTime= (float)PlayableExtensions.GetDuration(playable);
        var trans = EntityMgr.GetComponentObject<Transform>(Owner);
        if (EntityMgr.HasComponent<JumpState>(Owner) && trans!=null)
        {
            var jumpState = EntityMgr.GetComponentData<JumpState>(Owner);
            jumpCount = jumpState.JumpCount;
            jumpState.OriginYPos = trans.localPosition.y;
            if (jumpState.JumpCount == 0)
            {
                jumpState.AscentHeight = 5;//首次跳，跳高5米
            }
            else if (jumpState.JumpCount == 1)
            {
                jumpState.AscentHeight = 3;
                
            }
            else if (jumpState.JumpCount == 2)
            {
                //第三段跳只是前进
                jumpState.AscentHeight = 0;
            }
            else if (jumpState.JumpCount == 3)
            {
                //第四段跳也只是前进
                jumpState.AscentHeight = 0;
            }
            jumpState.JumpCount++;
            jumpState.JumpCount = math.clamp(jumpState.JumpCount, 0, 3);
            jumpState.JumpStatus = JumpState.State.StartJump;
            Debug.Log("jumpState.JumpCount : "+jumpState.JumpCount);
            EntityMgr.SetComponentData<JumpState>(Owner, jumpState);
        }
        
        // EntityMgr.SetComponentData<TimelineState>(Owner, new TimelineState{NewStatus=newState, InterruptStatus=interruptState});
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // if (!EntityMgr.HasComponent<JumpState>(Owner))
            // return;
        // EntityMgr.SetComponentData<JumpState>(Owner, lastState);
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        curTime += info.deltaTime;
        var trans = EntityMgr.GetComponentObject<Transform>(Owner);
        if (EntityMgr.HasComponent<JumpState>(Owner) && trans!=null)
        {
            var jumpState = EntityMgr.GetComponentData<JumpState>(Owner);
            var percent = Curve.Evaluate(curTime/ maxTime);
            var targetY = jumpState.OriginYPos + jumpState.AscentHeight*percent;
            if (EntityMgr.HasComponent<PosOffset>(Owner))
            {
                var gravity = GameConst.Gravity*Time.deltaTime;//需要抵消重力
                var offsetY = targetY-trans.localPosition.y-gravity;
                Debug.Log("offsetY : "+offsetY);
                var posOffset = new PosOffset{Value=new float3(0, offsetY, 0)};
                EntityMgr.SetComponentData<PosOffset>(Owner, posOffset);
            }
        }
        // var posOffset = EntityMgr.GetComponentData<PosOffset>(Owner);
        // var newYOffset = Curve.Evaluate(curTime/ maxTime);
        // Debug.Log("newYOffset : "+newYOffset);
        // var posOffset = new PosOffset{Value=new float3(0, newYOffset, 0)};
        // if (EntityMgr.HasComponent<PosOffset>(Owner))
        //     EntityMgr.SetComponentData<PosOffset>(Owner, posOffset);
    }
}
