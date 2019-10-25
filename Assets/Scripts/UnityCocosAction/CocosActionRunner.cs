using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cocos
{
    
[DisallowMultipleComponent]
public class ActionRunner : MonoBehaviour
{
    bool paused = true;
    Action currentAction;

    public static ActionRunner GetOrCreate(GameObject obj)
    {
        var runner = obj.GetComponent<ActionRunner>();
        if (runner == null)
            runner = obj.AddComponent<ActionRunner>();
        return runner;
    }

    void Update()
    {
        if (paused || currentAction==null)
            return;
        float deltaTime = Time.deltaTime;
        currentAction.Step(deltaTime);
        if (currentAction.IsDone())
        {
            currentAction.Stop();
            currentAction = null;
        }
    }

    public void Reset() 
    {
        currentAction = null;
        paused = true;
    }

    //不需要支持AddAction了，需要同时播放几个action的用SpwanAction串起来再传进来吧
    public void PlayAction(Action action, bool isPause=false)
    {
        currentAction = action;
        action.StartWithTarget(transform);
        paused = isPause;
    }

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