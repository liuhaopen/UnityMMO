using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Assertions;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using ReadOnlyAttribute = Unity.Collections.ReadOnlyAttribute;

#if !UNITY_ZEROPLAYER

namespace Unity.Entities
{
    //@TODO: What about change or add?
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ChangedFilterAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Struct)]
    public class RequireComponentTagAttribute : Attribute
    {
        public Type[] TagComponents;

        public RequireComponentTagAttribute(params Type[] tagComponents)
        {
            TagComponents = tagComponents;
        }
    }

    [AttributeUsage(AttributeTargets.Struct)]
    public class RequireSubtractiveComponentAttribute : Attribute
    {
        public Type[] SubtractiveComponents;

        public RequireSubtractiveComponentAttribute(params Type[] subtractiveComponents)
        {
            SubtractiveComponents = subtractiveComponents;
        }
    }

    public static partial class JobProcessComponentDataExtensions
    {
        static ComponentType[] GetComponentTypes(Type jobType)
        {
            var interfaceType = GetIJobProcessComponentDataInterface(jobType);
            if (interfaceType != null)
            {
                int temp;
                ComponentType[] temp2;
                return GetComponentTypes(jobType, interfaceType, out temp, out temp2);
            }

            return null;
        }

        static ComponentType[] GetComponentTypes(Type jobType, Type interfaceType, out int processCount,
            out ComponentType[] changedFilter)
        {
            var genericArgs = interfaceType.GetGenericArguments();

            var executeMethodParameters = jobType.GetMethod("Execute").GetParameters();
            
            var componentTypes = new List<ComponentType>();
            var changedFilterTypes = new List<ComponentType>();

            
            // void Execute(Entity entity, int index, ref T0 data0, ref T1 data1, ref T2 data2);
            // First two parameters are optional, depending on the interface name used.
            var methodParameterOffset = genericArgs.Length != executeMethodParameters.Length ? 2 : 0;
            
            for (var i = 0; i < genericArgs.Length; i++)
            {
                var isReadonly = executeMethodParameters[i + methodParameterOffset].GetCustomAttribute(typeof(ReadOnlyAttribute)) != null;
                
                var type = new ComponentType(genericArgs[i],
                    isReadonly ? ComponentType.AccessMode.ReadOnly : ComponentType.AccessMode.ReadWrite);
                componentTypes.Add(type);

                var isChangedFilter = executeMethodParameters[i + methodParameterOffset].GetCustomAttribute(typeof(ChangedFilterAttribute)) != null;
                if (isChangedFilter)
                    changedFilterTypes.Add(type);
            }

            var subtractive = jobType.GetCustomAttribute<RequireSubtractiveComponentAttribute>();
            if (subtractive != null)
                foreach (var type in subtractive.SubtractiveComponents)
                    componentTypes.Add(ComponentType.Subtractive(type));

            var requiredTags = jobType.GetCustomAttribute<RequireComponentTagAttribute>();
            if (requiredTags != null)
                foreach (var type in requiredTags.TagComponents)
                    componentTypes.Add(ComponentType.ReadOnly(type));

            processCount = genericArgs.Length;
            changedFilter = changedFilterTypes.ToArray();
            return componentTypes.ToArray();
        }
        
        static int CalculateEntityCount(ComponentSystemBase system, Type jobType)
        {
            var componentGroup = GetComponentGroupForIJobProcessComponentData(system, jobType);

            int entityCount = componentGroup.CalculateLength();

            return entityCount;
        }
        
        static IntPtr GetJobReflection(Type jobType, Type wrapperJobType, Type interfaceType,
            bool isIJobParallelFor)
        {
            Assert.AreNotEqual(null, wrapperJobType);
            Assert.AreNotEqual(null, interfaceType);

            var genericArgs = interfaceType.GetGenericArguments();

            var jobTypeAndGenericArgs = new List<Type>();
            jobTypeAndGenericArgs.Add(jobType);
            jobTypeAndGenericArgs.AddRange(genericArgs);
            var resolvedWrapperJobType = wrapperJobType.MakeGenericType(jobTypeAndGenericArgs.ToArray());

            object[] parameters = {isIJobParallelFor ? JobType.ParallelFor : JobType.Single};
            var reflectionDataRes = resolvedWrapperJobType.GetMethod("Initialize").Invoke(null, parameters);
            return (IntPtr) reflectionDataRes;
        }

        static Type GetIJobProcessComponentDataInterface(Type jobType)
        {
            foreach (var iType in jobType.GetInterfaces())
                if (iType.Assembly == typeof(IBaseJobProcessComponentData).Assembly &&
                    iType.Name.StartsWith("IJobProcessComponentData"))
                    return iType;

            return null;
        }

        static void PrepareComponentGroup(ComponentSystemBase system, Type jobType)
        {
            var iType = GetIJobProcessComponentDataInterface(jobType);
            
            ComponentType[] filterChanged;
            int processTypesCount;
            var types = GetComponentTypes(jobType, iType, out processTypesCount, out filterChanged);
            system.GetComponentGroupInternal(types);
        }

        static unsafe void Initialize(ComponentSystemBase system, ComponentGroup componentGroup, Type jobType, Type wrapperJobType,
            bool isParallelFor, ref JobProcessComponentDataCache cache, out ProcessIterationData iterator)
        {
            // Get the job reflection data and cache it if we don't already have it cached.
            if (isParallelFor && cache.JobReflectionDataParallelFor == IntPtr.Zero ||
                !isParallelFor && cache.JobReflectionData == IntPtr.Zero)
            {
                var iType = GetIJobProcessComponentDataInterface(jobType);
                if (cache.Types == null)
                    cache.Types = GetComponentTypes(jobType, iType, out cache.ProcessTypesCount,
                        out cache.FilterChanged);

                var res = GetJobReflection(jobType, wrapperJobType, iType, isParallelFor);

                if (isParallelFor)
                    cache.JobReflectionDataParallelFor = res;
                else
                    cache.JobReflectionData = res;
            }

            // Update cached ComponentGroup and ComponentSystem data.
            if (system != null)
            {
                if (cache.ComponentSystem != system)
                {
                    cache.ComponentGroup = system.GetComponentGroupInternal(cache.Types);

                    // If the cached filter has changed, update the newly cached ComponentGroup with those changes.
                    if (cache.FilterChanged.Length != 0)
                        cache.ComponentGroup.SetFilterChanged(cache.FilterChanged);

                    // Otherwise, just reset our newly cached ComponentGroup's filter.
                    else
                        cache.ComponentGroup.ResetFilter();

                    cache.ComponentSystem = system;
                }
            }
            else if (componentGroup != null)
            {
                if (cache.ComponentGroup != componentGroup)
                {
                    // Cache the new ComponentGroup and cache that our system is null.
                    cache.ComponentGroup = componentGroup;
                    cache.ComponentSystem = null;
                }
            }

            var group = cache.ComponentGroup;
            
            iterator.IsReadOnly0 = iterator.IsReadOnly1 = iterator.IsReadOnly2 = iterator.IsReadOnly3 = iterator.IsReadOnly4 = iterator.IsReadOnly5= 0;
            fixed (int* isReadOnly = &iterator.IsReadOnly0)
            {
                for (var i = 0; i != cache.ProcessTypesCount; i++)
                    isReadOnly[i] = cache.Types[i].AccessModeType == ComponentType.AccessMode.ReadOnly ? 1 : 0;
            }

            // Set process iteration data's ComponentChunkIterator
            iterator.Iterator = group.GetComponentChunkIterator();

            // Get each type's index in the component group and store them on the process iteration data for output.
            iterator.IndexInGroup0 = iterator.IndexInGroup1 = iterator.IndexInGroup2 = iterator.IndexInGroup3 = iterator.IndexInGroup4 = iterator.IndexInGroup5 = -1;
            fixed (int* groupIndices = &iterator.IndexInGroup0)
            {
                for (var i = 0; i != cache.ProcessTypesCount; i++)
                    groupIndices[i] = group.GetIndexInComponentGroup(cache.Types[i].TypeIndex);
            }

            iterator.m_IsParallelFor = isParallelFor;
            iterator.m_Length = group.CalculateNumberOfChunksWithoutFiltering();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            iterator.m_MaxIndex = iterator.m_Length - 1;
            iterator.m_MinIndex = 0;
            
            iterator.m_Safety0 = iterator.m_Safety1 = iterator.m_Safety2 = iterator.m_Safety3 = iterator.m_Safety4 = iterator.m_Safety5= default(AtomicSafetyHandle);

            iterator.m_SafetyReadOnlyCount = 0;
            fixed (AtomicSafetyHandle* safety = &iterator.m_Safety0)
            {
                for (var i = 0; i != cache.ProcessTypesCount; i++)
                    if (cache.Types[i].AccessModeType == ComponentType.AccessMode.ReadOnly)
                    {
                        safety[iterator.m_SafetyReadOnlyCount] = group.GetSafetyHandle(group.GetIndexInComponentGroup(cache.Types[i].TypeIndex));
                        iterator.m_SafetyReadOnlyCount++;
                    }
            }

            iterator.m_SafetyReadWriteCount = 0;
            fixed (AtomicSafetyHandle* safety = &iterator.m_Safety0)
            {
                for (var i = 0; i != cache.ProcessTypesCount; i++)
                    if (cache.Types[i].AccessModeType == ComponentType.AccessMode.ReadWrite)
                    {
                        safety[iterator.m_SafetyReadOnlyCount + iterator.m_SafetyReadWriteCount] = group.GetSafetyHandle(group.GetIndexInComponentGroup(cache.Types[i].TypeIndex));
                        iterator.m_SafetyReadWriteCount++;
                    }
            }

            Assert.AreEqual(cache.ProcessTypesCount, iterator.m_SafetyReadWriteCount + iterator.m_SafetyReadOnlyCount);
#endif
        }
    
        [EditorBrowsable(EditorBrowsableState.Never)]
        public interface IBaseJobProcessComponentData
        {
        }

        internal struct JobProcessComponentDataCache
        {
            public IntPtr JobReflectionData;
            public IntPtr JobReflectionDataParallelFor;
            public ComponentType[] Types;
            public ComponentType[] FilterChanged;

            public int ProcessTypesCount;

            public ComponentGroup ComponentGroup;
            public ComponentSystemBase ComponentSystem;
        }

        [NativeContainer]
        [NativeContainerSupportsMinMaxWriteRestriction]
        [StructLayout(LayoutKind.Sequential)]
        internal struct ProcessIterationData
        {
            public ComponentChunkIterator Iterator;
            public int IndexInGroup0;
            public int IndexInGroup1;
            public int IndexInGroup2;
            public int IndexInGroup3;
            public int IndexInGroup4;
            public int IndexInGroup5;

            public int IsReadOnly0;
            public int IsReadOnly1;
            public int IsReadOnly2;
            public int IsReadOnly3;
            public int IsReadOnly4;
            public int IsReadOnly5;
        
            public bool m_IsParallelFor;

            public int m_Length;

#if ENABLE_UNITY_COLLECTIONS_CHECKS

            public int m_MinIndex;
            public int m_MaxIndex;

#pragma warning disable 414
            public int m_SafetyReadOnlyCount;
            public int m_SafetyReadWriteCount;
            public AtomicSafetyHandle m_Safety0;
            public AtomicSafetyHandle m_Safety1;
            public AtomicSafetyHandle m_Safety2;
            public AtomicSafetyHandle m_Safety3;
            public AtomicSafetyHandle m_Safety4;
            public AtomicSafetyHandle m_Safety5;
#pragma warning restore
#endif
        }
        public static ComponentGroup GetComponentGroupForIJobProcessComponentData(this ComponentSystemBase system,
            Type jobType)
        {
            var types = GetComponentTypes(jobType);
            if (types != null)
                return system.GetComponentGroupInternal(types);
            else
                return null;
        }
        
        //NOTE: It would be much better if C# could resolve the branch with generic resolving,
        //      but apparently the interface constraint is not enough..

        public static void PrepareComponentGroup<T>(this T jobData, ComponentSystemBase system)
            where T : struct, IBaseJobProcessComponentData
        {
            PrepareComponentGroup(system, typeof(T));
        }
        
        public static int CalculateEntityCount<T>(this T jobData, ComponentSystemBase system)
            where T : struct, IBaseJobProcessComponentData
        {
            return CalculateEntityCount(system, typeof(T));
        }


        
        static unsafe JobHandle Schedule(void* fullData, int length, int innerloopBatchCount,
            bool isParallelFor, ref JobProcessComponentDataCache cache, JobHandle dependsOn, ScheduleMode mode)
        {
            if (isParallelFor)
            {
                var scheduleParams =
                    new JobsUtility.JobScheduleParameters(fullData, cache.JobReflectionDataParallelFor, dependsOn,
                        mode);
                return JobsUtility.ScheduleParallelFor(ref scheduleParams, length, innerloopBatchCount);
            }
            else
            {
                var scheduleParams =
                    new JobsUtility.JobScheduleParameters(fullData, cache.JobReflectionData, dependsOn, mode);
                return JobsUtility.Schedule(ref scheduleParams);
            }
        }
    }
}
#endif