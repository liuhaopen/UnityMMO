using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

#if false
namespace Unity.Entities
{
    public static class IJobProcessComponentDataGenerator
    {
        enum Combination
        {
            D
        }
        
        [MenuItem("Tools/Gen IJobProcessComponentData")]
        static void GenTest()
        {
            var combinations = new List<Combination[]>();
            for (int count = 1; count <= 6; count++)
            {
                var array = new Combination[count];
                for (var c = 0; c != array.Length; c++)
                    array[c] = Combination.D;
                
                combinations.Add(array);
            }

            var res = GenerateFile(combinations);
            
            File.WriteAllText("Packages/com.unity.entities/Unity.Entities/IJobProcessComponentData.generated.cs", res);
            AssetDatabase.Refresh();
            
        }

        static string GetComboString(bool withEntity, Combination[] combinations)
        {
            var baseType = new StringBuilder();

            if (withEntity)
                baseType.Append("E");
            foreach (var c in combinations)
                baseType.Append(Enum.GetName(typeof(Combination), c));
            return baseType.ToString();
        }

        static void GenerateScheduleFunc(List<Combination[]> combinations, string funcName, int forEach,
            string scheduleMode, StringBuilder gen)
        {
            gen.Append
            (
                $@"            
            public static JobHandle {funcName}<T>(this T jobData, ComponentSystemBase system, JobHandle dependsOn = default(JobHandle))
                where T : struct, IBaseJobProcessComponentData
            {{
                var typeT = typeof(T);"
            );
            
            foreach (var combination in combinations)
            {
                string comboString;
                comboString = GetComboString(false, combination);
                gen.Append(
                    $@"             
                if (typeof(IBaseJobProcessComponentData_{comboString}).IsAssignableFrom(typeT))
                    return ScheduleInternal_{comboString}(ref jobData, system, null, {forEach}, dependsOn, {scheduleMode});"
                );
               
                comboString = GetComboString(true, combination);
                gen.Append(
                    $@"             
                if (typeof(IBaseJobProcessComponentData_{comboString}).IsAssignableFrom(typeT))
                    return ScheduleInternal_{comboString}(ref jobData, system, null, {forEach}, dependsOn, {scheduleMode});"
                );
            }

            gen.Append
            (
                $@"             
                throw new System.ArgumentException({'"'}Not supported{'"'});
            }}
"
            );
        }

        static void GenerateScheduleGroupFunc(List<Combination[]> combinations, string funcName, int forEach,
            string scheduleMode, StringBuilder gen)
        {
            gen.Append
            (
                $@"            
            public static JobHandle {funcName}<T>(this T jobData, ComponentGroup componentGroup, JobHandle dependsOn = default(JobHandle))
                where T : struct, IBaseJobProcessComponentData
            {{
                var typeT = typeof(T);"
            );
            
            foreach (var combination in combinations)
            {
                string comboString;
                comboString = GetComboString(false, combination);
                gen.Append(
                    $@"             
                if (typeof(IBaseJobProcessComponentData_{comboString}).IsAssignableFrom(typeT))
                    return ScheduleInternal_{comboString}(ref jobData, null, componentGroup, {forEach}, dependsOn, {scheduleMode});"
                );
               
                comboString = GetComboString(true, combination);
                gen.Append(
                    $@"             
                if (typeof(IBaseJobProcessComponentData_{comboString}).IsAssignableFrom(typeT))
                    return ScheduleInternal_{comboString}(ref jobData, null, componentGroup, {forEach}, dependsOn, {scheduleMode});"
                );
            }

            gen.Append
            (
                $@"             
                throw new System.ArgumentException({'"'}Not supported{'"'});
            }}
"
            );
        }

        static string GenerateFile(List<Combination[]> combinations)
        {
            var gen = new StringBuilder();
            var igen = new StringBuilder();

            GenerateScheduleFunc(combinations, "Schedule", 1, "ScheduleMode.Batched", gen);
            GenerateScheduleFunc(combinations, "ScheduleSingle", -1, "ScheduleMode.Batched", gen);
            GenerateScheduleFunc(combinations, "Run", -1, "ScheduleMode.Run", gen);

            GenerateScheduleGroupFunc(combinations, "ScheduleGroup", 1, "ScheduleMode.Batched", gen);
            GenerateScheduleGroupFunc(combinations, "ScheduleGroupSingle", -1, "ScheduleMode.Batched", gen);
            GenerateScheduleGroupFunc(combinations, "RunGroup", -1, "ScheduleMode.Run", gen);

            foreach (var combination in combinations)
            {
                Generate(false, combination, gen, igen);
                Generate(true, combination, gen, igen);
            }
            
            var res = 
            (
                $@"// Generated by IJobProcessComponentDataGenerator.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#if !UNITY_ZEROPLAYER

using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using System;


namespace Unity.Entities
{{
    
{igen}

    public static partial class JobProcessComponentDataExtensions
    {{
{gen}        
    }}
}}

#endif
"
            );

            return res;
        }


        static string Generate(bool withEntity, Combination[] combination, StringBuilder gen, StringBuilder igen)
        {
            var comboString = GetComboString(withEntity, combination);
            var untypedGenericParams = new StringBuilder();
            var genericParams = new StringBuilder();
            var genericConstraints = new StringBuilder();

            var executeParams = new StringBuilder();
            var executeCallParams = new StringBuilder();
            var ptrs = new StringBuilder();

            var interfaceName = withEntity ? "IJobProcessComponentDataWithEntity" : "IJobProcessComponentData";
            
            if (withEntity)
            {
                executeCallParams.Append("ptrE[i], i + beginIndex, ");
                executeParams.Append("Entity entity, int index, ");
                ptrs.AppendLine
                (
$"                        var ptrE = (Entity*)UnsafeUtilityEx.RestrictNoAlias(chunkIterator.GetCurrentChunkComponentDataPtr(false, 0));"
                );
            }
                        
            for (int i = 0; i != combination.Length; i++)
            {
                ptrs.AppendLine
                (
$"                        var ptr{i} = UnsafeUtilityEx.RestrictNoAlias(chunkIterator.GetCurrentChunkComponentDataPtr(jobData.Iterator.IsReadOnly{i} == 0, jobData.Iterator.IndexInGroup{i}));"
                );

                genericConstraints.Append($"            where U{i} : struct, IComponentData");
                if (i != combination.Length - 1)
                    genericConstraints.AppendLine();
                untypedGenericParams.Append(",");

                genericParams.Append($"U{i}");
                if (i != combination.Length - 1)
                    genericParams.Append(", ");

                executeCallParams.Append($"ref UnsafeUtilityEx.ArrayElementAsRef<U{i}>(ptr{i}, i)");
                if (i != combination.Length - 1)
                    executeCallParams.Append(", ");
                
                executeParams.Append($"ref U{i} c{i}");
                if (i != combination.Length - 1)
                    executeParams.Append(", ");
            }
            
            
            igen.Append
            (
            $@"

        [JobProducerType(typeof(JobProcessComponentDataExtensions.JobStruct_Process_{comboString}<{untypedGenericParams}>))]
        public interface {interfaceName}<{genericParams}> : JobProcessComponentDataExtensions.IBaseJobProcessComponentData_{comboString}
{genericConstraints}
        {{
            void Execute({executeParams});
        }}"
            );


            gen.Append
            (
                    $@"            
            internal static unsafe JobHandle ScheduleInternal_{comboString}<T>(ref T jobData, ComponentSystemBase system, ComponentGroup componentGroup, int innerloopBatchCount, JobHandle dependsOn, ScheduleMode mode)
                where T : struct
            {{
                JobStruct_ProcessInfer_{comboString}<T> fullData;
                fullData.Data = jobData;

                var isParallelFor = innerloopBatchCount != -1;
                Initialize(system, componentGroup, typeof(T), typeof(JobStruct_Process_{comboString}<{untypedGenericParams}>), isParallelFor, ref JobStruct_ProcessInfer_{comboString}<T>.Cache, out fullData.Iterator);
                return Schedule(UnsafeUtility.AddressOf(ref fullData), fullData.Iterator.m_Length, innerloopBatchCount, isParallelFor, ref JobStruct_ProcessInfer_{comboString}<T>.Cache, dependsOn, mode);
            }}


            [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
            public interface IBaseJobProcessComponentData_{comboString} : IBaseJobProcessComponentData {{ }}

            [StructLayout(LayoutKind.Sequential)]
            private struct JobStruct_ProcessInfer_{comboString}<T> where T : struct
            {{
                public static JobProcessComponentDataCache Cache;
    
                public ProcessIterationData Iterator;
                public T Data;
            }}
    
            [StructLayout(LayoutKind.Sequential)]
            internal struct JobStruct_Process_{comboString}<T, {genericParams}>
                where T : struct, {interfaceName}<{genericParams}>
{genericConstraints}
            {{
                public ProcessIterationData Iterator;
                public T Data;
    
                [Preserve]
                public static IntPtr Initialize(JobType jobType)
                {{
                    return JobsUtility.CreateJobReflectionData(typeof(JobStruct_Process_{comboString}<T, {genericParams}>), typeof(T), jobType, (ExecuteJobFunction) Execute);
                }}
    
                delegate void ExecuteJobFunction(ref JobStruct_Process_{comboString}<T, {genericParams}> data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

                public static unsafe void Execute(ref JobStruct_Process_{comboString}<T, {genericParams}> jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
                {{
                    if (jobData.Iterator.m_IsParallelFor)
                    {{
                        int begin, end;
                        while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out begin, out end))
                            ExecuteChunk(ref jobData, bufferRangePatchData, begin, end);
                    }}
                    else
                    {{
                        ExecuteChunk(ref jobData, bufferRangePatchData, 0, jobData.Iterator.m_Length);
                    }}
                }}

                static unsafe void ExecuteChunk(ref JobStruct_Process_{comboString}<T, {genericParams}> jobData, IntPtr bufferRangePatchData, int begin, int end)
                {{
                    ref ComponentChunkIterator chunkIterator = ref jobData.Iterator.Iterator;
    
                    for (var blockIndex = begin; blockIndex != end; ++blockIndex)
                    {{
                        jobData.Iterator.Iterator.MoveToChunkWithoutFiltering(blockIndex);
    
                        var processBlock = jobData.Iterator.Iterator.MatchesFilter();
    
                        if (!processBlock)
                            continue;
    
                        int beginIndex, endIndex;
                        chunkIterator.GetCurrentChunkRange(out beginIndex, out endIndex);
    #if ENABLE_UNITY_COLLECTIONS_CHECKS
                        JobsUtility.PatchBufferMinMaxRanges(bufferRangePatchData, UnsafeUtility.AddressOf(ref jobData), beginIndex, endIndex - beginIndex);
    #endif
    
                        var count = chunkIterator.GetCurrentChunkCount();
    
{ptrs}
    
                        for (var i = 0; i != count; i++)
                        {{
                            jobData.Data.Execute({executeCallParams});
                        }}
                    }}
                }}
            }}
"
            );

            return gen.ToString();
        }        
    }
}
#endif
