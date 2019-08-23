using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;
using UnityMMO.Component;

// A behaviour that is attached to a playable
public class SpeedControlBehaviour : PlayableBehaviour
{
    public float SpeedFactor=1.0f;
    public Entity SpeedOwner;
    public EntityManager EntityMgr;

    // Called when the owning graph starts playing
    // public override void OnGraphStart(Playable playable)
    // {
    // }

    // // Called when the owning graph stops playing
    // public override void OnGraphStop(Playable playable)
    // {
    // }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!EntityMgr.HasComponent<MoveSpeed>(SpeedOwner))
            return;
        var lastSpeed = EntityMgr.GetComponentData<MoveSpeed>(SpeedOwner);
        var newSpeedValue = lastSpeed.BaseValue*SpeedFactor;
        // Debug.Log("newSpeedValue.Value : "+newSpeedValue);
        EntityMgr.SetComponentData<MoveSpeed>(SpeedOwner, new MoveSpeed{Value=newSpeedValue, BaseValue=lastSpeed.BaseValue});
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!EntityMgr.HasComponent<MoveSpeed>(SpeedOwner))
            return;
        var lastSpeed = EntityMgr.GetComponentData<MoveSpeed>(SpeedOwner);
        lastSpeed.Value = lastSpeed.BaseValue;
        // Debug.Log("lastSpeed.Value : "+lastSpeed.Value);
        EntityMgr.SetComponentData<MoveSpeed>(SpeedOwner, lastSpeed);
    }

    // Called each frame while the state is set to Play
    // public override void PrepareFrame(Playable playable, FrameData info)
    // {
    // }
}
