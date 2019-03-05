using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.Entities.Tests
{
    class ComponentDataWrapper_Prefab_IntegrationTests
    {
        static TestCaseData[] wrapperTypeTestCases =
        {
            new TestCaseData(
                "Packages/com.unity.entities/Unity.Entities.Hybrid.Tests/Prefab_With_ComponentDataWrapper.prefab",
                (Func<GameObjectEntity, int>)(goe => goe.EntityManager.GetComponentData<MockData>(goe.Entity).Value)
            ).Returns(null).SetName("Prefab_ComponentDataWrapper"),
            new TestCaseData(
                "Packages/com.unity.entities/Unity.Entities.Hybrid.Tests/Prefab_With_SharedComponentDataWrapper.prefab",
                (Func<GameObjectEntity, int>)(goe => goe.EntityManager.GetSharedComponentData<MockSharedData>(goe.Entity).Value)
            ).Returns(null).SetName("Prefab_SharedComponentDataWrapper")
        };

        static readonly MethodInfo k_OpenPrefab =
            typeof(PrefabStageUtility).GetMethod("OpenPrefab", BindingFlags.Static | BindingFlags.NonPublic, null, new[] { typeof(string) }, Array.Empty<ParameterModifier>());

        [Ignore("Disabled for now since it enters playmode and we need to avoid those tests")]
        [UnityTest, TestCaseSource(nameof(wrapperTypeTestCases))]
        public IEnumerator ComponentDataWrapper_WhenEnterPlayMode_ThenEditPrefab_ThenExitPlayMode_StillSynchronizesWithEntityManager(
            string prefabPath, Func<GameObjectEntity, int> getComponentDataValueFromEntityManager
        )
        {
            yield return new EnterPlayMode();
            k_OpenPrefab.Invoke(null, new object[] { prefabPath });
            yield return new ExitPlayMode();

            var go = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot;
            var wrapper = (IIntegerContainer)go.GetComponent<ComponentDataWrapperBase>();
            var expected = wrapper.Integer + 1;
            wrapper.Integer = expected;
            var valueFromEntityManager = getComponentDataValueFromEntityManager(go.GetComponent<GameObjectEntity>());

            Assert.That(valueFromEntityManager, Is.EqualTo(expected), $"{wrapper} is no longer synchronizing with EntityManager");
        }
    }
}
