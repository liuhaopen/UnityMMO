using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

[System.Serializable]
public class CastSkillClip : PlayableAsset
{
    public int skillID;
    // public int targetID;//有些技能有预选目标，这时会先把该坐标先缓存下来
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<CastSkillBehaviour>.Create(graph);
        var goe = go.GetComponent<GameObjectEntity>();
        if (goe!=null)
        {
            playable.GetBehaviour().Init(goe.Entity, goe.EntityManager, skillID);
        }
        return playable;
    }
}
