using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
    /// <summary>
    ///     Define a query to find archetypes with specific component(s).
    ///     Any - Gets archetypes that have one or more of the given components
    ///     None - Gets archetypes that do not have the given components
    ///     All - Gets archetypes that have all the given components
    ///     Example:
    ///     Player has components: Position, Rotation, Player
    ///     Enemy1 has components: Position, Rotation, Melee
    ///     Enemy2 has components: Position, Rotation, Ranger
    ///     The query below would give you all of the archetypes that:
    ///         have any of [Melee or Ranger], AND have none of [Player], AND have all of [Position and Rotation]
    ///     new EntityArchetypeQuery {
    ///     Any = new ComponentType[] {typeof(Melee), typeof(Ranger)},
    ///     None = new ComponentType[] {typeof(Player)},
    ///     All = new ComponentType[] {typeof(Position), typeof(Rotation)} }
    /// </summary>
    public class EntityArchetypeQuery
    {
        public ComponentType[] Any = Array.Empty<ComponentType>();
        public ComponentType[] None = Array.Empty<ComponentType>();
        public ComponentType[] All = Array.Empty<ComponentType>();        
    }

    //@TODO: Rename to ComponentQuery
    public unsafe class ComponentGroup : IDisposable
    {
        readonly ComponentJobSafetyManager m_SafetyManager;
        readonly EntityGroupData*          m_GroupData;
        readonly EntityDataManager*        m_EntityDataManager;
        ComponentGroupFilter               m_Filter;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal string                    DisallowDisposing = null;
#endif

        // TODO: this is temporary, used to cache some state to avoid recomputing the TransformAccessArray. We need to improve this.
        internal IDisposable               m_CachedState;

        internal ComponentGroup(EntityGroupData* groupData, ComponentJobSafetyManager safetyManager, ArchetypeManager typeManager, EntityDataManager* entityDataManager)
        {
            m_GroupData = groupData;
            m_EntityDataManager = entityDataManager;
            m_Filter = default(ComponentGroupFilter);
            m_SafetyManager = safetyManager;
            ArchetypeManager = typeManager;
            EntityDataManager = entityDataManager;
        }

        internal EntityDataManager* EntityDataManager { get; }

        /// <summary>
        ///      Ignore this ComponentGroup if it has no entities in any of its archetypes.
        /// </summary>
        /// <returns>True if this ComponentGroup has no entities. False if it has 1 or more entities.</returns>
        public bool IsEmptyIgnoreFilter
        {
            get
            {
                for (var match = m_GroupData->FirstMatchingArchetype; match != null; match = match->Next)
                    if (match->Archetype->EntityCount > 0)
                        return false;

                return true;
            }
        }
#if UNITY_CSHARP_TINY
        internal class SlowListSet<T>
        {
            internal List<T> items;

            internal SlowListSet() {
                items = new List<T>();
            }

            internal void Add(T item)
            {
                if (!items.Contains(item))
                    items.Add(item);
            }

            internal int Count => items.Count;

            internal T[] ToArray()
            {
                return items.ToArray();
            }
        }
#endif

        /// <summary>
        ///     Gets array of all ComponentTypes in this ComponentGroup's ArchetypeQueries.
        /// </summary>
        /// <returns>Array of ComponentTypes</returns>
        internal ComponentType[] GetQueryTypes()
        {
#if !UNITY_CSHARP_TINY
            var types = new HashSet<ComponentType>();
#else
            var types = new SlowListSet<ComponentType>();
#endif

            for (var i = 0; i < m_GroupData->ArchetypeQueryCount; ++i)
            {
                for (var j = 0; j < m_GroupData->ArchetypeQuery[i].AnyCount; ++j)
                {
                    types.Add(TypeManager.GetType(m_GroupData->ArchetypeQuery[i].Any[j]));
                }
                for (var j = 0; j < m_GroupData->ArchetypeQuery[i].AllCount; ++j)
                {
                    types.Add(TypeManager.GetType(m_GroupData->ArchetypeQuery[i].All[j]));
                }
                for (var j = 0; j < m_GroupData->ArchetypeQuery[i].NoneCount; ++j)
                {
                    types.Add(ComponentType.Subtractive(TypeManager.GetType(m_GroupData->ArchetypeQuery[i].None[j])));
                }
            }

#if !UNITY_CSHARP_TINY
            var array = new ComponentType[types.Count];
            var t = 0;
            foreach (var type in types)
                array[t++] = type;
            return array;
#else
            return types.ToArray();
#endif
        }

        /// <summary>
        ///     Packed array of this ComponentGroup's ReadOnly and writable ComponentTypes.
        ///     ReadOnly ComponentTypes come before writable types in this array.
        /// </summary>
        /// <returns>Array of ComponentTypes</returns>
        internal ComponentType[] GetReadAndWriteTypes()
        {
            var types = new ComponentType[m_GroupData->ReaderTypesCount + m_GroupData->WriterTypesCount];
            var typeArrayIndex = 0;
            for (var i = 0; i < m_GroupData->ReaderTypesCount; ++i)
            {
                types[typeArrayIndex++] = ComponentType.ReadOnly(TypeManager.GetType(m_GroupData->ReaderTypes[i]));
            }
            for (var i = 0; i < m_GroupData->WriterTypesCount; ++i)
            {
                types[typeArrayIndex++] = TypeManager.GetType(m_GroupData->WriterTypes[i]);
            }

            return types;
        }

        internal ArchetypeManager ArchetypeManager { get; }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (DisallowDisposing != null)
                throw new ArgumentException(DisallowDisposing);
#endif

            if (m_CachedState != null)
                m_CachedState.Dispose();

            ResetFilter();
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        /// <summary>
        ///     Gets safety handle to a ComponentType required by this ComponentGroup.
        /// </summary>
        /// <param name="indexInComponentGroup">Index of a ComponentType in this ComponentGroup's RequiredComponents list./param>
        /// <returns>AtomicSafetyHandle for a ComponentType</returns>
        internal AtomicSafetyHandle GetSafetyHandle(int indexInComponentGroup)
        {
            var type = m_GroupData->RequiredComponents + indexInComponentGroup;
            var isReadOnly = type->AccessModeType == ComponentType.AccessMode.ReadOnly;
            return m_SafetyManager.GetSafetyHandle(type->TypeIndex, isReadOnly);
        }

        /// <summary>
        ///     Gets buffer safety handle to a ComponentType required by this ComponentGroup.
        /// </summary>
        /// <param name="indexInComponentGroup">Index of a ComponentType in this ComponentGroup's RequiredComponents list./param>
        /// <returns>AtomicSafetyHandle for a buffer</returns>
        internal AtomicSafetyHandle GetBufferSafetyHandle(int indexInComponentGroup)
        {
            var type = m_GroupData->RequiredComponents + indexInComponentGroup;
            return m_SafetyManager.GetBufferSafetyHandle(type->TypeIndex);
        }
#endif

        bool GetIsReadOnly(int indexInComponentGroup)
        {
            var type = m_GroupData->RequiredComponents + indexInComponentGroup;
            var isReadOnly = type->AccessModeType == ComponentType.AccessMode.ReadOnly;
            return isReadOnly;
        }

        /// <summary>
        ///     Calculates number of entities in this ComponentGroup.
        /// </summary>
        /// <returns>Number of entities</returns>
        public int CalculateLength()
        {
            return ComponentChunkIterator.CalculateLength(m_GroupData->FirstMatchingArchetype, ref m_Filter);
        }

        /// <summary>
        ///     Gets iterator to chunks associated with this ComponentGroup.
        /// </summary>
        /// <returns>ComponentChunkIterator for this ComponentGroup</returns>
        internal ComponentChunkIterator GetComponentChunkIterator()
        {
            return new ComponentChunkIterator(m_GroupData->FirstMatchingArchetype, m_EntityDataManager->GlobalSystemVersion, ref m_Filter);
        }

        /// <summary>
        ///     Index of a ComponentType in this ComponentGroup's RequiredComponents list.
        ///     For example, you have a ComponentGroup that requires these ComponentTypes: Position, Velocity, and Color.
        ///
        ///     These are their type indices (according to the TypeManager):
        ///         Position.TypeIndex == 3
        ///         Velocity.TypeIndex == 5
        ///            Color.TypeIndex == 17
        ///
        ///     RequiredComponents: [Position -> Velocity -> Color] (a linked list)
        ///     Given Velocity's TypeIndex (5), the return value would be 1, since Velocity is in slot 1 of RequiredComponents.
        /// </summary>
        /// <param name="componentType">Index of a ComponentType in the TypeManager</param>
        /// <returns>An index into RequiredComponents.</returns>
        internal int GetIndexInComponentGroup(int componentType)
        {
            // Go through all the required component types in this ComponentGroup until you find the matching component type index.
            var componentIndex = 0;
            while (componentIndex < m_GroupData->RequiredComponentsCount && m_GroupData->RequiredComponents[componentIndex].TypeIndex != componentType)
                ++componentIndex;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (componentIndex >= m_GroupData->RequiredComponentsCount)
                throw new InvalidOperationException( $"Trying to get iterator for {TypeManager.GetType(componentType)} but the required component type was not declared in the EntityGroup.");
#endif
            return componentIndex;
        }

        internal void GetComponentDataArray<T>(ref ComponentChunkIterator iterator, int indexInComponentGroup,
            int length, out ComponentDataArray<T> output) where T : struct, IComponentData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var typeIndex = TypeManager.GetTypeIndex<T>();
            var componentType = ComponentType.FromTypeIndex(typeIndex);
            if (componentType.IsZeroSized)
                throw new ArgumentException($"GetComponentDataArray<{typeof(T)}> cannot be called on zero-sized IComponentData");
#endif

            iterator.IndexInComponentGroup = indexInComponentGroup;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            output = new ComponentDataArray<T>(iterator, length, GetSafetyHandle(indexInComponentGroup));
#else
			output = new ComponentDataArray<T>(iterator, length);
#endif
        }

        public ComponentDataArray<T> GetComponentDataArray<T>() where T : struct, IComponentData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var componentType = ComponentType.FromTypeIndex(typeIndex);
            if (componentType.IsZeroSized)
                throw new ArgumentException($"GetComponentDataArray<{typeof(T)}> cannot be called on zero-sized IComponentData");
#endif

            int length = CalculateLength();
            ComponentChunkIterator iterator = GetComponentChunkIterator();
            var indexInComponentGroup = GetIndexInComponentGroup(typeIndex);

            ComponentDataArray<T> res;
            GetComponentDataArray(ref iterator, indexInComponentGroup, length, out res);
            return res;
        }

        internal void GetSharedComponentDataArray<T>(ref ComponentChunkIterator iterator, int indexInComponentGroup,
            int length, out SharedComponentDataArray<T> output) where T : struct, ISharedComponentData
        {
            iterator.IndexInComponentGroup = indexInComponentGroup;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var componentTypeIndex = m_GroupData->RequiredComponents[indexInComponentGroup].TypeIndex;
            output = new SharedComponentDataArray<T>(ArchetypeManager.GetSharedComponentDataManager(),
                indexInComponentGroup, iterator, length, m_SafetyManager.GetSafetyHandle(componentTypeIndex, true));
#else
            output = new SharedComponentDataArray<T>(ArchetypeManager.GetSharedComponentDataManager(),
                                                     indexInComponentGroup, iterator, length);
#endif
        }

        /// <summary>
        ///     Creates an array containing ISharedComponentData of a given type T.
        /// </summary>
        /// <returns>NativeArray of ISharedComponentData in this ComponentGroup.</returns>
        public SharedComponentDataArray<T> GetSharedComponentDataArray<T>() where T : struct, ISharedComponentData
        {
            int length = CalculateLength();
            ComponentChunkIterator iterator = GetComponentChunkIterator();
            var indexInComponentGroup = GetIndexInComponentGroup(TypeManager.GetTypeIndex<T>());

            SharedComponentDataArray<T> res;
            GetSharedComponentDataArray(ref iterator, indexInComponentGroup, length, out res);
            return res;
        }

        internal void GetBufferArray<T>(ref ComponentChunkIterator iterator, int indexInComponentGroup, int length,
            out BufferArray<T> output) where T : struct, IBufferElementData
        {
            iterator.IndexInComponentGroup = indexInComponentGroup;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            output = new BufferArray<T>(iterator, length, GetIsReadOnly(indexInComponentGroup),
                GetSafetyHandle(indexInComponentGroup),
                GetBufferSafetyHandle(indexInComponentGroup));
#else
			output = new BufferArray<T>(iterator, length, GetIsReadOnly(indexInComponentGroup));
#endif
        }

        public BufferArray<T> GetBufferArray<T>() where T : struct, IBufferElementData
        {
            int length = CalculateLength();
            ComponentChunkIterator iterator = GetComponentChunkIterator();
            var indexInComponentGroup = GetIndexInComponentGroup(TypeManager.GetTypeIndex<T>());

            BufferArray<T> res;
            GetBufferArray(ref iterator, indexInComponentGroup, length, out res);
            return res;
        }

        /// <summary>
        ///     Creates an array with all the chunks in this ComponentGroup.
        ///     Gives the caller a job handle so it can wait for GatherChunks to finish.
        /// </summary>
        /// <param name="allocator">Allocator to use for the array.</param>
        /// <param name="jobHandle">Handle to the GatherChunks job used to fill the output array.</param>
        /// <returns>NativeArray of all the chunks in this ComponentChunkIterator.</returns>
        public NativeArray<ArchetypeChunk> CreateArchetypeChunkArray(Allocator allocator, out JobHandle jobhandle)
        {
            return ComponentChunkIterator.CreateArchetypeChunkArray(m_GroupData->FirstMatchingArchetype, allocator, out jobhandle);
        }

        /// <summary>
        ///     Creates an array with all the chunks in this ComponentGroup.
        ///     Waits for the GatherChunks job to complete here.
        /// </summary>
        /// <param name="allocator">Allocator to use for the array.</param>
        /// <returns>NativeArray of all the chunks in this ComponentChunkIterator.</returns>
        public NativeArray<ArchetypeChunk> CreateArchetypeChunkArray(Allocator allocator)
        {
            JobHandle job;
            var res = ComponentChunkIterator.CreateArchetypeChunkArray(m_GroupData->FirstMatchingArchetype, allocator, out job);
            job.Complete();
            return res;
        }


        public NativeArray<Entity> ToEntityArray(Allocator allocator, out JobHandle jobhandle)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var entityType = new ArchetypeChunkEntityType(m_SafetyManager.GetSafetyHandle(TypeManager.GetTypeIndex<Entity>(), true));
#else
            var entityType = new ArchetypeChunkEntityType();
#endif
            
            return ComponentChunkIterator.CreateEntityArray(m_GroupData->FirstMatchingArchetype, allocator, entityType,  this, ref m_Filter, out jobhandle, GetDependency());
        }

        public NativeArray<Entity> ToEntityArray(Allocator allocator)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var entityType = new ArchetypeChunkEntityType(m_SafetyManager.GetSafetyHandle(TypeManager.GetTypeIndex<Entity>(), true));
#else
            var entityType = new ArchetypeChunkEntityType();
#endif    
            JobHandle job;
            var res = ComponentChunkIterator.CreateEntityArray(m_GroupData->FirstMatchingArchetype, allocator, entityType, this, ref m_Filter, out job, GetDependency());
            job.Complete();
            return res;
        }

        public NativeArray<T> ToComponentDataArray<T>(Allocator allocator, out JobHandle jobhandle)
            where T : struct,IComponentData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var componentType = new ArchetypeChunkComponentType<T>(m_SafetyManager.GetSafetyHandle(TypeManager.GetTypeIndex<T>(), true), true, EntityDataManager->GlobalSystemVersion);
#else
            var componentType = new ArchetypeChunkComponentType<T>(true, EntityDataManager->GlobalSystemVersion);
#endif
            return ComponentChunkIterator.CreateComponentDataArray(m_GroupData->FirstMatchingArchetype, allocator, componentType, this, ref m_Filter, out jobhandle, GetDependency());
        }

        public NativeArray<T> ToComponentDataArray<T>(Allocator allocator)
            where T : struct, IComponentData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var componentType = new ArchetypeChunkComponentType<T>(m_SafetyManager.GetSafetyHandle(TypeManager.GetTypeIndex<T>(), true), true, EntityDataManager->GlobalSystemVersion);
#else
            var componentType = new ArchetypeChunkComponentType<T>(true, EntityDataManager->GlobalSystemVersion);
#endif
            JobHandle job;
            var res = ComponentChunkIterator.CreateComponentDataArray(m_GroupData->FirstMatchingArchetype, allocator, componentType, this, ref m_Filter, out job, GetDependency());
            job.Complete();
            return res;
        }

        /// <summary>
        ///     Creates an EntityArray that gives you access to the entities in this ComponentGroup.
        /// </summary>
        /// <returns>EntityArray of all the entities in this ComponentGroup.</returns>
        public EntityArray GetEntityArray()
        {
            int length = CalculateLength();
            ComponentChunkIterator iterator = GetComponentChunkIterator();

            EntityArray output;
            iterator.IndexInComponentGroup = 0;
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            output = new EntityArray(iterator, length, m_SafetyManager.GetSafetyHandle(TypeManager.GetTypeIndex<Entity>(), true));
#else
			output = new EntityArray(iterator, length);
#endif
            return output;
        }

        internal bool CompareComponents(ComponentType* componentTypes, int count)
        {
            return EntityGroupManager.CompareComponents(componentTypes, count, m_GroupData);
        }

        public bool CompareComponents(ComponentType[] componentTypes)
        {
            fixed (ComponentType* componentTypesPtr = componentTypes)
            {
                return EntityGroupManager.CompareComponents(componentTypesPtr, componentTypes.Length, m_GroupData);
            }
        }

        public bool CompareQuery(EntityArchetypeQuery[] query)
        {
            return EntityGroupManager.CompareQuery(query, m_GroupData);
        }

        /// <summary>
        ///     Resets this ComponentGroup's filter.
        ///     Removes references to shared component data, if applicable, then resets the filter type to None.
        /// </summary>
        public void ResetFilter()
        {
            if (m_Filter.Type == FilterType.SharedComponent)
            {
                var filteredCount = m_Filter.Shared.Count;

                var sm = ArchetypeManager.GetSharedComponentDataManager();
                fixed (int* sharedComponentIndexPtr = m_Filter.Shared.SharedComponentIndex)
                {
                    for (var i = 0; i < filteredCount; ++i)
                        sm.RemoveReference(sharedComponentIndexPtr[i]);
                }
            }

            m_Filter.Type = FilterType.None;
        }

        /// <summary>
        ///     Sets this ComponentGroup's filter while preserving its version number.
        /// </summary>
        /// <param name="filter">ComponentGroupFilter to use all data but RequiredChangeVersion from.</param>
        void SetFilter(ref ComponentGroupFilter filter)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            filter.AssertValid();
#endif
            var version = m_Filter.RequiredChangeVersion;
            ResetFilter();
            m_Filter = filter;
            m_Filter.RequiredChangeVersion = version;
        }

        /// <summary>
        ///     Creates a new SharedComponent filter on a given ISharedComponentData type for this ComponentGroup. 
        /// </summary>
        /// <param name="sharedComponent1">A struct that implements ISharedComponentData</param>
        public void SetFilter<SharedComponent1>(SharedComponent1 sharedComponent1)
            where SharedComponent1 : struct, ISharedComponentData
        {
            var sm = ArchetypeManager.GetSharedComponentDataManager();

            var filter = new ComponentGroupFilter();
            filter.Type = FilterType.SharedComponent;
            filter.Shared.Count = 1;
            filter.Shared.IndexInComponentGroup[0] = GetIndexInComponentGroup(TypeManager.GetTypeIndex<SharedComponent1>());
            filter.Shared.SharedComponentIndex[0] = sm.InsertSharedComponent(sharedComponent1);

            SetFilter(ref filter);
        }

        public void SetFilter<SharedComponent1, SharedComponent2>(SharedComponent1 sharedComponent1,
            SharedComponent2 sharedComponent2)
            where SharedComponent1 : struct, ISharedComponentData
            where SharedComponent2 : struct, ISharedComponentData
        {
            var sm = ArchetypeManager.GetSharedComponentDataManager();

            var filter = new ComponentGroupFilter();
            filter.Type = FilterType.SharedComponent;
            filter.Shared.Count = 2;
            filter.Shared.IndexInComponentGroup[0] = GetIndexInComponentGroup(TypeManager.GetTypeIndex<SharedComponent1>());
            filter.Shared.SharedComponentIndex[0] = sm .InsertSharedComponent(sharedComponent1);

            filter.Shared.IndexInComponentGroup[1] = GetIndexInComponentGroup(TypeManager.GetTypeIndex<SharedComponent2>());
            filter.Shared.SharedComponentIndex[1] = sm.InsertSharedComponent(sharedComponent2);

            SetFilter(ref filter);
        }

        /// <summary>
        ///     Saves a given ComponentType's index in RequiredComponents in this group's Changed filter.
        /// </summary>
        /// <param name="componentType">ComponentType to mark as changed on this ComponentGroup's filter.</param>
        public void SetFilterChanged(ComponentType componentType)
        {
            var filter = new ComponentGroupFilter();
            filter.Type = FilterType.Changed;
            filter.Changed.Count = 1;
            filter.Changed.IndexInComponentGroup[0] = GetIndexInComponentGroup(componentType.TypeIndex);

            SetFilter(ref filter);
        }

        internal void SetFilterChangedRequiredVersion(uint requiredVersion)
        {
            m_Filter.RequiredChangeVersion = requiredVersion;
        }

        /// <summary>
        ///     Saves given ComponentTypes' indices in RequiredComponents in this group's Changed filter.
        /// </summary>
        /// <param name="componentType">Array of ComponentTypes to mark as changed on this ComponentGroup's filter.</param>
        public void SetFilterChanged(ComponentType[] componentType)
        {
            if (componentType.Length > ComponentGroupFilter.ChangedFilter.Capacity)
                throw new ArgumentException(
                    $"ComponentGroup.SetFilterChanged accepts a maximum of {ComponentGroupFilter.ChangedFilter.Capacity} component array length");
            if (componentType.Length <= 0)
                throw new ArgumentException(
                    $"ComponentGroup.SetFilterChanged component array length must be larger than 0");

            var filter = new ComponentGroupFilter();
            filter.Type = FilterType.Changed;
            filter.Changed.Count = componentType.Length;
            for (var i = 0; i != componentType.Length; i++)
                filter.Changed.IndexInComponentGroup[i] = GetIndexInComponentGroup(componentType[i].TypeIndex);

            SetFilter(ref filter);
        }

        /// <summary>
        ///     Ensures all jobs running on this ComponentGroup complete.
        /// </summary>
        public void CompleteDependency()
        {
            m_SafetyManager.CompleteDependenciesNoChecks(m_GroupData->ReaderTypes, m_GroupData->ReaderTypesCount,
                m_GroupData->WriterTypes, m_GroupData->WriterTypesCount);
        }

        /// <summary>
        ///     Combines all dependencies in this ComponentGroup into a single JobHandle.
        /// </summary>
        /// <returns>JobHandle that represents the combined dependencies of this ComponentGroup</returns>
        public JobHandle GetDependency()
        {
            return m_SafetyManager.GetDependency(m_GroupData->ReaderTypes, m_GroupData->ReaderTypesCount,
                m_GroupData->WriterTypes, m_GroupData->WriterTypesCount);
        }

        /// <summary>
        ///     Adds another job handle to this ComponentGroup's dependencies.
        /// </summary>
        public void AddDependency(JobHandle job)
        {
            m_SafetyManager.AddDependency(m_GroupData->ReaderTypes, m_GroupData->ReaderTypesCount,
                m_GroupData->WriterTypes, m_GroupData->WriterTypesCount, job);
        }

        public int GetCombinedComponentOrderVersion()
        {
            var version = 0;

            for (var i = 0; i < m_GroupData->RequiredComponentsCount; ++i)
                version += m_EntityDataManager->GetComponentTypeOrderVersion(m_GroupData->RequiredComponents[i].TypeIndex);

            return version;
        }

        /// <summary>
        ///     Total number of chunks in this ComponentGroup's MatchingArchetypes list.
        /// </summary>
        /// <param name="firstMatchingArchetype">First node of MatchingArchetypes linked list.</param>
        /// <returns>Number of chunks in this ComponentGroup.</returns>
        internal int CalculateNumberOfChunksWithoutFiltering()
        {
            return ComponentChunkIterator.CalculateNumberOfChunksWithoutFiltering(m_GroupData->FirstMatchingArchetype);
        }


		//TODO: Remove this once CreateArchetypeChunkArray supports filtering shared components
        internal NativeArray<ArchetypeChunk> GetAllMatchingChunks(Allocator allocator)
        {
            if (((m_Filter.Type & FilterType.SharedComponent) != 0) && (m_Filter.Shared.Count == 1))
            {
                int indexInComponentGroup;
                int sharedComponentIndex;

                fixed (int* indexInComponentGroupPtr = m_Filter.Shared.IndexInComponentGroup,
                    sharedComponentIndexPtr = m_Filter.Shared.SharedComponentIndex)
                {
                    indexInComponentGroup = indexInComponentGroupPtr[0];
                    sharedComponentIndex = sharedComponentIndexPtr[0];
                }

                {
                    int totalChunkCount = 0;
                    var searchElements = CreateArchetypeChunkSearchElements(out totalChunkCount);
                    var foundChunks = new NativeArray<ArchetypeChunk>(totalChunkCount, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

                    new AllChunksMatchingSingleSharedComponentJob
                    {
                        chunks = foundChunks,
                        indexInComponentGroup = indexInComponentGroup,
                        searchElements = searchElements,
                        sharedComponentIndex = sharedComponentIndex
                    }.Schedule(searchElements.Length, 1).Complete();

                    var joinedChunks = JoinChunks(searchElements, foundChunks);

                    foundChunks.Dispose();
                    searchElements.Dispose();

                    return joinedChunks;
                }
            }
            else
            {
                var chunks = new NativeList<ArchetypeChunk>(allocator);
                for (var match = m_GroupData->FirstMatchingArchetype; match != null; match = match->Next)
                {
                    var archeType = match->Archetype;
                    for (var i = 0; i < archeType->Chunks.Count; ++i)
                    {
                        var chunk = archeType->Chunks.p[i];
                        if (chunk->MatchesFilter(match, ref m_Filter))
                        {
                            chunks.Add(new ArchetypeChunk { m_Chunk = chunk });
                        }
                    }
                }

                var chunkArray = new NativeArray<ArchetypeChunk>(chunks, Allocator.TempJob);
                chunks.Dispose();
                return chunkArray;
            }
        }

        NativeArray<ArchetypeChunk> JoinChunks(NativeList<ArchetypeChunkSearchElement> searchElements, NativeArray<ArchetypeChunk> foundChunks)
        {
            int totalChunks = 0;
            for (int i = 0; i < searchElements.Length; ++i)
            {
                totalChunks += searchElements[i].filteredCount;
            }

            var joinedChunks = new NativeArray<ArchetypeChunk>(totalChunks, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            ArchetypeChunk* dst = (ArchetypeChunk*)joinedChunks.GetUnsafePtr();

            //TODO: for each job
            for (int i = 0; i < searchElements.Length; ++i)
            {
                UnsafeUtility.MemCpy(dst, (ArchetypeChunk*)foundChunks.GetUnsafePtr() + searchElements[i].indexStart, searchElements[i].filteredCount*sizeof(ArchetypeChunk));
                dst += searchElements[i].filteredCount;
            }

            return joinedChunks;
        }

        NativeList<ArchetypeChunkSearchElement> CreateArchetypeChunkSearchElements(out int totalChunkCount)
        {
            var searchElements = new NativeList<ArchetypeChunkSearchElement>(100, Allocator.TempJob);
            totalChunkCount = 0;
            for (var match = m_GroupData->FirstMatchingArchetype; match != null; match = match->Next)
            {
                var chunkCount = match->Archetype->Chunks.Count;
                searchElements.Add(new ArchetypeChunkSearchElement
                {
                    matchingArchetype = match,
                    filteredCount = 0,
                    indexStart = totalChunkCount
                });
                totalChunkCount += chunkCount;
            }

            return searchElements;
        }

        struct ArchetypeChunkSearchElement
        {
            public MatchingArchetypes* matchingArchetype;
            public int filteredCount;
            public int indexStart;
        }

        [BurstCompile]
        unsafe struct AllChunksMatchingSingleSharedComponentJob : IJobParallelFor
        {
            public NativeArray<ArchetypeChunkSearchElement> searchElements;

            [NativeDisableParallelForRestriction]
            public NativeArray<ArchetypeChunk> chunks;
            [NativeDisableUnsafePtrRestriction]
            public int indexInComponentGroup;
            public int sharedComponentIndex;

            public void Execute(int index)
            {
                var element = searchElements[index];
                var match = element.matchingArchetype;
                var archetype = match->Archetype;
                var componentIndexInArchetype = match->IndexInArchetype[indexInComponentGroup];
                var componentIndexInChunk = archetype->SharedComponentOffset[componentIndexInArchetype];
                var writeIndex = element.indexStart;
                int filteredCount = 0;
                for (var i = 0; i < archetype->Chunks.Count; ++i)
                {
                    var chunk = archetype->Chunks.p[i];
                    var sharedComponentsInChunk = chunk->SharedComponentValueArray;
                    if (sharedComponentsInChunk[componentIndexInChunk] == sharedComponentIndex)
                        chunks[writeIndex + filteredCount++] = new ArchetypeChunk { m_Chunk = chunk };
                }

                element.filteredCount = filteredCount;
                searchElements[index] = element;
            }
        }
    }
}
