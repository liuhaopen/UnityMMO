using System.Collections;
using System.Collections.Generic;
using SprotoType;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;

// A behaviour that is attached to a playable
public class FlyHurtWordBehaviour : PlayableBehaviour
{
    public List<scene_fight_defender_info> Defenders;
    public Entity Owner;
    public EntityManager EntityMgr;

    private int leftShowCount = 1;
    
    public void Init(Entity owner, EntityManager entityMgr, object param)
    {
        Owner = owner;
        EntityMgr = entityMgr;
        Defenders = param as List<scene_fight_defender_info>;
        Debug.Log("Defenders : "+(Defenders!=null).ToString());
    }
    
    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (Defenders==null || Defenders.Count<=0 || leftShowCount<=0)
            return;
        for (int i=0; i<Defenders.Count; i++)
        {
            var defender = Defenders[i];
            Debug.Log("defender uid : "+defender.uid+" damage:"+defender.damage+" hp:"+defender.cur_hp+" damagetype:"+defender.damage_type);
        }
        leftShowCount--;
    }
}
