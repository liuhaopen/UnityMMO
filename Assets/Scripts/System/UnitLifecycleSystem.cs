using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

#if false
namespace UnityMMO
{
// [UpdateAfter(typeof())]
public class UnitLifecycleSystem : JobComponentSystem
{
	public struct Units
	{
		public ComponentDataArray<SceneObjectData> transform;
		public EntityArray entities;
		[ReadOnly] public int Length;
	}

	[Inject]
	private Units units;

	public NativeQueue<Entity> queueForKillingEntities;
	public NativeQueue<Entity> deathQueue;
	public NativeQueue<Entity> entitiesForFlying;

	public int MaxDyingUnitsPerFrame = 250;

	public NativeQueue<SceneObjectData> createdUnit;
	private const int CreatedArrowsQueueSize = 100000;

	private const int DeathQueueSize = 80000;

	private Queue<Entity> entitiesThatNeedToBeKilled = new Queue<Entity>(100000);

	protected override unsafe void OnDestroyManager()
	{
		if (queueForKillingEntities.IsCreated) queueForKillingEntities.Dispose();
		if (deathQueue.IsCreated) deathQueue.Dispose();
		if (createdUnit.IsCreated) createdUnit.Dispose();
		if (entitiesForFlying.IsCreated) entitiesForFlying.Dispose();

		base.OnDestroyManager();
	}

	protected override void OnCreateManager(int capacity)
	{
		base.OnCreateManager(capacity);
		if (!queueForKillingEntities.IsCreated) queueForKillingEntities = new NativeQueue<Entity>(Allocator.Persistent);
		if (!entitiesForFlying.IsCreated) entitiesForFlying = new NativeQueue<Entity>(Allocator.Persistent);
	}

	protected override JobHandle OnUpdate(JobHandle inputDeps)
	{
		if (units.entities.Length == 0) return inputDeps;
		inputDeps.Complete();

		if (!deathQueue.IsCreated) deathQueue = new NativeQueue<Entity>(Allocator.Persistent);
		if (!createdUnit.IsCreated) createdUnit = new NativeQueue<SceneObjectData>(Allocator.Persistent);

		while (createdUnit.Count > 0)
		{
			var data = createdUnit.Dequeue();
			// SceneMgr.Instance.AddSceneObject(data);
		}

		UpdateInjectedComponentGroups();

		return new JobHandle();
	}

}
}
#endif