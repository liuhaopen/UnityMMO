using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Unity.Entities.Tests
{
    public class WorldDebuggingToolsTests : ECSTestsFixture
    {

        [DisableAutoCreation]
        class RegularSystem : ComponentSystem
        {
#pragma warning disable 649
            struct Entities
            {

                public readonly int Length;
                public ComponentDataArray<EcsTestData> tests;
            }

            [Inject] private Entities entities;
#pragma warning restore 649
            
            protected override void OnUpdate()
            {
                throw new NotImplementedException();
            }
        }

        [DisableAutoCreation]
        class SubtractiveSystem : ComponentSystem
        {
#pragma warning disable 649            
            struct Entities
            {
                public readonly int Length;
                public ComponentDataArray<EcsTestData> tests;
                public SubtractiveComponent<EcsTestData2> noTest2;
            }

            [Inject] private Entities entities;
 #pragma warning restore 649
            
            protected override void OnUpdate()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void SystemInclusionList_MatchesComponents()
        {
            var system = World.Active.GetOrCreateManager<RegularSystem>();
            
            var entity = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));

            var matchList = new List<Tuple<ScriptBehaviourManager, List<ComponentGroup>>>();
            
            WorldDebuggingTools.MatchEntityInComponentGroups(World.Active, entity, matchList);
            
            Assert.AreEqual(1, matchList.Count);
            Assert.AreEqual(system, matchList[0].Item1);
            Assert.AreEqual(system.ComponentGroups[0], matchList[0].Item2[0]);
        }

        [Test]
        public void SystemInclusionList_IgnoresSubtractedComponents()
        {
            World.Active.GetOrCreateManager<SubtractiveSystem>();
            
            var entity = m_Manager.CreateEntity(typeof(EcsTestData), typeof(EcsTestData2));

            var matchList = new List<Tuple<ScriptBehaviourManager, List<ComponentGroup>>>();
            
            WorldDebuggingTools.MatchEntityInComponentGroups(World.Active, entity, matchList);
            
            Assert.AreEqual(0, matchList.Count);
        }
        
    }
}
