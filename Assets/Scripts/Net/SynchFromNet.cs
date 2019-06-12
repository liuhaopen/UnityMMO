using System;
using System.Collections.Generic;
using Sproto;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
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
            SprotoType.scene_listen_fight_event.request req = new SprotoType.scene_listen_fight_event.request();
            NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_listen_fight_event>(req, OnAckFightEvents);
        }
        else
        {
            Timer.Register(0.5f, () => ReqNewFightEvens());
        }
    }

    public void OnAckFightEvents(SprotoTypeBase result)
    {
        SprotoType.scene_listen_fight_event.request req = new SprotoType.scene_listen_fight_event.request();
        NetMsgDispatcher.GetInstance().SendMessage<Protocol.scene_listen_fight_event>(req, OnAckFightEvents);
        SprotoType.scene_listen_fight_event.response ack = result as SprotoType.scene_listen_fight_event.response;
        Debug.Log("ack : "+(ack!=null).ToString()+" fightevents:"+(ack.fight_events!=null).ToString());
        if (ack==null || ack.fight_events==null)
            return;
        var len = ack.fight_events.Count;
        Debug.Log("lisend fight event : "+len);
        for (int i = 0; i < len; i++)
        {
            HandleCastSkill(ack.fight_events[i]);
        }
    }

    private void HandleCastSkill(SprotoType.scene_fight_event_info fight_event)
    {
        long uid = fight_event.attacker_uid;
        Entity scene_entity = SceneMgr.Instance.GetSceneObject(uid);
        var isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(scene_entity);
        // isMainRole = false;//test
        if (scene_entity==Entity.Null || isMainRole)
            return;

        //TODO:预先判断是否能使用技能
        bool is_can_cast = true;
        if (!is_can_cast)
            return;

        //更新朝向
        Transform trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(scene_entity);
        // trans.rotation = Quaternion.Euler(trans.eulerAngles.x, fight_event.direction/100, trans.eulerAngles.z);
        trans.LookAt(new Vector3(fight_event.target_pos_x/GameConst.RealToLogic, trans.localPosition.y, fight_event.target_pos_z/GameConst.RealToLogic));
        
        //播放攻击动作
        string assetPath = SkillManager.GetInstance().GetSkillResPath((int)fight_event.skill_id);
        // Debug.Log("OnAckFightEvents assetPath : "+assetPath);
        var param = new Dictionary<string, object>();
        param["FlyHurtWord"] = fight_event.defenders;
        var timelineInfo = new TimelineInfo{ResPath=assetPath, Owner=scene_entity, Param=param};
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
                    Debug.Log("some one enter scene:uid:"+uid+" scene_obj==null:"+(scene_obj==Entity.Null).ToString());
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
                else if (scene_obj != Entity.Null && changeFuncDic.ContainsKey((SceneInfoKey)cur_change_info.key))
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
        // if (SceneMgr.Instance.EntityManager.HasComponent<Transform>(entity))
        {
            Transform trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(entity);
            trans.localPosition = SceneMgr.Instance.GetCorrectPos(new Vector3(new_x/GameConst.RealToLogic, new_y/GameConst.RealToLogic, new_z/GameConst.RealToLogic));
            SceneMgr.Instance.EntityManager.SetComponentData(entity, new TargetPosition {Value = trans.localPosition});
        }
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

        var trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(entity);
        var curPos = trans.localPosition;
        curPos.y = 0;
        var distance = Vector3.Distance(curPos, newTargetPos);
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
    
    private void ApplyChangeInfoHPChange(Entity entity, SprotoType.info_item change_info)
    {
        // Debug.Log("hp change : "+change_info.value);
        string[] strs = change_info.value.Split(',');
        float curHp = (float)Int64.Parse(strs[0])/GameConst.RealToLogic;
        var healthData = SceneMgr.Instance.EntityManager.GetComponentData<HealthStateData>(entity);
        healthData.CurHp = curHp;
        SceneMgr.Instance.EntityManager.SetComponentData(entity, healthData);
        bool hasNameboardData = SceneMgr.Instance.EntityManager.HasComponent<NameboardData>(entity);
        if (hasNameboardData)
        {
            var nameboardData = SceneMgr.Instance.EntityManager.GetComponentData<NameboardData>(entity);
            if (nameboardData.UIResState==NameboardData.ResState.Loaded)
            {
                var nameboardNode = SceneMgr.Instance.EntityManager.GetComponentObject<Nameboard>(nameboardData.UIEntity);
                if (nameboardNode != null)
                {
                    nameboardNode.CurHp = curHp;
                    //remove nameboard when dead
                    var isDead = strs.Length == 2 && strs[1]=="dead";
                    if (isDead)
                    {
                        SceneMgr.Instance.World.RequestDespawn(nameboardNode.gameObject);
                        nameboardData.UIResState = NameboardData.ResState.DontLoad;
                        nameboardData.UIEntity = Entity.Null;
                        SceneMgr.Instance.EntityManager.SetComponentData(entity, nameboardData);
                    }
                }
            }
            else if (nameboardData.UIResState==NameboardData.ResState.DontLoad)
            {
                var isRelive = strs.Length == 2 && strs[1]=="relive";
                Debug.Log("isRelive : "+isRelive);
                if (isRelive)
                {
                    nameboardData.UIResState = NameboardData.ResState.WaitLoad;
                    SceneMgr.Instance.EntityManager.SetComponentData(entity, nameboardData);
                }
            }
        }
        if (strs.Length == 2)
        {
            var isRelive = strs[1]=="relive";
            var locoState = SceneMgr.Instance.EntityManager.GetComponentData<LocomotionState>(entity);
            locoState.LocoState = isRelive?LocomotionState.State.Idle:LocomotionState.State.Dead;
            // Debug.Log("Time : "+TimeEx.ServerTime.ToString()+" time:"+change_info.time+" isRelive:"+isRelive+" state:"+locoState.LocoState.ToString());
            locoState.StartTime = Time.time - (TimeEx.ServerTime-change_info.time)/1000.0f;
            SceneMgr.Instance.EntityManager.SetComponentData(entity, locoState);
        }
    }
    
}
}