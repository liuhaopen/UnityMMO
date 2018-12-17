using System;
using NUnit.Framework;

using UnityEngine;
using Unity.Mathematics;
using Unity.Properties.Serialization;

namespace Unity.Entities.Properties.Tests
{
    [TestFixture]
    public sealed class EntitySerializationTests
    {
        private World 			m_PreviousWorld;
        private World 			m_World;
        private EntityManager   m_Manager;

        [SetUp]
        public void Setup()
        {
            m_PreviousWorld = World.Active;
            m_World = World.Active = new World ("Test World");
            m_Manager = m_World.GetOrCreateManager<EntityManager> ();
        }

        [TearDown]
        public void TearDown()
        {
            if (m_Manager != null)
            {
                m_World.Dispose();
                m_World = null;

                World.Active = m_PreviousWorld;
                m_PreviousWorld = null;
                m_Manager = null;
            }
        }

        [System.Serializable]
        public struct TestComponentWrapper<T>
        {
            public T[] Components;
        }

        /// <summary>
        /// Writes an entity to json
        /// </summary>
        [Test]
        public void SimpleFlat()
        {
            var entity = m_Manager.CreateEntity(typeof(TestComponent), typeof(TestComponent2));

            var testComponent = m_Manager.GetComponentData<TestComponent>(entity);
            testComponent.x = 123f;
            m_Manager.SetComponentData(entity, testComponent);

            var container = new EntityContainer(m_Manager, entity);

            var json = JsonSerializer.Serialize(ref container);

            Assert.AreEqual(
                testComponent,
                UnityEngine.JsonUtility.FromJson<TestComponentWrapper<TestComponent>>(json).Components[0]
                );
        }

        [Test]
        public void SimpleNested()
        {
            var entity = m_Manager.CreateEntity(typeof(NestedComponent));

            var nestedComponent = m_Manager.GetComponentData<NestedComponent>(entity);
            nestedComponent.test.x = 123f;
            m_Manager.SetComponentData(entity, nestedComponent);

            var container = new EntityContainer(m_Manager, entity);
            var json = JsonSerializer.Serialize(ref container);

            Assert.AreEqual(
                nestedComponent,
                UnityEngine.JsonUtility.FromJson<TestComponentWrapper<NestedComponent>>(json).Components[0]
                );
        }

        [Test]
        public void MathOverrides()
        {
            var entity = m_Manager.CreateEntity(typeof(MathComponent));

            var math = m_Manager.GetComponentData<MathComponent>(entity);
            math.v2 = new float2(1f, 2f);
            math.v3 = new float3(2f, 4f, 9f);
            math.v4 = new float4(3f, 8f, 18f, 32f);
            m_Manager.SetComponentData(entity, math);

            var container = new EntityContainer(m_Manager, entity);
            var json = JsonSerializer.Serialize(ref container);

            // Note: This test is to be improved, for various reasons,
            //  (SimpleJson being ignored in unity >= 18.2, Roslyn issues preventing
            //  to upgrade the properties packages (that would fix the SimpleJson issue))
            //  we fallback on string matching for now.

            Assert.IsTrue(json.Contains("float2(1f, 2f)"));
            Assert.IsTrue(json.Contains("float3(2f, 4f, 9f)"));
            Assert.IsTrue(json.Contains("float4(3f, 8f, 18f, 32f)"));
        }

        [Test]
        public void BlittableTest()
        {
            var entity = m_Manager.CreateEntity(typeof(BlitComponent));

            var comp = m_Manager.GetComponentData<BlitComponent>(entity);
            comp.blit.x = 123f;
            comp.blit.y = 456.789;
            comp.blit.z = -12;
            comp.flt = 0.01f;
            m_Manager.SetComponentData(entity, comp);

            var container = new EntityContainer(m_Manager, entity);
            var json = JsonSerializer.Serialize(ref container);

            Assert.AreEqual(
                comp,
                UnityEngine.JsonUtility.FromJson<TestComponentWrapper<BlitComponent>>(json).Components[0]
                );
        }

        [Test]
        public void EnumComponentTest()
        {
            var entity = m_Manager.CreateEntity(typeof(TestEnumComponent));

            var c = m_Manager.GetComponentData<TestEnumComponent>(entity);
            c.e = MyEnum.THREE;
            m_Manager.SetComponentData(entity, c);

            var container = new EntityContainer(m_Manager, entity);
            var json = JsonSerializer.Serialize(ref container);

            Assert.AreEqual(MyEnum.THREE, UnityEngine.JsonUtility.FromJson<TestComponentWrapper<TestEnumComponent>>(json).Components[0].e);
        }

        [Serializable]
        public class TestBufferElementDataCollection
        {
            public TestBufferElementData[] TestBufferElementData;
        }

        [Test]
        public void BufferComponentDataTest()
        {
            var entity = m_Manager.CreateEntity(typeof(TestBufferElementData));

            var buffer = m_Manager.GetBuffer<TestBufferElementData>(entity);

            var v1 = new TestBufferElementData();

            v1.blit.x = 123f;
            v1.blit.y = 456.789;
            v1.blit.z = -12;
            v1.flt = 0.01f;

            buffer.Add(v1);

            v1.blit.x = 123f * 2f;
            buffer.Add(v1);

            v1.blit.x = 123f * 3f;
            buffer.Add(v1);

            var container = new EntityContainer(m_Manager, entity);

            var json = JsonSerializer.Serialize(ref container);

            for (int i = 0; i < buffer.Length; ++i)
            {
                Assert.AreEqual(
                    123*(i + 1),
                    JsonUtility.FromJson<TestComponentWrapper<TestBufferElementDataCollection>>(json)
                        .Components[0].TestBufferElementData[i].blit.x);
            }
        }
    }
}
