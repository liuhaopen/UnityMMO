using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
    // mock type
    class GameObjectEntity
    {
    }
}

namespace Unity.Entities.Tests
{
    public class TypeManagerTests : ECSTestsFixture
    {
        struct TestType1 : IComponentData
        {
            int empty;
        }
        struct TestType2 : IComponentData
        {
            int empty;
        }
        [Test]
        public void CreateArchetypes()
        {
            var archetype1 = m_Manager.CreateArchetype(ComponentType.Create<TestType1>(), ComponentType.Create<TestType2>());
            var archetype1Same = m_Manager.CreateArchetype(ComponentType.Create<TestType1>(), ComponentType.Create<TestType2>());
            Assert.AreEqual(archetype1, archetype1Same);

            var archetype2 = m_Manager.CreateArchetype(ComponentType.Create<TestType1>());
            var archetype2Same = m_Manager.CreateArchetype(ComponentType.Create<TestType1>());
            Assert.AreEqual(archetype2Same, archetype2Same);

            Assert.AreNotEqual(archetype1, archetype2);
        }

        [InternalBufferCapacity(99)]
        public struct IntElement : IBufferElementData
        {
            public int Value;
        }

        [Test]
        public void BufferTypeClassificationWorks()
        {
            var t  = TypeManager.GetTypeInfo<IntElement>();
            Assert.AreEqual(TypeManager.TypeCategory.BufferData, t.Category);
            Assert.AreEqual(99, t.BufferCapacity);
            Assert.AreEqual(UnsafeUtility.SizeOf<BufferHeader>() + 99 * sizeof(int), t.SizeInChunk);
        }

        [Test]
        public void TestTypeManager()
        {
            var entity = ComponentType.Create<Entity>();
            var testData = ComponentType.Create<EcsTestData>();

            Assert.AreEqual(entity, ComponentType.Create<Entity>());
            Assert.AreEqual(entity, new ComponentType(typeof(Entity)));
            Assert.AreEqual(testData, ComponentType.Create<EcsTestData>());
            Assert.AreEqual(testData, new ComponentType(typeof(EcsTestData)));
            Assert.AreNotEqual(ComponentType.Create<Entity>(), ComponentType.Create<EcsTestData>());
            Assert.AreNotEqual(entity, ComponentType.ReadOnly<EcsTestData>());

            Assert.AreEqual(typeof(Entity), entity.GetManagedType());
        }

        struct NonBlittableComponentData : IComponentData
        {
            string empty;
        }

        class ClassComponentData : IComponentData
        {
        }

        interface InterfaceComponentData : IComponentData
        {
        }

        struct NonBlittableBuffer: IBufferElementData
        {
            string empty;
        }

        class ClassBuffer: IBufferElementData
        {
        }

        interface InterfaceBuffer : IBufferElementData
        {
        }

        class ClassShared : ISharedComponentData
        {
        }

        interface InterfaceShared : ISharedComponentData
        {
        }

        [TestCase(typeof(InterfaceComponentData), "Unity.Entities.Tests.TypeManagerTests+InterfaceComponentData is an interface.")]
        [TestCase(typeof(ClassComponentData), "Unity.Entities.Tests.TypeManagerTests+ClassComponentData is an IComponentData, and thus must be a struct.")]
        [TestCase(typeof(NonBlittableComponentData), "Unity.Entities.Tests.TypeManagerTests+NonBlittableComponentData is an IComponentData, and thus must be blittable")]

        [TestCase(typeof(ClassBuffer), "Unity.Entities.Tests.TypeManagerTests+ClassBuffer is an IBufferElementData, and thus must be a struct.")]
        [TestCase(typeof(NonBlittableBuffer), "Unity.Entities.Tests.TypeManagerTests+NonBlittableBuffer is an IBufferElementData, and thus must be blittable")]
        [TestCase(typeof(InterfaceBuffer), "Unity.Entities.Tests.TypeManagerTests+InterfaceBuffer is an interface.")]

        [TestCase(typeof(ClassShared), "Unity.Entities.Tests.TypeManagerTests+ClassShared is an ISharedComponentData, and thus must be a struct.")]
        [TestCase(typeof(InterfaceShared), "Unity.Entities.Tests.TypeManagerTests+InterfaceShared is an interface.")]

        [TestCase(typeof(GameObjectEntity), "GameObjectEntity cannot be used from EntityManager.")]

        [TestCase(typeof(float), "System.Single is not a valid component.")]
        public void BuildComponentType_ThrowsArgumentException_WithExpectedFailures(
            Type type, string messageStartsWith
            )
        {
            var e = Assert.Throws<ArgumentException>(() => TypeManager.BuildComponentType(type));
            Assert.That(e.Message, new StartsWithConstraint(messageStartsWith));
        }

        [TestCase(typeof(UnityEngine.Transform))]
        [TestCase(typeof(TypeManagerTests))]
        public void BuildComponentType_WithClass_WhenUnityEngineComponentTypeIsNull_ThrowsArgumentException(Type type)
        {
            var componentType = TypeManager.UnityEngineComponentType;
            TypeManager.UnityEngineComponentType = null;
            try
            {
                var e = Assert.Throws<ArgumentException>(() => TypeManager.BuildComponentType(type));
                Assert.That(e.Message, new StartsWithConstraint($"{type} cannot be used from EntityManager. If it inherits UnityEngine.Component"));
            }
            finally
            {
                TypeManager.UnityEngineComponentType = componentType;
            }
        }

        [Test]
        public void BuildComponentType_WithNonComponent_WhenUnityEngineComponentTypeIsCorrect_ThrowsArgumentException()
        {
            var componentType = TypeManager.UnityEngineComponentType;
            TypeManager.UnityEngineComponentType = typeof(UnityEngine.Component);
            try
            {
                var type = typeof(TypeManagerTests);
                var e = Assert.Throws<ArgumentException>(() => TypeManager.BuildComponentType(type));
                Assert.That(e.Message, new StartsWithConstraint($"{type} must inherit {typeof(UnityEngine.Component)}"));
            }
            finally
            {
                TypeManager.UnityEngineComponentType = componentType;
            }
        }

        [Test]
        public void BuildComponentType_WithComponent_WhenUnityEngineComponentTypeIsCorrect_Works()
        {
            var componentType = TypeManager.UnityEngineComponentType;
            TypeManager.UnityEngineComponentType = typeof(UnityEngine.Component);
            try
            {
                TypeManager.BuildComponentType(typeof(UnityEngine.Transform));
            }
            finally
            {
                TypeManager.UnityEngineComponentType = componentType;
            }
        }

        [TestCase(null)]
        [TestCase(typeof(TestType1))]
        [TestCase(typeof(InterfaceShared))]
        [TestCase(typeof(ClassShared))]
        [TestCase(typeof(UnityEngine.Transform))]
        public void RegisterUnityEngineComponentType_WithWrongType_ThrowsArgumentException(Type type)
        {
            Assert.Throws<ArgumentException>(() => TypeManager.RegisterUnityEngineComponentType(type));
        }
    }
}
