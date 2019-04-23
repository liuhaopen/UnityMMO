using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityMMO;

public class CommonAnimatorBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log("stateInfo : "+stateInfo.IsName("behit")+" "+stateInfo.ToString());
        var goe = animator.transform.parent.GetComponent<GameObjectEntity>();
        if (SceneMgr.Instance.EntityManager.HasComponent<LocomotionState>(goe.Entity))
        {
            var locoState = SceneMgr.Instance.EntityManager.GetComponentData<LocomotionState>(goe.Entity);
            // if (stateInfo.IsName("behit"))
            // {
                locoState.LocoState = LocomotionState.State.Idle;
                locoState.StartTime = Time.time;
                SceneMgr.Instance.EntityManager.SetComponentData<LocomotionState>(goe.Entity, locoState);
            // }
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
