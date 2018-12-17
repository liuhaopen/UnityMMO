using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine.Jobs;

namespace Unity.Transforms
{
    [UnityEngine.ExecuteInEditMode]
    [UpdateBefore(typeof(EndFrameTransformSystem))]
    public class CopyTransformToGameObjectSystem : JobComponentSystem
    {
        [BurstCompile]
        struct CopyTransforms : IJobParallelForTransform
        {
            [ReadOnly] public ComponentDataFromEntity<Position> positions;
            [ReadOnly] public ComponentDataFromEntity<Rotation> rotations;

            [ReadOnly]
            public EntityArray entities;

            public void Execute(int index, TransformAccess transform)
            {
                var entity = entities[index];

                if (positions.Exists(entity))
                    transform.position = positions[entity].Value;

                if (rotations.Exists(entity))
                    transform.rotation = rotations[entity].Value;
            }
        }

        ComponentGroup m_TransformGroup;

        protected override void OnCreateManager()
        {
            m_TransformGroup = GetComponentGroup(ComponentType.ReadOnly(typeof(CopyTransformToGameObject)),typeof(UnityEngine.Transform));
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var transforms = m_TransformGroup.GetTransformAccessArray();
            var entities = m_TransformGroup.GetEntityArray();

            var copyTransformsJob = new CopyTransforms
            {
                positions = GetComponentDataFromEntity<Position>(true),
                rotations = GetComponentDataFromEntity<Rotation>(true),
                entities = entities
            };

            return copyTransformsJob.Schedule(transforms,inputDeps);
        }
    }
}
