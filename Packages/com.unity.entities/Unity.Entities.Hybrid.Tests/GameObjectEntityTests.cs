using NUnit.Framework;
using Unity.Entities;
using Unity.Entities.Tests;

namespace UnityEngine.Entities.Tests
{
    public class GameObjectEntityTests : ECSTestsFixture
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
        public void GameObjectArrayIsPopulated()
        {
            var go = new GameObject("test", typeof(BoxCollider));
            GameObjectEntity.AddToEntityManager(m_Manager, go);

            var manager = World.GetOrCreateManager<GameObjectArraySystem>();

            manager.UpdateInjectedComponentGroups();

            Assert.AreEqual(1, manager.group.Length);
            Assert.AreEqual(go, manager.group.gameObjects[0]);
            Assert.AreEqual(go, manager.group.colliders[0].gameObject);

            Object.DestroyImmediate (go);
            TearDown();
        }

        [Test]
        public void ComponentDataAndTransformArray()
        {
            var go = new GameObject("test", typeof(EcsTestComponent)).GetComponent<GameObjectEntity>();

            m_Manager.SetComponentData(go.Entity, new EcsTestData(5));

            var grp = EmptySystem.GetComponentGroup(typeof(Transform), typeof(EcsTestData));
            var arr = grp.GetComponentArray<Transform>();

            Assert.AreEqual(1, arr.Length);
            Assert.AreEqual(go.transform, arr[0]);
            Assert.AreEqual(5, grp.GetComponentDataArray<EcsTestData>()[0].value);

            Object.DestroyImmediate(go.gameObject);
        }

        [Test]
        public void RigidbodyComponentArray()
        {
            var go = new GameObject("test", typeof(Rigidbody));
            /*var entity =*/ GameObjectEntity.AddToEntityManager(m_Manager, go);

            var grp = EmptySystem.GetComponentGroup(typeof(Rigidbody));

            var arr = grp.GetComponentArray<Rigidbody>();
            Assert.AreEqual(1, arr.Length);
            Assert.AreEqual(go.GetComponent<Rigidbody>(), arr[0]);

            Object.DestroyImmediate(go);
        }

        struct MockData : IComponentData
        {
        }

        [DisallowMultipleComponent]
        class MockWrapper : ComponentDataWrapper<MockData>
        {
        }

        [Test]
        public void ComponentDataWrapper_AddDisableEnableRemove_WhenGameObjectEntityAlreadyActive_UpdatesEntityManager()
        {
            var go = new GameObject("test");
            try
            {
                var entity = go.AddComponent<GameObjectEntity>().Entity;
                Assert.That(m_Manager.Exists(entity), Is.True);

                var c = go.AddComponent<MockWrapper>();
                Assert.That(m_Manager.HasComponent(entity, typeof(MockData)), Is.True, "No data after adding wrapper.");

                c.enabled = false;
                Assert.That(m_Manager.HasComponent(entity, typeof(MockData)), Is.False, "Data after disabling wrapper.");

                c.enabled = true;
                Assert.That(m_Manager.HasComponent(entity, typeof(MockData)), Is.True, "No data after re-enabling wrapper.");

                Object.DestroyImmediate(c);
                Assert.That(m_Manager.HasComponent(entity, typeof(MockData)), Is.False, "Data after destroying wrapper.");
            }
            finally
            {
                if (go != null)
                    GameObject.DestroyImmediate(go);
                TearDown();
            }
        }

        [Test]
        public void GameObjectEntity_ActivateDeactivateGameObject_UpdatesEntityManager()
        {
            var parent = new GameObject("test parent");
            var goEntity = new GameObject("test", typeof(GameObjectEntity), typeof(MockWrapper)).GetComponent<GameObjectEntity>();
            goEntity.transform.SetParent(parent.transform);
            try
            {
                Assert.That(m_Manager.Exists(goEntity.Entity), Is.True);

                goEntity.gameObject.SetActive(false);
                Assert.That(m_Manager.Exists(goEntity.Entity), Is.False, "Entity exists after deactivating GameObject");

                goEntity.gameObject.SetActive(true);
                Assert.That(m_Manager.Exists(goEntity.Entity), Is.True, "Entity does not exist after reactivating GameObject");
                Assert.That(m_Manager.HasComponent(goEntity.Entity, typeof(MockData)), Is.True, "MockData not exist after reactivating GameObject.");

                parent.gameObject.SetActive(false);
                Assert.That(m_Manager.Exists(goEntity.Entity), Is.False, "Entity exists after deactivating parent GameObject");

                parent.gameObject.SetActive(true);
                Assert.That(m_Manager.Exists(goEntity.Entity), Is.True, "Entity does not exist after reactivating parent GameObject");
                Assert.That(m_Manager.HasComponent(goEntity.Entity, typeof(MockData)), Is.True, "MockData not exist after reactivating parent GameObject.");
            }
            finally
            {
                if (goEntity.gameObject != null)
                    GameObject.DestroyImmediate(goEntity.gameObject);
                if (parent != null)
                    GameObject.DestroyImmediate(parent);
                TearDown();
            }
        }
    }
}
