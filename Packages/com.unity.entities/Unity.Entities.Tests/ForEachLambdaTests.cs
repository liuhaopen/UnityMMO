using System;
using NUnit.Framework;

namespace Unity.Entities.Tests
{
    class ForEachLambdaTests : ECSTestsFixture
    {
        [DisableAutoCreation]
        class ForEachSystem : ComponentSystem
        {
            void ComponentData(int expectedCount)
            {
                int counter;

                counter = 0;
                ForEach((ref EcsTestData testData) =>
                {
                    Assert.AreEqual(5, testData.value);
                    testData.value++;
                    counter++;
                });
                Assert.AreEqual(expectedCount, counter);
                                
                counter = 0;
                ForEach((Entity entity, ref EcsTestData testData) =>
                {
                    Assert.AreEqual(6, testData.value);
                    testData.value++;
                    
                    Assert.AreEqual(7, EntityManager.GetComponentData<EcsTestData>(entity).value);
                    
                    counter++;
                });
                Assert.AreEqual(expectedCount, counter);
            }
            
            void SharedComponentData(int expectedCount)
            {
                int counter = 0;
                ForEach((SharedData1 testData) =>
                {
                    Assert.AreEqual(7, testData.value);
                    counter++;
                });
                Assert.AreEqual(expectedCount, counter);
            }
            
            void DynamicBuffer(int expectedCount)
            {
                int counter;

                counter = 0;
                ForEach((DynamicBuffer<EcsIntElement> testData) =>
                {
                    testData.Add(0);
                    testData.Add(1);
                    counter++;
                });
                Assert.AreEqual(expectedCount, counter);
            }
            
            protected override void OnUpdate()
            {
                ComponentData(1);
                SharedComponentData(1);
                DynamicBuffer(1);
            }
        }


        [Test]
        public void ForEach_1()
        {
            m_Manager.AddComponentData(m_Manager.CreateEntity(), new EcsTestData(5));

            m_Manager.AddSharedComponentData(m_Manager.CreateEntity(), new SharedData1(7));

            m_Manager.CreateEntity(typeof(EcsIntElement));
            
            World.GetOrCreateManager<ForEachSystem>().Update();
        }
        
        [DisableAutoCreation]
        class ForEachSystem_Many : ComponentSystem
        {
            void ComponentData(int expectedCount)
            {
                int counter;

                counter = 0;
                ForEach((Entity entity, ref EcsTestData t0, ref EcsTestData2 t1, ref EcsTestData3 t2, ref EcsTestData4 t3, ref EcsTestData5 t4) =>
                {
                    Assert.AreEqual(0, t0.value);
                    Assert.AreEqual(1, t1.value0);
                    Assert.AreEqual(2, t2.value0);
                    Assert.AreEqual(3, t3.value0);
                    Assert.AreEqual(4, t4.value0);
                    
                    counter++;
                });
                Assert.AreEqual(expectedCount, counter);
                
            }
            
            protected override void OnUpdate()
            {
                ComponentData(1);
            }
        }
        
        [Test]
        public void ForEach_Many()
        {
            var entity = m_Manager.CreateEntity();
            m_Manager.AddComponentData(entity, new EcsTestData(0));
            m_Manager.AddComponentData(entity, new EcsTestData2(1));
            m_Manager.AddComponentData(entity, new EcsTestData3(2));
            m_Manager.AddComponentData(entity, new EcsTestData4(3));
            m_Manager.AddComponentData(entity, new EcsTestData5(4));
            
            World.GetOrCreateManager<ForEachSystem_Many>().Update();
        }
        
        
        [DisableAutoCreation]
        class ForEachEntityManagerSafety_Test : ComponentSystem
        {
            protected override void OnUpdate()
            {
                int counter;

                counter = 0;
                ForEach((Entity entity, ref EcsTestData t0) =>
                {
                    Assert.Throws<InvalidOperationException>(()=>EntityManager.CreateEntity());
                    Assert.Throws<InvalidOperationException>(()=>EntityManager.DestroyEntity(entity));
                    Assert.Throws<InvalidOperationException>(()=>EntityManager.AddComponent(entity, typeof(EcsTestData2)));
                    Assert.Throws<InvalidOperationException>(()=>EntityManager.RemoveComponent<EcsTestData>(entity));
                    counter++;
                });
                Assert.AreEqual(1, counter);

                Assert.Throws<ArgumentException>(() =>
                {
                    ForEach((Entity entity, ref EcsTestData t0) => throw new System.ArgumentException());
                });
                
                Assert.AreEqual(0, EntityManager.m_InsideForEach);
            }
        }
        
        [Test]
        public void ForEach_Safety()
        {
            var entity = m_Manager.CreateEntity();
            m_Manager.AddComponentData(entity, new EcsTestData(0));
            
            World.GetOrCreateManager<ForEachEntityManagerSafety_Test>().Update();
        }
        
        //@TODO: Class iterator test coverage...
    }
 }