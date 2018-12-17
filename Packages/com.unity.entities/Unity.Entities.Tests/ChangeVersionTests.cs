using NUnit.Framework;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Unity.Entities.Tests
{
    public class ChangeVersionTests : ECSTestsFixture
    {
        [DisableAutoCreation]
        class BumpVersionSystemInJob : ComponentSystem
        {
#pragma warning disable 649
            struct MyStruct
            {
                public readonly int Length;
                public ComponentDataArray<EcsTestData> Data;
                public ComponentDataArray<EcsTestData2> Data2;
            }

            [Inject]
            MyStruct DataStruct;
#pragma warning restore 649

            struct UpdateData : IJob
            {
                public int Length;
                public ComponentDataArray<EcsTestData> Data;
                public ComponentDataArray<EcsTestData2> Data2;

                public void Execute()
                {
                    for (int i = 0; i < Length; ++i)
                    {
                        var d2 = Data2[i];
                        d2.value0 = 10;
                        Data2[i] = d2;
                    }
                }
            }

            protected override void OnUpdate()
            {
                var updateDataJob = new UpdateData
                {
                    Length = DataStruct.Length,
                    Data = DataStruct.Data,
                    Data2 = DataStruct.Data2
                };
                var updateDataJobHandle = updateDataJob.Schedule();
                updateDataJobHandle.Complete();
            }
        }

        [DisableAutoCreation]
        class BumpVersionSystem : ComponentSystem
        {
            struct MyStruct
            {
#pragma warning disable 649
                public readonly int Length;
                public ComponentDataArray<EcsTestData> Data;
                public ComponentDataArray<EcsTestData2> Data2;
            }

            [Inject]
            MyStruct DataStruct;
#pragma warning restore 649

            protected override void OnUpdate()
            {
                for (int i = 0; i < DataStruct.Length; ++i) {
                    var d2 = DataStruct.Data2[i];
                    d2.value0 = 10;
                    DataStruct.Data2[i] = d2;
                }
            }
        }

        [DisableAutoCreation]
        class BumpChunkTypeVersionSystem : ComponentSystem
        {
            struct UpdateChunks : IJobParallelFor
            {
                public NativeArray<ArchetypeChunk> Chunks;
                public ArchetypeChunkComponentType<EcsTestData> EcsTestDataType;

                public void Execute(int chunkIndex)
                {
                    var chunk = Chunks[chunkIndex];
                    var ecsTestData = chunk.GetNativeArray(EcsTestDataType);
                    for (int i = 0; i < chunk.Count; i++)
                    {
                        ecsTestData[i] = new EcsTestData {value = ecsTestData[i].value + 1};
                    }
                }
            }

            ComponentGroup m_Group;
            private bool m_LastAllChanged;

            protected override void OnCreateManager()
            {
                m_Group = GetComponentGroup(typeof(EcsTestData));
                m_LastAllChanged = false;
            }

            protected override void OnUpdate()
            {
                var chunks = m_Group.CreateArchetypeChunkArray(Allocator.TempJob);
                var ecsTestDataType = GetArchetypeChunkComponentType<EcsTestData>();
                var updateChunksJob = new UpdateChunks
                {
                    Chunks = chunks,
                    EcsTestDataType = ecsTestDataType
                };
                var updateChunksJobHandle = updateChunksJob.Schedule(chunks.Length, 32);
                updateChunksJobHandle.Complete();

                // LastSystemVersion bumped after update. Check for change
                // needs to occur inside system update.
                m_LastAllChanged = true;
                for (int i = 0; i < chunks.Length; i++)
                {
                    m_LastAllChanged &= chunks[i].DidAddOrChange(ecsTestDataType,LastSystemVersion);
                }

                chunks.Dispose();
            }

            public bool AllEcsTestDataChunksChanged()
            {
                return m_LastAllChanged;
            }
        }

        [Test]
        public void CHG_IncrementedOnInjectionInJob()
        {
            var entity0 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var bumpSystem = World.CreateManager<BumpVersionSystemInJob>();

            var oldGlobalVersion = m_Manager.GlobalSystemVersion;

            bumpSystem.Update();

            var value0 = m_Manager.GetComponentData<EcsTestData2>(entity0).value0;
            Assert.AreEqual(10, value0);

            Assert.That(m_Manager.GlobalSystemVersion > oldGlobalVersion);

            unsafe {
                // a system ran, the version should match the global
                var chunk0 = m_Manager.Entities->GetComponentChunk(entity0);
                var td2index0 = ChunkDataUtility.GetIndexInTypeArray(chunk0->Archetype, TypeManager.GetTypeIndex<EcsTestData2>());
                Assert.AreEqual(m_Manager.GlobalSystemVersion, chunk0->ChangeVersion[td2index0]);
            }
        }

        [Test]
        public void CHG_IncrementedOnInjection()
        {
            var entity0 = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));
            var bumpSystem = World.CreateManager<BumpVersionSystem>();

            var oldGlobalVersion = m_Manager.GlobalSystemVersion;

            bumpSystem.Update();

            var value0 = m_Manager.GetComponentData<EcsTestData2>(entity0).value0;
            Assert.AreEqual(10, value0);

            Assert.That(m_Manager.GlobalSystemVersion > oldGlobalVersion);

            unsafe {
                // a system ran, the version should match the global
                var chunk0 = m_Manager.Entities->GetComponentChunk(entity0);
                var td2index0 = ChunkDataUtility.GetIndexInTypeArray(chunk0->Archetype, TypeManager.GetTypeIndex<EcsTestData2>());
                Assert.AreEqual(m_Manager.GlobalSystemVersion, chunk0->ChangeVersion[td2index0]);
            }
        }

        [Test]
        public void CHG_BumpValueChangesChunkTypeVersion()
        {
            m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));

            var bumpChunkTypeVersionSystem = World.CreateManager<BumpChunkTypeVersionSystem>();

            bumpChunkTypeVersionSystem.Update();
            Assert.AreEqual(true, bumpChunkTypeVersionSystem.AllEcsTestDataChunksChanged());

            bumpChunkTypeVersionSystem.Update();
            Assert.AreEqual(true, bumpChunkTypeVersionSystem.AllEcsTestDataChunksChanged());
        }
    }
}
