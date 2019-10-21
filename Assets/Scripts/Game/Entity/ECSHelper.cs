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
            var healthData = entityMgr.GetComponentData<HealthStateData>(entity);
            healthData.CurHp = curHp;
            entityMgr.SetComponentData(entity, healthData);
            var isMainRole = RoleMgr.GetInstance().IsMainRoleEntity(entity);
            if (isMainRole)
                XLuaFramework.CSLuaBridge.GetInstance().CallLuaFunc2Num(GlobalEvents.MainRoleHPChanged, hp, (long)healthData.MaxHp);
            bool hasNameboardData = entityMgr.HasComponent<NameboardData>(entity);
            var isRelive = flag==5;//复活
            var isDead = hp==0;//死亡
            if (hasNameboardData)
            {
                var nameboardData = entityMgr.GetComponentObject<NameboardData>(entity);
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
                            // entityMgr.SetComponentData(entity, nameboardData);
                        }
                    }
                }
                else if (nameboardData.UIResState==NameboardData.ResState.DontLoad)
                {
                    if (isRelive)
                    {
                        nameboardData.UIResState = NameboardData.ResState.WaitLoad;
                        // entityMgr.SetComponentData(entity, nameboardData);
                    }
                }
            }
            if (isDead || isRelive)
            {
                // var isRelive = strs[1]=="relive";
                var locoState = entityMgr.GetComponentData<LocomotionState>(entity);
                locoState.LocoState = isRelive?LocomotionState.State.Idle:LocomotionState.State.Dead;
                // Debug.Log("Time : "+TimeEx.ServerTime.ToString()+" isRelive:"+isRelive+" state:"+locoState.LocoState.ToString());
                // locoState.StartTime = Time.time - (TimeEx.ServerTime-change_info.time)/1000.0f;//CAT_TODO:dead time
                entityMgr.SetComponentData(entity, locoState);
                if (isDead && isMainRole)
                {
                    // var attackerName = SceneMgr.Instance.GetNameByUID(attackerUID);
                    RoleMgr.GetInstance().StopMainRoleRunning();
                    XLuaFramework.CSLuaBridge.GetInstance().CallLuaFuncNum(GlobalEvents.MainRoleDie, attackerUID);
                }
            }
        }

        public static void UpdateNameboardHeight(Entity entity, Transform looksTrans)
        {
            if (entityMgr.HasComponent<NameboardData>(entity))
            {
                var meshRender = looksTrans.GetComponentInChildren<SkinnedMeshRenderer>();
                // Debug.Log(string.Format("RoleLooksModule[260] (meshRender!=null):{0}", (meshRender!=null)));
                if (meshRender != null)
                {
                    var nameboardData = entityMgr.GetComponentObject<NameboardData>(entity);
                    nameboardData.Height = meshRender.bounds.size.y;
                    // Debug.Log(string.Format("RoleLooksModule[264] nameboardData.Height:{0}", nameboardData.Height));
                }
            }
        }

    }

}