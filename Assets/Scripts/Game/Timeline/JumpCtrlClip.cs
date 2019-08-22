using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

[System.Serializable]
public class JumpCtrlClip : PlayableAsset
{
//    public AnimationCurve Curve;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<JumpCtrlBehaviour>.Create(graph);
        // playable.GetBehaviour().Curve = Curve;
        var goe = go.GetComponent<GameObjectEntity>();
        if (goe!=null)
        {
            playable.GetBehaviour().Init(goe.Entity, goe.EntityManager);
        }
        return playable;
    }
}
