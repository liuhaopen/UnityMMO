using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace Unity.Entities.Tests
{
    class ComponentDataWrapper_UnitTests
    {
        static bool IsSubclassOfOpenGenericType(Type type, Type genericType)
        {
            if (type.IsSubclassOf(genericType))
                return true;
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (genericType == cur)
                    return true;
                type = type.BaseType;
            }
            return false;
        }

        static IEnumerable<Type> GetAllSubTypes(Type genericType, params Type[] ignoreTypes)
        {
            var result = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    result.AddRange(
                        assembly.GetTypes()
                            .Where(t =>
                                !t.IsAbstract
                                && !t.IsGenericType
                                && !ignoreTypes.Contains(t)
                                && IsSubclassOfOpenGenericType(t, genericType)
                            )
                    );
                }
                // ignore if error loading some type from a dll
                catch (TypeLoadException) { }
            }
            return result;
        }

        static readonly IEnumerable k_AllComponentDataWrapperTypes =
            GetAllSubTypes(typeof(ComponentDataWrapper<>)).Select(t => new TestCaseData(t).SetName(t.FullName));

        [TestCaseSource(nameof(k_AllComponentDataWrapperTypes))]
        public void AllComponentDataWrappers_DisallowMultipleComponent(Type type)
        {
            Assert.That(Attribute.IsDefined(type, typeof(DisallowMultipleComponent)), Is.True);
        }

        static readonly IEnumerable k_AllSharedComponentDataWrapperTypes = GetAllSubTypes(
            typeof(SharedComponentDataWrapper<>), ignoreTypes: typeof(MockSharedDisallowMultipleComponent)
        ).Select(t => new TestCaseData(t).SetName(t.FullName));

        // currently enforced due to implementation of SerializeUtilityHybrid
        // ideally all types should ultimately have DisallowMultipleComponent
        [TestCaseSource(nameof(k_AllSharedComponentDataWrapperTypes))]
        public void NoSharedComponentDataWrappers_DisallowMultipleComponent(Type type)
        {
            Assert.That(Attribute.IsDefined(type, typeof(DisallowMultipleComponent)), Is.False);
        }
    }
}
