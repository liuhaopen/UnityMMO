using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

[System.Serializable]
public class FlyHurtWordClip : ParamPlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<FlyHurtWordBehaviour>.Create(graph);
        var goe = go.GetComponent<GameObjectEntity>();
        if (goe!=null)
        {
            Debug.Log("Param : !=null "+(Param!=null).ToString());
            playable.GetBehaviour().Init(goe.Entity, goe.EntityManager, Param);
        }
        return playable;
    }
}
