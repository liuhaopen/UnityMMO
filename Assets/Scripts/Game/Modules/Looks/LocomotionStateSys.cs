using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Playables;
using UnityMMO;
using UnityMMO.Component;

[DisableAutoCreation]
public class LocomotionStateSys : BaseComponentSystem
{
    public LocomotionStateSys(GameWorld world) : base(world) {}

    EntityQuery group;

    protected override void OnCreate()
    {
        base.OnCreate();
        group = GetEntityQuery(typeof(LocomotionState), typeof(LocomotionStateStack));
    }

    protected override void OnUpdate()
    {
        var locoStates = group.ToComponentDataArray<LocomotionState>(Allocator.TempJob);
        var stacks = group.ToComponentArray<LocomotionStateStack>();
        var entities = group.ToEntityArray(Allocator.TempJob);
        for (int i=0; i<entities.Length; i++)
        {
            UpdateState(entities[i], locoStates[i], stacks[i]);
        }
        locoStates.Dispose();
        entities.Dispose();
    }

    void UpdateState(Entity entity, LocomotionState locoData, LocomotionStateStack stack)
    {
        LocomotionState.State locoState = locoData.LocoState;
        if (locoData.StateEndType == LocomotionState.EndType.None)
            return;
        // Debug.Log("be end type : "+locoData.StateEndType+" locoData.EndTime:"+locoData.EndTime);
        if (locoData.StateEndType == LocomotionState.EndType.PlayAnimationOnce)
        {
            if (locoData.EndTime == 0)
            {
                //最少要播放动画一帧
                locoData.EndTime = 1;
                EntityManager.SetComponentData<LocomotionState>(entity, locoData);
                return;
            }
            var hasLooks = EntityManager.HasComponent<LooksInfo>(entity);
            if (!hasLooks)
            {
                Debug.LogError("has no looks! uid : "+EntityManager.GetComponentData<UID>(entity).Value);
                return;
            }
            var looksInfo = EntityManager.GetComponentData<LooksInfo>(entity);
            if (looksInfo.CurState!=LooksInfo.State.Loaded)
                return;
            var looksEntity = looksInfo.LooksEntity;
            Animator animator = EntityManager.GetComponentObject<Animator>(looksEntity);
            if (animator==null)
                return;
            // Debug.Log("animator.GetCurrentAnimatorStateInfo(0).normalizedTime : "+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                BackToLastState(entity, stack);
            }
        }
        else if (locoData.StateEndType == LocomotionState.EndType.EndTime)
        {
            // Debug.Log("locoData.EndTime >= TimeEx.ServerTime : "+(TimeEx.ServerTime >= locoData.EndTime)+" "+locoData.EndTime+" "+TimeEx.ServerTime);
            if (TimeEx.ServerTime >= locoData.EndTime)
            {
                BackToLastState(entity, stack);
            }
        }
    }

    void BackToLastState(Entity entity, LocomotionStateStack stack)
    {
        if (stack.Stack.Count > 0)
        {
            var lastState = stack.Stack.Pop();
            // Debug.Log("ret back state : "+lastState.LocoState+" type:"+lastState.StateEndType+" endTime:"+lastState.EndTime+" "+TimeEx.ServerTime+" "+(TimeEx.ServerTime >= lastState.EndTime));
            EntityManager.SetComponentData<LocomotionState>(entity, lastState);
        }
        else
        {
            EntityManager.SetComponentData<LocomotionState>(entity, new LocomotionState{LocoState=LocomotionState.State.Idle});
        }
    }
}