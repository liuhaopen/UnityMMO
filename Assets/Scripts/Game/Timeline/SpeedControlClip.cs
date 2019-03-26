using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SpeedControlClip : PlayableAsset
{
    public float SpeedFactor = 1.0f;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SpeedControlBehaviour>.Create(graph);
        playable.GetBehaviour().SpeedFactor = SpeedFactor;
        var goe = go.GetComponent<GameObjectEntity>();
        if (goe!=null)
        {
            playable.GetBehaviour().SpeedOwner = goe.Entity;
            playable.GetBehaviour().EntityMgr = goe.EntityManager;
        }
        return playable;
    }
}
