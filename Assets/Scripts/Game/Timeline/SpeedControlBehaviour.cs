using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;
using UnityMMO.Component;

public class SpeedControlBehaviour : PlayableBehaviour
{
    public float SpeedFactor=1.0f;
    public Entity SpeedOwner;
    public EntityManager EntityMgr;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!EntityMgr.HasComponent<SpeedData>(SpeedOwner))
            return;
        var lastSpeed = EntityMgr.GetComponentObject<SpeedData>(SpeedOwner);
        lastSpeed.ChangeSpeed("SpeedCtrlTL", true, SpeedFactor);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!EntityMgr.HasComponent<SpeedData>(SpeedOwner))
            return;
        var lastSpeed = EntityMgr.GetComponentObject<SpeedData>(SpeedOwner);
        lastSpeed.ChangeSpeed("SpeedCtrlTL", false, SpeedFactor);
    }
}
