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
    // public AnimationCurve Curve;
    public Entity Owner;
    public EntityManager EntityMgr;
    bool isMainRole = false;

    public void Init(Entity owner, EntityManager entityMgr)
    {
        Owner = owner;
        EntityMgr = entityMgr;
        isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(Owner);
    }
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }
#if UNITY_EDITOR || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        if (isMainRole)
        {
            GameInput.GetInstance().JoystickDir = Vector2.zero;
        }
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        var trans = EntityMgr.GetComponentObject<Transform>(Owner);
        if (isMainRole)
        {
            GameInput.GetInstance().JoystickDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
#endif 
}
