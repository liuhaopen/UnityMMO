using NUnit.Framework;
using Unity.Collections;

namespace Unity.Entities.Tests
{
    public class WorldDiffTestFixture
    {
        protected World m_PreviousWorld;
        protected World m_Shadow;
        protected World m_After;
        protected World m_DstWorld;
        protected EntityManager m_Manager;
        protected EntityManager m_DstManager;


        // There's no particular meaning to this bit pattern. The important thing is that the whole bit pattern isn't 0.
        // If all the bits are 0, then the GUID has the special value of NULL ("invalid") GUID.
        public static EntityGuid GenerateEntityGuid(int i)
        {
            return new EntityGuid {a = (ulong)i, b = ~(ulong)i};
        }
        
        [SetUp]
        public void SetUp()
        {
            m_PreviousWorld = World.Active;
            m_Shadow = new World("Before");
            m_After = new World("After");
            m_DstWorld = new World("DstWorld");
            
            m_Manager = m_After.GetOrCreateManager<EntityManager>();
            m_DstManager = m_DstWorld.GetOrCreateManager<EntityManager>();
        }

        
        [TearDown]
        public void TearDown()
        {
            World.Active = m_PreviousWorld;

            m_Shadow.GetOrCreateManager<EntityManager>().Debug.CheckInternalConsistency();
            m_After.GetOrCreateManager<EntityManager>().Debug.CheckInternalConsistency();
            m_DstWorld.GetOrCreateManager<EntityManager>().Debug.CheckInternalConsistency();

            m_Shadow.Dispose();
            m_After.Dispose();
            m_DstWorld.Dispose();
        }

        public NativeHashMap<EntityGuid, Entity> BuildLookup()
        {
            var map = new NativeHashMap<EntityGuid, Entity>(m_DstManager.Debug.EntityCount, Allocator.Temp);
            var entities = m_DstManager.GetAllEntities();
            foreach (var e in entities)
            {
                var guidOnEntity = m_DstManager.GetComponentData<EntityGuid>(e);
                Assert.IsTrue(map.TryAdd(guidOnEntity, e));
            }

            return map;
        }

        public T GetDstWorldData<T>(EntityGuid guid, NativeHashMap<EntityGuid, Entity> lookup) where T : struct, IComponentData
        {
            var e = lookup[guid];
            return m_DstManager.GetComponentData<T>(e);
        }

        public T GetDstWorldSharedData<T>(EntityGuid guid, NativeHashMap<EntityGuid, Entity> lookup) where T : struct, ISharedComponentData
        {
            var e = lookup[guid];
            return m_DstManager.GetSharedComponentData<T>(e);
        }
        
        public bool HasDstWorldData<T>(EntityGuid guid, NativeHashMap<EntityGuid, Entity> lookup) where T : struct
        {
            var e = lookup[guid];
            return m_DstManager.HasComponent<T>(e);
        }
        
        public void SetDstWorldData<T>(EntityGuid guid, T value, NativeHashMap<EntityGuid, Entity> lookup) where T : struct, IComponentData
        {
            var e = lookup[guid];
            m_DstManager.SetComponentData(e, value);
        }
        
        
        public T GetDstWorldData<T>(EntityGuid guid) where T : struct, IComponentData
        {
            var entities = m_DstManager.GetAllEntities();

            foreach (var e in entities)
            {
                var guidOnEntity = m_DstManager.GetComponentData<EntityGuid>(e);
                if (guidOnEntity .Equals(guid))
                    return m_DstManager.GetComponentData<T>(e);
            }
            
            throw new System.ArgumentException($"Couldn't find {guid}");
        }

        public Entity GetDstWorldEntity(EntityGuid guid)
        {
            var entities = m_DstManager.GetAllEntities();

            foreach (var e in entities)
            {
                var guidOnEntity = m_DstManager.GetComponentData<EntityGuid>(e);
                if (guidOnEntity.Equals(guid))
                    return e;
            }

            return default(Entity);
        }

        public T GetDstWorldSharedData<T>(EntityGuid guid) where T : struct, ISharedComponentData
        {
            var entities = m_DstManager.GetAllEntities();

            foreach (var e in entities)
            {
                var guidOnEntity = m_DstManager.GetComponentData<EntityGuid>(e);
                if (guidOnEntity .Equals(guid))
                    return m_DstManager.GetSharedComponentData<T>(e);
            }
            
            throw new System.ArgumentException($"Couldn't find {guid}");
        }
        
        public bool HasDstWorldData<T>(EntityGuid guid) where T : struct, IComponentData
        {
            var entities = m_DstManager.GetAllEntities();

            foreach (var e in entities)
            {
                var guidOnEntity = m_DstManager.GetComponentData<EntityGuid>(e);
                if (guidOnEntity.Equals(guid))
                    return m_DstManager.HasComponent<T>(e);
            }
            
            throw new System.ArgumentException($"Couldn't find {guid}");
        }
        
        public void SetDstWorldData<T>(EntityGuid guid, T value) where T : struct, IComponentData
        {
            var entities = m_DstManager.GetAllEntities();

            foreach (var e in entities)
            {
                var guidOnEntity = m_DstManager.GetComponentData<EntityGuid>(e);
                if (guidOnEntity.Equals(guid))
                {
                    m_DstManager.SetComponentData(e, value);
                    return;
                }
            }
            
            throw new System.ArgumentException($"Couldn't find {guid}");
        }

        public void SyncDiff()
        {
            using (var diff = WorldDiffer.UpdateDiff(m_After, m_Shadow, Allocator.TempJob))
            {
                WorldDiffer.ApplyDiff(m_DstWorld, diff);
            }
        }
        
        public void CreateStressData(int beginIndex, int endIndex, bool testData, bool testData2, bool testShared)
        {
            for (int i = beginIndex; i < endIndex; i++)
            {
                var guid = GenerateEntityGuid(i);

                var e = m_Manager.CreateEntity();
                m_Manager.AddComponentData(e, guid);
                if (testData)
                    m_Manager.AddComponentData(e, new EcsTestData {value = i * 3});
                if (testData2)
                    m_Manager.AddComponentData(e, new EcsTestData2 {value0 = i, value1 = i * 2});
                if (testShared)
                    m_Manager.AddSharedComponentData(e, new EcsTestSharedComp() {value = i / 31});
            }
        }

        public void TestStressData(int baseIndex, int endIndex, bool testData, bool testData2, bool testShared)
        {
            var lookup = BuildLookup();
            for (int i = baseIndex; i < endIndex; i++)
            {
                var guid = GenerateEntityGuid(i);

                if (testData)
                    Assert.AreEqual(i * 3, GetDstWorldData<EcsTestData>(guid, lookup).value);
                else
                    Assert.IsFalse(HasDstWorldData<EcsTestData>(guid, lookup));

                if (testData2)
                {
                    Assert.AreEqual(i, GetDstWorldData<EcsTestData2>(guid, lookup).value0);
                    Assert.AreEqual(i * 2, GetDstWorldData<EcsTestData2>(guid, lookup).value1);
                }
                else
                {
                    Assert.IsFalse(HasDstWorldData<EcsTestData2>(guid, lookup));
                }
                
                if (testShared)
                    Assert.AreEqual(i / 31, GetDstWorldSharedData<EcsTestSharedComp>(guid, lookup).value);
                else
                    Assert.IsFalse(HasDstWorldData<EcsTestSharedComp>(guid, lookup));

            }
            
            lookup.Dispose();
        }
    }

    public class WorldDiffTests : WorldDiffTestFixture
    {
        [Test]
        public void DiffOnly()
        {
            using (var diff = WorldDiffer.UpdateDiff(m_After, m_Shadow, Allocator.TempJob))
            {
                Assert.IsFalse(diff.HasChanges);
            }

            var guid = GenerateEntityGuid(0);
            
            var e = m_Manager.CreateEntity(typeof(EntityGuid), typeof(EcsTestData));
            m_Manager.SetComponentData(e, guid);
            m_Manager.SetComponentData(e, new EcsTestData {value = 9});

            using (var diff = WorldDiffer.UpdateDiff(m_After, m_Shadow, Allocator.TempJob))
            {
                Assert.AreEqual(0, diff.DeletedEntityCount);
                Assert.AreEqual(1, diff.NewEntityCount);
                Assert.AreEqual(2, diff.AddComponents.Length);
                Assert.AreEqual(2, diff.SetCommands.Length);
            }

            m_Manager.SetComponentData(e, guid);
            m_Manager.SetComponentData(e, new EcsTestData {value = 10});

            using (var diff = WorldDiffer.UpdateDiff(m_After, m_Shadow, Allocator.TempJob))
            {
                Assert.AreEqual(0, diff.DeletedEntityCount);
                Assert.AreEqual(0, diff.NewEntityCount);
                Assert.AreEqual(1, diff.SetCommands.Length);
                Assert.AreEqual(0, diff.AddComponents.Length);
            }

            m_Manager.DestroyEntity(e);

            using (var diff = WorldDiffer.UpdateDiff(m_After, m_Shadow, Allocator.TempJob))
            {
                Assert.AreEqual(1, diff.DeletedEntityCount);
                Assert.AreEqual(0, diff.NewEntityCount);
                Assert.AreEqual(0, diff.SetCommands.Length);
                Assert.AreEqual(0, diff.RemoveComponents.Length);

            }
        }

        [Test]
        public void CreateWithComponentData()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid );
            m_Manager.AddComponentData(e, new EcsTestData {value = 9});

            SyncDiff();
            
            Assert.AreEqual(9, GetDstWorldData<EcsTestData>(guid).value);
        }


        [Test]
        public void CreateWithPrefabComponent()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid );
            m_Manager.AddComponentData(e, new Prefab());

            SyncDiff();
            
            Assert.IsTrue(HasDstWorldData<Prefab>(guid));
        }

        [Test]
        public void CreateWithDisabledComponent()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid );
            m_Manager.AddComponentData(e, new Disabled());

            SyncDiff();
            
            Assert.IsTrue(HasDstWorldData<Disabled>(guid));
        }
        
        [Test]
        [Ignore("Disabled components are currently not included in GetAllEntities. That seems like wrong behaviour.")]
        public void CreateWithPrefabAndDisabledComponent()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid );
            m_Manager.AddComponentData(e, new Prefab());
            m_Manager.AddComponentData(e, new Disabled());

            SyncDiff();
            
            Assert.IsTrue(HasDstWorldData<Disabled>(guid));
            Assert.IsTrue(HasDstWorldData<Prefab>(guid));
        }
        
        [Test]
        public void RemapEntityRef()
        {
            // Create extra entity to make sure test doesn't accidentally succeed with no remapping
            m_Manager.CreateEntity();

            var g0 = GenerateEntityGuid(0);
            var g1 = GenerateEntityGuid(1);
            var e0 = m_Manager.CreateEntity();
            var e1 = m_Manager.CreateEntity();
            
            m_Manager.AddComponentData(e0, g0);
            m_Manager.AddComponentData(e0, new EcsTestDataEntity(){value1 = e1});

            m_Manager.AddComponentData(e1, g1);
            m_Manager.AddComponentData(e1, new EcsTestDataEntity(){value1 = e0});

            
            SyncDiff();
            
            Assert.AreEqual(GetDstWorldEntity(g1), GetDstWorldData<EcsTestDataEntity>(g0).value1);
            Assert.AreEqual(GetDstWorldEntity(g0), GetDstWorldData<EcsTestDataEntity>(g1).value1);
        }
        
        [Test]
        public void MissingEntityRefBecomesNull()
        {
            var guid = GenerateEntityGuid(0);

            var missing = m_Manager.CreateEntity();
            
            var entity = m_Manager.CreateEntity();
            m_Manager.AddComponentData(entity, guid);
            m_Manager.AddComponentData(entity, new EcsTestDataEntity(){value1 = missing});
            
            SyncDiff();
            
            // missing entity has no GUID, so the reference becomes null.
            Assert.AreEqual(Entity.Null, GetDstWorldData<EcsTestDataEntity>(guid).value1);
        }
        
        [Test]
        public void AddComponent()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid );
            m_Manager.AddComponentData(e, new EcsTestData {value = 9});

            SyncDiff();
            
            m_Manager.AddComponentData(e, new EcsTestData2(10));
            SetDstWorldData(guid, new EcsTestData(-1));
            
            SyncDiff();

            Assert.AreEqual(-1, GetDstWorldData<EcsTestData>(guid).value);
            Assert.AreEqual(10, GetDstWorldData<EcsTestData2>(guid).value0);
        }
        
        [Test]
        public void RemoveComponent()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid );
            m_Manager.AddComponentData(e, new EcsTestData(9));
            m_Manager.AddComponentData(e, new EcsTestData2(7));

            SyncDiff();
            
            m_Manager.RemoveComponent<EcsTestData>(e);
            SetDstWorldData(guid, new EcsTestData2(-1));
            
            SyncDiff();

            Assert.IsFalse(HasDstWorldData<EcsTestData>(guid));
            Assert.AreEqual(-1, GetDstWorldData<EcsTestData2>(guid).value0);
        }
        
        [Test]
        public void CreateSharedComponent()
        {
            for (int i = 0; i != 3; i++)
            {
                var e = m_Manager.CreateEntity();
                m_Manager.AddComponentData(e, GenerateEntityGuid(i));
                m_Manager.AddSharedComponentData(e, new EcsTestSharedComp {value = i * 2});
            }

            SyncDiff();
            
            for (int i = 0; i != 3; i++)
            {
                var sharedData = GetDstWorldSharedData<EcsTestSharedComp>(GenerateEntityGuid(i));
                Assert.AreEqual(i * 2, sharedData.value);
            }
        }

        [Test]
        public void ChangeSharedComponent()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid);
            m_Manager.AddSharedComponentData(e, new EcsTestSharedComp {value = 1});

            SyncDiff();
            Assert.AreEqual(1, GetDstWorldSharedData<EcsTestSharedComp>(guid).value);
            
            m_Manager.SetSharedComponentData(e, new EcsTestSharedComp {value = 2});

            SyncDiff();
            Assert.AreEqual(2, GetDstWorldSharedData<EcsTestSharedComp>(guid).value);

        }

        [Test]
        public void SharedComponentDiff()
        {
            var e = m_Manager.CreateEntity();
            var guid = GenerateEntityGuid(0);
            m_Manager.AddComponentData(e, guid);
            m_Manager.AddSharedComponentData(e, new EcsTestSharedComp {value = 2});

            using (var diff = WorldDiffer.UpdateDiff(m_After, m_Shadow, Allocator.TempJob))
            {
                Assert.AreEqual(1, diff.NewEntityCount);
                Assert.AreEqual(2, diff.AddComponents.Length);
                Assert.AreEqual(1, diff.SetCommands.Length);
                Assert.AreEqual(1, diff.SharedSetCommands.Length);
            }
        }

        [Test]
        public void ChangeSrcAppliesToAllDstInstances([Values]bool prefabTag)
        {
            var guid = GenerateEntityGuid(0);
            var prefab = m_Manager.CreateEntity();
            m_Manager.AddComponentData(prefab, guid);
            m_Manager.AddComponentData(prefab, new EcsTestData());
            if (prefabTag)
                m_Manager.AddComponentData(prefab, new Prefab());

            SyncDiff();
            
            var dstPrefab = GetDstWorldEntity(guid);
            var dstInstance0 = m_DstManager.Instantiate(dstPrefab);
            var dstInstance1 = m_DstManager.Instantiate(dstPrefab);
            
            m_Manager.SetComponentData(prefab, new EcsTestData(10));
            
            SyncDiff();

            Assert.AreEqual(10, m_DstManager.GetComponentData<EcsTestData>(dstPrefab).value);
            Assert.AreEqual(10, m_DstManager.GetComponentData<EcsTestData>(dstInstance0).value);
            Assert.AreEqual(10, m_DstManager.GetComponentData<EcsTestData>(dstInstance1).value);
        }

        [Test]
        public void DynamicBuffer([Values(1, 100)]int bufferLength)
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid);

            var buffer = m_Manager.AddBuffer<EcsIntElement>(e);
            for (var i = 0; i < bufferLength; ++i)
                buffer.Add(new EcsIntElement() { Value = i });

            SyncDiff();

            var f = GetDstWorldEntity(guid);
            var dstBuffer = m_DstManager.GetBuffer<EcsIntElement>(f);
            Assert.AreEqual(bufferLength, dstBuffer.Length);
            for (int i = 0;i!= dstBuffer.Length;i++)
                Assert.AreEqual(i, dstBuffer[i].Value);
        }
        
        [Test]
        public void DynamicBufferWithEntityStress()
        {
            int[] sizes = {10, 0, 100, 100, 7, 9, 13, 13, 13, 1};
            
            // Create extra entity to make sure test doesn't accidentally succeed with no remapping
            m_Manager.CreateEntity();

            var targetEntityGuid = GenerateEntityGuid(0);
            var targetEntity = m_Manager.CreateEntity();
            m_Manager.AddComponentData(targetEntity, targetEntityGuid);

            var guid = GenerateEntityGuid(1);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid);

            m_Manager.AddBuffer<EcsComplexEntityRefElement>(e);

            for (int iter = 0;iter != sizes.Length;iter++)
            {
                var size = sizes[iter];
                
                var buffer = m_Manager.GetBuffer<EcsComplexEntityRefElement>(e);
                buffer.ResizeUninitialized(size);
                var curTargetEntity = iter % 2 == 0 ? targetEntity : e;
                for (var i = 0; i < size; ++i)
                    buffer[i] = new EcsComplexEntityRefElement { Dummy = i, Entity = curTargetEntity };

                SyncDiff();

                var dstTargetEntity = GetDstWorldEntity(iter % 2 == 0 ? targetEntityGuid : guid); 

                var dstEntity = GetDstWorldEntity(guid);
                var dstBuffer = m_DstManager.GetBuffer<EcsComplexEntityRefElement>(dstEntity);
                Assert.AreEqual(size, dstBuffer.Length);
                
                for (var i = 0; i < size; ++i)
                {
                    Assert.AreEqual(dstTargetEntity, dstBuffer[i].Entity);
                    Assert.AreEqual(i, dstBuffer[i].Dummy);
                }
            }
        }
        
        [Test]
        [Ignore("Need field replacement - Should be driven by explicit opt-in attribute")]
        public void ModifySingleProperty()
        {
            var guid = GenerateEntityGuid(0);
            var e = m_Manager.CreateEntity();
            m_Manager.AddComponentData(e, guid );
            m_Manager.AddComponentData(e, new EcsTestData2 {value0 = 9, value1 = 11});

            SyncDiff();

            SetDstWorldData(guid, new EcsTestData2 {value0 = -1, value1 = -2});
            m_Manager.SetComponentData(e, new EcsTestData2 {value0 = 9, value1 = 1023});

            SyncDiff();

            // Retain value from dst because it was not changed in src
            Assert.AreEqual(-1, GetDstWorldData<EcsTestData2>(guid).value0);
            // Change to new value in dst because it was changed in src
            Assert.AreEqual(1023, GetDstWorldData<EcsTestData2>(guid).value1);
        }
        
        [Test]
        public void StressCreation()
        {
            // Add shared component to scramble src shared indices
            var ent = m_Manager.CreateEntity();
            m_Manager.AddSharedComponentData(ent, new EcsTestSharedComp(3));
            
            CreateStressData(0,    99,  false, true, false);
            CreateStressData(99,   173, true, true, false);
            CreateStressData(173,  250, true, true, true);

            SyncDiff();

            TestStressData(0,   99,  false, true, false);
            TestStressData(99,  173, true, true, false);
            TestStressData(173, 250, true, true, true);
        }

        [Test]
        public void StressDestroyOnClient()
        {
            CreateStressData(0,    2000, false, true, false);
            CreateStressData(2000, 4000, true, true, false);

            SyncDiff();

            m_DstManager.DestroyEntity(m_DstManager.GetAllEntities());
            SyncDiff();
            
            Assert.AreEqual(0, m_DstManager.GetAllEntities().Length);
        }
        
        [Test]
        public void StressDestroyOnServer()
        {
            CreateStressData(0,    2000, false, true, false);
            CreateStressData(2000, 4000, true, true, false);

            SyncDiff();

            m_Manager.DestroyEntity(m_Manager.GetAllEntities());
            SyncDiff();
            
            Assert.AreEqual(0, m_DstManager.GetAllEntities().Length);
        }

        
        [Test]
        public void StressTestRecreation()
        {
            // Add shared component to scramble src shared indices
            var ent = m_Manager.CreateEntity();
            m_Manager.AddSharedComponentData(ent, new EcsTestSharedComp(3));


            int end = 100;
            CreateStressData(0,    end, false, true, true);
            SyncDiff();
            
            for (int i = 0; i < 10; i++)
            {
                m_Manager.DestroyEntity(m_Manager.GetAllEntities());
                CreateStressData(0,    end, false, true, true);

                using (var diff = WorldDiffer.UpdateDiff(m_After, m_Shadow, Allocator.TempJob))
                {
                    Assert.IsFalse(diff.HasChanges);
                    WorldDiffer.ApplyDiff(m_DstWorld, diff);
                }

                TestStressData(0, end, false, true, true);
            }
            
            Assert.AreEqual(end, m_DstManager.GetAllEntities().Length);
        }
        
        [Test]
        [Ignore("TODO")]
        public void EntityIDOnComponentChangedButNotReferencedEntityGUID()
        {
           // Ensure that no change is generated in this case...
        }
        
        //@TODO:
        [Test]
        [Ignore("For the time being disabled")]
        public void NewEntityIsReplicatedIntoExistingPrefabInstances([Values(1, 10)]int instanceCount)
        {
            var rootGUID = GenerateEntityGuid(1);
            var childGUID = GenerateEntityGuid(2);
            
            var root = m_Manager.CreateEntity(typeof(EcsTestDataEntity), typeof(Prefab), typeof(LinkedEntityGroup));
            m_Manager.AddComponentData(root, rootGUID);
            m_Manager.GetBuffer<LinkedEntityGroup>(root).Add(root);
            
            SyncDiff();

            var dstRootPrefab = GetDstWorldEntity(rootGUID);
            
            // Instantiate root in dst world
            var dstRootInstances = new Entity[instanceCount];
            for (var i = 0; i != dstRootInstances.Length; i++)
            {
                var dstRootInstance = m_DstManager.Instantiate(GetDstWorldEntity(rootGUID));
                dstRootInstances[i] = dstRootInstance;
                Assert.AreEqual(1, m_DstManager.GetBuffer<LinkedEntityGroup>(dstRootInstance).Length);
                Assert.AreEqual(dstRootInstance, m_DstManager.GetBuffer<LinkedEntityGroup>(dstRootInstance)[0].Value);            
            }
            
            // Add a new entity into the prefab
            var child = m_Manager.CreateEntity(typeof(EcsTestDataEntity), typeof(Prefab));
            m_Manager.AddComponentData(child, childGUID);
            m_Manager.GetBuffer<LinkedEntityGroup>(root).Add(child);

            m_Manager.SetComponentData(root, new EcsTestDataEntity { value1 = child});
            m_Manager.SetComponentData(child, new EcsTestDataEntity { value1 = root});

            SyncDiff();

            for (var i = 0; i != dstRootInstances.Length; i++)
            {
                var dstRootInstance = dstRootInstances[i];
                
                var dstInstanceGroup = m_DstManager.GetBuffer<LinkedEntityGroup>(dstRootInstance);
                Assert.AreEqual(2, dstInstanceGroup.Length);
                Assert.AreEqual(dstRootInstance, dstInstanceGroup[0].Value);            
                var dstChildInstance = dstInstanceGroup[1].Value;

                Assert.IsTrue(m_DstManager.HasComponent<Prefab>(dstRootPrefab));
                Assert.IsFalse(m_DstManager.HasComponent<Prefab>(dstRootInstance));
                Assert.IsFalse(m_DstManager.HasComponent<Prefab>(dstChildInstance));
            
                Assert.AreEqual(dstRootInstance, m_DstManager.GetComponentData<EcsTestDataEntity>(dstChildInstance).value1);
                Assert.AreEqual(dstChildInstance, m_DstManager.GetComponentData<EcsTestDataEntity>(dstRootInstance).value1);
            }
        }
        //@TODO: Full test coverage for add / remove / destroy        
        //@TODO: Test for when world contains two entities with same EntityGUID (Is an error but should at minimum give a usable error message)
    }
}
