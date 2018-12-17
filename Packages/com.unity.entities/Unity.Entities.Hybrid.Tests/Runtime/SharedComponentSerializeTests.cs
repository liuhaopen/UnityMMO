//#define WRITE_TO_DISK

using System;
using NUnit.Framework;
using UnityEngine;
using Unity.Entities.Serialization;
using Object = UnityEngine.Object;

namespace Unity.Entities.Tests
{
    public class SharedComponentSerializeTests : ECSTestsFixture
    {
        [Test]
        public void SerializingSharedComponent_WhenMoreThanOne_AndWrapperHasDisallowMultiple_DoesNotCrash()
        {
            for (var i = 0; i < 20; ++i)
            {
                var entity = m_Manager.CreateEntity();
                m_Manager.AddSharedComponentData(entity, new MockSharedDisallowMultiple { Value = i });
                m_Manager.AddComponentData(entity, new EcsTestData(i));
            }

            var writer = new TestBinaryWriter();
            GameObject sharedComponents = null;

            try
            {
                var ex = Assert.Throws<ArgumentException>(
                    () => SerializeUtilityHybrid.Serialize(m_Manager, writer, out sharedComponents)
                );
                Assert.That(
                    ex.Message,
                    Is.EqualTo(
                        string.Format(
                            "{0} is marked with {1}, but current implementation of {2} serializes all shared components on a single GameObject.",
                            typeof(MockSharedDisallowMultipleComponent),
                            typeof(DisallowMultipleComponent),
                            nameof(SerializeUtilityHybrid.SerializeSharedComponents)
                        )
                    )
                );
            }
            finally
            {
                writer.Dispose();
                if (sharedComponents != null)
                    GameObject.DestroyImmediate(sharedComponents);
            }
        }

        [Test]
        public void SharedComponentSerialize()
        {
            for (int i = 0; i != 20; i++)
            {
                var entity = m_Manager.CreateEntity();
                m_Manager.AddSharedComponentData(entity, new TestShared(i));
                m_Manager.AddComponentData(entity, new EcsTestData(i));
            }

            var writer = new TestBinaryWriter();

            GameObject sharedComponents;
            SerializeUtilityHybrid.Serialize(m_Manager, writer, out sharedComponents);

            var reader = new TestBinaryReader(writer);

            var world = new World("temp");
            SerializeUtilityHybrid.Deserialize (world.GetOrCreateManager<EntityManager>(), reader, sharedComponents);

            var newWorldEntities = world.GetOrCreateManager<EntityManager>();

            {
                var entities = newWorldEntities.GetAllEntities();

                Assert.AreEqual(20, entities.Length);

                for (int i = 0; i != 20; i++)
                {
                    Assert.AreEqual(i, newWorldEntities.GetComponentData<EcsTestData>(entities[i]).value);
                    Assert.AreEqual(i, newWorldEntities.GetSharedComponentData<TestShared>(entities[i]).Value);
                }
                for (int i = 0; i != 20; i++)
                    newWorldEntities.DestroyEntity(entities[i]);

                entities.Dispose();
            }

            Assert.IsTrue(newWorldEntities.Debug.IsSharedComponentManagerEmpty());

            world.Dispose();
            reader.Dispose();

            Object.DestroyImmediate(sharedComponents);
        }
    }
}
