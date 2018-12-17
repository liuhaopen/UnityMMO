using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Unity.Entities.Tests
{
    public class ComponentDataWrapperTests
    {
        interface IIntegerContainer
        {
            int Integer { get; set; }
        }

        struct MockData : IComponentData
        {
            public int Value;
        }

        [DisallowMultipleComponent]
        class MockWrapper : ComponentDataWrapper<MockData>, IIntegerContainer
        {
            public int Integer
            {
                get { return Value.Value; }
                set { Value = new MockData {Value = value}; }
            }
        }

        struct MockSharedData : ISharedComponentData
        {
            public int Value;
        }

        class MockSharedWrapper : SharedComponentDataWrapper<MockSharedData>, IIntegerContainer
        {
            public int Integer
            {
                get { return Value.Value; }
                set { Value = new MockSharedData {Value = value}; }
            }
        }

        GameObject m_GameObject;

        [TearDown]
        public void TearDown()
        {
            if (m_GameObject != null)
                GameObject.DestroyImmediate(m_GameObject);
        }

        [TestCase(typeof(MockWrapper))]
        [TestCase(typeof(MockSharedWrapper))]
        public void ComponentDataWrapper_SetValue_SynchronizesWithEntityManager(Type wrapperType)
        {
            m_GameObject = new GameObject(TestContext.CurrentContext.Test.Name, wrapperType);
            var wrapper = m_GameObject.GetComponent<ComponentDataWrapperBase>();
            EntityManager entityManager;
            Entity entity;
            Assert.That(
                wrapper.CanSynchronizeWithEntityManager(out entityManager, out entity), Is.True,
                "EntityManager is not in correct state in arrangement for synchronization to occur"
            );
            var integerWrapper = wrapper as IIntegerContainer;
            Assert.That(
                integerWrapper.Integer, Is.EqualTo(0),
                $"{wrapper.GetComponentType()} did not initialize with default value in arrangement"
            );

            integerWrapper.Integer = 1;
            Assert.That(integerWrapper.Integer, Is.EqualTo(1), $"Setting value on {wrapperType} failed");

            new SerializedObject(wrapper);
            Assert.That(integerWrapper.Integer, Is.EqualTo(1), $"Value was reset after deserializing {wrapperType}");
        }

        [Test]
        public void AllComponentDataWrappers_DisallowMultipleComponent()
        {
            TestTypes(typeof(ComponentDataWrapper<>), true);
        }

        // currently enforced due to implementation of SerializeUtilityHybrid
        // ideally all types should ultimately have DisallowMultipleComponent
        [Test]
        public void NoSharedComponentDataWrappers_DisallowMultipleComponent()
        {
            TestTypes(
                typeof(SharedComponentDataWrapper<>),
                false,
                ignoreTypes: typeof(MockSharedDisallowMultipleComponent)
            );
        }

        void TestTypes(Type testType, bool shouldDisallowMultiple, params Type[] ignoreTypes)
        {
            var errorTypes = new HashSet<Type>();
            var whiteList = new HashSet<Type>(ignoreTypes);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (
                            type.BaseType == null
                            || type.IsAbstract
                            || !typeof(ComponentDataWrapperBase).IsAssignableFrom(type)
                            || whiteList.Contains(type)
                        )
                            continue;

                        var t = type;
                        while (t != typeof(ComponentDataWrapperBase))
                        {
                            if (t.IsGenericType && t.GetGenericTypeDefinition() == testType)
                            {
                                t = t.GetGenericTypeDefinition();
                                break;
                            }
                            t = t.BaseType;
                        }
                        if (t != testType)
                            continue;

                        var disallowsMultiple = Attribute.IsDefined(type, typeof(DisallowMultipleComponent), true);
                        if (disallowsMultiple != shouldDisallowMultiple)
                            errorTypes.Add(type);
                    }
                }
                // ignore if error loading some type from a dll
                catch (TypeLoadException)
                {
                }
            }

            Assert.That(
                errorTypes.Count, Is.EqualTo(0),
                string.Format("Following {0} types {1} have {2}\n - {3}\n",
                    testType,
                    shouldDisallowMultiple ? $"should" : "should not",
                    typeof(DisallowMultipleComponent),
                    string.Join("\n - ", errorTypes.Select(t => t.FullName).ToArray())
                )
            );
        }
    }
}