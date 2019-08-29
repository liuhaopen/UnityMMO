using System;
using System.Collections.Generic;
using Cinemachine;
using Sproto;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO.Component;
using static Protocol;

namespace UnityMMO {
public class SynchFromNet {
    private static SynchFromNet instance = null;
    Dictionary<SceneInfoKey, Action<Entity, SprotoType.info_item> > changeFuncDic;

    public static SynchFromNet Instance 
    { 
        get  {
            if (instance != null)
                return instance;
            return instance = new SynchFromNet();
        }
    }

    public void Init()
    {
        changeFuncDic = new Dictionary<SceneInfoKey, Action<Entity, SprotoType.info_item>>();
        changeFuncDic[SceneInfoKey.PosChange] = ApplyChangeInfoPos;
        changeFuncDic[SceneInfoKey.TargetPos] = ApplyChangeInfoTargetPos;
        changeFuncDic[SceneInfoKey.JumpState] = ApplyChangeInfoJumpState;
        changeFuncDic[SceneInfoKey.HPChange] = ApplyChangeInfoHPChange;
        //The main role may not exist until the scene change event is received
        changeFuncDic[SceneInfoKey.SceneChange] = ApplyChangeInfoSceneChange;
    }

    public void StartSynchFromNet()
    {
        ReqSceneObjInfoChange();
        ReqNewFightEvens();
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
            // Debug.Log("defender uid : "+defender.uid+" count:"+hurtEvent.defenders.Count+" damage:"+defender.damage+" hp:"+defender.cur_hp+" damagetype:"+defender.flag);
            var defenderEntity = SceneMgr.Instance.GetSceneObject(defender.uid);
            // Debug.Log("has health : "+entityMgr.HasComponent<HealthStateData>(defenderEntity));
            if (defenderEntity.Equals(Entity.Null) || ECSHelper.IsDead(defenderEntity, entityMgr))
                continue;
            if (entityMgr.HasComponent<LocomotionState>(defenderEntity))
            {
                //进入受击状态
                bool playBehit = UnityEngine.Random.RandomRange(0, 100) > 90.0f;
                if (playBehit)
                {
                    var locomotionState = entityMgr.GetComponentData<LocomotionState>(defenderEntity);
                    locomotionState.LocoState = LocomotionState.State.BeHit;
                    locomotionState.StartTime = Time.time;
                    entityMgr.SetComponentData<LocomotionState>(defenderEntity, locomotionState);
                }
                if (entityMgr.HasComponent<CinemachineImpulseSource>(defenderEntity))
                {
                    var impulseCom = entityMgr.GetComponentObject<CinemachineImpulseSource>(defenderEntity);
                    var velocity = Vector3.one * defender.change_num/5;
                    impulseCom.GenerateImpulse();
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
            ChangeHP(defenderEntity, defender.cur_hp, defender.flag, hurtEvent.attacker_uid);
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
        // trans.rotation = Quaternion.Euler(trans.eulerAngles.x, skillEvent.direction/100, trans.eulerAngles.z);
        trans.LookAt(new Vector3(skillEvent.target_pos_x/GameConst.RealToLogic, trans.localPosition.y, skillEvent.target_pos_z/GameConst.RealToLogic));
        
        //播放攻击动作
        string assetPath = SkillManager.GetInstance().GetSkillResPath((int)skillEvent.skill_id);
        // Debug.Log("OnAckFightEvents assetPath : "+assetPath);
        // var param = new Dictionary<string, object>();
        // param["FlyHurtWord"] = event.defenders;
        var timelineInfo = new TimelineInfo{ResPath=assetPath, Owner=scene_entity, Param=null};
        TimelineManager.GetInstance().AddTimeline(uid, timelineInfo, SceneMgr.Instance.EntityManager);  
    }

    public void ReqSceneObjInfoChange()
    {
        // Debug.Log("GameVariable.IsNeedSynchSceneInfo : "+GameVariable.IsNeedSynchSceneInfo.ToString());
        if (GameVariable.IsNeedSynchSceneInfo)
        {
            SprotoType.scene_get_objs_info_change.request req = new SprotoType.scene_get_objs_info_change.request();
            NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_get_objs_info_change>(req, OnAckSceneObjInfoChange);
        }
        else
        {
            Timer.Register(0.5f, () => ReqSceneObjInfoChange());
        }
    }

    public void OnAckSceneObjInfoChange(SprotoTypeBase result)
    {
        SprotoType.scene_get_objs_info_change.request req = new SprotoType.scene_get_objs_info_change.request();
        NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_get_objs_info_change>(req, OnAckSceneObjInfoChange);
        SprotoType.scene_get_objs_info_change.response ack = result as SprotoType.scene_get_objs_info_change.response;
        if (ack==null || ack.obj_infos==null)
            return;
        int len = ack.obj_infos.Count;
        for (int i = 0; i < len; i++)
        {
            long uid = ack.obj_infos[i].scene_obj_uid;
            Entity scene_obj = SceneMgr.Instance.GetSceneObject(uid);
            var change_info_list = ack.obj_infos[i].info_list;
            int info_len = change_info_list.Count;
            // Debug.Log("uid : "+uid.ToString()+ " info_len:"+info_len.ToString());
            for (int info_index = 0; info_index < info_len; info_index++)
            {
                var cur_change_info = change_info_list[info_index];
                // Debug.Log("cur_change_info.key : "+cur_change_info.key.ToString()+" scene_obj:"+(scene_obj!=Entity.Null).ToString()+ " ContainsKey:"+changeFuncDic.ContainsKey((SceneInfoKey)cur_change_info.key).ToString()+" uid"+uid.ToString()+" value:"+cur_change_info.value.ToString());
                if (cur_change_info.key == (int)SceneInfoKey.EnterView)
                {
                    Debug.Log("some one enter scene:uid:"+uid+" scene_obj==null:"+(scene_obj==Entity.Null).ToString()+" info:"+cur_change_info.value);
                    if (scene_obj==Entity.Null)
                    {
                        scene_obj = SceneMgr.Instance.AddSceneObject(uid, cur_change_info.value);
                    }
                }
                else if(cur_change_info.key == (int)SceneInfoKey.LeaveView)
                {
                    if (scene_obj!=Entity.Null)
                    {
                        SceneMgr.Instance.RemoveSceneObject(uid);
                        scene_obj = Entity.Null;
                    }
                }
                else if ((scene_obj != Entity.Null || (SceneInfoKey)cur_change_info.key == SceneInfoKey.SceneChange) && changeFuncDic.ContainsKey((SceneInfoKey)cur_change_info.key))
                {
                    changeFuncDic[(SceneInfoKey)cur_change_info.key](scene_obj, cur_change_info);
                }
            }
        }
    }

    private void ApplyChangeInfoPos(Entity entity, SprotoType.info_item change_info)
    {
        string[] pos_strs = change_info.value.Split(',');
        // Debug.Log("SynchFromNet recieve pos value : "+change_info.value);
        if (pos_strs.Length < 3)
        {
            Debug.Log("SynchFromNet recieve a wrong pos value : "+change_info.value);
            return;
        }
        long new_x = Int64.Parse(pos_strs[0]);
        long new_y = Int64.Parse(pos_strs[1]);
        long new_z = Int64.Parse(pos_strs[2]);
        Transform trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(entity);
        trans.localPosition = SceneMgr.Instance.GetCorrectPos(new Vector3(new_x/GameConst.RealToLogic, new_y/GameConst.RealToLogic, new_z/GameConst.RealToLogic));
        SceneMgr.Instance.EntityManager.SetComponentData(entity, new TargetPosition {Value = trans.localPosition});
    }

    private void ApplyChangeInfoTargetPos(Entity entity, SprotoType.info_item change_info)
    {
        string[] pos_strs = change_info.value.Split(',');
        // Debug.Log("SynchFromNet recieve pos value : "+change_info.value);
        if (pos_strs.Length != 2)
        {
            Debug.Log("SynchFromNet recieve a wrong pos value : "+change_info.value);
            return;
        }
        long new_x = Int64.Parse(pos_strs[0]);
        // long new_y = Int64.Parse(pos_strs[1]);
        long new_z = Int64.Parse(pos_strs[1]);
        var newTargetPos = new float3(new_x/GameConst.RealToLogic, 0, new_z/GameConst.RealToLogic);

        // var trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(entity);
        // var curPos = trans.localPosition;
        // curPos.y = 0;
        // var distance = Vector3.Distance(curPos, newTargetPos);
        // if (distance > 100)
        // {
        //     //如果目标坐标距离人物当前坐标很远就直接设置
        //     newTargetPos.y = trans.localPosition.y;
        //     var newPos = SceneMgr.Instance.GetCorrectPos(newTargetPos);
        //     trans.localPosition = newPos;
        //     return;
        // }
        SceneMgr.Instance.EntityManager.SetComponentData(entity, new TargetPosition {Value = newTargetPos});
    }

    private void ApplyChangeInfoJumpState(Entity entity, SprotoType.info_item change_info)
    {
        var actionData = SceneMgr.Instance.EntityManager.GetComponentData<ActionData>(entity);
        actionData.Jump = 1;
        SceneMgr.Instance.EntityManager.SetComponentData(entity, actionData);
    }

    private void ApplyChangeInfoSceneChange(Entity entity, SprotoType.info_item change_info)
    {
        Debug.Log("ApplyChangeInfoSceneChange : "+change_info.value);
        string[] strs = change_info.value.Split(',');
        int sceneID = int.Parse(strs[0]);
        LoadingView.Instance.SetActive(true);
        LoadingView.Instance.ResetData();
        SceneMgr.Instance.LoadScene(sceneID);

        if (entity != Entity.Null)
        {
            long new_x = Int64.Parse(strs[2]);
            long new_y = Int64.Parse(strs[3]);
            long new_z = Int64.Parse(strs[4]);
            Transform trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(entity);
            trans.localPosition = SceneMgr.Instance.GetCorrectPos(new Vector3(new_x/GameConst.RealToLogic, new_y/GameConst.RealToLogic, new_z/GameConst.RealToLogic));
            SceneMgr.Instance.EntityManager.SetComponentData(entity, new TargetPosition {Value = trans.localPosition});
            // var uidData = SceneMgr.Instance.EntityManager.GetComponentData<UID>(entity);
            long uid = Int64.Parse(strs[1]);
            SceneMgr.Instance.EntityManager.SetComponentData<UID>(entity, new UID{Value=uid});
            // SceneMgr.Instance.EntityManager.SetComponentData<UID>(entity);
            MoveQuery moveQuery = SceneMgr.Instance.EntityManager.GetComponentObject<MoveQuery>(entity);
            // Debug.Log("ApplyChangeInfoSceneChange new uid : "+uid+" moveQuery:"+(moveQuery!=null));
            if (moveQuery != null)
                moveQuery.ChangeUID(uid);
            // var uidProxy = SceneMgr.Instance.EntityManager.GetComponentObject<UIDProxy>(entity);
            // // SceneMgr.Instance.EntityManager.SetComponentData<UID>(entity);
            // if (uidProxy!=null)
            // {
            //     long uid = Int64.Parse(strs[1]);
            //     uidProxy.Value = new UID{Value=uid};
            //     MoveQuery moveQuery = SceneMgr.Instance.EntityManager.GetComponentObject<MoveQuery>(entity);
            //     // Debug.Log("ApplyChangeInfoSceneChange new uid : "+uid+" moveQuery:"+(moveQuery!=null));
            //     if (moveQuery != null)
            //     {
            //         moveQuery.ChangeUID(uid);
            //     }
            // }
        }
    }

    private void ChangeHP(Entity entity, long hp, long flag, long attackerUID)
    {
        float curHp = (float)hp;
        var healthData = SceneMgr.Instance.EntityManager.GetComponentData<HealthStateData>(entity);
        healthData.CurHp = curHp;
        SceneMgr.Instance.EntityManager.SetComponentData(entity, healthData);
        bool hasNameboardData = SceneMgr.Instance.EntityManager.HasComponent<NameboardData>(entity);
        var isRelive = flag==5;//复活
        var isDead = hp==0;//死亡
        if (hasNameboardData)
        {
            var nameboardData = SceneMgr.Instance.EntityManager.GetComponentObject<NameboardData>(entity);
            if (nameboardData.UIResState==NameboardData.ResState.Loaded)
            {
                var nameboardNode = nameboardData.LooksNode.GetComponent<Nameboard>();
                if (nameboardNode != null)
                {
                    nameboardNode.CurHp = curHp;
                    //remove nameboard when dead
                    if (isDead)
                    {
                        nameboardData.UnuseLooks();
                        // SceneMgr.Instance.World.RequestDespawn(nameboardNode.gameObject);
                        nameboardData.UIResState = NameboardData.ResState.DontLoad;
                        // nameboardData.LooksNode = null;
                        // SceneMgr.Instance.EntityManager.SetComponentData(entity, nameboardData);
                    }
                }
            }
            else if (nameboardData.UIResState==NameboardData.ResState.DontLoad)
            {
                if (isRelive)
                {
                    nameboardData.UIResState = NameboardData.ResState.WaitLoad;
                    // SceneMgr.Instance.EntityManager.SetComponentData(entity, nameboardData);
                }
            }
        }
        if (isDead || isRelive)
        {
            // var isRelive = strs[1]=="relive";
            var locoState = SceneMgr.Instance.EntityManager.GetComponentData<LocomotionState>(entity);
            locoState.LocoState = isRelive?LocomotionState.State.Idle:LocomotionState.State.Dead;
            Debug.Log("Time : "+TimeEx.ServerTime.ToString()+" isRelive:"+isRelive+" state:"+locoState.LocoState.ToString());
            // locoState.StartTime = Time.time - (TimeEx.ServerTime-change_info.time)/1000.0f;//CAT_TODO:dead time
            SceneMgr.Instance.EntityManager.SetComponentData(entity, locoState);
            if (isDead && RoleMgr.GetInstance().IsMainRoleEntity(entity))
            {
                // var attackerName = SceneMgr.Instance.GetNameByUID(attackerUID);
                RoleMgr.GetInstance().StopMainRoleRunning();
                XLuaFramework.CSLuaBridge.GetInstance().CallLuaFuncNum(GlobalEvents.MainRoleDie, attackerUID);
            }
        }
    }
    
    private void ApplyChangeInfoHPChange(Entity entity, SprotoType.info_item change_info)
    {
        // Debug.Log("hp change : "+change_info.value);
        string[] strs = change_info.value.Split(',');
        float curHp = (float)Int64.Parse(strs[0])/GameConst.RealToLogic;
        long flag = 0;
        if (strs.Length == 2)
        {
            // if (strs[1]=="dead")
            //     flag = 4;
            // else 
            if (strs[1]=="relive")
                flag = 5;
        }
        ChangeHP(entity, Int64.Parse(strs[0]), flag, 0);
    }
    
}
}