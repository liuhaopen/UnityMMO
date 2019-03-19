using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class GameInputBlockClip : PlayableAsset
{
    public bool IsBlock;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<GameInputBlockBehaviour>.Create(graph);
        playable.GetBehaviour().IsBlock = IsBlock;
        return playable;
    }
}
