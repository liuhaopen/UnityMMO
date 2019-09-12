using Cinemachine;
using Sproto;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO;
using UnityMMO.Component;

namespace UnityMMO
{
public class FightMgr
{
    private static FightMgr instance;
    GameWorld world;
    EntityManager entityMgr;

    public static FightMgr GetInstance()
    {
        if (instance != null)
            return instance;
        instance = new FightMgr();
        return instance;
    }

    public void Init(GameWorld world)
    {
        this.world = world;
        entityMgr = world.GetEntityManager();
        
    }

    public void ReqNewFightEvens()
    {
        // Debug.Log("GameVariable.IsNeedSynchSceneInfo : "+GameVariable.IsNeedSynchSceneInfo.ToString());
        if (GameVariable.IsNeedSynchSceneInfo)
        {
            SprotoType.scene_listen_skill_event.request req = new SprotoType.scene_listen_skill_event.request();
            NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_listen_skill_event>(req, OnAckSkillEvents);
            SprotoType.scene_listen_hurt_event.request req2 = new SprotoType.scene_listen_hurt_event.request();
            NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_listen_hurt_event>(req2, OnAckHurtEvents);
        }
        else
        {
            Timer.Register(0.5f, () => ReqNewFightEvens());
        }
    }

    public void OnAckHurtEvents(SprotoTypeBase result)
    {
        SprotoType.scene_listen_hurt_event.request req = new SprotoType.scene_listen_hurt_event.request();
        NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_listen_hurt_event>(req, OnAckHurtEvents);
        SprotoType.scene_listen_hurt_event.response ack = result as SprotoType.scene_listen_hurt_event.response;
        // Debug.Log("ack : "+(ack!=null).ToString()+" fightevents:"+(ack.events!=null).ToString());
        if (ack==null || ack.events==null)
            return;
        var len = ack.events.Count;
        // Debug.Log("lisend hurt event : "+len);
        // ack.events.Sort((SprotoType.scene_hurt_event_info a, SprotoType.scene_hurt_event_info b)=>DisallowRefReturnCrossingThisAttribute a.time)
        for (int i = 0; i < len; i++)
        {
            HandleHurtEvent(ack.events[i]);
        }
    }

    private void HandleHurtEvent(SprotoType.scene_hurt_event_info hurtEvent)
    {
        // long uid = hurtEvent.attacker_uid;
        var entityMgr = SceneMgr.Instance.EntityManager;
        if (hurtEvent.defenders==null || hurtEvent.defenders.Count<=0)
            return;
        for (int i=0; i<hurtEvent.defenders.Count; i++)
        {
            var defender = hurtEvent.defenders[i];
            // Debug.Log("defender uid : "+defender.uid+" count:"+hurtEvent.defenders.Count+" hp:"+defender.cur_hp+" damagetype:"+defender.flag);
            var defenderEntity = SceneMgr.Instance.GetSceneObject(defender.uid);
            // Debug.Log("has LocomotionState : "+entityMgr.HasComponent<LocomotionState>(defenderEntity)+" isdead:"+ECSHelper.IsDead(defenderEntity, entityMgr)+" isnull:"+defenderEntity.Equals(Entity.Null));
            if (defenderEntity.Equals(Entity.Null) || ECSHelper.IsDead(defenderEntity))
                continue;
            if (entityMgr.HasComponent<LocomotionState>(defenderEntity))
            {
                var isRole = RoleMgr.GetInstance().IsRoleEntity(defenderEntity);
                //进入受击状态
                bool playBehit = UnityEngine.Random.Range(0, 100) > 40.0f;
                if (!isRole && playBehit)
                {
                    var locomotionState = entityMgr.GetComponentData<LocomotionState>(defenderEntity);
                    locomotionState.LocoState = LocomotionState.State.BeHit;
                    locomotionState.StateEndType = LocomotionState.EndType.PlayAnimationOnce;
                    ECSHelper.ChangeLocoState(defenderEntity, locomotionState);
                    // entityMgr.SetComponentData<LocomotionState>(defenderEntity, locomotionState);
                }
                bool isNeedShakeCamera = (isRole&&playBehit) || !isRole;
                if (isNeedShakeCamera && entityMgr.HasComponent<CinemachineImpulseSource>(defenderEntity))
                {
                    var impulseCom = entityMgr.GetComponentObject<CinemachineImpulseSource>(defenderEntity);
                    var velocity = Vector3.one * defender.change_num/5;
                    impulseCom.GenerateImpulse();
                }
                if (entityMgr.HasComponent<BeHitEffect>(defenderEntity))
                {
                    var behitEffect = entityMgr.GetComponentObject<BeHitEffect>(defenderEntity);
                    behitEffect.EndTime = TimeEx.ServerTime+300;
                    behitEffect.Status = EffectStatus.WaitForRender;
                }
                //显示战斗飘字
                var defenderTrans = entityMgr.GetComponentObject<Transform>(defenderEntity);
                var flyWordObj = ResMgr.GetInstance().GetGameObject("FightFlyWord");
                FightFlyWord flyWord = flyWordObj.GetComponent<FightFlyWord>();
                flyWord.SetData(defender.change_num, defender.flag);
                var pos = defenderTrans.position;
                pos += Vector3.up * 1;
                flyWord.transform.SetParent(UnityMMO.SceneMgr.Instance.FlyWordContainer);
                flyWord.transform.position = pos;
                flyWord.StartFly();
            }
            ECSHelper.ChangeHP(defenderEntity, defender.cur_hp, defender.flag, hurtEvent.attacker_uid);
        }
    }

    public void OnAckSkillEvents(SprotoTypeBase result)
    {
        SprotoType.scene_listen_skill_event.request req = new SprotoType.scene_listen_skill_event.request();
        NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_listen_skill_event>(req, OnAckSkillEvents);
        SprotoType.scene_listen_skill_event.response ack = result as SprotoType.scene_listen_skill_event.response;
        // Debug.Log("ack : "+(ack!=null).ToString()+" skillevents:"+(ack.skill_events!=null).ToString());
        if (ack==null || ack.events==null)
            return;
        var len = ack.events.Count;
        // Debug.Log("lisend skill event : "+len);
        for (int i = 0; i < len; i++)
        {
            HandleCastSkill(ack.events[i]);
        }
    }

    private void HandleCastSkill(SprotoType.scene_skill_event_info skillEvent)
    {
        long uid = skillEvent.attacker_uid;
        Entity scene_entity = SceneMgr.Instance.GetSceneObject(uid);
        var isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(scene_entity);
        // isMainRole = false;//test
        if (scene_entity==Entity.Null || isMainRole)
            return;

        //更新朝向
        Transform trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(scene_entity);
        trans.rotation = Quaternion.Euler(trans.eulerAngles.x, skillEvent.direction/GameConst.RealToLogic, trans.eulerAngles.z);
        // trans.LookAt(new Vector3(skillEvent.target_pos_x/GameConst.RealToLogic, trans.localPosition.y, skillEvent.target_pos_z/GameConst.RealToLogic));
        
        //播放攻击动作
        string assetPath = SkillManager.GetInstance().GetSkillResPath((int)skillEvent.skill_id);
        // Debug.Log("OnAckFightEvents assetPath : "+assetPath);
        // var param = new Dictionary<string, object>();
        // param["FlyHurtWord"] = event.defenders;
        var timelineInfo = new TimelineInfo{ResPath=assetPath, Owner=scene_entity, Param=null};
        TimelineManager.GetInstance().AddTimeline(uid, timelineInfo, SceneMgr.Instance.EntityManager);  
    }
}
}
