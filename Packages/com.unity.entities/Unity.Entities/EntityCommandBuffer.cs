using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.Assertions;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine.Profiling;

namespace Unity.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BasicCommand
    {
        public int CommandType;
        public int TotalSize;
        public int SortIndex;  /// Used to order command execution during playback
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CreateCommand
    {
        public BasicCommand Header;
        public EntityArchetype Archetype;
        public int IdentityIndex;
        public int BatchCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct EntityCommand
    {
        public BasicCommand Header;
        public Entity Entity;
        public int IdentityIndex;
        public int BatchCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct EntityComponentCommand
    {
        public EntityCommand Header;
        public int ComponentTypeIndex;

        public int ComponentSize;
        // Data follows if command has an associated component payload
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct EntityBufferCommand
    {
        public EntityCommand Header;
        public int ComponentTypeIndex;

        public int ComponentSize;
        public BufferHeader TempBuffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct EntitySharedComponentCommand
    {
        public EntityCommand Header;
        public int ComponentTypeIndex;
        public int HashCode;
        public GCHandle BoxedObject;
        public EntitySharedComponentCommand* Prev;

        internal object GetBoxedObject()
        {
            if (BoxedObject.IsAllocated)
                return BoxedObject.Target;
            return null;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = (64 > JobsUtility.CacheLineSize) ? 64 : JobsUtility.CacheLineSize)]
    internal unsafe struct EntityCommandBufferChain
    {
        public ECBChunk* m_Tail;
        public ECBChunk* m_Head;
        public EntitySharedComponentCommand* m_CleanupList;
        public CreateCommand*                m_PrevCreateCommand;
        public EntityCommand*                m_PrevEntityCommand;
        public EntityCommandBufferChain* m_NextChain;
        public int m_LastSortIndex;
    }

    internal unsafe struct ECBSharedPlaybackState
    {
        public Entity* CreateEntityBatch;
    }

    internal unsafe struct ECBChainPlaybackState
    {
        public ECBChunk* Chunk;
        public int Offset;
        public int NextSortIndex;
    }

    internal unsafe struct ECBChainHeapElement
    {
        public int SortIndex;
        public int ChainIndex;
    }
    internal unsafe struct ECBChainPriorityQueue : IDisposable
    {
        private readonly ECBChainHeapElement* m_Heap;
        private int m_Size;
        private readonly Allocator m_Allocator;
        private static readonly int BaseIndex = 1;
        public ECBChainPriorityQueue(NativeArray<ECBChainPlaybackState> chainStates, Allocator alloc)
        {
            m_Size = chainStates.Length;
            m_Allocator = alloc;
            m_Heap = (ECBChainHeapElement*)UnsafeUtility.Malloc((m_Size + BaseIndex) * sizeof(ECBChainHeapElement), 64, m_Allocator);
            for (int i = m_Size - 1; i >= m_Size / 2; --i)
            {
                m_Heap[BaseIndex + i].SortIndex = chainStates[i].NextSortIndex;
                m_Heap[BaseIndex + i].ChainIndex = i;
            }
            for (int i = m_Size/2 - 1; i >= 0; --i)
            {
                m_Heap[BaseIndex + i].SortIndex = chainStates[i].NextSortIndex;
                m_Heap[BaseIndex + i].ChainIndex = i;
                Heapify(BaseIndex + i);
            }
        }
        public void Dispose()
        {
            UnsafeUtility.Free(m_Heap, m_Allocator);
        }
        public bool Empty { get { return m_Size <= 0; } }
        public ECBChainHeapElement Peek()
        {
            //Assert.IsTrue(!Empty, "Can't Peek() an empty heap");
            if (Empty)
            {
                return new ECBChainHeapElement{ ChainIndex = -1, SortIndex = -1};
            }
            return m_Heap[BaseIndex];
        }
        public ECBChainHeapElement Pop()
        {
            //Assert.IsTrue(!Empty, "Can't Pop() an empty heap");
            if (Empty)
            {
                return new ECBChainHeapElement{ ChainIndex = -1, SortIndex = -1};
            }
            ECBChainHeapElement top = Peek();
            m_Heap[BaseIndex] = m_Heap[m_Size--];
            if (!Empty)
            {
                Heapify(BaseIndex);
            }
            return top;
        }
        public void ReplaceTop(ECBChainHeapElement value)
        {
            Assert.IsTrue(!Empty, "Can't ReplaceTop() an empty heap");
            m_Heap[BaseIndex] = value;
            Heapify(BaseIndex);
        }
        private void Heapify(int i)
        {
            // The index taken by this function is expected to be already biased by BaseIndex.
            // Thus, m_Heap[size] is a valid element (specifically, the final element in the heap)
            Assert.IsTrue(i >= BaseIndex && i <= m_Size, "heap index " + i + " is out of range with size=" + m_Size);
            ECBChainHeapElement val = m_Heap[i];
            while (i <= m_Size / 2)
            {
                int child = 2 * i;
                if (child < m_Size && (m_Heap[child+1].SortIndex < m_Heap[child].SortIndex))
                {
                    child++;
                }
                if (val.SortIndex < m_Heap[child].SortIndex)
                {
                    break;
                }
                m_Heap[i] = m_Heap[child];
                i = child;
            }
            m_Heap[i] = val;
        }
    }

    internal enum ECBCommand
    {
        InstantiateEntity,

        CreateEntity,
        DestroyEntity,

        AddComponent,
        AddComponentWithEntityFixUp,
        RemoveComponent,
        SetComponent,
        SetComponentWithEntityFixUp,

        AddBuffer,
        SetBuffer,

        AddSharedComponentData,
        SetSharedComponentData
    }

    /// <summary>
    /// Organized in memory like a single block with Chunk header followed by Size bytes of data.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct ECBChunk
    {
        internal int Used;
        internal int Size;
        internal ECBChunk* Next;
        internal ECBChunk* Prev;

        internal int Capacity => Size - Used;

        internal int Bump(int size)
        {
            var off = Used;
            Used += size;
            return off;
        }

        internal int BaseSortIndex
        {
            get
            {
                fixed (ECBChunk* pThis = &this)
                {
                    if (Used < sizeof(BasicCommand))
                    {
                        return -1;
                    }
                    var buf = (byte*)pThis + sizeof(ECBChunk);
                    var header = (BasicCommand*)(buf);
                    return header->SortIndex;
                }
            }
        }
    }

    internal unsafe struct EntityCommandBufferData
    {
        public EntityCommandBufferChain m_MainThreadChain;

        public EntityCommandBufferChain* m_ThreadedChains;

        public int m_RecordedChainCount;

        public int m_MinimumChunkSize;

        public Allocator m_Allocator;

        public bool m_ShouldPlayback;

        public Entity m_Entity;

        internal void InitConcurrentAccess()
        {
            if (m_ThreadedChains != null)
                return;

            // PERF: It's be great if we had a way to actually get the number of worst-case threads so we didn't have to allocate 128.
            int allocSize = sizeof(EntityCommandBufferChain) * JobsUtility.MaxJobThreadCount;

            m_ThreadedChains = (EntityCommandBufferChain*) UnsafeUtility.Malloc(allocSize, JobsUtility.CacheLineSize, m_Allocator);
            UnsafeUtility.MemClear(m_ThreadedChains, allocSize);
        }

        internal void DestroyConcurrentAccess()
        {
            if (m_ThreadedChains != null)
            {
                UnsafeUtility.Free(m_ThreadedChains, m_Allocator);
                m_ThreadedChains = null;
            }
        }

        internal void AddCreateCommand(EntityCommandBufferChain* chain, int jobIndex, ECBCommand op, int index, EntityArchetype archetype, bool batchable)
        {
            if (batchable &&
                chain->m_PrevCreateCommand != null &&
                chain->m_PrevCreateCommand->Archetype == archetype)
            {
                ++chain->m_PrevCreateCommand->BatchCount;
            }
            else
            {
                var cmd = (CreateCommand*) Reserve(chain, jobIndex, sizeof(CreateCommand));

                cmd->Header.CommandType = (int) op;
                cmd->Header.TotalSize = sizeof(CreateCommand);
                cmd->Header.SortIndex = chain->m_LastSortIndex;
                cmd->Archetype = archetype;
                cmd->IdentityIndex = index;
                cmd->BatchCount = 1;

                chain->m_PrevCreateCommand = cmd;
            }
        }

        internal void AddEntityCommand(EntityCommandBufferChain* chain, int jobIndex, ECBCommand op, int index, Entity e, bool batchable)
        {
            if (batchable &&
                chain->m_PrevEntityCommand != null &&
                chain->m_PrevEntityCommand->Entity == e)
            {
                ++chain->m_PrevEntityCommand->BatchCount;
            }
            else
            {
                var cmd = (EntityCommand*) Reserve(chain, jobIndex, sizeof(EntityCommand));

                cmd->Header.CommandType = (int) op;
                cmd->Header.TotalSize = sizeof(EntityCommand);
                cmd->Header.SortIndex = chain->m_LastSortIndex;
                cmd->Entity = e;
                cmd->IdentityIndex = index;
                cmd->BatchCount = 1;
                chain->m_PrevEntityCommand = cmd;
            }
        }

        internal bool RequiresEntityFixUp(byte* data, int typeIndex)
        {
            var componentInfo = TypeManager.GetTypeInfo(typeIndex);
            bool bRequiresFixUp = false;

            #if !UNITY_CSHARP_TINY
                if (componentInfo.EntityOffsets == null)
                {
                    return bRequiresFixUp;
                }

                ulong handle = ~0UL;
                var offsets = (TypeManager.EntityOffsetInfo*) UnsafeUtility.PinGCArrayAndGetDataAddress(componentInfo.EntityOffsets, out handle);
                var offsetCount = componentInfo.EntityOffsetCount;
            #else
                var offsets = componentInfo.EntityOffsets;
                var offsetCount = componentInfo.EntityOffsetCount;

                if (offsets == null) return bRequiresFixUp;
            #endif

            for (int i = 0; i < offsetCount; i++)
            {
                if (((Entity*) (data + offsets[i].Offset))->Index < 0)
                {
                    bRequiresFixUp = true;
                    break;
                }
            }

            #if !UNITY_CSHARP_TINY
                UnsafeUtility.ReleaseGCObject(handle);
            #endif

            return bRequiresFixUp;
        }

        internal void AddEntityComponentCommand<T>(EntityCommandBufferChain* chain, int jobIndex, ECBCommand op, Entity e, T component) where T : struct
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            // NOTE: This has to be sizeof not TypeManager.SizeInChunk since we use UnsafeUtility.CopyStructureToPtr
            //       even on zero size components.
            var typeSize = UnsafeUtility.SizeOf<T>();
            var sizeNeeded = Align(sizeof(EntityComponentCommand) + typeSize, 8);

            var cmd = (EntityComponentCommand*)Reserve(chain, jobIndex, sizeNeeded);

            cmd->Header.Header.CommandType = (int)op;
            cmd->Header.Header.TotalSize = sizeNeeded;
            cmd->Header.Header.SortIndex = chain->m_LastSortIndex;
            cmd->Header.Entity = e;
            cmd->ComponentTypeIndex = typeIndex;
            cmd->ComponentSize = typeSize;

            byte* data = (byte*) (cmd + 1);
            UnsafeUtility.CopyStructureToPtr(ref component, data);
            if (RequiresEntityFixUp(data, typeIndex))
            {
                if (op == ECBCommand.AddComponent)
                    cmd->Header.Header.CommandType = (int) ECBCommand.AddComponentWithEntityFixUp;
                else if (op == ECBCommand.SetComponent)
                    cmd->Header.Header.CommandType = (int) ECBCommand.SetComponentWithEntityFixUp;
            }
        }

        internal BufferHeader* AddEntityBufferCommand<T>(EntityCommandBufferChain* chain, int jobIndex, ECBCommand op, Entity e) where T : struct, IBufferElementData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            var type = TypeManager.GetTypeInfo<T>();
            var sizeNeeded = Align(sizeof(EntityBufferCommand) + type.SizeInChunk, 8);

            var data = (EntityBufferCommand*)Reserve(chain, jobIndex, sizeNeeded);

            data->Header.Header.CommandType = (int)op;
            data->Header.Header.TotalSize = sizeNeeded;
            data->Header.Header.SortIndex = chain->m_LastSortIndex;
            data->Header.Entity = e;
            data->ComponentTypeIndex = typeIndex;
            data->ComponentSize = type.SizeInChunk;

            BufferHeader.Initialize(&data->TempBuffer, type.BufferCapacity);

            return &data->TempBuffer;
        }

        internal static int Align(int size, int alignmentPowerOfTwo)
        {
            return (size + alignmentPowerOfTwo - 1) & ~(alignmentPowerOfTwo - 1);
        }

        internal void AddEntityComponentTypeCommand(EntityCommandBufferChain* chain, int jobIndex, ECBCommand op, Entity e, ComponentType t)
        {
            var sizeNeeded = Align(sizeof(EntityComponentCommand), 8);

            var data = (EntityComponentCommand*)Reserve(chain, jobIndex, sizeNeeded);

            data->Header.Header.CommandType = (int)op;
            data->Header.Header.TotalSize = sizeNeeded;
            data->Header.Header.SortIndex = chain->m_LastSortIndex;
            data->Header.Entity = e;
            data->ComponentTypeIndex = t.TypeIndex;
        }

        internal void AddEntitySharedComponentCommand<T>(EntityCommandBufferChain* chain, int jobIndex, ECBCommand op, Entity e, int hashCode, object boxedObject)
            where T : struct
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            var sizeNeeded = Align(sizeof(EntitySharedComponentCommand), 8);
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (TypeManager.GetTypeInfo<T>().EntityOffsets != null)
                throw new System.ArgumentException("EntityCommandBuffer.AddSharedComponentData does not support shared componenents with Entity fields.");
#endif
            
            var data = (EntitySharedComponentCommand*)Reserve(chain, jobIndex, sizeNeeded);

            data->Header.Header.CommandType = (int)op;
            data->Header.Header.TotalSize = sizeNeeded;
            data->Header.Header.SortIndex = chain->m_LastSortIndex;
            data->Header.Entity = e;
            data->ComponentTypeIndex = typeIndex;
            data->HashCode = hashCode;

            if (boxedObject != null)
            {
                data->BoxedObject = GCHandle.Alloc(boxedObject);
                // We need to store all GCHandles on a cleanup list so we can dispose them later, regardless of if playback occurs or not.
                data->Prev = chain->m_CleanupList;
                chain->m_CleanupList = data;
            }
            else
            {
                data->BoxedObject = new GCHandle();
            }
        }

        internal byte* Reserve(EntityCommandBufferChain* chain, int jobIndex, int size)
        {
            int newSortIndex = jobIndex;
            if (newSortIndex < chain->m_LastSortIndex)
            {
                EntityCommandBufferChain* archivedChain = (EntityCommandBufferChain*) UnsafeUtility.Malloc(sizeof(EntityCommandBufferChain), 8, m_Allocator);
                *archivedChain = *chain;
                UnsafeUtility.MemClear(chain, sizeof(EntityCommandBufferChain));
                chain->m_NextChain = archivedChain;
            }
            chain->m_LastSortIndex = newSortIndex;

            if (chain->m_Tail == null || chain->m_Tail->Capacity < size)
            {
                var chunkSize = math.max(m_MinimumChunkSize, size);

                var c = (ECBChunk*)UnsafeUtility.Malloc(sizeof(ECBChunk) + chunkSize, 16, m_Allocator);
                var prev = chain->m_Tail;
                c->Next = null;
                c->Prev = prev;
                c->Used = 0;
                c->Size = chunkSize;

                if (prev != null) prev->Next = c;

                if (chain->m_Head == null)
                {
                    chain->m_Head = c;
                    // This seems to be the best place to track the number of non-empty command buffer chunks
                    // during the recording process.
                    Interlocked.Increment(ref m_RecordedChainCount);
                }

                chain->m_Tail = c;
            }

            var offset = chain->m_Tail->Bump(size);
            var ptr = (byte*)chain->m_Tail + sizeof(ECBChunk) + offset;
            return ptr;
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        public DynamicBuffer<T> CreateBufferCommand<T>(ECBCommand commandType, EntityCommandBufferChain* chain, int jobIndex, Entity e, AtomicSafetyHandle bufferSafety, AtomicSafetyHandle arrayInvalidationSafety) where T : struct, IBufferElementData
#else
        public DynamicBuffer<T> CreateBufferCommand<T>(ECBCommand commandType, EntityCommandBufferChain* chain, int jobIndex, Entity e) where T : struct, IBufferElementData
#endif
        {
            BufferHeader* header = AddEntityBufferCommand<T>(chain, jobIndex, commandType, e);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var safety = bufferSafety;
            AtomicSafetyHandle.UseSecondaryVersion(ref safety);
            var arraySafety = arrayInvalidationSafety;
            return new DynamicBuffer<T>(header, safety, arraySafety, false);
#else
            return new DynamicBuffer<T>(header);
#endif
        }

    }

    /// <summary>
    ///     A thread-safe command buffer that can buffer commands that affect entities and components for later playback.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    [NativeContainer]
    public unsafe struct EntityCommandBuffer : IDisposable
    {
        /// <summary>
        ///     The minimum chunk size to allocate from the job allocator.
        /// </summary>
        /// We keep this relatively small as we don't want to overload the temp allocator in case people make a ton of command buffers.
        private const int kDefaultMinimumChunkSize = 4 * 1024;

        [NativeDisableUnsafePtrRestriction] private EntityCommandBufferData* m_Data;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private AtomicSafetyHandle m_Safety0;
        private AtomicSafetyHandle m_BufferSafety;
        private AtomicSafetyHandle m_ArrayInvalidationSafety;
        private int m_SafetyReadOnlyCount;
        private int m_SafetyReadWriteCount;

        [NativeSetClassTypeToNullOnSchedule] private DisposeSentinel m_DisposeSentinel;

        internal int SystemID;
#endif

        /// <summary>
        ///     Allows controlling the size of chunks allocated from the temp job allocator to back the command buffer.
        /// </summary>
        /// Larger sizes are more efficient, but create more waste in the allocator.
        public int MinimumChunkSize
        {
            get { return m_Data->m_MinimumChunkSize > 0 ? m_Data->m_MinimumChunkSize : kDefaultMinimumChunkSize; }
            set { m_Data->m_MinimumChunkSize = Math.Max(0, value); }
        }

        /// <summary>
        /// Controls whether this command buffer should play back.
        /// </summary>
        ///
        /// This property is normally true, but can be useful to prevent
        /// the buffer from playing back when the user code is not in control
        /// of the site of playback.
        ///
        /// For example, is a buffer has been acquired from a barrier and partially
        /// filled in with data, but it is discovered that the work should be aborted,
        /// this property can be set to false to prevent the buffer from playing back.
        public bool ShouldPlayback
        {
            get { return m_Data != null ? m_Data->m_ShouldPlayback : false; }
            set { if (m_Data != null) m_Data->m_ShouldPlayback = value; }
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void EnforceSingleThreadOwnership()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety0);
#endif
        }

        /// <summary>
        ///  Creates a new command buffer.
        /// </summary>
        /// <param name="label">Memory allocator to use for chunks and data</param>
        public EntityCommandBuffer(Allocator label)
        {
            m_Data = (EntityCommandBufferData*)UnsafeUtility.Malloc(sizeof(EntityCommandBufferData),
                UnsafeUtility.AlignOf<EntityCommandBufferData>(), label);
            m_Data->m_Allocator = label;
            m_Data->m_MinimumChunkSize = kDefaultMinimumChunkSize;
            m_Data->m_ShouldPlayback = true;

            m_Data->m_MainThreadChain.m_CleanupList = null;
            m_Data->m_MainThreadChain.m_Tail = null;
            m_Data->m_MainThreadChain.m_Head = null;
            m_Data->m_MainThreadChain.m_PrevCreateCommand = null;
            m_Data->m_MainThreadChain.m_PrevEntityCommand = null;
            m_Data->m_MainThreadChain.m_LastSortIndex = -1;
            m_Data->m_MainThreadChain.m_NextChain = null;

            m_Data->m_ThreadedChains = null;
            m_Data->m_RecordedChainCount = 0;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Create(out m_Safety0, out m_DisposeSentinel, 8, label);
            // Used for all buffers returned from the API, so we can invalidate them once Playback() has been called.
            m_BufferSafety = AtomicSafetyHandle.Create();
            // Used to invalidate array aliases to buffers
            m_ArrayInvalidationSafety = AtomicSafetyHandle.Create();

            m_SafetyReadOnlyCount = 0;
            m_SafetyReadWriteCount = 3;
            SystemID = 0;
#endif
            m_Data->m_Entity = new Entity();
        }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Dispose(ref m_Safety0, ref m_DisposeSentinel);
            AtomicSafetyHandle.Release(m_ArrayInvalidationSafety);
            AtomicSafetyHandle.Release(m_BufferSafety);
#endif

            if (m_Data != null)
            {
                FreeChain(&m_Data->m_MainThreadChain);

                if (m_Data->m_ThreadedChains != null)
                {
                    for (int i = 0; i < JobsUtility.MaxJobThreadCount; ++i)
                    {
                        FreeChain(&m_Data->m_ThreadedChains[i]);
                    }

                    m_Data->DestroyConcurrentAccess();
                }

                UnsafeUtility.Free(m_Data, m_Data->m_Allocator);
                m_Data = null;
            }
        }

        private void FreeChain(EntityCommandBufferChain* chain)
        {
            if (chain == null)
            {
                return;
            }
            var cleanup_list = chain->m_CleanupList;
            while (cleanup_list != null)
            {
                cleanup_list->BoxedObject.Free();
                cleanup_list = cleanup_list->Prev;
            }

            chain->m_CleanupList = null;

            while (chain->m_Tail != null)
            {
                var prev = chain->m_Tail->Prev;
                UnsafeUtility.Free(chain->m_Tail, m_Data->m_Allocator);
                chain->m_Tail = prev;
            }

            chain->m_Head = null;
            if (chain->m_NextChain != null)
            {
                FreeChain(chain->m_NextChain);
                UnsafeUtility.Free(chain->m_NextChain, m_Data->m_Allocator);
                chain->m_NextChain = null;
            }
        }

        private const int MainThreadJobIndex = Int32.MaxValue;
        private const bool kBatchableCommand = true;

        public Entity CreateEntity(EntityArchetype archetype = new EntityArchetype())
        {
            EnforceSingleThreadOwnership();
            int index = --m_Data->m_Entity.Index;
            m_Data->AddCreateCommand(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.CreateEntity,
                index, archetype, kBatchableCommand);
            return m_Data->m_Entity;
        }

        public Entity Instantiate(Entity e)
        {
            EnforceSingleThreadOwnership();
            int index = --m_Data->m_Entity.Index;
            m_Data->AddEntityCommand(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.InstantiateEntity,
                index, e, kBatchableCommand);
            return m_Data->m_Entity;
        }

        public void DestroyEntity(Entity e)
        {
            EnforceSingleThreadOwnership();
            m_Data->AddEntityCommand(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.DestroyEntity, 0, e, false);
        }

        public DynamicBuffer<T> AddBuffer<T>() where T : struct, IBufferElementData
        {
            return AddBuffer<T>(m_Data->m_Entity);
        }

        public DynamicBuffer<T> AddBuffer<T>(Entity e) where T : struct, IBufferElementData
        {
            EnforceSingleThreadOwnership();
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return m_Data->CreateBufferCommand<T>(ECBCommand.AddBuffer, &m_Data->m_MainThreadChain, MainThreadJobIndex, e, m_BufferSafety, m_ArrayInvalidationSafety);
#else
            return m_Data->CreateBufferCommand<T>(ECBCommand.AddBuffer, &m_Data->m_MainThreadChain, MainThreadJobIndex, e);
#endif
        }

        public DynamicBuffer<T> SetBuffer<T>() where T : struct, IBufferElementData
        {
            return SetBuffer<T>(m_Data->m_Entity);
        }

        public DynamicBuffer<T> SetBuffer<T>(Entity e) where T : struct, IBufferElementData
        {
            EnforceSingleThreadOwnership();
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return m_Data->CreateBufferCommand<T>(ECBCommand.SetBuffer, &m_Data->m_MainThreadChain, MainThreadJobIndex, e, m_BufferSafety, m_ArrayInvalidationSafety);
#else
            return m_Data->CreateBufferCommand<T>(ECBCommand.SetBuffer, &m_Data->m_MainThreadChain, MainThreadJobIndex, e);
#endif
        }

        public void AddComponent<T>(Entity e, T component) where T : struct, IComponentData
        {
            EnforceSingleThreadOwnership();
            m_Data->AddEntityComponentCommand(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.AddComponent, e, component);
        }

        public void AddComponent<T>(T component) where T : struct, IComponentData
        {
            AddComponent(m_Data->m_Entity, component);
        }

        public void SetComponent<T>(T component) where T : struct, IComponentData
        {
            SetComponent(m_Data->m_Entity, component);
        }

        public void SetComponent<T>(Entity e, T component) where T : struct, IComponentData
        {
            EnforceSingleThreadOwnership();
            m_Data->AddEntityComponentCommand(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.SetComponent, e, component);
        }

        public void RemoveComponent<T>(Entity e)
        {
            RemoveComponent(e, ComponentType.Create<T>());
        }

        public void RemoveComponent(Entity e, ComponentType componentType)
        {
            EnforceSingleThreadOwnership();
            m_Data->AddEntityComponentTypeCommand(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.RemoveComponent, e, componentType);
        }


        private static bool IsDefaultObject<T>(ref T component, out int hashCode) where T : struct, ISharedComponentData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            var defaultValue = default(T);

            #if !UNITY_CSHARP_TINY
                var typeInfo = TypeManager.GetTypeInfo(typeIndex).FastEqualityTypeInfo;
                hashCode = FastEquality.GetHashCode(ref component, typeInfo);
                return FastEquality.Equals(ref defaultValue, ref component, typeInfo);
            #else
                hashCode = TypeManager.GetHashCode(ref component);
                return TypeManager.Equals(ref defaultValue, ref component);
            #endif
        }

        public void AddSharedComponent<T>(T component) where T : struct, ISharedComponentData
        {
            AddSharedComponent(m_Data->m_Entity, component);
        }

        public void AddSharedComponent<T>(Entity e, T component) where T : struct, ISharedComponentData
        {
            EnforceSingleThreadOwnership();
            int hashCode;
            if (IsDefaultObject(ref component, out hashCode))
                m_Data->AddEntitySharedComponentCommand<T>(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.AddSharedComponentData, e, hashCode, null);
            else
                m_Data->AddEntitySharedComponentCommand<T>(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.AddSharedComponentData, e, hashCode, component);
        }

        public void SetSharedComponent<T>(T component) where T : struct, ISharedComponentData
        {
            SetSharedComponent(m_Data->m_Entity, component);
        }

        public void SetSharedComponent<T>(Entity e, T component) where T : struct, ISharedComponentData
        {
            EnforceSingleThreadOwnership();
            int hashCode;
            if (IsDefaultObject(ref component, out hashCode))
                m_Data->AddEntitySharedComponentCommand<T>(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.SetSharedComponentData, e, hashCode, null);
            else
                m_Data->AddEntitySharedComponentCommand<T>(&m_Data->m_MainThreadChain, MainThreadJobIndex, ECBCommand.SetSharedComponentData, e, hashCode, component);
        }

        /// <summary>
        /// Play back all recorded operations against an entity manager.
        /// </summary>
        /// <param name="mgr">The entity manager that will receive the operations</param>
        public void Playback(EntityManager mgr)
        {
            if (mgr == null)
                throw new NullReferenceException($"{nameof(mgr)} cannot be null");

            EnforceSingleThreadOwnership();

            if (!ShouldPlayback || m_Data == null)
                return;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_BufferSafety);
            AtomicSafetyHandle.CheckWriteAndBumpSecondaryVersion(m_ArrayInvalidationSafety);
#endif

            Profiler.BeginSample("EntityCommandBuffer.Playback");

            // Walk all chains (Main + Threaded) and build a NativeArray of PlaybackState objects.
            // Only chains with non-null Head pointers will be included.
            var chainStates = new NativeArray<ECBChainPlaybackState>(m_Data->m_RecordedChainCount, Allocator.Temp);
            using (chainStates)
            {
                int initialChainCount = 0;
                for (var chain = &m_Data->m_MainThreadChain; chain != null; chain = chain->m_NextChain)
                {
                    if (chain->m_Head != null)
                    {
#pragma warning disable 728
                        chainStates[initialChainCount++] = new ECBChainPlaybackState
                        {
                            Chunk = chain->m_Head,
                            Offset = 0,
                            NextSortIndex = chain->m_Head->BaseSortIndex
                        };
#pragma warning restore 728
                    }
                }
                if (m_Data->m_ThreadedChains != null)
                {
                    for (int i = 0; i < JobsUtility.MaxJobThreadCount; ++i)
                    {
                        for (var chain = &m_Data->m_ThreadedChains[i]; chain != null; chain = chain->m_NextChain)
                        {
                            if (chain->m_Head != null)
                            {
#pragma warning disable 728
                                chainStates[initialChainCount++] = new ECBChainPlaybackState
                                {
                                    Chunk = chain->m_Head,
                                    Offset = 0,
                                    NextSortIndex = chain->m_Head->BaseSortIndex
                                };
#pragma warning restore 728
                            }
                        }
                    }
                }
                Assert.AreEqual<int>(m_Data->m_RecordedChainCount, initialChainCount);

                // Play back the recorded commands in increasing sortIndex order
                const int kMaxEntitiesOnStack = 10000;
                int entityCount = -m_Data->m_Entity.Index;
                Entity* createEntitiesBatch = null;
                if (entityCount > kMaxEntitiesOnStack)
                {
                    createEntitiesBatch = (Entity*) UnsafeUtility.Malloc(
                        entityCount * sizeof(Entity), 16, Allocator.Temp);
                }
                else
                {
                    var stacke = stackalloc Entity[entityCount];
                    createEntitiesBatch = stacke;
                }

                ECBSharedPlaybackState playbackState = new ECBSharedPlaybackState
                {
                    CreateEntityBatch = createEntitiesBatch,
                };

                using (ECBChainPriorityQueue chainQueue = new ECBChainPriorityQueue(chainStates, Allocator.Temp))
                {
                    ECBChainHeapElement currentElem = chainQueue.Pop();
                    while (currentElem.ChainIndex != -1)
                    {
                        ECBChainHeapElement nextElem = chainQueue.Peek();
                        PlaybackChain(mgr, ref playbackState, chainStates, currentElem.ChainIndex, nextElem.ChainIndex);
                        if (chainStates[currentElem.ChainIndex].Chunk == null)
                        {
                            chainQueue.Pop(); // ignore return value; we already have it as nextElem
                        }
                        else
                        {
                            currentElem.SortIndex = chainStates[currentElem.ChainIndex].NextSortIndex;
                            chainQueue.ReplaceTop(currentElem);
                        }
                        currentElem = nextElem;
                    }
                }
                if (entityCount > kMaxEntitiesOnStack)
                {
                    UnsafeUtility.Free(createEntitiesBatch, Allocator.Temp);
                }
            }

            Profiler.EndSample();
        }

        private static unsafe Entity SelectEntity(Entity cmdEntity, ECBSharedPlaybackState playbackState)
        {
            Assert.IsTrue(cmdEntity != Entity.Null);
            if (cmdEntity.Index < 0)
            {
                int index = -cmdEntity.Index - 1;
                Entity e = *(playbackState.CreateEntityBatch + index);
                Assert.IsTrue(e.Version > 0);
                return e;
            }
            return cmdEntity;
        }

        private static void FixupComponentData(byte* data, int typeIndex, ECBSharedPlaybackState playbackState)
        {
            var componentTypeInfo = TypeManager.GetTypeInfo(typeIndex);
            Assert.IsTrue(componentTypeInfo.EntityOffsets != null);

            #if !UNITY_CSHARP_TINY
                ulong handle = ~0UL;
                var offsets = (TypeManager.EntityOffsetInfo*) UnsafeUtility.PinGCArrayAndGetDataAddress(componentTypeInfo.EntityOffsets, out handle);
                var offsetCount = componentTypeInfo.EntityOffsetCount;
            #else
                var offsets = componentTypeInfo.EntityOffsets;
                var offsetCount = componentTypeInfo.EntityOffsetCount;
            #endif

            for (int i=0; i < offsetCount; i++)
            {
                // Need fix ups
                Entity* e = (Entity*) (data + offsets[i].Offset);
                if (e->Index < 0)
                {
                    var index = -e->Index - 1;
                    Entity real = *(playbackState.CreateEntityBatch + index);
                    *e = real;
                }
            }

            #if !UNITY_CSHARP_TINY
                UnsafeUtility.ReleaseGCObject(handle);
            #endif
        }

        private static unsafe void SetCommandDataWithFixup(
                EntityManager mgr, EntityComponentCommand* cmd, Entity entity,
                ECBSharedPlaybackState playbackState)
        {
            byte* data = (byte*) mgr.GetComponentDataRawRW(entity, cmd->ComponentTypeIndex);
            UnsafeUtility.MemCpy(data, cmd + 1, cmd->ComponentSize);
            FixupComponentData(data, cmd->ComponentTypeIndex,
                playbackState);
        }

        private static unsafe void PlaybackChain(EntityManager mgr, ref ECBSharedPlaybackState playbackState, NativeArray<ECBChainPlaybackState> chainStates, int currentChain, int nextChain)
        {
            int nextChainSortIndex = (nextChain != -1) ? chainStates[nextChain].NextSortIndex : -1;

            var chunk = chainStates[currentChain].Chunk;
            Assert.IsTrue(chunk != null);
            var off = chainStates[currentChain].Offset;
            Assert.IsTrue(off >= 0 && off < chunk->Used);

            while (chunk != null)
            {
                var buf = (byte*)chunk + sizeof(ECBChunk);
                while (off < chunk->Used)
                {
                    var header = (BasicCommand*)(buf + off);
                    if (nextChain != -1 && header->SortIndex > nextChainSortIndex)
                    {
                        // early out because a different chain needs to playback
                        var state = chainStates[currentChain];
                        state.Chunk = chunk;
                        state.Offset = off;
                        state.NextSortIndex = header->SortIndex;
                        chainStates[currentChain] = state;
                        return;
                    }

                    switch ((ECBCommand)header->CommandType)
                    {
                        case ECBCommand.DestroyEntity:
                            {
                                var cmd = (EntityCommand*) header;
                                Entity entity = SelectEntity(cmd->Entity, playbackState);
                                mgr.DestroyEntity(entity);
                            }
                            break;

                        case ECBCommand.RemoveComponent:
                            {
                                var cmd = (EntityComponentCommand*)header;
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.RemoveComponent(entity, TypeManager.GetType(cmd->ComponentTypeIndex));
                            }
                            break;

                        case ECBCommand.CreateEntity:
                            {
                                var cmd = (CreateCommand*)header;
                                EntityArchetype at = cmd->Archetype;

                                if (!at.Valid)
                                {
                                    at = mgr.GetEntityOnlyArchetype();
                                }

                                int index = -cmd->IdentityIndex -1;
                                mgr.CreateEntityInternal(at, playbackState.CreateEntityBatch + index,
                                    cmd->BatchCount);
                            }
                            break;

                        case ECBCommand.InstantiateEntity:
                            {
                                var cmd = (EntityCommand*)header;

                                var index = -cmd->IdentityIndex - 1;
                                Entity srcEntity = SelectEntity(cmd->Entity, playbackState);
                                mgr.InstantiateInternal(srcEntity, playbackState.CreateEntityBatch + index,
                                    cmd->BatchCount);
                            }
                            break;

                        case ECBCommand.AddComponent:
                            {
                                var cmd = (EntityComponentCommand*)header;
                                var componentType = (ComponentType)TypeManager.GetType(cmd->ComponentTypeIndex);
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.AddComponent(entity, componentType);
                                if (!componentType.IsZeroSized)
                                    mgr.SetComponentDataRaw(entity, cmd->ComponentTypeIndex, cmd + 1, cmd->ComponentSize);
                            }
                            break;

                        case ECBCommand.AddComponentWithEntityFixUp:
                            {
                                var cmd = (EntityComponentCommand*)header;
                                var componentType = (ComponentType)TypeManager.GetType(cmd->ComponentTypeIndex);
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.AddComponent(entity, componentType);
                                SetCommandDataWithFixup(mgr, cmd, entity, playbackState);
                            }
                            break;

                        case ECBCommand.SetComponent:
                            {
                                var cmd = (EntityComponentCommand*)header;
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.SetComponentDataRaw(entity, cmd->ComponentTypeIndex, cmd + 1, cmd->ComponentSize);
                            }
                            break;

                        case ECBCommand.SetComponentWithEntityFixUp:
                            {
                                var cmd = (EntityComponentCommand*)header;
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                SetCommandDataWithFixup(mgr, cmd, entity, playbackState);
                            }
                            break;

                        case ECBCommand.AddBuffer:
                            {
                                var cmd = (EntityBufferCommand*)header;
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.AddComponent(entity, ComponentType.FromTypeIndex(cmd->ComponentTypeIndex));
                                mgr.SetBufferRaw(entity, cmd->ComponentTypeIndex, &cmd->TempBuffer, cmd->ComponentSize);
                            }
                            break;

                        case ECBCommand.SetBuffer:
                            {
                                var cmd = (EntityBufferCommand*)header;
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.SetBufferRaw(entity, cmd->ComponentTypeIndex, &cmd->TempBuffer, cmd->ComponentSize);
                            }
                            break;

                        case ECBCommand.AddSharedComponentData:
                            {
                                var cmd = (EntitySharedComponentCommand*)header;
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.AddSharedComponentDataBoxed(entity, cmd->ComponentTypeIndex, cmd->HashCode,
                                    cmd->GetBoxedObject());
                            }
                            break;

                        case ECBCommand.SetSharedComponentData:
                            {
                                var cmd = (EntitySharedComponentCommand*)header;
                                var entity = SelectEntity(cmd->Header.Entity, playbackState);
                                mgr.SetSharedComponentDataBoxed(entity, cmd->ComponentTypeIndex, cmd->HashCode,
                                    cmd->GetBoxedObject());
                            }
                            break;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                        default:
                            throw new System.InvalidOperationException("Invalid command buffer");
#endif
                    }

                    off += header->TotalSize;
                }
                // Reached the end of a chunk; advance to the next one
                chunk = chunk->Next;
                off = 0;
            }
            // Reached the end of the chain; update its playback state to make sure it's ignored
            // for the remainder of playback.
            {
                var state = chainStates[currentChain];
                state.Chunk = null;
                state.Offset = 0;
                state.NextSortIndex = Int32.MinValue;
                chainStates[currentChain] = state;
            }
        }

        public Concurrent ToConcurrent()
        {
            EntityCommandBuffer.Concurrent concurrent;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety0);
            concurrent.m_Safety0 = m_Safety0;
            AtomicSafetyHandle.UseSecondaryVersion(ref concurrent.m_Safety0);
            concurrent.m_BufferSafety = m_BufferSafety;
            concurrent.m_ArrayInvalidationSafety = m_ArrayInvalidationSafety;
            concurrent.m_SafetyReadOnlyCount = 0;
            concurrent.m_SafetyReadWriteCount = 3;

            if (m_Data->m_Allocator == Allocator.Temp)
            {
                throw new InvalidOperationException("EntityCommandBuffer.Concurrent can not use Allocator.Temp; use Allocator.TempJob instead");
            }
#endif
            concurrent.m_Data = m_Data;
            concurrent.m_ThreadIndex = -1;

            if (concurrent.m_Data != null)
            {
                concurrent.m_Data->InitConcurrentAccess();
            }

            return concurrent;
        }

        /// <summary>
        /// Allows concurrent (deterministic) command buffer recording.
        /// </summary>
        [NativeContainer]
        [NativeContainerIsAtomicWriteOnly]
        [StructLayout(LayoutKind.Sequential)]
        unsafe public struct Concurrent
        {
            [NativeDisableUnsafePtrRestriction] internal EntityCommandBufferData* m_Data;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            internal AtomicSafetyHandle m_Safety0;
            internal AtomicSafetyHandle m_BufferSafety;
            internal AtomicSafetyHandle m_ArrayInvalidationSafety;
            internal int m_SafetyReadOnlyCount;
            internal int m_SafetyReadWriteCount;
#endif

            // NOTE: Until we have a way to safely batch, let's keep it off
            private const bool kBatchableCommand = false;


            //internal ref int m_EntityIndex;
            [NativeSetThreadIndex]
            internal int m_ThreadIndex;

            [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
            private void CheckWriteAccess()
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckWriteAndThrow(m_Safety0);
#endif
            }

            private EntityCommandBufferChain* ThreadChain
            {
                get {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                    if (m_ThreadIndex == -1)
                    {
                        throw new InvalidOperationException("EntityCommandBuffer.Concurrent must only be used in a Job");
                    }
#endif
                    return &m_Data->m_ThreadedChains[m_ThreadIndex];
                }
            }

            public Entity CreateEntity(int jobIndex, EntityArchetype archetype = new EntityArchetype())
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                // NOTE: Contention could be a performance problem especially on ARM
                // architecture. Maybe reserve a few indices for each job would be a better
                // approach or hijack the Version field of an Entity and store jobIndex
                int index = Interlocked.Decrement(ref m_Data->m_Entity.Index);
                m_Data->AddCreateCommand(chain, jobIndex, ECBCommand.CreateEntity,  index, archetype, kBatchableCommand);
                return new Entity {Index = index};
            }

            public Entity Instantiate(int jobIndex, Entity e)
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                int index = Interlocked.Decrement(ref m_Data->m_Entity.Index);
                m_Data->AddEntityCommand(chain, jobIndex, ECBCommand.InstantiateEntity, index, e, kBatchableCommand);
                return new Entity {Index = index};
            }

            public void DestroyEntity(int jobIndex, Entity e)
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                m_Data->AddEntityCommand(chain, jobIndex, ECBCommand.DestroyEntity, 0, e, false);
            }

            public void AddComponent<T>(int jobIndex, Entity e, T component) where T : struct, IComponentData
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                m_Data->AddEntityComponentCommand(chain, jobIndex, ECBCommand.AddComponent, e, component);
            }

            public DynamicBuffer<T> AddBuffer<T>(int jobIndex, Entity e) where T : struct, IBufferElementData
            {
                CheckWriteAccess();
                var chain = ThreadChain;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                return m_Data->CreateBufferCommand<T>(ECBCommand.AddBuffer, chain, jobIndex, e, m_BufferSafety, m_ArrayInvalidationSafety);
#else
                return m_Data->CreateBufferCommand<T>(ECBCommand.AddBuffer, chain, jobIndex, e);
#endif
            }

            public DynamicBuffer<T> SetBuffer<T>(int jobIndex, Entity e) where T : struct, IBufferElementData
            {
                CheckWriteAccess();
                var chain = ThreadChain;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                return m_Data->CreateBufferCommand<T>(ECBCommand.SetBuffer, chain, jobIndex, e, m_BufferSafety, m_ArrayInvalidationSafety);
#else
                return m_Data->CreateBufferCommand<T>(ECBCommand.SetBuffer, chain, jobIndex, e);
#endif
            }

            public void SetComponent<T>(int jobIndex, Entity e, T component) where T : struct, IComponentData
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                m_Data->AddEntityComponentCommand(chain, jobIndex, ECBCommand.SetComponent, e, component);
            }

            public void RemoveComponent<T>(int jobIndex, Entity e)
            {
                RemoveComponent(jobIndex, e, ComponentType.Create<T>());
            }

            public void RemoveComponent(int jobIndex, Entity e, ComponentType componentType)
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                m_Data->AddEntityComponentTypeCommand(chain, jobIndex, ECBCommand.RemoveComponent, e, componentType);
            }

            public void AddSharedComponent<T>(int jobIndex, Entity e, T component) where T : struct, ISharedComponentData
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                int hashCode;
                if (IsDefaultObject(ref component, out hashCode))
                    m_Data->AddEntitySharedComponentCommand<T>(chain, jobIndex, ECBCommand.AddSharedComponentData, e, hashCode, null);
                else
                    m_Data->AddEntitySharedComponentCommand<T>(chain, jobIndex, ECBCommand.AddSharedComponentData, e, hashCode, component);
            }

            public void SetSharedComponent<T>(int jobIndex, Entity e, T component) where T : struct, ISharedComponentData
            {
                CheckWriteAccess();
                var chain = ThreadChain;
                int hashCode;
                if (IsDefaultObject(ref component, out hashCode))
                    m_Data->AddEntitySharedComponentCommand<T>(chain, jobIndex, ECBCommand.SetSharedComponentData, e, hashCode, null);
                else
                    m_Data->AddEntitySharedComponentCommand<T>(chain, jobIndex, ECBCommand.SetSharedComponentData, e, hashCode, component);
            }
        }
    }
}
