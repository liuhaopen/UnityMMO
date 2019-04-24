using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cocos
{
[DisallowMultipleComponent]
public class ActionRunner : MonoBehaviour
{
    // List<Action> actions = new List<Action>();
    // int actionIndex;
    bool paused = true;
    // bool currentActionSalvaged;
    Action currentAction;

    void Update()
    {
        if (paused || currentAction==null)
            return;
        float deltaTime = Time.deltaTime;
        // currentActionSalvaged = false;
        currentAction.Step(deltaTime);
        if (currentAction.IsDone())
        {
            currentAction.Stop();
            currentAction = null;
        }
        // for (actionIndex = 0; actionIndex < actions.Count; actionIndex++)
        // {
        //     currentAction = actions[actionIndex];
        //     if (currentAction == null)
        //         continue;
        //     currentActionSalvaged = false;
        //     currentAction.Step(deltaTime);
        //     if (currentAction.IsDone())
        //     {
        //         Action action = currentAction;
        //         currentAction.Stop();
        //         currentAction = null;
        //         RemoveAction(action);
        //     }
        //     currentAction = null;
        // }
    }

    // public void ClearActions()
    // {
    //     actions.Clear();
    // }

    public void Reset() 
    {
        // ClearActions();
        currentAction = null;
        paused = true;
    }

    //不需要支持AddAction了，需要同时播放几个action的用SpwanAction串起来再传进来吧
    public void PlayAction(Action action, bool isPause=false)
    {
        // actions.Add(action);
        currentAction = action;
        action.StartWithTarget(transform);
        paused = isPause;
        // if (!isPause)
        //     Play();
    }

    // public void RemoveAction(Action action)
    // {
    //     if (action == null)
    //     {
    //         return;
    //     }
    //     if (currentAction == action && !currentActionSalvaged)
    //     {
    //         currentActionSalvaged = true;
    //     }
    //     int index = actions.FindIndex(0, actions.Count, a=>a==action);
    //     Debug.Log("remove action : "+index);
    //     // actions.Remove(action);
    //     if (index != -1)
    //         RemoveActionAtIndex(index, action);
    // }

    // public void RemoveActionAtIndex(int index, Action action)
    // {
    //     if (action == currentAction && !currentActionSalvaged)
    //     {
    //         currentActionSalvaged = true;
    //     }
    //     actions.Remove(action);
    //     // ccArrayRemoveObjectAtIndex(actions, index, true);

    //     // update actionIndex in case we are in tick. looping over the actions
    //     if (actionIndex >= index)
    //     {
    //         actionIndex--;
    //     }
    //     // if (actions.Count == 0)
    //     // {
    //     //     currentTargetSalvaged = true;
    //     // }
    // }

    public void Play()
    {
        paused = false;
    }

    public void Pause()
    {
        paused = true;
    }

    public void Stop()
    {
        Reset();
    }

    private void OnDestroy() 
    {
        Reset();    
    }
}
}