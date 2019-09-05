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
        else if (locoData.StateEndType == LocomotionState.EndType.PlayAnimationOnce)
        {
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
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                BackToLastState(entity, stack);
            }
        }
        else if (locoData.StateEndType == LocomotionState.EndType.EndTime)
        {
            if (locoData.EndTime >= TimeEx.ServerTime)
            {
                BackToLastState(entity, stack);
            }
        }
    }

    void BackToLastState(Entity entity, LocomotionStateStack stack)
    {
        var lastState = stack.Stack.Pop();
        EntityManager.SetComponentData<LocomotionState>(entity, lastState);
    }
}