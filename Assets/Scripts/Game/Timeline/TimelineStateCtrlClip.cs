using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;
using UnityMMO.Component;

[System.Serializable]
public class TimelineStateCtrlClip : PlayableAsset
{
    public bool IsControllNewState = false;
    public bool IsControllInterruptState = false;
    public TimelineState.NewState NewState = TimelineState.NewState.Allow;
    public TimelineState.InterruptState InterruptState = TimelineState.InterruptState.Allow;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<TimelineStateCtrlBehaviour>.Create(graph);
        playable.GetBehaviour().IsControllNewState = IsControllNewState;
        playable.GetBehaviour().IsControllInterruptState = IsControllInterruptState;
        playable.GetBehaviour().NewState = NewState;
        playable.GetBehaviour().InterruptState = InterruptState;

        var goe = go.GetComponent<GameObjectEntity>();
        if (goe!=null)
        {
            playable.GetBehaviour().Owner = goe.Entity;
            playable.GetBehaviour().EntityMgr = goe.EntityManager;
        }
        return playable;
    }
}
