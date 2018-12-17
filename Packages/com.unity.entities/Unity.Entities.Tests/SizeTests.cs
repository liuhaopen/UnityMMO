using System;
using NUnit.Framework;
using Unity.Collections;

namespace Unity.Entities.Tests
{
    public class SizeTests : ECSTestsFixture
    {
#pragma warning disable 0219 // assigned but its value is never used
        [Test]
        public void SIZ_TagComponentDoesNotChangeCapacity()
        {
            var entity0 = m_Manager.CreateEntity(typeof(EcsTestData));
            var entity1 = m_Manager.CreateEntity(typeof(EcsTestData),typeof(EcsTestTag));
            
            unsafe {
                // a system ran, the version should match the global
                var chunk0 = m_Manager.Entities->GetComponentChunk(entity0);
                var chunk1 = m_Manager.Entities->GetComponentChunk(entity1);
                var archetype0 = chunk0->Archetype;
                var archetype1 = chunk1->Archetype;

                ChunkDataUtility.GetIndexInTypeArray(chunk0->Archetype, TypeManager.GetTypeIndex<EcsTestData2>());

                Assert.AreEqual(archetype0->ChunkCapacity, archetype1->ChunkCapacity);
            }
        }
        
        [Test]
        public void SIZ_TagComponentZeroSize()
        {
            var entity0 = m_Manager.CreateEntity(typeof(EcsTestTag));
            
            unsafe {
                // a system ran, the version should match the global
                var chunk0 = m_Manager.Entities->GetComponentChunk(entity0);
                var archetype0 = chunk0->Archetype;
                var indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(chunk0->Archetype, TypeManager.GetTypeIndex<EcsTestTag>());

                Assert.AreEqual(0, archetype0->SizeOfs[indexInTypeArray]);
            }
        }
        
        [Test]
        unsafe public void SIZ_TagThrowsOnGetComponentData()
        {
            var entity0 = m_Manager.CreateEntity(typeof(EcsTestTag));

            Assert.Throws<ArgumentException>(() =>
            {
               var data = m_Manager.GetComponentData<EcsTestTag>(entity0);
            });
            Assert.Throws<ArgumentException>(() =>
            {
                m_Manager.GetComponentDataRawRW(entity0, ComponentType.Create<EcsTestTag>().TypeIndex);
            });
        }
        
        [Test]
        unsafe public void SIZ_TagThrowsOnSetComponentData()
        {
            var entity0 = m_Manager.CreateEntity(typeof(EcsTestTag));

            Assert.Throws<ArgumentException>(() =>
            {
               m_Manager.SetComponentData(entity0, default(EcsTestTag));
            });
            Assert.Throws<ArgumentException>(() =>
            {
                var value = new EcsTestTag();
                m_Manager.SetComponentDataRaw(entity0, ComponentType.Create<EcsTestTag>().TypeIndex, &value, sizeof(EcsTestTag));
            });
        }

        [Test]
        public void SIZ_TagCanAddComponentData()
        {
            var entity = m_Manager.CreateEntity();
            m_Manager.AddComponentData(entity, default(EcsTestTag));
            Assert.IsTrue(m_Manager.HasComponent<EcsTestTag>(entity));
        }
                
        [Test]
        public void SIZ_TagCannotGetComponentDataArray()
        {
            var group = m_Manager.CreateComponentGroup(typeof(EcsTestTag));
            var entity0 = m_Manager.CreateEntity(typeof(EcsTestTag));

            Assert.Throws<ArgumentException>(() => { group.GetComponentDataArray<EcsTestTag>(); });
        }
        
        [Test]
        public void SIZ_TagThrowsOnComponentDataFromEntity()
        {
            var entity = m_Manager.CreateEntity(typeof(EcsTestTag));
            var fromEntity = m_Manager.GetComponentDataFromEntity<EcsTestTag>();
            Assert.IsTrue(fromEntity.Exists(entity));            
            Assert.Throws<ArgumentException>(() => { var res = fromEntity[entity]; });
        }
        
        [Test]
        public void SIZ_TagCannotGetNativeArrayFromArchetypeChunk()
        {
            m_Manager.CreateEntity(typeof(EcsTestTag));
            var chunks = m_Manager.CreateArchetypeChunkArray(
                new EntityArchetypeQuery
                {
                    Any = Array.Empty<ComponentType>(),
                    None = Array.Empty<ComponentType>(),
                    All = new ComponentType[] {typeof(EcsTestTag)},
                }, Allocator.TempJob);
            
            var tagType = m_Manager.GetArchetypeChunkComponentType<EcsTestTag>(false);

            Assert.AreEqual(1, ArchetypeChunkArray.CalculateEntityCount(chunks));
            
            for (int i = 0; i < chunks.Length; i++)
            {
                var chunk = chunks[i];
                Assert.IsTrue(chunk.Has(tagType));
                Assert.Throws<ArgumentException>(() =>
                {
                    chunk.GetNativeArray(tagType);
                });
            }
            
            chunks.Dispose();
        }
#pragma warning restore 0219
    }
}
