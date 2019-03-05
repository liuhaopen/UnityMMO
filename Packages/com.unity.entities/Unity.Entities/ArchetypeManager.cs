using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Profiling;
using UnityEngine.Profiling;

namespace Unity.Entities
{
    internal struct ComponentTypeInArchetype
    {
        public readonly int TypeIndex;
        public readonly int BufferCapacity;

        public bool IsBuffer => BufferCapacity >= 0;
        public bool IsSystemStateComponent => TypeManager.IsSystemStateComponent(TypeIndex);
        public bool IsSystemStateSharedComponent => TypeManager.IsSystemStateSharedComponent(TypeIndex);
        public bool IsSharedComponent => TypeManager.IsSharedComponent(TypeIndex);
        public bool IsZeroSized => TypeManager.GetTypeInfo(TypeIndex).IsZeroSized;
        public bool IsChunkComponent => (TypeIndex & TypeManager.ChunkComponentTypeFlag) != 0;

        public ComponentTypeInArchetype(ComponentType type)
        {
            TypeIndex = type.TypeIndex;
            BufferCapacity = type.BufferCapacity;
        }

        public static bool operator == (ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
        {
            return lhs.TypeIndex == rhs.TypeIndex && lhs.BufferCapacity == rhs.BufferCapacity;
        }

        public static bool operator != (ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
        {
            return lhs.TypeIndex != rhs.TypeIndex || lhs.BufferCapacity != rhs.BufferCapacity;
        }

        public static bool operator < (ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
        {
            return lhs.TypeIndex != rhs.TypeIndex
                ? lhs.TypeIndex < rhs.TypeIndex
                : lhs.BufferCapacity < rhs.BufferCapacity;
        }

        public static bool operator > (ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
        {
            return lhs.TypeIndex != rhs.TypeIndex
                ? lhs.TypeIndex > rhs.TypeIndex
                : lhs.BufferCapacity > rhs.BufferCapacity;
        }

        public static bool operator <= (ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
        {
            return !(lhs > rhs);
        }

        public static bool operator >= (ComponentTypeInArchetype lhs, ComponentTypeInArchetype rhs)
        {
            return !(lhs < rhs);
        }

        public static unsafe bool CompareArray(ComponentTypeInArchetype* type1, int typeCount1,
            ComponentTypeInArchetype* type2, int typeCount2)
        {
            if (typeCount1 != typeCount2)
                return false;
            for (var i = 0; i < typeCount1; ++i)
                if (type1[i] != type2[i])
                    return false;
            return true;
        }

        public ComponentType ToComponentType()
        {
            ComponentType type;
            type.BufferCapacity = BufferCapacity;
            type.TypeIndex = TypeIndex;
            type.AccessModeType = ComponentType.AccessMode.ReadWrite;
            return type;
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        public override string ToString()
        {
            return ToComponentType().ToString();
        }
#endif
        public override bool Equals(object obj)
        {
            if (obj is ComponentTypeInArchetype) return (ComponentTypeInArchetype) obj == this;

            return false;
        }

        public override int GetHashCode()
        {
            return (TypeIndex * 5819) ^ BufferCapacity;
        }
    }

    [Flags]
    internal enum ChunkFlags
    {
        None = 0,
        Locked = 1 << 0
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct Chunk
    {
        // offset of field in architecture with   // 64 / 32 bits
        // Chunk header START
        public Archetype* Archetype;              //  0 /  0

        // Array of shared component data indices used by this chunk (in SharedComponentDataManager array)
        public int* SharedComponentValueArray;    //  8 /  4

        public uint* ChangeVersion;               // 16 /  8
        public Entity metaChunkEntity;            // 24 / 16

        // This is meant as read-only.
        // ArchetypeManager.SetChunkCount should be used to change the count.
        public int Count;                         // 32 / 20
        public int Capacity;                      // 36 / 24

        // In hybrid mode, archetypes can contain non-ECS-type components which are managed objects.
        // In order to access them without a lot of overhead we conceptually store an Object[] in each chunk which contains the managed components.
        // The chunk does not really own the array though since we cannot store managed references in unmanaged memory,
        // so instead the ArchetypeManager has a list of Object[]s and the chunk just has an int to reference an Object[] by index in that list.
        public int ManagedArrayIndex;             // 40 / 28

        public int ListIndex;                     // 44 / 32
        public int ListWithEmptySlotsIndex;       // 48 / 36

        // Incrementing automatically for each chunk
        public uint SequenceNumber;               // 52 / 40

        // Special chunk behaviors
        public uint Flags;                        // 56 / 44

        public int Padding2;                      // 60 / 48
        // Chunk header END

        // Component data buffer
        // This is where the actual chunk data starts.
        // It's declared like this so we can skip the header part of the chunk and just get to the data.
        public fixed byte Buffer[4];              // 64 / 48 (must be multiple of 16)

        public const int kChunkSize = 16 * 1024 - 256; // allocate a bit less to allow for header overhead
        public const int kMaximumEntitiesPerChunk = kChunkSize / 8;

        public static int GetChunkBufferSize(int numComponents, int numSharedComponents)
        {
            var bufferSize = kChunkSize -
                             (sizeof(Chunk) - 4 + numSharedComponents * sizeof(int) + numComponents * sizeof(uint));
            return bufferSize;
        }

        public static int GetSharedComponentOffset(int numSharedComponents)
        {
            return kChunkSize - numSharedComponents * sizeof(int);
        }

        public static int GetChangedComponentOffset(int numComponents, int numSharedComponents)
        {
            return GetSharedComponentOffset(numSharedComponents) - numComponents * sizeof(uint);
        }

        public bool MatchesFilter(MatchingArchetypes* match, ref ComponentGroupFilter filter)
        {
            if ((filter.Type & FilterType.SharedComponent) != 0)
            {
                var sharedComponentsInChunk = SharedComponentValueArray;
                var filteredCount = filter.Shared.Count;

                fixed (int* indexInComponentGroupPtr = filter.Shared.IndexInComponentGroup, sharedComponentIndexPtr =
                    filter.Shared.SharedComponentIndex)
                {
                    for (var i = 0; i < filteredCount; ++i)
                    {
                        var indexInComponentGroup = indexInComponentGroupPtr[i];
                        var sharedComponentIndex = sharedComponentIndexPtr[i];
                        var componentIndexInArcheType = match->IndexInArchetype[indexInComponentGroup];
                        var componentIndexInChunk = match->Archetype->SharedComponentOffset[componentIndexInArcheType];
                        if (sharedComponentsInChunk[componentIndexInChunk] != sharedComponentIndex)
                            return false;
                    }
                }

                return true;
            }

            if ((filter.Type & FilterType.Changed) != 0)
            {
                var changedCount = filter.Changed.Count;

                var requiredVersion = filter.RequiredChangeVersion;
                fixed (int* indexInComponentGroupPtr = filter.Changed.IndexInComponentGroup)
                {
                    for (var i = 0; i < changedCount; ++i)
                    {
                        var indexInArchetype = match->IndexInArchetype[indexInComponentGroupPtr[i]];

                        var changeVersion = ChangeVersion[indexInArchetype];
                        if (ChangeVersionUtility.DidChange(changeVersion, requiredVersion))
                            return true;
                    }
                }

                return false;
            }

            return true;
        }

        public int GetSharedComponentIndex(MatchingArchetypes* match, int indexInComponentGroup)
        {
            var sharedComponentsInChunk = SharedComponentValueArray;

            var componentIndexInArcheType = match->IndexInArchetype[indexInComponentGroup];
            var componentIndexInChunk = match->Archetype->SharedComponentOffset[componentIndexInArcheType];
            return sharedComponentsInChunk[componentIndexInChunk];
        }

        /// <summary>
        /// Returns true if Chunk is Locked
        /// </summary>
        public bool Locked => (Flags & (uint) ChunkFlags.Locked) != 0;
    }

    [DebuggerTypeProxy(typeof(ChunkListDebugView))]
    internal unsafe struct ChunkList
    {
        public Chunk** p;
        public int Count;
        public int Capacity;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct Archetype
    {
        public ChunkList Chunks;
        public ChunkList ChunksWithEmptySlots;


        public ChunkListMap FreeChunksBySharedComponents;

        public int EntityCount;
        public int ChunkCapacity;
        public int BytesPerInstance;

        public ComponentTypeInArchetype* Types;
        public int TypesCount;

        // Index matches archetype types
        public int* Offsets;
        public int* SizeOfs;

        // TypesCount indices into Types/Offsets/SizeOfs in the order that the
        // components are laid out in memory.
        public int* TypeMemoryOrder;

        public int* ManagedArrayOffset;
        public int NumManagedArrays;

        public int* SharedComponentOffset;
        public int NumSharedComponents;

        public Archetype* InstantiableArchetype;
        public Archetype* SystemStateResidueArchetype;
        public Archetype* MetaChunkArchetype;

        public EntityDataManager* EntityDataManager;

        public EntityRemapUtility.EntityPatchInfo* ScalarEntityPatches;
        public int                                 ScalarEntityPatchCount;

        public EntityRemapUtility.BufferEntityPatchInfo* BufferEntityPatches;
        public int                                       BufferEntityPatchCount;

        public bool SystemStateCleanupComplete;
        public bool SystemStateCleanupNeeded;
        public bool Disabled;
        public bool Prefab;
        public bool HasChunkComponents;
        public bool HasChunkHeader;

        public override string ToString()
        {
            var info = "";
            for (var i = 0; i < TypesCount; i++)
            {
                var componentTypeInArchetype = Types[i];
                info += $"  - {componentTypeInArchetype}";
            }

            return info;
        }

        public ref UnsafePtrList ChunksUnsafePtrList
        {
            get { return ref *(UnsafePtrList*)UnsafeUtility.AddressOf(ref Chunks); }
        }

        public ref UnsafePtrList ChunksWithEmptySlotsUnsafePtrList
        {
            get { return ref *(UnsafePtrList*)UnsafeUtility.AddressOf(ref ChunksWithEmptySlots); }
        }

        public void AddToChunkList(Chunk *chunk)
        {
            chunk->ListIndex = Chunks.Count;
            ChunksUnsafePtrList.Add(chunk);
        }
        public void RemoveFromChunkList(Chunk *chunk)
        {
            ChunksUnsafePtrList.RemoveAtSwapBack(chunk->ListIndex, chunk);
            if (chunk->ListIndex < Chunks.Count)
            {
                var chunkThatMoved = Chunks.p[chunk->ListIndex];
                chunkThatMoved->ListIndex = chunk->ListIndex;
            }
        }
        public void AddToChunkListWithEmptySlots(Chunk *chunk)
        {
            chunk->ListWithEmptySlotsIndex = ChunksWithEmptySlots.Count;
            ChunksWithEmptySlotsUnsafePtrList.Add(chunk);
        }
        public void RemoveFromChunkListWithEmptySlots(Chunk *chunk)
        {
            ChunksWithEmptySlotsUnsafePtrList.RemoveAtSwapBack(chunk->ListWithEmptySlotsIndex, chunk);
            if (chunk->ListWithEmptySlotsIndex < ChunksWithEmptySlots.Count)
            {
                var chunkThatMoved = ChunksWithEmptySlots.p[chunk->ListWithEmptySlotsIndex];
                chunkThatMoved->ListWithEmptySlotsIndex = chunk->ListWithEmptySlotsIndex;
            }
        }
    }

    [DebuggerTypeProxy(typeof(ArchetypeListDebugView))]
    internal unsafe struct ArchetypeList
    {
        public Archetype** p;
        public int Count;
        public int Capacity;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe class ArchetypeManager : IDisposable
    {
        private ChunkAllocator m_ArchetypeChunkAllocator;

        private const int kMaximumEmptyChunksInPool = 16; // can't alloc forever

        internal ChunkList m_EmptyChunks;

        private readonly SharedComponentDataManager m_SharedComponentManager;
        private readonly EntityGroupManager m_groupManager;

        internal ArchetypeList m_Archetypes;

        private ManagedArrayStorage[] m_ManagedArrays = new ManagedArrayStorage[1];
        private ArchetypeListMap m_TypeLookup;

        private Archetype* m_entityOnlyArchetype;
        private Archetype* m_metaChunkRootArchetype;

        private static uint ms_SequenceNumber;

        private NativeHashMap<uint, IntPtr> m_ChunksBySequenceNumber;

        private EntityDataManager* m_Entities;

        public ref UnsafePtrList EmptyChunksUnsafePtrList
        {
            get { return ref *(UnsafePtrList*)UnsafeUtility.AddressOf(ref m_EmptyChunks); }
        }

        public ref UnsafePtrList ArchetypeUnsafePtrList
        {
            get { return ref *(UnsafePtrList*)UnsafeUtility.AddressOf(ref m_Archetypes); }
        }


        public ArchetypeManager(SharedComponentDataManager sharedComponentManager, EntityDataManager* entityDataManager, EntityGroupManager groupManager)
        {
            m_ArchetypeChunkAllocator = new ChunkAllocator();

            m_SharedComponentManager = sharedComponentManager;
            m_groupManager = groupManager;
            m_Entities = entityDataManager;
            m_TypeLookup = new ArchetypeListMap();
            m_TypeLookup.Init(16);
            m_ChunksBySequenceNumber = new NativeHashMap<uint, IntPtr>(4096, Allocator.Persistent);
            m_EmptyChunks = new ChunkList();
            m_Archetypes = new ArchetypeList();
#if UNITY_ASSERTIONS
            // Buffer should be 16 byte aligned to ensure component data layout itself can gurantee being aligned
            var offset = UnsafeUtility.GetFieldOffset(typeof(Chunk).GetField("Buffer"));
            Assert.IsTrue(offset % 16 == 0, "Chunk buffer must be 16 byte aligned");
#endif
        }

        public void Dispose()
        {
            // Move all chunks to become pooled chunks
            for (var i = m_Archetypes.Count - 1; i >= 0; --i)
            {
                var archetype = m_Archetypes.p[i];
                while (archetype->Chunks.Count != 0)
                {
                    var chunk = archetype->Chunks.p[archetype->Chunks.Count - 1];
                    SetChunkCount(chunk, 0);
                }
                Assert.AreEqual(archetype->Chunks.Count, 0);
                Assert.AreEqual(archetype->ChunksWithEmptySlots.Count, 0);
                Assert.IsTrue(archetype->FreeChunksBySharedComponents.IsEmpty);
                archetype->ChunksUnsafePtrList.Dispose();
                archetype->ChunksWithEmptySlotsUnsafePtrList.Dispose();
                archetype->FreeChunksBySharedComponents.Dispose();
            }

            ArchetypeUnsafePtrList.Dispose();

            // And all pooled chunks
            for (var i = 0; i != m_EmptyChunks.Count; ++i)
            {
                var chunk = m_EmptyChunks.p[i];
                UnsafeUtility.Free(chunk, Allocator.Persistent);
            }
            EmptyChunksUnsafePtrList.Dispose();

            m_ManagedArrays = null;
            m_TypeLookup.Dispose();
            m_ChunksBySequenceNumber.Dispose();
            m_ArchetypeChunkAllocator.Dispose();
        }

        internal void UnlockAllChunks(EntityDataManager* entities)
        {
            for (var i = m_Archetypes.Count - 1; i >= 0; --i)
            {
                var archetype = m_Archetypes.p[i];
                for (int j=0;j<archetype->Chunks.Count;j++)
                {
                    var chunk = archetype->Chunks.p[j];
                    if (chunk->Locked)
                        entities->UnlockChunk(chunk);
                }
            }
        }

        private void DeallocateManagedArrayStorage(int index)
        {
            Assert.IsTrue(m_ManagedArrays[index].ManagedArray != null);
            m_ManagedArrays[index].ManagedArray = null;
        }

        private int AllocateManagedArrayStorage(int length)
        {
            for (var i = 0; i < m_ManagedArrays.Length; i++)
                if (m_ManagedArrays[i].ManagedArray == null)
                {
                    m_ManagedArrays[i].ManagedArray = new object[length];
                    return i;
                }

            var oldLength = m_ManagedArrays.Length;
            Array.Resize(ref m_ManagedArrays, m_ManagedArrays.Length * 2);

            m_ManagedArrays[oldLength].ManagedArray = new object[length];

            return oldLength;
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public static void AssertArchetypeComponents(ComponentTypeInArchetype* types, int count)
        {
            if (count < 1)
                throw new ArgumentException($"Invalid component count");
            if (types[0].TypeIndex == 0)
                throw new ArgumentException($"Component type may not be null");
            if (types[0].TypeIndex != TypeManager.GetTypeIndex<Entity>())
                throw new ArgumentException($"The Entity ID must always be the first component");

            for (var i = 1; i < count; i++)
            {
                if (types[i - 1].TypeIndex == types[i].TypeIndex)
                    throw new ArgumentException(
                        $"It is not allowed to have two components of the same type on the same entity. ({types[i - 1]} and {types[i]})");
            }
        }

        public Archetype* GetExistingArchetype(ComponentTypeInArchetype* types, int count)
        {
            return m_TypeLookup.TryGet(types, count);
        }

        private Archetype* GetMetaChunkRootArchetype(EntityGroupManager groupManager)
        {
            if (m_metaChunkRootArchetype == null)
            {
                var types = stackalloc ComponentTypeInArchetype[2];
                types[0] = new ComponentTypeInArchetype(new ComponentType(typeof(Entity)));
                types[1] = new ComponentTypeInArchetype(new ComponentType(typeof(ChunkHeader)));

                m_metaChunkRootArchetype = GetOrCreateArchetypeInternal(types, 2, groupManager);
                m_metaChunkRootArchetype->InstantiableArchetype = m_entityOnlyArchetype;
                m_metaChunkRootArchetype->SystemStateResidueArchetype = m_metaChunkRootArchetype;
                m_metaChunkRootArchetype->MetaChunkArchetype = null;
            }
            return m_metaChunkRootArchetype;
        }

        internal Archetype* GetEntityOnlyArchetype(EntityGroupManager groupManager)
        {
            if (m_entityOnlyArchetype == null)
            {
                ComponentTypeInArchetype entityType = new ComponentTypeInArchetype(ComponentType.Create<Entity>());
                m_entityOnlyArchetype = GetOrCreateArchetypeInternal(&entityType, 1, groupManager);
                m_entityOnlyArchetype->InstantiableArchetype = m_entityOnlyArchetype;
                m_entityOnlyArchetype->SystemStateResidueArchetype = null;
                m_entityOnlyArchetype->MetaChunkArchetype = null;
            }
            return m_entityOnlyArchetype;
        }

        public Archetype* GetArchetypeWithAddedComponentType(Archetype* archetype, ComponentType addedComponentType, EntityGroupManager groupManager, int* indexInTypeArray = null)
        {
            var componentType = new ComponentTypeInArchetype(addedComponentType);
            ComponentTypeInArchetype* newTypes = stackalloc ComponentTypeInArchetype[archetype->TypesCount + 1];

            var t = 0;
            while (t < archetype->TypesCount && archetype->Types[t] < componentType)
            {
                newTypes[t] = archetype->Types[t];
                ++t;
            }

            if(indexInTypeArray != null)
                *indexInTypeArray = t;
            newTypes[t] = componentType;
            while (t < archetype->TypesCount)
            {
                newTypes[t + 1] = archetype->Types[t];
                ++t;
            }

            return GetOrCreateArchetype(newTypes,archetype->TypesCount + 1, groupManager);
        }

        public Archetype* GetArchetypeWithRemovedComponentType(Archetype* archetype, ComponentType addedComponentType, EntityGroupManager groupManager, int* indexInOldTypeArray = null)
        {
            var componentType = new ComponentTypeInArchetype(addedComponentType);
            ComponentTypeInArchetype* newTypes = stackalloc ComponentTypeInArchetype[archetype->TypesCount];

            var removedTypes = 0;
            for (var t = 0; t < archetype->TypesCount; ++t)
                if (archetype->Types[t].TypeIndex == componentType.TypeIndex)
                {
                    if(indexInOldTypeArray != null)
                        *indexInOldTypeArray = t;
                    ++removedTypes;
                }
                else
                    newTypes[t - removedTypes] = archetype->Types[t];

            return GetOrCreateArchetype(newTypes,archetype->TypesCount - removedTypes, groupManager);
        }

        public Archetype* GetOrCreateArchetype(ComponentTypeInArchetype* inTypes, int count, EntityGroupManager groupManager)
        {
            var srcArchetype = GetExistingArchetype(inTypes, count);
            if (srcArchetype != null)
                return srcArchetype;

            srcArchetype = CreateArchetypeInternal(inTypes, count, groupManager);

            ComponentTypeInArchetype* types = stackalloc ComponentTypeInArchetype[count + 1];
            UnsafeUtility.MemCpy(types, inTypes, sizeof(ComponentTypeInArchetype) * count);

            var removedTypes = 0;
            var prefabTypeIndex = TypeManager.GetTypeIndex<Prefab>();
            for (var t = 0; t < srcArchetype->TypesCount; ++t)
            {
                var type = srcArchetype->Types[t];
                var skip = type.IsSystemStateComponent || type.IsSystemStateSharedComponent || (type.TypeIndex == prefabTypeIndex);
                if (skip)
                    ++removedTypes;
                else
                    types[t - removedTypes] = srcArchetype->Types[t];
            }

            srcArchetype->InstantiableArchetype = srcArchetype;
            if (removedTypes > 0)
            {
                var instantiableArchetype = GetOrCreateArchetypeInternal(types, count-removedTypes, groupManager);

                srcArchetype->InstantiableArchetype = instantiableArchetype;
                instantiableArchetype->InstantiableArchetype = instantiableArchetype;
                instantiableArchetype->SystemStateResidueArchetype = null;
            }

            if (srcArchetype->SystemStateCleanupNeeded)
            {
                var cleanupEntityType = new ComponentTypeInArchetype(ComponentType.Create<CleanupEntity>());
                bool cleanupAdded = false;

                var newTypeCount = 1;
                for (var t = 1; t < srcArchetype->TypesCount; ++t)
                {
                    var type = srcArchetype->Types[t];

                    if (type.IsSystemStateComponent || type.IsSystemStateSharedComponent)
                    {
                        if (!cleanupAdded && (cleanupEntityType < srcArchetype->Types[t]))
                        {
                            types[newTypeCount++] = cleanupEntityType;
                            cleanupAdded = true;
                        }

                        types[newTypeCount++] = srcArchetype->Types[t];
                    }
                }

                if (!cleanupAdded)
                {
                    types[newTypeCount++] = cleanupEntityType;
                }

                var systemStateResidueArchetype = GetOrCreateArchetype(types, newTypeCount, groupManager);
                systemStateResidueArchetype->SystemStateResidueArchetype = systemStateResidueArchetype;
                systemStateResidueArchetype->InstantiableArchetype = GetEntityOnlyArchetype(groupManager);

                srcArchetype->SystemStateResidueArchetype = systemStateResidueArchetype;
            }

            if(count > 1)
            {
                var metaArchetypeTypes = stackalloc ComponentTypeInArchetype[count + 1];
                metaArchetypeTypes[0] = new ComponentTypeInArchetype(typeof(Entity));
                int metaArchetypeTypeCount = 1;
                for (int i = 1; i < count; ++i)
                {
                    var t = inTypes[i];
                    ComponentType typeToInsert;
                    if (inTypes[i].IsChunkComponent)
                    {
                        typeToInsert = new ComponentType
                        {
                            TypeIndex = t.TypeIndex & ~TypeManager.ChunkComponentTypeFlag, BufferCapacity = t.BufferCapacity
                        };
                        SortingUtilities.InsertSorted(metaArchetypeTypes, metaArchetypeTypeCount++, typeToInsert);
                    }
                }

                if (metaArchetypeTypeCount > 1)
                {
                    SortingUtilities.InsertSorted(metaArchetypeTypes, metaArchetypeTypeCount++, new ComponentType(typeof(ChunkHeader)));
                    srcArchetype->MetaChunkArchetype = GetOrCreateArchetype(metaArchetypeTypes, metaArchetypeTypeCount, groupManager);
                }
            }

            return srcArchetype;
        }

        private Archetype* GetOrCreateArchetypeInternal(ComponentTypeInArchetype* types, int count,
            EntityGroupManager groupManager)
        {
            var type = GetExistingArchetype(types, count);
            return type != null ? type : CreateArchetypeInternal(types, count, groupManager);
        }

        void ChunkAllocate<T>(void* pointer, int count = 1) where T : struct
        {
            void** pointerToPointer = (void**)pointer;
            *pointerToPointer =
                m_ArchetypeChunkAllocator.Allocate(UnsafeUtility.SizeOf<T>() * count, UnsafeUtility.AlignOf<T>());
        }

        private Archetype* CreateArchetypeInternal(ComponentTypeInArchetype* types, int count,
            EntityGroupManager groupManager)
        {
            AssertArchetypeComponents(types, count);

            // Compute how many IComponentData types store Entities and need to be patched.
            // Types can have more than one entity, which means that this count is not necessarily
            // the same as the type count.
            var scalarEntityPatchCount = 0;
            var bufferEntityPatchCount = 0;
            var NumManagedArrays = 0;
            var NumSharedComponents = 0;
            for (var i = 0; i < count; ++i)
            {
                var ct = TypeManager.GetTypeInfo(types[i].TypeIndex);
                switch (ct.Category)
                {
                    case TypeManager.TypeCategory.ISharedComponentData:
                        ++NumSharedComponents;
                        break;
                    case TypeManager.TypeCategory.Class:
                        ++NumManagedArrays;
                        break;
                }
                var entityOffsets = ct.EntityOffsets;
                if (entityOffsets == null)
                    continue;
                if (ct.BufferCapacity >= 0)
                    bufferEntityPatchCount += ct.EntityOffsetCount;
                else if (ct.SizeInChunk > 0)
                    scalarEntityPatchCount += ct.EntityOffsetCount;
            }

            Archetype* type = null;
            ChunkAllocate<Archetype>(&type);
            ChunkAllocate<ComponentTypeInArchetype>(&type->Types, count);
            ChunkAllocate<int>(&type->Offsets, count);
            ChunkAllocate<int>(&type->SizeOfs, count);
            ChunkAllocate<int>(&type->TypeMemoryOrder, count);
            ChunkAllocate<EntityRemapUtility.EntityPatchInfo>(&type->ScalarEntityPatches, scalarEntityPatchCount);
            ChunkAllocate<EntityRemapUtility.BufferEntityPatchInfo>(&type->BufferEntityPatches, bufferEntityPatchCount);
            type->ManagedArrayOffset = null;
            if (NumManagedArrays > 0)
                ChunkAllocate<int>(&type->ManagedArrayOffset, count);
            type->SharedComponentOffset = null;
            if (NumSharedComponents > 0)
                ChunkAllocate<int>(&type->SharedComponentOffset, count);

            type->TypesCount = count;
            UnsafeUtility.MemCpy(type->Types, types, sizeof(ComponentTypeInArchetype) * count);
            type->EntityCount = 0;
            type->Chunks = new ChunkList();
            type->ChunksWithEmptySlots = new ChunkList();
            type->MetaChunkArchetype = null;
            type->NumSharedComponents = 0;

            type->EntityDataManager = m_Entities;

            var disabledTypeIndex = TypeManager.GetTypeIndex<Disabled>();
            var prefabTypeIndex = TypeManager.GetTypeIndex<Prefab>();
            var chunkHeaderTypeIndex = TypeManager.GetTypeIndex<ChunkHeader>();
            type->Disabled = false;
            type->Prefab = false;
            type->HasChunkHeader = false;
            type->HasChunkComponents = false;
            for (var i = 0; i < count; ++i)
            {
                if (TypeManager.GetTypeInfo(types[i].TypeIndex).Category == TypeManager.TypeCategory.ISharedComponentData)
                    ++type->NumSharedComponents;
                if (types[i].TypeIndex == disabledTypeIndex)
                    type->Disabled = true;
                if (types[i].TypeIndex == prefabTypeIndex)
                    type->Prefab = true;
                if (types[i].TypeIndex == chunkHeaderTypeIndex)
                    type->HasChunkHeader = true;
                if (types[i].IsChunkComponent)
                    type->HasChunkComponents = true;
            }

            var chunkDataSize = Chunk.GetChunkBufferSize(type->TypesCount, type->NumSharedComponents);

            type->ScalarEntityPatchCount = scalarEntityPatchCount;
            type->BufferEntityPatchCount = bufferEntityPatchCount;

            type->BytesPerInstance = 0;

            for (var i = 0; i < count; ++i)
            {
                var cType = TypeManager.GetTypeInfo(types[i].TypeIndex);
                var sizeOf = cType.SizeInChunk; // Note that this includes internal capacity and header overhead for buffers.
                if (types[i].IsChunkComponent)
                {
                    sizeOf = 0;
                }
                type->SizeOfs[i] = sizeOf;

                type->BytesPerInstance += sizeOf;
            }

            type->ChunkCapacity = chunkDataSize / type->BytesPerInstance;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (type->BytesPerInstance > chunkDataSize)
                throw new ArgumentException(
                    $"Entity archetype component data is too large. The maximum component data is {chunkDataSize} but the component data is {type->BytesPerInstance}");

            Assert.IsTrue(Chunk.kMaximumEntitiesPerChunk >= type->ChunkCapacity);
#endif

            // For serialization a stable ordering of the components in the
            // chunk is desired. The type index is not stable, since it depends
            // on the order in which types are added to the TypeManager.
            // A permutation of the types ordered by a TypeManager-generated
            // memory ordering is used instead.
            var memoryOrderings = stackalloc UInt64[count];
            for (int i = 0; i < count; ++i)
                memoryOrderings[i] = TypeManager.GetTypeInfo(types[i].TypeIndex).MemoryOrdering;
            for (int i = 0; i < count; ++i)
            {
                int index = i;
                while (index > 1 && memoryOrderings[i] < memoryOrderings[type->TypeMemoryOrder[index - 1]])
                {
                    type->TypeMemoryOrder[index] = type->TypeMemoryOrder[index - 1];
                    --index;
                }
                type->TypeMemoryOrder[index] = i;
            }

            var usedBytes = 0;
            for (var i = 0; i < count; ++i)
            {
                var index = type->TypeMemoryOrder[i];
                var sizeOf = type->SizeOfs[index];

                type->Offsets[index] = usedBytes;

                usedBytes += sizeOf * type->ChunkCapacity;
            }

            type->NumManagedArrays = NumManagedArrays;
            if (type->NumManagedArrays > 0)
            {
                var mi = 0;
                for (var i = 0; i < count; ++i)
                {
                    var index = type->TypeMemoryOrder[i];
                    var cType = TypeManager.GetTypeInfo(types[index].TypeIndex);
                    if (cType.Category == TypeManager.TypeCategory.Class)
                        type->ManagedArrayOffset[index] = mi++;
                    else
                        type->ManagedArrayOffset[index] = -1;
                }
            }

            type->NumSharedComponents = NumSharedComponents;
            if (type->NumSharedComponents > 0)
            {
                var mi = 0;
                for (var i = 0; i < count; ++i)
                {
                    var index = type->TypeMemoryOrder[i];
                    var cType = TypeManager.GetTypeInfo(types[index].TypeIndex);
                    if (cType.Category == TypeManager.TypeCategory.ISharedComponentData)
                        type->SharedComponentOffset[index] = mi++;
                    else
                        type->SharedComponentOffset[index] = -1;
                }
            }

            // Fill in arrays of scalar and buffer entity patches
            var scalarPatchInfo = type->ScalarEntityPatches;
            var bufferPatchInfo = type->BufferEntityPatches;
            for (var i = 0; i != count; i++)
            {
                var ct = TypeManager.GetTypeInfo(types[i].TypeIndex);
                #if !UNITY_CSHARP_TINY
                    ulong handle = ~0UL;
                    var offsets = ct.EntityOffsets == null ? null : (TypeManager.EntityOffsetInfo*) UnsafeUtility.PinGCArrayAndGetDataAddress(ct.EntityOffsets, out handle);
                    var offsetCount = ct.EntityOffsetCount;
                #else
                    var offsets = ct.EntityOffsets;
                    var offsetCount = ct.EntityOffsetCount;
                #endif

                if (ct.BufferCapacity >= 0)
                {
                    bufferPatchInfo = EntityRemapUtility.AppendBufferEntityPatches(bufferPatchInfo, offsets, offsetCount, type->Offsets[i], type->SizeOfs[i], ct.ElementSize);
                }
                else if (ct.SizeInChunk > 0)
                {
                    scalarPatchInfo = EntityRemapUtility.AppendEntityPatches(scalarPatchInfo, offsets, offsetCount, type->Offsets[i], type->SizeOfs[i]);
                }

                #if !UNITY_CSHARP_TINY
                    if(offsets != null)
                        UnsafeUtility.ReleaseGCObject(handle);
                #endif
            }
            Assert.AreEqual(scalarPatchInfo - type->ScalarEntityPatches, scalarEntityPatchCount);

            type->ScalarEntityPatchCount = scalarEntityPatchCount;
            type->BufferEntityPatchCount = bufferEntityPatchCount;

            // Update the list of all created archetypes
            ArchetypeUnsafePtrList.Add(type);

            type->FreeChunksBySharedComponents = new ChunkListMap();
            type->FreeChunksBySharedComponents.Init(16);

            m_TypeLookup.Add(type);

            type->SystemStateCleanupComplete = ArchetypeSystemStateCleanupComplete(type);
            type->SystemStateCleanupNeeded = ArchetypeSystemStateCleanupNeeded(type);

            groupManager.AddArchetypeIfMatching(type);

            return type;
        }

        private bool ArchetypeSystemStateCleanupComplete(Archetype* archetype)
        {
            if (archetype->TypesCount == 2 && archetype->Types[1].TypeIndex == TypeManager.GetTypeIndex<CleanupEntity>()) return true;
            return false;
        }

        private bool ArchetypeSystemStateCleanupNeeded(Archetype* archetype)
        {
            for (var t = 1; t < archetype->TypesCount; ++t)
            {
                var type = archetype->Types[t];
                if (type.IsSystemStateComponent || type.IsSystemStateSharedComponent)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddExistingChunk(Chunk* chunk)
        {
            var archetype = chunk->Archetype;
            archetype->AddToChunkList(chunk);
            archetype->EntityCount += chunk->Count;
            for (var i = 0; i < archetype->NumSharedComponents; ++i)
                m_SharedComponentManager.AddReference(chunk->SharedComponentValueArray[i]);

            if (chunk->Count < chunk->Capacity)
                EmptySlotTrackingAddChunk(chunk);
        }

        public void ConstructChunk(Archetype* archetype, Chunk* chunk, int* sharedComponentDataIndices)
        {
            chunk->Archetype = archetype;

            chunk->Count = 0;
            chunk->Capacity = archetype->ChunkCapacity;
            chunk->SequenceNumber = ms_SequenceNumber++;
            chunk->metaChunkEntity = Entity.Null;

            var numSharedComponents = archetype->NumSharedComponents;
            var numTypes = archetype->TypesCount;
            var sharedComponentOffset = Chunk.GetSharedComponentOffset(numSharedComponents);
            var changeVersionOffset = Chunk.GetChangedComponentOffset(numTypes, numSharedComponents);

            chunk->SharedComponentValueArray = (int*) ((byte*) chunk + sharedComponentOffset);
            chunk->ChangeVersion = (uint*) ((byte*) chunk + changeVersionOffset);

	        archetype->AddToChunkList(chunk);

            Assert.IsTrue(archetype->Chunks.Count != 0);

            if (numSharedComponents > 0)
            {
                var sharedComponentValueArray = chunk->SharedComponentValueArray;
                UnsafeUtility.MemCpy(sharedComponentValueArray, sharedComponentDataIndices, archetype->NumSharedComponents*sizeof(int));

                for (var i = 0; i < archetype->NumSharedComponents; ++i)
                {
                    var sharedComponentIndex = sharedComponentValueArray[i];
                    m_SharedComponentManager.AddReference(sharedComponentIndex);
                }
            }

            // Chunk can't be locked at at construction time
            EmptySlotTrackingAddChunk(chunk);

            if (numSharedComponents == 0)
            {
                Assert.IsTrue(archetype->ChunksWithEmptySlots.Count != 0);
            }
            else
            {
                Assert.IsTrue(archetype->FreeChunksBySharedComponents.TryGet(sharedComponentDataIndices, archetype->NumSharedComponents) != null);
            }

            if (archetype->NumManagedArrays > 0)
                chunk->ManagedArrayIndex = AllocateManagedArrayStorage(archetype->NumManagedArrays * chunk->Capacity);
            else
                chunk->ManagedArrayIndex = -1;

            for (var i = 0; i < archetype->TypesCount; i++)
                chunk->ChangeVersion[i] = 0;

            chunk->Flags = 0;

            bool insertResult = m_ChunksBySequenceNumber.TryAdd(chunk->SequenceNumber, (IntPtr) chunk);
            Assert.IsTrue(insertResult);

            if (archetype->MetaChunkArchetype != null)
            {
                m_Entities->CreateEntities(this, archetype->MetaChunkArchetype, &chunk->metaChunkEntity, 1);
                var typeIndex = TypeManager.GetTypeIndex<ChunkHeader>();
                var systemVersion = m_Entities->GlobalSystemVersion;
                var chunkHeader = (ChunkHeader*)m_Entities->GetComponentDataWithTypeRW(chunk->metaChunkEntity, typeIndex, systemVersion);
                chunkHeader->chunk = chunk;
            }
        }

        public Chunk* GetCleanChunk(Archetype* archetype, int* sharedComponentDataIndices)
        {
            Chunk* newChunk;
            // Try empty chunk pool
            if (m_EmptyChunks.Count == 0)
            {
                // Allocate new chunk
                newChunk = (Chunk*)UnsafeUtility.Malloc(Chunk.kChunkSize, 64, Allocator.Persistent);
            }
            else
            {
                Assert.IsTrue(m_EmptyChunks.Count > 0);
                var back = m_EmptyChunks.Count - 1;
                newChunk = m_EmptyChunks.p[back];
                EmptyChunksUnsafePtrList.Resize(back);
            }

            ConstructChunk(archetype, newChunk, sharedComponentDataIndices);

            return newChunk;
        }

        public Chunk* GetChunkWithEmptySlots(Archetype* archetype, int* sharedComponentDataIndices)
        {
            if (archetype->NumSharedComponents == 0)
            {
                if (archetype->ChunksWithEmptySlots.Count != 0)
                {
                    var chunk = archetype->ChunksWithEmptySlots.p[0];
                    Assert.AreNotEqual(chunk->Count, chunk->Capacity);
                    return chunk;
                }
            }
            else
            {
                var chunk = archetype->FreeChunksBySharedComponents.TryGet(sharedComponentDataIndices,
                    archetype->NumSharedComponents);
                if (chunk != null)
                {
                    return chunk;
                }
            }

            return GetCleanChunk(archetype, sharedComponentDataIndices);
        }

        public int AllocateIntoChunk(Chunk* chunk)
        {
            int outIndex;
            var res = AllocateIntoChunk(chunk, 1, out outIndex);
            Assert.AreEqual(1, res);
            return outIndex;
        }

        public int AllocateIntoChunk(Chunk* chunk, int count, out int outIndex)
        {
            var allocatedCount = Math.Min(chunk->Capacity - chunk->Count, count);
            outIndex = chunk->Count;
            SetChunkCount(chunk, chunk->Count + allocatedCount);
            chunk->Archetype->EntityCount += allocatedCount;
            return allocatedCount;
        }

        /// <summary>
        /// Remove chunk from archetype tracking of chunks with available slots.
        /// - Does not check if chunk has space.
        /// - Does not check if chunk is locked.
        /// </summary>
        /// <param name="chunk"></param>
        internal static void EmptySlotTrackingRemoveChunk(Chunk* chunk)
        {
            if (chunk->Archetype->NumSharedComponents == 0)
                chunk->Archetype->RemoveFromChunkListWithEmptySlots(chunk);
            else
                chunk->Archetype->FreeChunksBySharedComponents.Remove(chunk);
        }

        /// <summary>
        /// Add chunk to archetype tracking of chunks with available slots.
        /// - Does not check if chunk has space.
        /// - Does not check if chunk is locked.
        /// </summary>
        /// <param name="chunk"></param>
        internal static void EmptySlotTrackingAddChunk(Chunk* chunk)
        {
            if (chunk->Archetype->NumSharedComponents == 0)
                chunk->Archetype->AddToChunkListWithEmptySlots(chunk);
            else
                chunk->Archetype->FreeChunksBySharedComponents.Add(chunk);
        }

        public void DestroyMetaChunkEntity(Entity entity)
        {
            m_Entities->RemoveComponent(entity, ComponentType.Create<ChunkHeader>(), this, m_SharedComponentManager, m_groupManager);
            EntityDataManager.TryRemoveEntityId(&entity, 1, m_Entities, this, m_SharedComponentManager);
        }

        public void SetChunkCount(Chunk* chunk, int newCount)
        {
            Assert.AreNotEqual(newCount, chunk->Count);
            Assert.IsFalse(chunk->Locked);

            var capacity = chunk->Capacity;

            // Chunk released to empty chunk pool
            if (newCount == 0)
            {
                m_ChunksBySequenceNumber.Remove(chunk->SequenceNumber);

                // Remove references to shared components
                if (chunk->Archetype->NumSharedComponents > 0)
                {
                    var sharedComponentValueArray = chunk->SharedComponentValueArray;

                    for (var i = 0; i < chunk->Archetype->NumSharedComponents; ++i)
                        m_SharedComponentManager.RemoveReference(sharedComponentValueArray[i]);
                }

                if (chunk->ManagedArrayIndex != -1)
                {
                    DeallocateManagedArrayStorage(chunk->ManagedArrayIndex);
                    chunk->ManagedArrayIndex = -1;
                }

                if (chunk->metaChunkEntity != Entity.Null)
                    DestroyMetaChunkEntity(chunk->metaChunkEntity);

                chunk->Archetype->RemoveFromChunkList(chunk);

                // this chunk is going away, so it shouldn't be in the empty slot list.
                if (chunk->Count < chunk->Capacity)
                  EmptySlotTrackingRemoveChunk(chunk);

                chunk->Archetype = null;
                if (m_EmptyChunks.Count == kMaximumEmptyChunksInPool)
                    UnsafeUtility.Free(chunk, Allocator.Persistent);
                else
                {
                    EmptyChunksUnsafePtrList.Add(chunk);
                    chunk->Count = newCount;
                }
                return;
            }
            // Chunk is now full
            else if (newCount == capacity)
            {
                // this chunk no longer has empty slots, so it shouldn't be in the empty slot list.
                EmptySlotTrackingRemoveChunk(chunk);
            }
            // Chunk is no longer full
            else if (chunk->Count == capacity)
            {
                Assert.IsTrue(newCount < chunk->Count);
                EmptySlotTrackingAddChunk(chunk);
            }

            chunk->Count = newCount;
        }

        public object GetManagedObject(Chunk* chunk, ComponentType type, int index)
        {
            var typeOfs = ChunkDataUtility.GetIndexInTypeArray(chunk->Archetype, type.TypeIndex);
            if (typeOfs < 0 || chunk->Archetype->ManagedArrayOffset[typeOfs] < 0)
                throw new InvalidOperationException("Trying to get managed object for non existing component");
            return GetManagedObject(chunk, typeOfs, index);
        }

        internal object GetManagedObject(Chunk* chunk, int type, int index)
        {
            var managedStart = chunk->Archetype->ManagedArrayOffset[type] * chunk->Capacity;
            return m_ManagedArrays[chunk->ManagedArrayIndex].ManagedArray[index + managedStart];
        }

        public object[] GetManagedObjectRange(Chunk* chunk, int type, out int rangeStart, out int rangeLength)
        {
            rangeStart = chunk->Archetype->ManagedArrayOffset[type] * chunk->Capacity;
            rangeLength = chunk->Count;
            return m_ManagedArrays[chunk->ManagedArrayIndex].ManagedArray;
        }

        public void SetManagedObject(Chunk* chunk, int type, int index, object val)
        {
            var managedStart = chunk->Archetype->ManagedArrayOffset[type] * chunk->Capacity;
            m_ManagedArrays[chunk->ManagedArrayIndex].ManagedArray[index + managedStart] = val;
        }

        public void SetManagedObject(Chunk* chunk, ComponentType type, int index, object val)
        {
            var typeOfs = ChunkDataUtility.GetIndexInTypeArray(chunk->Archetype, type.TypeIndex);
            if (typeOfs < 0 || chunk->Archetype->ManagedArrayOffset[typeOfs] < 0)
                throw new InvalidOperationException("Trying to set managed object for non existing component");
            SetManagedObject(chunk, typeOfs, index, val);
        }

        public int CountEntities()
        {
            int entityCount = 0;
            for (var i = m_Archetypes.Count - 1; i >= 0; --i)
            {
                var archetype = m_Archetypes.p[i];
                entityCount += archetype->EntityCount;
            }

            return entityCount;
        }

        [BurstCompile]
        struct MoveAllChunksJob : IJob
        {
            [NativeDisableUnsafePtrRestriction]
            public EntityDataManager* srcEntityDataManager;
            [NativeDisableUnsafePtrRestriction]
            public EntityDataManager* dstEntityDataManager;
            public NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping;

            public void Execute()
            {
                dstEntityDataManager->AllocateEntitiesForRemapping(srcEntityDataManager, ref entityRemapping);
                srcEntityDataManager->FreeAllEntities();
            }
        }

        struct RemapChunk
        {
            public Chunk* chunk;
            public Archetype* dstArchetype;
        }

        [BurstCompile]
        struct RemapChunksJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping;
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<RemapChunk> remapChunks;
            [ReadOnly] public NativeArray<int> remapShared;

            [NativeDisableUnsafePtrRestriction]
            public EntityDataManager* dstEntityDataManager;

            public void Execute(int index)
            {
                Chunk* chunk = remapChunks[index].chunk;
                Archetype* dstArchetype = remapChunks[index].dstArchetype;

                dstEntityDataManager->RemapChunk(dstArchetype, chunk, 0, chunk->Count, ref entityRemapping);
                EntityRemapUtility.PatchEntities(dstArchetype->ScalarEntityPatches + 1, dstArchetype->ScalarEntityPatchCount - 1, dstArchetype->BufferEntityPatches, dstArchetype->BufferEntityPatchCount, chunk->Buffer, chunk->Count, ref entityRemapping);
                chunk->Archetype = dstArchetype;
                chunk->ListIndex += dstArchetype->Chunks.Count;
                chunk->ListWithEmptySlotsIndex += dstArchetype->ChunksWithEmptySlots.Count;

                for (int i = 0; i < dstArchetype->NumSharedComponents; ++i)
                {
                    var componentIndex = chunk->SharedComponentValueArray[i];
                    componentIndex = remapShared[componentIndex];
                    chunk->SharedComponentValueArray[i] = componentIndex;
                }
            }
        }

        struct RemapArchetype
        {
            public Archetype* srcArchetype;
            public Archetype* dstArchetype;
        }

        [BurstCompile]
        struct RemapArchetypesJob : IJobParallelFor
        {
            [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<RemapArchetype> remapArchetypes;

            [NativeDisableUnsafePtrRestriction]
            public EntityDataManager* dstEntityDataManager;

            public int chunkHeaderType;

            // This must be run after chunks have been remapped since FreeChunksBySharedComponents needs the shared component
            // indices in the chunks to be remapped
            public void Execute(int index)
            {
                var srcArchetype = remapArchetypes[index].srcArchetype;
                var dstArchetype = remapArchetypes[index].dstArchetype;

                if (srcArchetype->HasChunkComponents)
                {
                    for (int i = 0; i < srcArchetype->Chunks.Count; ++i)
                    {
                        var version = dstEntityDataManager->GlobalSystemVersion;
                        var chunk = srcArchetype->Chunks.p[i];
                        var chunkHeader = (ChunkHeader*)dstEntityDataManager->GetComponentDataWithTypeRW(chunk->metaChunkEntity, chunkHeaderType, version);
                        chunkHeader->chunk = chunk;
                    }
                }

                dstArchetype->ChunksUnsafePtrList.Append(srcArchetype->ChunksUnsafePtrList);
                srcArchetype->ChunksUnsafePtrList.Resize(0);

                if (srcArchetype->NumSharedComponents == 0)
                {
                    if (srcArchetype->ChunksWithEmptySlots.Count != 0)
                    {
                        dstArchetype->ChunksWithEmptySlotsUnsafePtrList.Append(srcArchetype->ChunksWithEmptySlotsUnsafePtrList);
                        srcArchetype->ChunksWithEmptySlotsUnsafePtrList.Resize(0);
                    }
                }
                else
                {
                    dstArchetype->FreeChunksBySharedComponents.AppendFrom(ref srcArchetype->FreeChunksBySharedComponents);
                }

                dstArchetype->EntityCount += srcArchetype->EntityCount;
                srcArchetype->EntityCount = 0;
            }
        }

        public static void MoveChunks(EntityManager srcEntities, ArchetypeManager dstArchetypeManager,
            EntityGroupManager dstGroupManager, EntityDataManager* dstEntityDataManager,
            SharedComponentDataManager dstSharedComponents)
        {
            var entityRemapping = new NativeArray<EntityRemapUtility.EntityRemapInfo>(srcEntities.Entities->Capacity, Allocator.TempJob);
            MoveChunks(srcEntities, dstArchetypeManager, dstGroupManager, dstEntityDataManager, dstSharedComponents, entityRemapping);
            entityRemapping.Dispose();
        }

        static readonly ProfilerMarker k_ProfileMoveSharedComponents = new ProfilerMarker("MoveSharedComponents");

        public static void MoveChunks(EntityManager srcEntities, ArchetypeManager dstArchetypeManager,
            EntityGroupManager dstGroupManager, EntityDataManager* dstEntityDataManager,
            SharedComponentDataManager dstSharedComponents, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
            var srcArchetypeManager = srcEntities.ArchetypeManager;
            var srcEntityDataManager = srcEntities.Entities;
            var srcSharedComponents = srcEntities.m_SharedComponentManager;

            var moveChunksJob = new MoveAllChunksJob
            {
                srcEntityDataManager = srcEntityDataManager,
                dstEntityDataManager = dstEntityDataManager,
                entityRemapping = entityRemapping
            }.Schedule();

            JobHandle.ScheduleBatchedJobs();

            int chunkCount = 0;
            for (var i = srcArchetypeManager.m_Archetypes.Count - 1; i >= 0; --i)
            {
                var srcArchetype = srcArchetypeManager.m_Archetypes.p[i];
                chunkCount += srcArchetype->Chunks.Count;
            }

            var remapChunks = new NativeArray<RemapChunk>(chunkCount, Allocator.TempJob);
            var remapArchetypes = new NativeArray<RemapArchetype>(srcArchetypeManager.m_Archetypes.Count, Allocator.TempJob);

            int chunkIndex = 0;
            int archetypeIndex = 0;
            for (var i = srcArchetypeManager.m_Archetypes.Count - 1; i >= 0; --i)
            {
                var srcArchetype = srcArchetypeManager.m_Archetypes.p[i];
                if (srcArchetype->Chunks.Count != 0)
                {
                    if (srcArchetype->NumManagedArrays != 0)
                        throw new ArgumentException("MoveEntitiesFrom is not supported with managed arrays");

                    var dstArchetype = dstArchetypeManager.GetOrCreateArchetype(srcArchetype->Types, srcArchetype->TypesCount, dstGroupManager);

                    // things that we can't do in the job, because they may allocate memory
                    dstArchetype->ChunksUnsafePtrList.SetCapacity(srcArchetype->Chunks.Count + dstArchetype->Chunks.Count);
                    dstArchetype->ChunksWithEmptySlotsUnsafePtrList.SetCapacity(srcArchetype->ChunksWithEmptySlots.Count + dstArchetype->ChunksWithEmptySlots.Count);
                    dstArchetype->FreeChunksBySharedComponents.SetCapacity(srcArchetype->FreeChunksBySharedComponents.Size + dstArchetype->FreeChunksBySharedComponents.Size);

                    remapArchetypes[archetypeIndex] = new RemapArchetype {srcArchetype = srcArchetype, dstArchetype = dstArchetype};

                    for (var j = 0; j < srcArchetype->Chunks.Count; ++j)
                    {
                        var srcChunk = srcArchetype->Chunks.p[j];
                        remapChunks[chunkIndex] = new RemapChunk { chunk = srcChunk, dstArchetype = dstArchetype };
                        chunkIndex++;
                    }

                    archetypeIndex++;

                    dstEntityDataManager->IncrementComponentTypeOrderVersion(dstArchetype);
                }
            }

            moveChunksJob.Complete();

            k_ProfileMoveSharedComponents.Begin();
            var remapShared = dstSharedComponents.MoveAllSharedComponents(srcSharedComponents, entityRemapping, Allocator.TempJob);
            k_ProfileMoveSharedComponents.End();

            var remapChunksJob = new RemapChunksJob
            {
                dstEntityDataManager = dstEntityDataManager,
                remapChunks = remapChunks,
                remapShared = remapShared,
                entityRemapping = entityRemapping
            }.Schedule(remapChunks.Length, 1);

            var remapArchetypesJob = new RemapArchetypesJob
            {
                remapArchetypes = remapArchetypes,
                dstEntityDataManager = dstEntityDataManager,
                chunkHeaderType = TypeManager.GetTypeIndex<ChunkHeader>()
            }.Schedule(archetypeIndex, 1, remapChunksJob);

            remapArchetypesJob.Complete();
            remapShared.Dispose();
        }

        [BurstCompile]
        struct MoveChunksJob : IJob
        {
            [NativeDisableUnsafePtrRestriction]
            public EntityDataManager* srcEntityDataManager;
            [NativeDisableUnsafePtrRestriction]
            public EntityDataManager* dstEntityDataManager;
            public NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping;
            [ReadOnly] public NativeArray<ArchetypeChunk> chunks;

            public void Execute()
            {
                int chunkCount = chunks.Length;
                for (int i = 0; i < chunkCount; ++i)
                {
                    var chunk = chunks[i].m_Chunk;
                    dstEntityDataManager->AllocateEntitiesForRemapping(chunk, ref entityRemapping);
                    srcEntityDataManager->FreeEntities(chunk);
                }
            }
        }

        [BurstCompile]
        struct RemoveChunksFromArchetypeJob : IJob
        {
            [ReadOnly] public NativeArray<ArchetypeChunk> chunks;

            public void Execute()
            {
                int chunkCount = chunks.Length;
                for (int i = 0; i < chunkCount; ++i)
                {
                    var chunk = chunks[i].m_Chunk;
                    var archetype = chunk->Archetype;

                    if (chunk->Count < chunk->Capacity)
                        EmptySlotTrackingRemoveChunk(chunk);

                    archetype->RemoveFromChunkList(chunk);
                    archetype->EntityCount -= chunk->Count;
                }
            }
        }

        [BurstCompile]
        struct AddChunksToArchetypeJob : IJob
        {
            [ReadOnly] public NativeArray<ArchetypeChunk> chunks;

            public void Execute()
            {
                int chunkCount = chunks.Length;
                for (int i = 0; i < chunkCount; ++i)
                {
                    var chunk = chunks[i].m_Chunk;
                    var archetype = chunk->Archetype;
                    archetype->AddToChunkList(chunk);
                    archetype->EntityCount += chunk->Count;

                    if (chunk->Count < chunk->Capacity)
                        EmptySlotTrackingAddChunk(chunk);
                }
            }
        }


        public static void MoveChunks(EntityManager srcEntities, NativeArray<ArchetypeChunk> chunks, ArchetypeManager dstArchetypeManager,
            EntityGroupManager dstGroupManager, EntityDataManager* dstEntityDataManager, SharedComponentDataManager dstSharedComponents,
            NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
            var srcArchetypeManager = srcEntities.ArchetypeManager;
            var srcEntityDataManager = srcEntities.Entities;
            var srcSharedComponents = srcEntities.m_SharedComponentManager;

            var moveChunksJob = new MoveChunksJob
            {
                srcEntityDataManager = srcEntityDataManager,
                dstEntityDataManager = dstEntityDataManager,
                entityRemapping = entityRemapping,
                chunks = chunks
            }.Schedule();
            JobHandle.ScheduleBatchedJobs();

            int chunkCount = chunks.Length;
            var remapChunks = new NativeArray<RemapChunk>(chunkCount, Allocator.TempJob);
            for (int i = 0; i < chunkCount; ++i)
            {
                var chunk = chunks[i].m_Chunk;
                var archetype = chunk->Archetype;

                //TODO: this should not be done more than once for each archetype
                var dstArchetype = dstArchetypeManager.GetOrCreateArchetype(archetype->Types, archetype->TypesCount, dstGroupManager);

                remapChunks[i] = new RemapChunk { chunk = chunk, dstArchetype = dstArchetype };
            }

            var removeChunksFromArchetypeJob = new RemoveChunksFromArchetypeJob
            {
                chunks = chunks
            }.Schedule(moveChunksJob);


            moveChunksJob.Complete();

            k_ProfileMoveSharedComponents.Begin();
            var remapShared = dstSharedComponents.MoveSharedComponents(srcSharedComponents, chunks, entityRemapping, Allocator.TempJob);
            k_ProfileMoveSharedComponents.End();

            var remapChunksJob = new RemapChunksJob
            {
                dstEntityDataManager = dstEntityDataManager,
                remapChunks = remapChunks,
                remapShared = remapShared,
                entityRemapping = entityRemapping
            }.Schedule(remapChunks.Length, 1, removeChunksFromArchetypeJob);

            var addChunksToArchetypeJob = new AddChunksToArchetypeJob
            {
                chunks = chunks
            }.Schedule(remapChunksJob);

            addChunksToArchetypeJob.Complete();

            remapShared.Dispose();
        }

        internal Chunk* CloneChunkForDiffing(Chunk* chunk, EntityDataManager* entities, EntityGroupManager groupManager, SharedComponentDataManager srcSharedManager)
        {
            int* sharedIndices = stackalloc int[chunk->Archetype->NumSharedComponents];

            for (int i = 0; i < chunk->Archetype->NumSharedComponents; ++i)
            {
                sharedIndices[i] = chunk->SharedComponentValueArray[i];
            }

            m_SharedComponentManager.CopySharedComponents(srcSharedManager, sharedIndices, chunk->Archetype->NumSharedComponents);

            // Allocate a new chunk
            Archetype* arch = GetOrCreateArchetype(chunk->Archetype->Types, chunk->Archetype->TypesCount, groupManager);

            Chunk* targetChunk = GetCleanChunk(arch, sharedIndices);

            // GetCleanChunk & CopySharedComponents both acquire a ref, once chunk owns, release CopySharedComponents ref
            for (int i = 0; i < chunk->Archetype->NumSharedComponents; ++i)
                m_SharedComponentManager.RemoveReference(sharedIndices[i]);

            UnityEngine.Assertions.Assert.AreEqual(0, targetChunk->Count);
            UnityEngine.Assertions.Assert.IsTrue(targetChunk->Capacity >= chunk->Count);

            int copySize = Chunk.GetChunkBufferSize(arch->TypesCount, arch->NumSharedComponents);
            UnsafeUtility.MemCpy(targetChunk->Buffer, chunk->Buffer, copySize);

            SetChunkCount(targetChunk, chunk->Count);
            targetChunk->Archetype->EntityCount += chunk->Count;

            BufferHeader.PatchAfterCloningChunkForDiff(targetChunk);

            var tempEntities = new NativeArray<Entity>(targetChunk->Count, Allocator.Temp);
            entities->AllocateEntities(targetChunk->Archetype, targetChunk, 0, targetChunk->Count, (Entity*)tempEntities.GetUnsafePtr());
            tempEntities.Dispose();

            return targetChunk;
        }

        internal void DestroyChunkForDiffing(Chunk* chunk, EntityDataManager* entities)
        {
            chunk->Archetype->EntityCount -= chunk->Count;
            entities->FreeEntities(chunk);
            SetChunkCount(chunk, 0);
        }

        public int CheckInternalConsistency()
        {
            var totalCount = 0;
            for(var i = m_Archetypes.Count - 1; i >= 0; --i)
            {
                var archetype = m_Archetypes.p[i];
                var countInArchetype = 0;
                for (var j = 0; j < archetype->Chunks.Count; ++j)
                {
                    var chunk = archetype->Chunks.p[j];
                    Assert.IsTrue(chunk->Archetype == archetype);
                    Assert.IsTrue(chunk->Capacity >= chunk->Count);

                    if (!chunk->Locked)
                    {
                        if (chunk->Count < chunk->Capacity)
                            if (archetype->NumSharedComponents == 0)
                            {
                                Assert.IsTrue(chunk->ListWithEmptySlotsIndex >= 0 && chunk->ListWithEmptySlotsIndex < archetype->ChunksWithEmptySlots.Count);
                                Assert.IsTrue(chunk == archetype->ChunksWithEmptySlots.p[chunk->ListWithEmptySlotsIndex]);
                            }
                            else
                                Assert.IsTrue(archetype->FreeChunksBySharedComponents.Contains(chunk));
                    }
                    countInArchetype += chunk->Count;
                }

                Assert.AreEqual(countInArchetype, archetype->EntityCount);

                totalCount += countInArchetype;
            }

            return totalCount;
        }

        internal SharedComponentDataManager GetSharedComponentDataManager()
        {
            return m_SharedComponentManager;
        }

        private struct ManagedArrayStorage
        {
            public object[] ManagedArray;
        }

        internal Chunk* GetChunkBySequenceNumber(uint seqno)
        {
            IntPtr result = default(IntPtr);
            if (m_ChunksBySequenceNumber.TryGetValue(seqno, out result))
                return (Chunk*) result;
            else
                return null;
        }
    }
}
