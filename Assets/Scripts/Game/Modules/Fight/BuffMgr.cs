using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityMMO.Component;

namespace UnityMMO
{    
public class BuffMgr
{
    private static BuffMgr instance;
    GameWorld world;
    EntityManager entityMgr;
    Dictionary<SceneConst.Buff, Action<Entity, string[]>> handleFuncDic;

    public static BuffMgr GetInstance()
    {
        if (instance != null)
            return instance;
        instance = new BuffMgr();
        return instance;
    }

    public void Init(GameWorld world)
    {
        this.world = world;
        entityMgr = world.GetEntityManager();
        handleFuncDic = new Dictionary<SceneConst.Buff, Action<Entity, string[]>>();
        handleFuncDic[SceneConst.Buff.SuckHP] = HandleSuckHP;
        handleFuncDic[SceneConst.Buff.Dizzy] = HandleDizzy;
    }

    public void HandleBuff(Entity entity, string buffStr)
    {
        Debug.Log("ApplyChangeInfoBuff : "+buffStr);
        string[] strs = buffStr.Split(',');
        int buffID = int.Parse(strs[0]);
        if (handleFuncDic.ContainsKey((SceneConst.Buff)buffID))
            handleFuncDic[(SceneConst.Buff)buffID](entity, strs);
    }

    public void HandleSuckHP(Entity entity, string[] buffStrs)
    {
        if (entityMgr.HasComponent<SuckHPEffect>(entity))
        {
            var suckHPEffect = entityMgr.GetComponentObject<SuckHPEffect>(entity);
            suckHPEffect.EndTime = TimeEx.ServerTime+400;
            suckHPEffect.Status = EffectStatus.WaitForRender;
        }
    }

    public void HandleDizzy(Entity entity, string[] buffStrs)
    {
        var hasLoco = SceneMgr.Instance.EntityManager.HasComponent<LocomotionState>(entity);
        if (hasLoco)
        {
            var locoState = SceneMgr.Instance.EntityManager.GetComponentData<LocomotionState>(entity);
            locoState.LocoState = LocomotionState.State.Dizzy;
            locoState.StateEndType = LocomotionState.EndType.EndTime;
            long endTime = Int64.Parse(buffStrs[1]);
            Debug.Log("dizzy buff : "+endTime+" "+TimeEx.ServerTime);
            locoState.EndTime = endTime;
            // SceneMgr.Instance.EntityManager.SetComponentData<LocomotionState>(entity, locoState);
            ECSHelper.ChangeLocoState(entity, locoState);
        }
    }

    private BuffMgr()
    {
    }
}
}