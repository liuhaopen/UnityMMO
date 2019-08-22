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
    private MoveSpeed lastSpeed;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        if (!EntityMgr.HasComponent<MoveSpeed>(SpeedOwner))
            return;
        lastSpeed = EntityMgr.GetComponentData<MoveSpeed>(SpeedOwner);
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
       
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        var newSpeedValue = lastSpeed.BaseValue*SpeedFactor;
        EntityMgr.SetComponentData<MoveSpeed>(SpeedOwner, new MoveSpeed{Value=newSpeedValue, BaseValue=lastSpeed.BaseValue});
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!EntityMgr.HasComponent<MoveSpeed>(SpeedOwner))
            return;
        lastSpeed.Value = lastSpeed.BaseValue;
        EntityMgr.SetComponentData<MoveSpeed>(SpeedOwner, lastSpeed);
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
