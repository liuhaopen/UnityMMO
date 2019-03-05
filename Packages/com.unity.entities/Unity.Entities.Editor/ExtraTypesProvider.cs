using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build.Player;
using Unity.Jobs.LowLevel.Unsafe;
    
namespace Unity.Entities.Editor
{
    [InitializeOnLoad]
    public sealed class ExtraTypesProvider
    {
        static void AddIJobProcessComponentData(Type type, HashSet<string> extraTypes)
        {
            foreach (var typeInterface in type.GetInterfaces())
            {
                if (typeInterface.Name.StartsWith("IJobProcessComponentData"))
                {
                    var genericArgumentList = new List<Type> { type };
                    genericArgumentList.AddRange(typeInterface.GetGenericArguments());

                    var producerAttribute = (JobProducerTypeAttribute) typeInterface.GetCustomAttribute(typeof(JobProducerTypeAttribute), true);
                                    
                    if (producerAttribute == null)
                        throw new System.ArgumentException("IJobProcessComponentData interface must have [JobProducerType]");

                    var generatedType = producerAttribute.ProducerType.MakeGenericType(genericArgumentList.ToArray());
                    extraTypes.Add(generatedType.ToString());
                    
                    return;
                }
            }
        }

        static ExtraTypesProvider()
        {
            //@TODO: Only produce JobProcessComponentDataExtensions.JobStruct_Process1
            //       if there is any use of that specific type in deployed code.
            
            PlayerBuildInterface.ExtraTypesProvider += () =>
            {
                var extraTypes = new HashSet<string>();

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (!TypeManager.IsAssemblyReferencingEntities(assembly))
                        continue;

                    foreach (var type in assembly.GetTypes())
                    {
                        if (typeof(JobProcessComponentDataExtensions.IBaseJobProcessComponentData).IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            AddIJobProcessComponentData(type, extraTypes);
                        }
                    }
                }

                return extraTypes;
            };
        }
    }
}