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

    }

}