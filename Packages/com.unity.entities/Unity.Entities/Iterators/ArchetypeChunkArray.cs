using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
    public unsafe struct ArchetypeChunk : IEquatable<ArchetypeChunk>
    {
        [NativeDisableUnsafePtrRestriction] internal Chunk* m_Chunk;
        public int Count => m_Chunk->Count;

        public static bool operator ==(ArchetypeChunk lhs, ArchetypeChunk rhs)
        {
            return lhs.m_Chunk == rhs.m_Chunk;
        }

        public static bool operator !=(ArchetypeChunk lhs, ArchetypeChunk rhs)
        {
            return lhs.m_Chunk != rhs.m_Chunk;
        }

        public override bool Equals(object compare)
        {
            return this == (ArchetypeChunk) compare;
        }

        public override int GetHashCode()
        {
            UIntPtr chunkAddr   = (UIntPtr) m_Chunk;
            long    chunkHiHash = ((long) chunkAddr) >> 15;
            int     chunkHash   = (int)chunkHiHash;
            return chunkHash;
        }

        public EntityArchetype Archetype
        {
            get
            {
                return new EntityArchetype()
                {
                    Archetype = m_Chunk->Archetype
                };
            }
        }

        public static ArchetypeChunk Null => new ArchetypeChunk();

        public bool Equals(ArchetypeChunk archetypeChunk)
        {
            return this.m_Chunk == archetypeChunk.m_Chunk;
        }

        public int NumSharedComponents()
        {
            return m_Chunk->Archetype->NumSharedComponents;
        }

        public NativeArray<Entity> GetNativeArray(ArchetypeChunkEntityType archetypeChunkEntityType)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(archetypeChunkEntityType.m_Safety);
#endif
            var archetype = m_Chunk->Archetype;
            var buffer = m_Chunk->Buffer;
            var length = m_Chunk->Count;
            var startOffset = archetype->Offsets[0];
            var result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<Entity>(buffer + startOffset, length, Allocator.None);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref result, archetypeChunkEntityType.m_Safety);
#endif
            return result;
        }

        public bool DidAddOrChange<T>(ArchetypeChunkComponentType<T> chunkComponentType, uint version) where T : struct, IComponentData
        {
            return ChangeVersionUtility.DidAddOrChange(GetComponentVersion(chunkComponentType), version);
        }

        public bool DidAddOrChange<T>(ArchetypeChunkBufferType<T> chunkBufferType, uint version) where T : struct, IBufferElementData
        {
            return ChangeVersionUtility.DidAddOrChange(GetComponentVersion(chunkBufferType), version);
        }

        public bool DidChange<T>(ArchetypeChunkComponentType<T> chunkComponentType) where T : struct, IComponentData
        {
            return ChangeVersionUtility.DidChange(GetComponentVersion(chunkComponentType), chunkComponentType.GlobalSystemVersion);
        }

        public bool DidChange<T>(ArchetypeChunkBufferType<T> chunkBufferType) where T : struct, IBufferElementData
        {
            return ChangeVersionUtility.DidChange(GetComponentVersion(chunkBufferType), chunkBufferType.GlobalSystemVersion);
        }

        public uint GetComponentVersion<T>(ArchetypeChunkComponentType<T> chunkComponentType)
            where T : struct, IComponentData
        {
            var typeIndexInArchetype = ChunkDataUtility.GetIndexInTypeArray(m_Chunk->Archetype, chunkComponentType.m_TypeIndex);
            if (typeIndexInArchetype == -1) return 0;
            return m_Chunk->ChangeVersion[typeIndexInArchetype];
        }

        public uint GetComponentVersion<T>(ArchetypeChunkBufferType<T> chunkBufferType)
            where T : struct, IBufferElementData
        {
            var typeIndexInArchetype = ChunkDataUtility.GetIndexInTypeArray(m_Chunk->Archetype, chunkBufferType.m_TypeIndex);
            if (typeIndexInArchetype == -1) return 0;
            return m_Chunk->ChangeVersion[typeIndexInArchetype];
        }

        public int GetSharedComponentIndex<T>(ArchetypeChunkSharedComponentType<T> chunkSharedComponentData)
            where T : struct, ISharedComponentData
        {
            var archetype = m_Chunk->Archetype;
            var typeIndexInArchetype = ChunkDataUtility.GetIndexInTypeArray(archetype, chunkSharedComponentData.m_TypeIndex);
            if (typeIndexInArchetype == -1) return -1;

            var chunkSharedComponentIndex = archetype->SharedComponentOffset[typeIndexInArchetype];
            var sharedComponentIndex = m_Chunk->SharedComponentValueArray[chunkSharedComponentIndex];
            return sharedComponentIndex;
        }

        public bool Has<T>(ArchetypeChunkComponentType<T> chunkComponentType)
            where T : struct, IComponentData
        {
            var typeIndexInArchetype = ChunkDataUtility.GetIndexInTypeArray(m_Chunk->Archetype, chunkComponentType.m_TypeIndex);
            return (typeIndexInArchetype != -1);
        }

        public bool Has<T>(ArchetypeChunkBufferType<T> chunkBufferType)
            where T : struct, IBufferElementData
        {
            var typeIndexInArchetype = ChunkDataUtility.GetIndexInTypeArray(m_Chunk->Archetype, chunkBufferType.m_TypeIndex);
            return (typeIndexInArchetype != -1);
        }

        public NativeArray<T> GetNativeArray<T>(ArchetypeChunkComponentType<T> chunkComponentType)
            where T : struct, IComponentData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (chunkComponentType.m_IsZeroSized)
                throw new ArgumentException($"ArchetypeChunk.GetNativeArray<{typeof(T)}> cannot be called on zero-sized IComponentData");

            AtomicSafetyHandle.CheckReadAndThrow(chunkComponentType.m_Safety);
#endif
            var archetype = m_Chunk->Archetype;
            var typeIndexInArchetype = ChunkDataUtility.GetIndexInTypeArray(m_Chunk->Archetype, chunkComponentType.m_TypeIndex);
            if (typeIndexInArchetype == -1)
            {
                var emptyResult =
                    NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(null, 0, 0);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref emptyResult, chunkComponentType.m_Safety);
#endif
                return emptyResult;
            }

            var buffer = m_Chunk->Buffer;
            var length = m_Chunk->Count;
            var startOffset = archetype->Offsets[typeIndexInArchetype];
            var result = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(buffer + startOffset, length, Allocator.None);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref result, chunkComponentType.m_Safety);
#endif
            if (!chunkComponentType.IsReadOnly)
                m_Chunk->ChangeVersion[typeIndexInArchetype] = chunkComponentType.GlobalSystemVersion;
            return result;
        }

        public BufferAccessor<T> GetBufferAccessor<T>(ArchetypeChunkBufferType<T> bufferComponentType)
            where T : struct, IBufferElementData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(bufferComponentType.m_Safety0);
#endif
            var archetype = m_Chunk->Archetype;
            var typeIndex = bufferComponentType.m_TypeIndex;
            var typeIndexInArchetype = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);
            if (typeIndexInArchetype == -1)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                return new BufferAccessor<T>(null, 0, 0, true, bufferComponentType.m_Safety0, bufferComponentType.m_Safety1);
#else
                return new BufferAccessor<T>(null, 0, 0);
#endif
            }

            if (!bufferComponentType.IsReadOnly)
                m_Chunk->ChangeVersion[typeIndexInArchetype] = bufferComponentType.GlobalSystemVersion;

            var buffer = m_Chunk->Buffer;
            var length = m_Chunk->Count;
            var startOffset = archetype->Offsets[typeIndexInArchetype];
            int stride = archetype->SizeOfs[typeIndexInArchetype];
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new BufferAccessor<T>(buffer + startOffset, length, stride, bufferComponentType.IsReadOnly, bufferComponentType.m_Safety0, bufferComponentType.m_Safety1);
#else
            return new BufferAccessor<T>(buffer + startOffset, length, stride);
#endif
        }
    }

    [NativeContainer]
    public unsafe struct BufferAccessor<T>
        where T: struct, IBufferElementData
    {
        [NativeDisableUnsafePtrRestriction]
        private byte* m_BasePointer;
        private int m_Length;
        private int m_Stride;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private bool m_IsReadOnly;
#endif

        public int Length => m_Length;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private AtomicSafetyHandle m_Safety0;
        private AtomicSafetyHandle m_ArrayInvalidationSafety;

#pragma warning disable 0414 // assigned but its value is never used
        private int m_SafetyReadOnlyCount;
        private int m_SafetyReadWriteCount;
#pragma warning restore 0414

#endif

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        public BufferAccessor(byte* basePointer, int length, int stride, bool readOnly, AtomicSafetyHandle safety, AtomicSafetyHandle arrayInvalidationSafety)
        {
            m_BasePointer = basePointer;
            m_Length = length;
            m_Stride = stride;
            m_Safety0 = safety;
            m_ArrayInvalidationSafety = arrayInvalidationSafety;
            m_IsReadOnly = readOnly;
            m_SafetyReadOnlyCount = readOnly ? 2 : 0;
            m_SafetyReadWriteCount = readOnly ? 0 : 2;
        }
#else
        public BufferAccessor(byte* basePointer, int length, int stride)
        {
            m_BasePointer = basePointer;
            m_Length = length;
            m_Stride = stride;
        }
#endif

        public DynamicBuffer<T> this[int index]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety0);

                if (index < 0 || index >= Length)
                    throw new InvalidOperationException($"index {index} out of range in LowLevelBufferAccessor of length {Length}");
#endif
                BufferHeader* hdr = (BufferHeader*) (m_BasePointer + index * m_Stride);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                return new DynamicBuffer<T>(hdr, m_Safety0, m_ArrayInvalidationSafety, m_IsReadOnly);
#else
                return new DynamicBuffer<T>(hdr);
#endif
            }
        }
    }

    [BurstCompile]
    unsafe struct GatherChunks : IJobParallelFor
    {
        [ReadOnly] public NativeList<EntityArchetype> Archetypes;
        [ReadOnly] public NativeArray<int> Offsets;
        [NativeDisableParallelForRestriction]
        public NativeArray<ArchetypeChunk> Chunks;

        public void Execute(int index)
        {
            var archetype = Archetypes[index];
            var chunkCount = archetype.Archetype->ChunkCount;
            var chunk = (Chunk*) archetype.Archetype->ChunkList.Begin;
            var offset = Offsets[index];
            var dstChunksPtr = (Chunk**) Chunks.GetUnsafePtr();

            for (int j = 0; j < chunkCount; j++)
            {
                dstChunksPtr[offset + j] = chunk;
                chunk = (Chunk*)chunk->ChunkListNode.Next;
            }
        }
    }

    public unsafe struct ArchetypeChunkArray
    {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        static internal NativeArray<ArchetypeChunk> Create(NativeList<EntityArchetype> archetypes, Allocator allocator, AtomicSafetyHandle safetyHandle)
#else
        static internal NativeArray<ArchetypeChunk> Create(NativeList<EntityArchetype> archetypes, Allocator allocator)
#endif
        {
            int length = 0;
            var archetypeCount = archetypes.Length;
            var offsets = new NativeArray<int>(archetypeCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            for (var i = 0; i < archetypeCount; i++)
            {
                offsets[i] = length;
                length += archetypes[i].Archetype->ChunkCount;
            }

            var chunks = new NativeArray<ArchetypeChunk>(length, allocator, NativeArrayOptions.UninitializedMemory);
            var gatherChunksJob = new GatherChunks
            {
                Archetypes = archetypes,
                Offsets = offsets,
                Chunks = chunks
            };
            var gatherChunksJobHandle = gatherChunksJob.Schedule(archetypeCount,1);
            gatherChunksJobHandle.Complete();

            offsets.Dispose();
            return chunks;
        }

        static public int CalculateEntityCount(NativeArray<ArchetypeChunk> chunks)
        {
            int entityCount = 0;
            for (var i = 0; i < chunks.Length; i++)
            {
                entityCount += chunks[i].Count;
            }

            return entityCount;
        }
    }

    [NativeContainer]
    [NativeContainerSupportsMinMaxWriteRestriction]
    public struct ArchetypeChunkComponentType<T>
        where T : struct, IComponentData
    {
        internal readonly int m_TypeIndex;
        internal readonly uint m_GlobalSystemVersion;
        internal readonly bool m_IsReadOnly;
        internal readonly bool m_IsZeroSized;

        public int TypeIndex => m_TypeIndex;
        public uint GlobalSystemVersion => m_GlobalSystemVersion;
        public bool IsReadOnly => m_IsReadOnly;

#pragma warning disable 0414
        private readonly int m_Length;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private readonly int m_MinIndex;
        private readonly int m_MaxIndex;
        internal readonly AtomicSafetyHandle m_Safety;
#endif
#pragma warning restore 0414

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal ArchetypeChunkComponentType(AtomicSafetyHandle safety, bool isReadOnly, uint globalSystemVersion)
#else
        internal ArchetypeChunkComponentType(bool isReadOnly, uint globalSystemVersion)
#endif
        {
            m_Length = 1;
            m_TypeIndex = TypeManager.GetTypeIndex<T>();
            m_IsZeroSized = TypeManager.GetTypeInfo(m_TypeIndex).IsZeroSized;
            m_GlobalSystemVersion = globalSystemVersion;
            m_IsReadOnly = isReadOnly;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_MinIndex = 0;
            m_MaxIndex = 0;
            m_Safety = safety;
#endif
        }
    }

    [NativeContainer]
    [NativeContainerSupportsMinMaxWriteRestriction]
    public struct ArchetypeChunkBufferType<T>
        where T : struct, IBufferElementData
    {
        internal readonly int m_TypeIndex;
        internal readonly uint m_GlobalSystemVersion;
        internal readonly bool m_IsReadOnly;

        public uint GlobalSystemVersion => m_GlobalSystemVersion;
        public bool IsReadOnly => m_IsReadOnly;

#pragma warning disable 0414
        private readonly int m_Length;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private readonly int m_MinIndex;
        private readonly int m_MaxIndex;
        
        internal AtomicSafetyHandle m_Safety0;
        internal AtomicSafetyHandle m_Safety1;
        internal int m_SafetyReadOnlyCount;
        internal int m_SafetyReadWriteCount;
#endif
#pragma warning restore 0414

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal ArchetypeChunkBufferType(AtomicSafetyHandle safety, AtomicSafetyHandle arrayInvalidationSafety, bool isReadOnly, uint globalSystemVersion)
#else
        internal ArchetypeChunkBufferType (bool isReadOnly, uint globalSystemVersion)
#endif
        {
            m_Length = 1;
            m_TypeIndex = TypeManager.GetTypeIndex<T>();
            m_GlobalSystemVersion = globalSystemVersion;
            m_IsReadOnly = isReadOnly;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_MinIndex = 0;
            m_MaxIndex = 0;
            m_Safety0 = safety;
            m_Safety1 = arrayInvalidationSafety;
            m_SafetyReadOnlyCount = isReadOnly ? 2 : 0;
            m_SafetyReadWriteCount = isReadOnly ? 0 : 2;
#endif
        }
    }

    [NativeContainer]
    [NativeContainerSupportsMinMaxWriteRestriction]
    public struct ArchetypeChunkSharedComponentType<T>
        where T : struct, ISharedComponentData
    {
        internal readonly int m_TypeIndex;

#pragma warning disable 0414
        private readonly int m_Length;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private readonly int m_MinIndex;
        private readonly int m_MaxIndex;
        internal readonly AtomicSafetyHandle m_Safety;
#endif
#pragma warning restore 0414

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal ArchetypeChunkSharedComponentType(AtomicSafetyHandle safety)
#else
        internal unsafe ArchetypeChunkSharedComponentType(bool unused)
#endif
        {
            m_Length = 1;
            m_TypeIndex = TypeManager.GetTypeIndex<T>();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_MinIndex = 0;
            m_MaxIndex = 0;
            m_Safety = safety;
#endif
        }
    }

    [NativeContainer]
    [NativeContainerSupportsMinMaxWriteRestriction]
    public struct ArchetypeChunkEntityType
    {
#pragma warning disable 0414
        private readonly int m_Length;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private readonly int m_MinIndex;
        private readonly int m_MaxIndex;
        internal readonly AtomicSafetyHandle m_Safety;
#endif
#pragma warning restore 0414

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal ArchetypeChunkEntityType(AtomicSafetyHandle safety)
#else
        internal unsafe ArchetypeChunkEntityType(bool unused)
#endif
        {
            m_Length = 1;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_MinIndex = 0;
            m_MaxIndex = 0;
            m_Safety = safety;
#endif
        }
    }
}
