using System;
using System.Collections.Generic;
using Cinemachine;
using Sproto;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityMMO.Component;
using XLuaFramework;
using static Protocol;

namespace UnityMMO {
public class SynchFromNet {
    private static SynchFromNet instance = null;
    private EntityManager entityMgr;
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
        entityMgr = SceneMgr.Instance.EntityManager;
        changeFuncDic = new Dictionary<SceneInfoKey, Action<Entity, SprotoType.info_item>>();
        changeFuncDic[SceneInfoKey.PosChange] = ApplyChangeInfoPos;
        changeFuncDic[SceneInfoKey.TargetPos] = ApplyChangeInfoTargetPos;
        changeFuncDic[SceneInfoKey.JumpState] = ApplyChangeInfoJumpState;
        changeFuncDic[SceneInfoKey.HPChange] = ApplyChangeInfoHPChange;
        //The main role may not exist until the scene change event is received
        changeFuncDic[SceneInfoKey.SceneChange] = ApplyChangeInfoSceneChange;
        changeFuncDic[SceneInfoKey.Buff] = ApplyChangeInfoBuff;
        changeFuncDic[SceneInfoKey.Speed] = ApplyChangeInfoSpeed;
        changeFuncDic[SceneInfoKey.Exp] = ApplyChangeInfoExp;
    }

    public void StartSynchFromNet()
    {
        ReqSceneObjInfoChange();
        FightMgr.GetInstance().ReqNewFightEvens();
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
                    // Debug.Log("some one enter scene:uid:"+uid+" scene_obj==null:"+(scene_obj==Entity.Null).ToString()+" info:"+cur_change_info.value);
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

    private void ApplyChangeInfoBuff(Entity entity, SprotoType.info_item change_info)
    {
        BuffMgr.GetInstance().HandleBuff(entity, change_info.value);
    }

    private void ApplyChangeInfoSpeed(Entity entity, SprotoType.info_item change_info)
    {
        if (!entityMgr.HasComponent<SpeedData>(entity))
        {
            Debug.Log(string.Format("SynchFromNet:ApplyChangeInfoSpeed[has no speed data com] entity:{0}", entity));
            return;
        }
        string[] strs = change_info.value.Split(',');
        string bodName = strs[0];        
        bool isSet = int.Parse(strs[1])==1;        
        long value = Int64.Parse(strs[2]);        
        var speedData = entityMgr.GetComponentObject<SpeedData>(entity);
        speedData.ChangeSpeed(bodName, isSet, value);
    }

    private void ApplyChangeInfoExp(Entity entity, SprotoType.info_item change_info)
    {
        string[] strs = change_info.value.Split(',');
        long newExp = Int64.Parse(strs[0]);        
        long isUpgrade = Int64.Parse(strs[1]);        
        CSLuaBridge.GetInstance().CallLuaFunc2Num(GlobalEvents.ExpChanged, newExp, isUpgrade);
    }

    private void ApplyChangeInfoSceneChange(Entity entity, SprotoType.info_item change_info)
    {
        //本条信息变更只会收到自己的
        var mainRoleGOE = RoleMgr.GetInstance().GetMainRole();
        entity = mainRoleGOE!=null?mainRoleGOE.Entity:Entity.Null;
        // Debug.Log("ApplyChangeInfoSceneChange : "+change_info.value+" entity:"+entity);
        string[] strs = change_info.value.Split(',');
        int sceneID = int.Parse(strs[0]);
        LoadingView.Instance.SetActive(true);
        LoadingView.Instance.ResetData();
        SceneMgr.Instance.LoadScene(sceneID);

        if (entity != Entity.Null && SceneMgr.Instance.EntityManager.Exists(entity))
        {
            long new_x = Int64.Parse(strs[2]);
            long new_y = Int64.Parse(strs[3]);
            long new_z = Int64.Parse(strs[4]);
            Transform trans = SceneMgr.Instance.EntityManager.GetComponentObject<Transform>(entity);
            trans.localPosition = SceneMgr.Instance.GetCorrectPos(new Vector3(new_x/GameConst.RealToLogic, new_y/GameConst.RealToLogic, new_z/GameConst.RealToLogic));
            SceneMgr.Instance.EntityManager.SetComponentData(entity, new TargetPosition {Value = trans.localPosition});
            long uid = Int64.Parse(strs[1]);
            SceneMgr.Instance.ChangeRoleUID(entity, uid);
            mainRoleGOE.name = "MainRole_"+uid;
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
            if (strs[1]=="relive")
                flag = 5;
        }
        ECSHelper.ChangeHP(entity, Int64.Parse(strs[0]), flag, 0);
    }
    
}
}