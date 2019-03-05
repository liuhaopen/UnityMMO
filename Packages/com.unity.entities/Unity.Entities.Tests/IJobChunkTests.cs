using System;
using NUnit.Framework;
using Unity.Collections;


namespace Unity.Entities.Tests
{
    class IJobChunkTests :ECSTestsFixture
    {
        struct ProcessChunks : IJobChunk
        {
            public ArchetypeChunkComponentType<EcsTestData> ecsTestType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int entityOffset)
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

        [Test]
        public void IJobChunkWithEntityOffsetCopy()
        {
            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2));
            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData), typeof(EcsTestData2));

            var entities = new NativeArray<Entity>(50000, Allocator.Temp);
            m_Manager.CreateEntity(archetype, entities);

            for (int i = 0; i < 50000; ++i)
                m_Manager.SetComponentData(entities[i], new EcsTestData { value = i });

            entities.Dispose();

            var copyIndices = group.ToComponentDataArray<EcsTestData>(Allocator.TempJob);

            for (int i = 0; i < 50000; ++i)
                Assert.AreEqual(copyIndices[i].value, i);
            
            copyIndices.Dispose();
        }
        
        struct ProcessChunkIndex : IJobChunk
        {
            public ArchetypeChunkComponentType<EcsTestData> ecsTestType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int entityOffset)
            {
                var testDataArray = chunk.GetNativeArray(ecsTestType);
                testDataArray[0] = new EcsTestData
                {
                    value = chunkIndex
                };
            }
        }
        
        struct ProcessEntityOffset : IJobChunk
        {
            public ArchetypeChunkComponentType<EcsTestData> ecsTestType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int entityOffset)
            {
                var testDataArray = chunk.GetNativeArray(ecsTestType);
                testDataArray[0] = new EcsTestData
                {
                    value = entityOffset
                };
            }
        }
        
        [Test]
        public void IJobChunkProcessChunkIndex()
        {
            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2), typeof(SharedData1));
            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData), typeof(EcsTestData2), typeof(SharedData1));

            var entity1 = m_Manager.CreateEntity(archetype);
            var entity2 = m_Manager.CreateEntity(archetype);

            m_Manager.SetSharedComponentData<SharedData1>(entity1, new SharedData1 { value = 10 });
            m_Manager.SetComponentData<EcsTestData>(entity1, new EcsTestData { value = 10 });

            m_Manager.SetSharedComponentData<SharedData1>(entity2, new SharedData1 { value = 20 });
            m_Manager.SetComponentData<EcsTestData>(entity2, new EcsTestData { value = 20 });

            group.SetFilter(new SharedData1 { value = 10 });

            var job = new ProcessChunkIndex
            {
                ecsTestType = m_Manager.GetArchetypeChunkComponentType<EcsTestData>(false)
            };
            job.Schedule(group).Complete();
            
            group.SetFilter(new SharedData1 { value = 20 });
            job.Schedule(group).Complete();

            Assert.AreEqual(0,  m_Manager.GetComponentData<EcsTestData>(entity1).value);
            Assert.AreEqual(0,  m_Manager.GetComponentData<EcsTestData>(entity2).value);

            group.Dispose();
        }
        
        [Test]
        public void IJobChunkProcessEntityOffset()
        {
            var archetype = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2), typeof(SharedData1));
            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData), typeof(EcsTestData2), typeof(SharedData1));

            var entity1 = m_Manager.CreateEntity(archetype);
            var entity2 = m_Manager.CreateEntity(archetype);

            m_Manager.SetSharedComponentData<SharedData1>(entity1, new SharedData1 { value = 10 });
            m_Manager.SetComponentData<EcsTestData>(entity1, new EcsTestData { value = 10 });

            m_Manager.SetSharedComponentData<SharedData1>(entity2, new SharedData1 { value = 20 });
            m_Manager.SetComponentData<EcsTestData>(entity2, new EcsTestData { value = 20 });

            group.SetFilter(new SharedData1 { value = 10 });

            var job = new ProcessEntityOffset
            {
                ecsTestType = m_Manager.GetArchetypeChunkComponentType<EcsTestData>(false)
            };
            job.Schedule(group).Complete();
            
            group.SetFilter(new SharedData1 { value = 20 });
            job.Schedule(group).Complete();

            Assert.AreEqual(0,  m_Manager.GetComponentData<EcsTestData>(entity1).value);
            Assert.AreEqual(0,  m_Manager.GetComponentData<EcsTestData>(entity2).value);

            group.Dispose();
        }
        
        [Test]
        public void IJobChunkProcessChunkMultiArchetype()
        {
            var archetypeA = m_Manager.CreateArchetype(typeof(EcsTestData));
            var archetypeB = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2));
            var archetypeC = m_Manager.CreateArchetype(typeof(EcsTestData), typeof(EcsTestData2), typeof(EcsTestData3));
            
            var entity1A = m_Manager.CreateEntity(archetypeA);
            var entity2A = m_Manager.CreateEntity(archetypeA);
            
            var entityB = m_Manager.CreateEntity(archetypeB);
            
            var entityC = m_Manager.CreateEntity(archetypeC);
            
            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData));

            m_Manager.SetComponentData<EcsTestData>(entity1A, new EcsTestData { value = -1 });
            m_Manager.SetComponentData<EcsTestData>(entity2A, new EcsTestData { value = -1 });
            m_Manager.SetComponentData<EcsTestData>(entityB,  new EcsTestData { value = -1 });
            m_Manager.SetComponentData<EcsTestData>(entityC,  new EcsTestData { value = -1 });

            var job = new ProcessEntityOffset
            {
                ecsTestType = m_Manager.GetArchetypeChunkComponentType<EcsTestData>(false)
            };
            job.Schedule(group).Complete();

            Assert.AreEqual(0,  m_Manager.GetComponentData<EcsTestData>(entity1A).value);
            Assert.AreEqual(2,  m_Manager.GetComponentData<EcsTestData>(entityB).value);
            Assert.AreEqual(3,  m_Manager.GetComponentData<EcsTestData>(entityC).value);

            group.Dispose();
        }
    }
}
