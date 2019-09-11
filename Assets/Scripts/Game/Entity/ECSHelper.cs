using UnityEngine;
using Unity.Entities;
using UnityMMO.Component;

namespace UnityMMO
{
    public class ECSHelper
    {
        public static EntityManager entityMgr;

        public static bool IsDead(Entity entity)
        {
            var hpData = entityMgr.GetComponentData<HealthStateData>(entity);
            return hpData.CurHp <= 0;
        }

        public static void ChangeLocoState(Entity entity, LocomotionState newState)
        {
            var hasStateStack = entityMgr.HasComponent<LocomotionStateStack>(entity);
            if (hasStateStack)
            {
                var lastState = entityMgr.GetComponentData<LocomotionState>(entity);
                // Debug.Log("change state entity : "+entity+" state:"+lastState.LocoState+"  "+newState.LocoState+" trace:" + new System.Diagnostics.StackTrace().ToString());
                if (lastState.LocoState != newState.LocoState && LocomotionState.IsStateNeedStack(lastState.LocoState))
                {
                    var stack = entityMgr.GetComponentObject<LocomotionStateStack>(entity);
                    stack.Stack.Push(lastState);
                }
            }
            entityMgr.SetComponentData<LocomotionState>(entity, newState);
        }

        public static void ChangeHP(Entity entity, long hp, long flag, long attackerUID)
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
                // Debug.Log("Time : "+TimeEx.ServerTime.ToString()+" isRelive:"+isRelive+" state:"+locoState.LocoState.ToString());
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

    }

}