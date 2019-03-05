using System;
using NUnit.Framework;
using Unity.Entities;
using Unity.Entities.Tests;
#pragma warning disable 649

namespace UnityEngine.Entities.Tests
{
    class ComponentSystem_GameObject_IntegrationTests : ECSTestsFixture
    {
        ComponentArrayInjectionHook m_ComponentArrayInjectionHook = new ComponentArrayInjectionHook();
        GameObjectArrayInjectionHook m_GameObjectArrayInjectionHook = new GameObjectArrayInjectionHook();

        [OneTimeSetUp]
        public void Init()
        {
            InjectionHookSupport.RegisterHook(m_ComponentArrayInjectionHook);
            InjectionHookSupport.RegisterHook(m_GameObjectArrayInjectionHook);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            InjectionHookSupport.UnregisterHook(m_GameObjectArrayInjectionHook);
            InjectionHookSupport.RegisterHook(m_ComponentArrayInjectionHook);
        }

        GameObject m_GameObject;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            m_GameObject = new GameObject(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            if (m_GameObject != null)
                GameObject.DestroyImmediate(m_GameObject);
        }

        [DisableAutoCreation]
        public class GameObjectArraySystem : ComponentSystem
        {
            public struct Group
            {
                public readonly int Length;
                public GameObjectArray gameObjects;

                public ComponentArray<BoxCollider> colliders;
            }

            [Inject]
            public Group group;

            protected override void OnUpdate()
            {
            }

            public new void UpdateInjectedComponentGroups()
            {
                base.UpdateInjectedComponentGroups();
            }
        }

        [Test]
        public void UpdateInjectedComponentGroups_WhenObjectWithMatchingComponentExists_GameObjectArrayIsPopulated()
        {
            m_GameObject.AddComponent<BoxCollider>();
            GameObjectEntity.AddToEntityManager(m_Manager, m_GameObject);
            var manager = World.GetOrCreateManager<GameObjectArraySystem>();

            manager.UpdateInjectedComponentGroups();

            Assert.That(manager.group.gameObjects.ToArray(), Is.EqualTo(new[] { m_GameObject }));
        }

        [Test]
        public void UpdateInjectedComponentGroups_WhenObjectWithMatchingComponentExists_ComponentArrayIsPopulated()
        {
            var c = m_GameObject.AddComponent<BoxCollider>();
            GameObjectEntity.AddToEntityManager(m_Manager, m_GameObject);
            var manager = World.GetOrCreateManager<GameObjectArraySystem>();

            manager.UpdateInjectedComponentGroups();

            Assert.That(manager.group.colliders.ToArray(), Is.EqualTo(new[] { c }));
        }

        [Test]
        public void GetComponentGroup_WhenObjectWithMatchingComponentExists_ComponentArrayIsPopulated()
        {
            var rb = m_GameObject.AddComponent<Rigidbody>();
            GameObjectEntity.AddToEntityManager(m_Manager, m_GameObject);

            var grp = EmptySystem.GetComponentGroup(typeof(Rigidbody));

            var array = grp.GetComponentArray<Rigidbody>();
            Assert.That(array.ToArray(), Is.EqualTo(new[] { rb }));
        }

        [Test]
        public void GetComponentGroup_WhenObjectWithMatchingComponentAndECSDataExists_ComponentArraysArePopulated()
        {
            var goe = m_GameObject.AddComponent<EcsTestComponent>().GetComponent<GameObjectEntity>();
            var expectedTestData = new EcsTestData(5);
            m_Manager.SetComponentData(goe.Entity, expectedTestData);

            var grp = EmptySystem.GetComponentGroup(typeof(Transform), typeof(EcsTestData));

            var transformArray = grp.GetComponentArray<Transform>();
            Assert.That(transformArray.ToArray(), Is.EqualTo(new[] { goe.transform }));
            var ecsDataArray = grp.GetComponentDataArray<EcsTestData>();
            Assert.That(ecsDataArray.Length, Is.EqualTo(1));
            Assert.That(ecsDataArray[0], Is.EqualTo(expectedTestData));
        }
    }
}
