using System;
using NUnit.Framework;
using Unity.Collections;
using Unity.Jobs;
using System.Collections.Generic;

namespace Unity.Entities.Tests
{
    public class EntityCommandBufferTests : ECSTestsFixture
    {
        [Test]
        public void EmptyOK()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.Playback(m_Manager);
            cmds.Dispose();
        }

        struct TestJob : IJob
        {
            public EntityCommandBuffer Buffer;

            public void Execute()
            {
                Buffer.CreateEntity();
                Buffer.AddComponent(new EcsTestData { value = 1 });
            }
        }

        [Test]
        public void SingleWriterEnforced()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            var job = new TestJob {Buffer = cmds};

            cmds.CreateEntity();
            cmds.AddComponent(new EcsTestData { value = 42 });

            var handle = job.Schedule();

            Assert.Throws<InvalidOperationException>(() => { cmds.CreateEntity(); });
            Assert.Throws<InvalidOperationException>(() => { job.Buffer.CreateEntity(); });

            handle.Complete();

            cmds.Playback(m_Manager);
            cmds.Dispose();

            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData));
            var arr = group.GetComponentDataArray<EcsTestData>();
            Assert.AreEqual(2, arr.Length);
            Assert.AreEqual(42, arr[0].value);
            Assert.AreEqual(1, arr[1].value);
            group.Dispose();
        }

        [Test]
        public void DisposeWhileJobRunningThrows()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            var job = new TestJob {Buffer = cmds};

            var handle = job.Schedule();

            Assert.Throws<InvalidOperationException>(() => { cmds.Dispose(); });

            handle.Complete();

            cmds.Dispose();
        }

        [Test]
        public void ModifiesWhileJobRunningThrows()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            var job = new TestJob {Buffer = cmds};

            var handle = job.Schedule();

            Assert.Throws<InvalidOperationException>(() => { cmds.CreateEntity(); });

            handle.Complete();

            cmds.Dispose();
        }

        [Test]
        public void PlaybackWhileJobRunningThrows()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            var job = new TestJob {Buffer = cmds};

            var handle = job.Schedule();

            Assert.Throws<InvalidOperationException>(() => { cmds.Playback(m_Manager); });

            handle.Complete();

            cmds.Dispose();
        }

        [Test]
        public void ImplicitCreateEntity()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            cmds.AddComponent(new EcsTestData { value = 12 });
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData));
            var arr = group.GetComponentDataArray<EcsTestData>();
            Assert.AreEqual(1, arr.Length);
            Assert.AreEqual(12, arr[0].value);
            group.Dispose();
        }

        [Test]
        public void ImplicitCreateEntityWithArchetype()
        {
            var a = m_Manager.CreateArchetype(typeof(EcsTestData));

            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity(a);
            cmds.SetComponent(new EcsTestData { value = 12 });
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData));
            var arr = group.GetComponentDataArray<EcsTestData>();
            Assert.AreEqual(1, arr.Length);
            Assert.AreEqual(12, arr[0].value);
            group.Dispose();
        }

        [Test]
        public void ImplicitCreateEntityTwice()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            cmds.AddComponent(new EcsTestData { value = 12 });
            cmds.CreateEntity();
            cmds.AddComponent(new EcsTestData { value = 13 });
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var group = m_Manager.CreateComponentGroup(typeof(EcsTestData));
            var arr = group.GetComponentDataArray<EcsTestData>();
            Assert.AreEqual(2, arr.Length);
            Assert.AreEqual(12, arr[0].value);
            Assert.AreEqual(13, arr[1].value);
            group.Dispose();
        }

        [Test]
        public void ImplicitCreateTwoComponents()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            cmds.AddComponent(new EcsTestData { value = 12 });
            cmds.AddComponent(new EcsTestData2 { value0 = 1, value1 = 2 });
            cmds.Playback(m_Manager);
            cmds.Dispose();

            {
                var group = m_Manager.CreateComponentGroup(typeof(EcsTestData));
                var arr = group.GetComponentDataArray<EcsTestData>();
                Assert.AreEqual(1, arr.Length);
                Assert.AreEqual(12, arr[0].value);
                group.Dispose();
            }

            {
                var group = m_Manager.CreateComponentGroup(typeof(EcsTestData2));
                var arr = group.GetComponentDataArray<EcsTestData2>();
                Assert.AreEqual(1, arr.Length);
                Assert.AreEqual(1, arr[0].value0);
                Assert.AreEqual(2, arr[0].value1);
                group.Dispose();
            }
        }

        [Test]
        public void TestMultiChunks()
        {
            const int count = 65536;

            var cmds = new EntityCommandBuffer(Allocator.Temp);
            cmds.MinimumChunkSize = 512;

            for (int i = 0; i < count; ++i)
            {
                cmds.CreateEntity();
                cmds.AddComponent(new EcsTestData { value = i });
                cmds.AddComponent(new EcsTestData2 { value0 = i, value1 = i });
            }

            cmds.Playback(m_Manager);
            cmds.Dispose();

            {
                var group = m_Manager.CreateComponentGroup(typeof(EcsTestData), typeof(EcsTestData2));
                var arr = group.GetComponentDataArray<EcsTestData>();
                var arr2 = group.GetComponentDataArray<EcsTestData2>();
                Assert.AreEqual(count, arr.Length);
                for (int i = 0; i < count; ++i)
                {
                    Assert.AreEqual(i, arr[i].value);
                    Assert.AreEqual(i, arr2[i].value0);
                    Assert.AreEqual(i, arr2[i].value1);
                }
                group.Dispose();
            }
        }

        [Test]
        public void AddSharedComponent()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            var entity = m_Manager.CreateEntity();
            cmds.AddSharedComponent(entity, new EcsTestSharedComp(10));
            cmds.AddSharedComponent(entity, new EcsTestSharedComp2(20));

            cmds.Playback(m_Manager);

            Assert.AreEqual(10, m_Manager.GetSharedComponentData<EcsTestSharedComp>(entity).value);
            Assert.AreEqual(20, m_Manager.GetSharedComponentData<EcsTestSharedComp2>(entity).value1);

            cmds.Dispose();
        }

        [Test]
        public void SetSharedComponent()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            var entity = m_Manager.CreateEntity();
            var sharedComponent = new EcsTestSharedComp(10);
            m_Manager.AddSharedComponentData(entity, sharedComponent);

            cmds.SetSharedComponent(entity, new EcsTestSharedComp(33));

            cmds.Playback(m_Manager);

            Assert.AreEqual(33, m_Manager.GetSharedComponentData<EcsTestSharedComp>(entity).value);

            cmds.Dispose();
        }

        [Test]
        public void RemoveSharedComponent()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            var entity = m_Manager.CreateEntity();
            var sharedComponent = new EcsTestSharedComp(10);
            m_Manager.AddSharedComponentData(entity, sharedComponent);

            cmds.RemoveComponent<EcsTestSharedComp>(entity);

            cmds.Playback(m_Manager);

            Assert.IsFalse(m_Manager.HasComponent<EcsTestSharedComp>(entity), "The shared component was not removed.");

            cmds.Dispose();
        }

        [Test]
        public void ImplicitAddSharedComponent()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            cmds.CreateEntity();
            cmds.AddSharedComponent(new EcsTestSharedComp(10));
            cmds.AddSharedComponent(new EcsTestSharedComp2(20));

            cmds.Playback(m_Manager);

            var sharedComp1List = new List<EcsTestSharedComp>();
            var sharedComp2List = new List<EcsTestSharedComp2>();

            m_Manager.GetAllUniqueSharedComponentData<EcsTestSharedComp>(sharedComp1List);
            m_Manager.GetAllUniqueSharedComponentData<EcsTestSharedComp2>(sharedComp2List);

            // the count must be 2 - the default value of the shared component and the one we actually set
            Assert.AreEqual(2, sharedComp1List.Count);
            Assert.AreEqual(2, sharedComp2List.Count);

            Assert.AreEqual(10, sharedComp1List[1].value);
            Assert.AreEqual(20, sharedComp2List[1].value1);

            cmds.Dispose();
        }

        [Test]
        public void ImplicitSetSharedComponent()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            cmds.CreateEntity();
            cmds.AddSharedComponent(new EcsTestSharedComp(10));
            cmds.SetSharedComponent(new EcsTestSharedComp(33));

            cmds.Playback(m_Manager);

            var sharedCompList = new List<EcsTestSharedComp>();
            m_Manager.GetAllUniqueSharedComponentData<EcsTestSharedComp>(sharedCompList);

            Assert.AreEqual(2, sharedCompList.Count);
            Assert.AreEqual(33, sharedCompList[1].value);

            cmds.Dispose();
        }

        [Test]
        public void ImplicitSetSharedComponentDefault()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            cmds.CreateEntity();
            cmds.AddSharedComponent(new EcsTestSharedComp());
            cmds.SetSharedComponent(new EcsTestSharedComp());

            cmds.Playback(m_Manager);

            var sharedCompList = new List<EcsTestSharedComp>();
            m_Manager.GetAllUniqueSharedComponentData<EcsTestSharedComp>(sharedCompList);

            Assert.AreEqual(1, sharedCompList.Count);
            Assert.AreEqual(0, sharedCompList[0].value);

            cmds.Dispose();
        }

        struct TestJobWithManagedSharedData : IJob
        {
            public EntityCommandBuffer Buffer;
            public EcsTestSharedComp2 Blah;

            public void Execute()
            {
                Buffer.CreateEntity();
                Buffer.AddSharedComponent(Blah);
            }
        }

        [Test]
        public void JobWithSharedComponentData()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            var job = new TestJobWithManagedSharedData { Buffer = cmds, Blah = new EcsTestSharedComp2(12) };

            job.Schedule().Complete();
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var list = new List<EcsTestSharedComp2>();
            m_Manager.GetAllUniqueSharedComponentData<EcsTestSharedComp2>(list);

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(0, list[0].value0);
            Assert.AreEqual(0, list[0].value1);
            Assert.AreEqual(12, list[1].value0);
            Assert.AreEqual(12, list[1].value1);
        }

        // TODO: Burst breaks this test.
        //[BurstCompile(CompileSynchronously = true)]
        public struct TestBurstCommandBufferJob : IJob
        {
            public Entity e0;
            public Entity e1;
            public EntityCommandBuffer Buffer;

            public void Execute()
            {
                Buffer.DestroyEntity(e0);
                Buffer.DestroyEntity(e1);
            }
        }

        [Test]
        public void TestCommandBufferDelete()
        {
            Entity[] entities = new Entity[2];
            for (int i = 0; i < entities.Length; ++i)
            {
                entities[i] = m_Manager.CreateEntity();
                m_Manager.AddComponentData(entities[i], new EcsTestData { value = i });
            }

            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            new TestBurstCommandBufferJob {
                e0 = entities[0],
                e1 = entities[1],
                Buffer = cmds,
            }.Schedule().Complete();

            cmds.Playback(m_Manager);

            cmds.Dispose();

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            allEntities.Dispose();

            Assert.AreEqual(0, count);
        }

        [Test]
        public void TestCommandBufferDeleteWithSystemState()
        {
            Entity[] entities = new Entity[2];
            for (int i = 0; i < entities.Length; ++i)
            {
                entities[i] = m_Manager.CreateEntity();
                m_Manager.AddComponentData(entities[i], new EcsTestData { value = i });
                m_Manager.AddComponentData(entities[i], new EcsState1 { Value = i });
            }

            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            new TestBurstCommandBufferJob {
                e0 = entities[0],
                e1 = entities[1],
                Buffer = cmds,
            }.Schedule().Complete();

            cmds.Playback(m_Manager);

            cmds.Dispose();

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            allEntities.Dispose();

            Assert.AreEqual(entities.Length, count);
        }

        [Test]
        public void TestCommandBufferDeleteRemoveSystemState()
        {
            Entity[] entities = new Entity[2];
            for (int i = 0; i < entities.Length; ++i)
            {
                entities[i] = m_Manager.CreateEntity();
                m_Manager.AddComponentData(entities[i], new EcsTestData { value = i });
                m_Manager.AddComponentData(entities[i], new EcsState1 { Value = i });
            }

            {
                var cmds = new EntityCommandBuffer(Allocator.TempJob);
                new TestBurstCommandBufferJob
                {
                    e0 = entities[0],
                    e1 = entities[1],
                    Buffer = cmds,
                }.Schedule().Complete();

                cmds.Playback(m_Manager);
                cmds.Dispose();
            }

            {
                var cmds = new EntityCommandBuffer(Allocator.TempJob);
                for (var i = 0; i < entities.Length; i++)
                {
                    cmds.RemoveComponent<EcsState1>(entities[i]);
                }

                cmds.Playback(m_Manager);
                cmds.Dispose();
            }

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            allEntities.Dispose();

            Assert.AreEqual(0, count);
        }


        [Test]
        public void Instantiate()
        {
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, new EcsTestData(5));

            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.Instantiate(e);
            cmds.Instantiate(e);
            cmds.Playback(m_Manager);
            cmds.Dispose();

            VerifyEcsTestData(3, 5);
        }

        [Test]
        public void InstantiateWithSetComponentDataWorks()
        {
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, new EcsTestData(5));

            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            cmds.Instantiate(e);
            cmds.SetComponent(new EcsTestData(11));

            cmds.Instantiate(e);
            cmds.SetComponent(new EcsTestData(11));

            cmds.Playback(m_Manager);
            cmds.Dispose();

            m_Manager.DestroyEntity(e);

            VerifyEcsTestData(2, 11);
        }

        [Test]
        public void DestroyEntityTwiceThrows()
        {
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, new EcsTestData(5));

            var cmds = new EntityCommandBuffer(Allocator.TempJob);

            cmds.DestroyEntity(e);
            cmds.DestroyEntity(e);

            Assert.Throws<ArgumentException>(() => cmds.Playback(m_Manager) );
            cmds.Dispose();
        }

        [Test]
        public void TestShouldPlaybackFalse()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            cmds.ShouldPlayback = false;
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            allEntities.Dispose();

            Assert.AreEqual(0, count);
        }

        struct TestConcurrentJob : IJob
        {
            public EntityCommandBuffer.Concurrent Buffer;

            public void Execute()
            {
                Buffer.CreateEntity(0);
                Buffer.AddComponent(0, new EcsTestData { value = 1 });
            }
        }

        [Test]
        public void ConcurrentRecord()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            new TestConcurrentJob { Buffer = cmds.ToConcurrent() }.Schedule().Complete();
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            allEntities.Dispose();

            Assert.AreEqual(2, count);
        }

        struct TestConcurrentParallelForJob : IJobParallelFor
        {
            public EntityCommandBuffer.Concurrent Buffer;

            public void Execute(int index)
            {
                Buffer.CreateEntity(index);
                Buffer.AddComponent(index, new EcsTestData { value = index });
            }
        }

        [Test]
        public void ConcurrentRecordParallelFor()
        {
            const int kCreateCount = 10000;
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            new TestConcurrentParallelForJob { Buffer = cmds.ToConcurrent() }.Schedule(kCreateCount, 64).Complete();
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            Assert.AreEqual(kCreateCount+1, count);
            bool[] foundEntity = new bool[kCreateCount];
            for (int i = 0; i < foundEntity.Length; ++i)
            {
                foundEntity[i] = false;
            }
            for (int i = 0; i < count; ++i)
            {
                if (m_Manager.HasComponent<EcsTestData>(allEntities[i]))
                {
                    var data1 = m_Manager.GetComponentData<EcsTestData>(allEntities[i]);
                    Assert.IsFalse(foundEntity[data1.value]);
                    foundEntity[data1.value] = true;
                }
            }
            for (int i = 0; i < foundEntity.Length; ++i)
            {
                Assert.IsTrue(foundEntity[i]);
            }
            allEntities.Dispose();

        }

        struct TestConcurrentInstantiateJob : IJobParallelFor
        {
            public Entity MasterCopy;
            public EntityCommandBuffer.Concurrent Buffer;

            public void Execute(int index)
            {
                Buffer.Instantiate(index, MasterCopy);
                Buffer.AddComponent(index, new EcsTestData { value = index });
            }
        }

        [Test]
        public void ConcurrentRecordInstantiate()
        {
            const int kInstantiateCount = 10000;
            Entity master = m_Manager.CreateEntity();
            m_Manager.AddComponentData(master, new EcsTestData2 {value0 = 42, value1 = 17});

            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            new TestConcurrentInstantiateJob { Buffer = cmds.ToConcurrent(), MasterCopy = master }.Schedule(kInstantiateCount, 64).Complete();
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            Assert.AreEqual(kInstantiateCount+1, count); // +1 for the master entity
            bool[] foundEntity = new bool[kInstantiateCount];
            for (int i = 0; i < foundEntity.Length; ++i)
            {
                foundEntity[i] = false;
            }
            for (int i = 0; i < count; ++i)
            {
                var data2 = m_Manager.GetComponentData<EcsTestData2>(allEntities[i]);
                Assert.AreEqual(data2.value0, 42);
                Assert.AreEqual(data2.value1, 17);
                if (m_Manager.HasComponent<EcsTestData>(allEntities[i]))
                {
                    var data1 = m_Manager.GetComponentData<EcsTestData>(allEntities[i]);
                    Assert.IsFalse(foundEntity[data1.value]);
                    foundEntity[data1.value] = true;
                }
            }
            for (int i = 0; i < foundEntity.Length; ++i)
            {
                Assert.IsTrue(foundEntity[i]);
            }
            allEntities.Dispose();
        }

        [Test]
        public void PlaybackInvalidatesBuffers()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            DynamicBuffer<EcsIntElement> buffer = cmds.AddBuffer<EcsIntElement>();
            buffer.CopyFrom(new EcsIntElement[] { 1, 2, 3 });
            cmds.Playback(m_Manager);

            // Should not be possible to access the temporary buffer after playback.
            Assert.Throws<InvalidOperationException>(() =>
            {
                buffer.Add(1);
            });
            cmds.Dispose();
        }

        [Test]
        public void ArrayAliasesOfPendingBuffersAreInvalidateOnResize()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            var buffer = cmds.AddBuffer<EcsIntElement>();
            buffer.CopyFrom(new EcsIntElement[] { 1, 2, 3 });
            var array = buffer.ToNativeArray();
            buffer.Add(12);
            Assert.Throws<InvalidOperationException>(() =>
            {
                int val = array[0];
            });
            // Refresh array alias
            array = buffer.ToNativeArray();
            cmds.Playback(m_Manager);

            // Should not be possible to access the temporary buffer after playback.
            Assert.Throws<InvalidOperationException>(() =>
            {
                buffer.Add(1);
            });
            // Array should not be accessible after playback
            Assert.Throws<InvalidOperationException>(() =>
            {
                int l = array[0];
            });
            cmds.Dispose();
        }

        [Test]
        public void AddBufferImplicitNoOverflow()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            DynamicBuffer<EcsIntElement> buffer = cmds.AddBuffer<EcsIntElement>();
            buffer.CopyFrom(new EcsIntElement[] { 1, 2, 3 });
            cmds.Playback(m_Manager);
            VerifySingleBuffer(3);
            cmds.Dispose();
        }

        [Test]
        public void AddBufferImplicitOverflow()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity();
            DynamicBuffer<EcsIntElement> buffer = cmds.AddBuffer<EcsIntElement>();
            buffer.CopyFrom(new EcsIntElement[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            cmds.Playback(m_Manager);
            VerifySingleBuffer(10);
            cmds.Dispose();
        }

        [Test]
        public void AddBufferExplicit()
        {
            var e = m_Manager.CreateEntity();
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            DynamicBuffer<EcsIntElement> buffer = cmds.AddBuffer<EcsIntElement>(e);
            buffer.CopyFrom(new EcsIntElement[] { 1, 2, 3 });
            cmds.Playback(m_Manager);

            VerifySingleBuffer(3);
            cmds.Dispose();
        }

        [Test]
        public void SetBufferImplicit()
        {
            var archetype = m_Manager.CreateArchetype(typeof(EcsIntElement));
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity(archetype);
            DynamicBuffer<EcsIntElement> buffer = cmds.SetBuffer<EcsIntElement>();
            buffer.CopyFrom(new EcsIntElement[] { 1, 2, 3 });
            cmds.Playback(m_Manager);
            VerifySingleBuffer(3);
            cmds.Dispose();
        }

        [Test]
        public void SetBufferExplicit()
        {
            var e = m_Manager.CreateEntity(typeof(EcsIntElement));
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            DynamicBuffer<EcsIntElement> buffer = cmds.SetBuffer<EcsIntElement>(e);
            buffer.CopyFrom(new EcsIntElement[] { 1, 2, 3 });
            cmds.Playback(m_Manager);
            VerifySingleBuffer(3);
            cmds.Dispose();
        }

        [Test]
        public void NoConcurrentOnMainThread()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            var c = cmds.ToConcurrent();
            Assert.Throws<InvalidOperationException>(() => c.CreateEntity(0));
            cmds.Dispose();
        }

        struct DeterminismTestJob : IJobParallelFor
        {
            public EntityCommandBuffer.Concurrent Cmds;

            public void Execute(int index)
            {
                Cmds.CreateEntity(index);
                Cmds.AddComponent(index, new EcsTestData { value = index });
            }
        }

        [Test]
        public void DeterminismTest()
        {
            var cmds = new EntityCommandBuffer(Allocator.TempJob);
            cmds.CreateEntity(); // implicitly, sortIndex=Int32.MaxValue on the main thread
            cmds.AddComponent(new EcsTestData { value = 10000 });
            new DeterminismTestJob { Cmds = cmds.ToConcurrent() }.Schedule(10000, 64).Complete();
            cmds.Playback(m_Manager);
            cmds.Dispose();

            var allEntities = m_Manager.GetAllEntities();
            int count = allEntities.Length;
            Assert.AreEqual(10001, count);
            for (int i = 0; i < count; ++i)
            {
                var data = m_Manager.GetComponentData<EcsTestData>(allEntities[i]);
                Assert.AreEqual(i, data.value);
            }
            allEntities.Dispose();
        }

        [Test]
        public void NoTempAllocatorInConcurrent()
        {
            var cmds = new EntityCommandBuffer(Allocator.Temp);
#pragma warning disable 0219 // assigned but its value is never used
            Assert.Throws<InvalidOperationException>(() => { EntityCommandBuffer.Concurrent c = cmds.ToConcurrent(); });
#pragma warning restore 0219
            cmds.Dispose();
        }


        private void VerifySingleBuffer(int length)
        {
            var allEntities = m_Manager.GetAllEntities();
            Assert.AreEqual(1, allEntities.Length);
            var resultBuffer = m_Manager.GetBuffer<EcsIntElement>(allEntities[0]);
            Assert.AreEqual(length, resultBuffer.Length);

            for (int i = 0; i < length; ++i)
            {
                Assert.AreEqual(i + 1, resultBuffer[i].Value);
            }
            allEntities.Dispose();
        }

        private void VerifyEcsTestData(int length, int expectedValue)
        {
            var allEntities = m_Manager.GetAllEntities();
            Assert.AreEqual(length, allEntities.Length);

            for (int i = 0; i < length; ++i)
            {
                Assert.AreEqual(expectedValue, m_Manager.GetComponentData<EcsTestData>(allEntities[i]).value);
            }
            allEntities.Dispose();
        }

        struct BufferCopyJob : IJobParallelFor
        {
            public EntityCommandBuffer.Concurrent CommandBuffer;
            public NativeArray<Entity> Entities;

            public void Execute(int index)
            {
                var buffer = CommandBuffer.AddBuffer<EcsIntElement>(index, Entities[index]);
                var sourceBuffer = new NativeArray<EcsIntElement>(100, Allocator.Persistent);

                for (var i = 0; i < sourceBuffer.Length; ++i)
                    sourceBuffer[i] = i;

                buffer.CopyFrom(sourceBuffer);

                sourceBuffer.Dispose();
            }
        }

        [Test]
        public void BufferCopyFromDoesNotThrowInJob()
        {
            var archetype = m_Manager.CreateArchetype(ComponentType.Create<EcsTestData>());
            var entities = new NativeArray<Entity>(100, Allocator.Persistent);
            m_Manager.CreateEntity(archetype, entities);

            EntityCommandBuffer cb = new EntityCommandBuffer(Allocator.Persistent);
            var handle = new BufferCopyJob
            {
                CommandBuffer = cb.ToConcurrent(),
                Entities = entities
            }.Schedule(100, 1);
            handle.Complete();
            cb.Playback(m_Manager);

            for (var i = 0; i < 100; ++i)
            {
                var buffer = m_Manager.GetBuffer<EcsIntElement>(entities[i]);
                Assert.AreEqual(100, buffer.Length);
            }

            cb.Dispose();
            entities.Dispose();
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        [Test]
        public void BarrierPlaybackExceptionIsolation()
        {
            var barrier = World.GetOrCreateManager<EndFrameBarrier>();

            var buf1 = barrier.CreateCommandBuffer();
            var buf2 = barrier.CreateCommandBuffer();

            buf1.CreateEntity();
            buf1.AddComponent(new EcsTestData());
            buf1.AddComponent(new EcsTestData());

            buf2.CreateEntity();
            buf2.AddComponent(new EcsTestData());
            buf2.AddComponent(new EcsTestData());

            // We exp both command buffers to execute, and an exception thrown afterwards
            // Essentially we want isolation of two systems that might fail independently.
            Assert.Throws<ArgumentException>(() => { barrier.Update(); });
            Assert.AreEqual(2, EmptySystem.GetComponentGroup(typeof(EcsTestData)).CalculateLength());

            // On second run, we expect all buffers to be removed...
            // So no more exceptions thrown.
            barrier.Update();

            Assert.AreEqual(2, EmptySystem.GetComponentGroup(typeof(EcsTestData)).CalculateLength());
        }
#endif
    }
}
