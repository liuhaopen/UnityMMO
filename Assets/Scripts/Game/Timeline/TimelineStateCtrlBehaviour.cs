using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;
using UnityMMO.Component;

// A behaviour that is attached to a playable
public class TimelineStateCtrlBehaviour : PlayableBehaviour
{
    public bool IsControllNewState = false;
    public bool IsControllInterruptState = false;
    public TimelineState.NewState NewState = TimelineState.NewState.Allow;
    public TimelineState.InterruptState InterruptState = TimelineState.InterruptState.Allow;
    public Entity Owner;
    public EntityManager EntityMgr;

    private TimelineState lastState;
    
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        if (!EntityMgr.HasComponent<TimelineState>(Owner))
            return;
        lastState = EntityMgr.GetComponentData<TimelineState>(Owner);
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!EntityMgr.HasComponent<TimelineState>(Owner))
            return;
        TimelineState.NewState newState = lastState.NewStatus;
        TimelineState.InterruptState interruptState = lastState.InterruptStatus;
        if (IsControllNewState)
            newState = NewState;
        if (IsControllInterruptState)
            interruptState = InterruptState;
        // Debug.Log("new state : "+newState.ToString()+" interruptState"+interruptState.ToString());
        EntityMgr.SetComponentData<TimelineState>(Owner, new TimelineState{NewStatus=newState, InterruptStatus=interruptState});
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!EntityMgr.HasComponent<TimelineState>(Owner))
            return;
        // Debug.Log("pause new state : "+lastState.NewStatus.ToString()+" interruptState"+lastState.InterruptStatus.ToString());
        EntityMgr.SetComponentData<TimelineState>(Owner, lastState);
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
