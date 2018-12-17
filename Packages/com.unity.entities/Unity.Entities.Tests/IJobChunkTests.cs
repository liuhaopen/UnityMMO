using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Unity.Entities.Tests
{
    public class IJobChunkTests :ECSTestsFixture
    {
        struct ProcessChunks : IJobChunk
        {
            public ArchetypeChunkComponentType<EcsTestData> ecsTestType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex)
            {
                var testDataArray = chunk.GetNativeArray(ecsTestType);
                testDataArray[0] = new EcsTestData
                {
                    value = 5
                };
            }
        }

        [Test]
        public void IJobChunkProcess()
        {
            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2));
            var group = m_Manager.CreateComponentGroup(new EntityArchetypeQuery
            {
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
                All = new ComponentType[] { typeof(EcsTestData), typeof(EcsTestData2) }
            });

            var entity = m_Manager.CreateEntity(archetype);
            var job = new ProcessChunks
            {
                ecsTestType = m_Manager.GetArchetypeChunkComponentType<EcsTestData>(false)
            };
            job.Run(group);
            
            Assert.AreEqual(5, m_Manager.GetComponentData<EcsTestData>(entity).value);
        }
                
        [Test]
        public void IJobChunkProcessFiltered()
        {
            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2), typeof(SharedData1));
            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData), typeof(EcsTestData2), typeof(SharedData1));

            var entity1 = m_Manager.CreateEntity(archetype);
            var entity2 = m_Manager.CreateEntity(archetype);

            m_Manager.SetSharedComponentData<SharedData1>(entity1, new SharedData1 { value = 10 });
            m_Manager.SetComponentData<EcsTestData>(entity1, new EcsTestData { value = 10 });

            m_Manager.SetSharedComponentData<SharedData1>(entity2, new SharedData1 { value = 20 });
            m_Manager.SetComponentData<EcsTestData>(entity2, new EcsTestData { value = 20 });

            group.SetFilter(new SharedData1 { value = 20 });

            var job = new ProcessChunks
            {
                ecsTestType = m_Manager.GetArchetypeChunkComponentType<EcsTestData>(false)
            };
            job.Schedule(group).Complete();

            Assert.AreEqual(10, m_Manager.GetComponentData<EcsTestData>(entity1).value);
            Assert.AreEqual(5,  m_Manager.GetComponentData<EcsTestData>(entity2).value);

            group.Dispose();
        }
    }
}
