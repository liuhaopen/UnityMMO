using System.Collections;
using System.Collections.Generic;
using SprotoType;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;
using UnityMMO.Component;

// A behaviour that is attached to a playable
public class ApplyDamageBehaviour : PlayableBehaviour
{
    // public List<scene_fight_defender_info> Defenders;
    public Entity Owner;
    public EntityManager EntityMgr;

    private int leftShowCount = 1;
    
    public void Init(Entity owner, EntityManager entityMgr, object param)
    {
        Owner = owner;
        EntityMgr = entityMgr;
        // Defenders = param as List<scene_fight_defender_info>;
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
        // if (Defenders==null || Defenders.Count<=0 || leftShowCount<=0)
        //     return;
        // for (int i=0; i<Defenders.Count; i++)
        // {
        //     var defender = Defenders[i];
        //     // Debug.Log("defender uid : "+defender.uid+" count:"+Defenders.Count+" damage:"+defender.damage+" hp:"+defender.cur_hp+" damagetype:"+defender.flag);
        //     var defenderEntity = SceneMgr.Instance.GetSceneObject(defender.uid);
        //     // Debug.Log("has health : "+EntityMgr.HasComponent<HealthStateData>(defenderEntity));
        //     if (defenderEntity.Equals(Entity.Null) || ECSHelper.IsDead(defenderEntity, EntityMgr))
        //         continue;
        //     if (EntityMgr.HasComponent<LocomotionState>(defenderEntity))
        //     {
        //         //进入受击状态
        //         var locomotionState = EntityMgr.GetComponentData<LocomotionState>(defenderEntity);
        //         locomotionState.LocoState = LocomotionState.State.BeHit;
        //         locomotionState.StartTime = Time.time;
        //         EntityMgr.SetComponentData<LocomotionState>(defenderEntity, locomotionState);
        //         //显示战斗飘字
        //         var defenderTrans = EntityMgr.GetComponentObject<Transform>(defenderEntity);
        //         var flyWordObj = ResMgr.GetInstance().SpawnGameObject("FightFlyWord");
        //         FightFlyWord flyWord = flyWordObj.GetComponent<FightFlyWord>();
        //         flyWord.SetData(defender.damage, defender.flag);
        //         var pos = defenderTrans.position;
        //         pos += Vector3.up * 1;
        //         flyWord.transform.SetParent(UnityMMO.SceneMgr.Instance.FlyWordContainer);
        //         flyWord.transform.position = pos;
        //         flyWord.StartFly();
        //     }
        // }
        // leftShowCount--;
    }
}
