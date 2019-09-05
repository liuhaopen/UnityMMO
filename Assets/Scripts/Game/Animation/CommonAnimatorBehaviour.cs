using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityMMO;
using UnityMMO.Component;

public class CommonAnimatorBehaviour : StateMachineBehaviour
{
    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log("stateInfo : "+stateInfo.IsName("behit")+" "+stateInfo.ToString());
        var goe = animator.transform.parent.GetComponent<GameObjectEntity>();
        bool hasLocomotion = SceneMgr.Instance.EntityManager.HasComponent<LocomotionState>(goe.Entity);
        // Debug.Log("animator beh haslocomo"+hasLocomotion);
        if (hasLocomotion)
        {
            var locoState = SceneMgr.Instance.EntityManager.GetComponentData<LocomotionState>(goe.Entity);
            // Debug.Log("loco state:"+locoState.LocoState);
            // if (locoState.LocoState == LocomotionState.State.BeHit)
            {
                locoState.LocoState = LocomotionState.State.Idle;
                // locoState.StartTime = Time.time;
                SceneMgr.Instance.EntityManager.SetComponentData<LocomotionState>(goe.Entity, locoState);
            }
        }
    }
}
