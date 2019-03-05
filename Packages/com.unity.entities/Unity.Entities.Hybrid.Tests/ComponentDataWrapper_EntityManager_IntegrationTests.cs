using System;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Unity.Entities.Tests
{
    class ComponentDataWrapper_EntityManager_IntegrationTests
    {
        GameObjectEntity m_GameObjectEntity;
        Entity Entity { get { return m_GameObjectEntity.Entity; } }
        EntityManager Manager { get { return m_GameObjectEntity.EntityManager; } }

        [SetUp]
        public void SetUp()
        {
            m_GameObjectEntity =
                new GameObject(TestContext.CurrentContext.Test.Name, typeof(GameObjectEntity)).GetComponent<GameObjectEntity>();
        }

        [TearDown]
        public void TearDown()
        {
            if (m_GameObjectEntity.gameObject != null)
                GameObject.DestroyImmediate(m_GameObjectEntity.gameObject);
        }

        [TestCase(typeof(MockDataComponent), TestName = "Sync_ComponentDataWrapper")]
        [TestCase(typeof(MockSharedDataComponent), TestName = "Sync_SharedComponentDataWrapper")]
        public void ComponentDataWrapper_SetValue_SynchronizesWithEntityManager(Type wrapperType)
        {
            m_GameObjectEntity.gameObject.AddComponent(wrapperType);
            var wrapper = m_GameObjectEntity.gameObject.GetComponent<ComponentDataWrapperBase>();
            EntityManager entityManager;
            Entity entity;
            Assume.That(
                wrapper.CanSynchronizeWithEntityManager(out entityManager, out entity), Is.True,
                "EntityManager is not in correct state in arrangement for synchronization to occur"
            );
            var integerWrapper = wrapper as IIntegerContainer;
            Assume.That(
                integerWrapper.Integer, Is.EqualTo(0),
                $"{wrapper.GetComponentType()} did not initialize with default value in arrangement"
            );

            integerWrapper.Integer = 1;
            new SerializedObject(wrapper);

            Assert.That(integerWrapper.Integer, Is.EqualTo(1), $"Value was reset after deserializing {wrapperType}");
        }

        static readonly FieldInfo k_EntityManagerBackingField =
            typeof(GameObjectEntity).GetField("m_EntityManager", BindingFlags.Instance | BindingFlags.NonPublic);

        [TestCase(typeof(MockDataComponent), TestName = "SyncInvalidManager_ComponentDataWrapper")]
        [TestCase(typeof(MockSharedDataComponent), TestName = "SyncInvalidManager_SharedComponentDataWrapper")]
        public void ComponentDataWrapper_WhenEntityManagerIsInvalid_SynchronizesWithEntityManager(Type wrapperType)
        {
            var entityManager = new EntityManager();
            k_EntityManagerBackingField.SetValue(m_GameObjectEntity, entityManager);
            Assume.That(entityManager.IsCreated, Is.False, "EntityManager was not in correct state in test arrangement");
            var wrapper = m_GameObjectEntity.gameObject.AddComponent(wrapperType) as IIntegerContainer;

            wrapper.Integer = 1;

            Assert.That(wrapper.Integer, Is.EqualTo(1), $"Setting value on {wrapperType} failed");
        }

        [Test]
        public void AddComponentDataWrapper_WhenGameObjectEntityAlreadyActive_EntityManagerHasComponent()
        {
            Assume.That(Manager.Exists(Entity));

            m_GameObjectEntity.gameObject.AddComponent<MockDataComponent>();

            Assert.That(Manager.HasComponent(Entity, typeof(MockData)), Is.True, "No data after adding wrapper.");
        }

        [Test]
        public void DestroyComponentDataWrapper_WhenGameObjectEntityAlreadyActive_EntityManagerDoesNotHaveComponent()
        {
            Assume.That(Manager.Exists(Entity));
            var c = m_GameObjectEntity.gameObject.AddComponent<MockDataComponent>();
            Assume.That(Manager.HasComponent(Entity, typeof(MockData)), Is.True, "No data after adding wrapper.");

            Component.DestroyImmediate(c);

            Assert.That(Manager.HasComponent(Entity, typeof(MockData)), Is.False, "Data after destroying wrapper.");
        }

        [Test]
        public void DisableComponentDataWrapper_WhenGameObjectEntityAlreadyActive_EntityManagerDoesNotHaveComponent()
        {
            Assume.That(Manager.Exists(Entity));
            var c = m_GameObjectEntity.gameObject.AddComponent<MockDataComponent>();
            Assume.That(Manager.HasComponent(Entity, typeof(MockData)), Is.True, "No data after adding wrapper.");

            c.enabled = false;

            Assert.That(Manager.HasComponent(Entity, typeof(MockData)), Is.False, "Data exists after disabling wrapper.");
        }

        [Test]
        public void ReEnableComponentDataWrapper_WhenGameObjectEntityAlreadyActive_EntityManagerHasComponent()
        {
            Assume.That(Manager.Exists(Entity));
            var c = m_GameObjectEntity.gameObject.AddComponent<MockDataComponent>();
            Assume.That(Manager.HasComponent(Entity, typeof(MockData)), Is.True, "No data after adding wrapper.");
            c.enabled = false;
            Assume.That(Manager.HasComponent(Entity, typeof(MockData)), Is.False, "Data exists after disabling wrapper.");

            c.enabled = true;

            Assert.That(Manager.HasComponent(Entity, typeof(MockData)), Is.True, "No data after re-enabling wrapper.");
        }
    }
}
