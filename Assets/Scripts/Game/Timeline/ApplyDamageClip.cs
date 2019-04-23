
using System.Collections;
using System.Collections.Generic;
using SprotoType;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

[System.Serializable]
public class ApplyDamageClip : ParamPlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<ApplyDamageBehaviour>.Create(graph);
        var goe = go.GetComponent<GameObjectEntity>();
        if (goe!=null)
        {
            playable.GetBehaviour().Init(goe.Entity, goe.EntityManager, Param);
        }
        return playable;
    }
}
