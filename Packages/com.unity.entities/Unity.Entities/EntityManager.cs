using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Assertions;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

[assembly: InternalsVisibleTo("Unity.Entities.Hybrid")]
[assembly: InternalsVisibleTo("Unity.Entities.Properties")]

namespace Unity.Entities
{
    //@TODO: There is nothing prevent non-main thread (non-job thread) access of EntityManager.
    //       Static Analysis or runtime checks?

    //@TODO: safety?
    [DebuggerTypeProxy(typeof(EntityArchetypeDebugView))]
    public unsafe struct EntityArchetype : IEquatable<EntityArchetype>
    {
        [NativeDisableUnsafePtrRestriction] internal Archetype* Archetype;

        public bool Valid => Archetype != null;

        public static bool operator ==(EntityArchetype lhs, EntityArchetype rhs)
        {
            return lhs.Archetype == rhs.Archetype;
        }

        public static bool operator !=(EntityArchetype lhs, EntityArchetype rhs)
        {
            return lhs.Archetype != rhs.Archetype;
        }

        public override bool Equals(object compare)
        {
            return this == (EntityArchetype) compare;
        }

        public bool Equals(EntityArchetype entityArchetype)
        {
            return Archetype == entityArchetype.Archetype;
        }

        public override int GetHashCode()
        {
            return (int) Archetype;
        }

        public ComponentType[] ComponentTypes
        {
            get
            {
                var types = new ComponentType[Archetype->TypesCount];
                for (var i = 0; i < types.Length; ++i)
                    types[i] = Archetype->Types[i].ToComponentType();
                return types;
            }
        }

        public int ChunkCount => Archetype->Chunks.Count;

        public int ChunkCapacity => Archetype->ChunkCapacity;
    }

    public struct Entity : IEquatable<Entity>
    {
        public int Index;
        public int Version;

        public static bool operator ==(Entity lhs, Entity rhs)
        {
            return lhs.Index == rhs.Index && lhs.Version == rhs.Version;
        }

        public static bool operator !=(Entity lhs, Entity rhs)
        {
            return lhs.Index != rhs.Index || lhs.Version != rhs.Version;
        }

        public override bool Equals(object compare)
        {
            return this == (Entity) compare;
        }

        public override int GetHashCode()
        {
            return Index;
        }

        public static Entity Null => new Entity();

        public bool Equals(Entity entity)
        {
            return entity.Index == Index && entity.Version == Version;
        }

        public override string ToString()
        {
            return $"Entity Index: {Index} Version: {Version}";
        }
    }

    [Preserve]
    public sealed unsafe class EntityManager : ScriptBehaviourManager
    {
        EntityDataManager* m_Entities;

        ArchetypeManager m_ArchetypeManager;
        EntityGroupManager m_GroupManager;

        internal SharedComponentDataManager m_SharedComponentManager;

        ExclusiveEntityTransaction m_ExclusiveEntityTransaction;

        World m_World;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal int                      m_InsideForEach;
#endif

        internal EntityDataManager* Entities
        {
            get { return m_Entities; }
        }

        internal EntityGroupManager GroupManager
        {
            get { return m_GroupManager; }
        }

        internal ArchetypeManager ArchetypeManager
        {
            get { return m_ArchetypeManager; }
        }

        public int Version => IsCreated ? m_Entities->Version : 0;

        public uint GlobalSystemVersion => IsCreated ? Entities->GlobalSystemVersion : 0;

        public bool IsCreated => m_Entities != null;

        public int EntityCapacity
        {
            get { return Entities->Capacity; }
            set
            {
                BeforeStructuralChange();
                Entities->Capacity = value;
            }
        }

        internal ComponentJobSafetyManager ComponentJobSafetyManager { get; private set; }

        public JobHandle ExclusiveEntityTransactionDependency
        {
            get { return ComponentJobSafetyManager.ExclusiveTransactionDependency; }
            set { ComponentJobSafetyManager.ExclusiveTransactionDependency = value; }
        }

        EntityManagerDebug m_Debug;

        public EntityManagerDebug Debug => m_Debug ?? (m_Debug = new EntityManagerDebug(this));

        protected override void OnBeforeCreateManagerInternal(World world)
        {
            m_World = world;
        }

        protected override void OnBeforeDestroyManagerInternal()
        {
        }

        protected override void OnAfterDestroyManagerInternal()
        {
        }

        protected override void OnCreateManager()
        {
            TypeManager.Initialize();

            m_Entities = (EntityDataManager*) UnsafeUtility.Malloc(sizeof(EntityDataManager), 64, Allocator.Persistent);
            m_Entities->OnCreate();

            m_SharedComponentManager = new SharedComponentDataManager();

            ComponentJobSafetyManager = new ComponentJobSafetyManager();
            m_GroupManager = new EntityGroupManager(ComponentJobSafetyManager);

            m_ArchetypeManager = new ArchetypeManager(m_SharedComponentManager, m_Entities, m_GroupManager);

            m_ExclusiveEntityTransaction = new ExclusiveEntityTransaction(ArchetypeManager, m_GroupManager,
                m_SharedComponentManager, Entities);
        }

        protected override void OnDestroyManager()
        {
            EndExclusiveEntityTransaction();

            ComponentJobSafetyManager.PreDisposeCheck();

            // Clean up all entities. This is needed to free all internal buffer allocations so memory is not leaked.
            m_ArchetypeManager.UnlockAllChunks(m_Entities);
            using (var allEntities = GetAllEntities())
            {
                DestroyEntity(allEntities);
            }

            ComponentJobSafetyManager.Dispose();
            ComponentJobSafetyManager = null;

            m_Entities->OnDestroy();
            UnsafeUtility.Free(m_Entities, Allocator.Persistent);
            m_Entities = null;
            m_ArchetypeManager.Dispose();
            m_ArchetypeManager = null;
            m_GroupManager.Dispose();
            m_GroupManager = null;
            m_ExclusiveEntityTransaction.OnDestroyManager();

            m_SharedComponentManager.Dispose();
        }

        internal override void InternalUpdate()
        {
        }

        internal static int FillSortedArchetypeArray(ComponentTypeInArchetype* dst, ComponentType* requiredComponents, int count)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (count + 1 > 1024)
                throw new System.ArgumentException($"Archetypes can't hold more than 1024 components");
#endif

            dst[0] = new ComponentTypeInArchetype(ComponentType.Create<Entity>());
            for (var i = 0; i < count; ++i)
                SortingUtilities.InsertSorted(dst, i + 1, requiredComponents[i]);
            return count + 1;
        }

        public ComponentGroup CreateComponentGroup(params ComponentType[] requiredComponents)
        {
            fixed (ComponentType* requiredComponentsPtr = requiredComponents)
            {
                return m_GroupManager.CreateEntityGroup(ArchetypeManager, Entities, requiredComponentsPtr, requiredComponents.Length);
            }
        }
        internal ComponentGroup CreateComponentGroup(ComponentType* requiredComponents, int count)
        {
            return m_GroupManager.CreateEntityGroup(ArchetypeManager, Entities, requiredComponents, count);
        }
        internal ComponentGroup CreateComponentGroup(params EntityArchetypeQuery[] queries)
        {
            return m_GroupManager.CreateEntityGroup(ArchetypeManager, Entities, queries);
        }

        internal EntityArchetype CreateArchetype(ComponentType* types, int count)
        {
            ComponentTypeInArchetype* typesInArchetype = stackalloc ComponentTypeInArchetype[count + 1];
            var cachedComponentCount = FillSortedArchetypeArray(typesInArchetype, types, count);

            // Lookup existing archetype (cheap)
            EntityArchetype entityArchetype;
            entityArchetype.Archetype =
                ArchetypeManager.GetExistingArchetype(typesInArchetype, cachedComponentCount);
            if (entityArchetype.Archetype != null)
                return entityArchetype;

            // Creating an archetype invalidates all iterators / jobs etc
            // because it affects the live iteration linked lists...
            BeforeStructuralChange();

            entityArchetype.Archetype = ArchetypeManager.GetOrCreateArchetype(typesInArchetype,
                cachedComponentCount, m_GroupManager);
            return entityArchetype;
        }

        public EntityArchetype CreateArchetype(params ComponentType[] types)
        {
            fixed (ComponentType* typesPtr = types)
            {
                return CreateArchetype(typesPtr, types.Length);
            }
        }

        /// <summary>
        /// Create a set of Entity of the specified EntityArchetype.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="entities"></param>
        public void CreateEntity(EntityArchetype archetype, NativeArray<Entity> entities)
        {
            CreateEntityInternal(archetype, (Entity*) entities.GetUnsafePtr(), entities.Length);
        }

        public void LockChunk(ArchetypeChunk chunk)
        {
            LockChunksInternal(&chunk, 1);
        }

        public void LockChunk(NativeArray<ArchetypeChunk> chunks)
        {
            LockChunksInternal((ArchetypeChunk*) chunks.GetUnsafePtr(), chunks.Length);
        }

        internal void LockChunksInternal(ArchetypeChunk* chunks, int count)
        {
            Entities->LockChunks(chunks, count);
        }
        public void UnlockChunk(ArchetypeChunk chunk)
        {
            UnlockChunksInternal(&chunk, 1);
        }

        public void UnlockChunk(NativeArray<ArchetypeChunk> chunks)
        {
            UnlockChunksInternal((ArchetypeChunk*) chunks.GetUnsafePtr(), chunks.Length);
        }

        internal void UnlockChunksInternal(ArchetypeChunk* chunks, int count)
        {
            Entities->UnlockChunks(chunks, count);
        }

        /// <summary>
        /// Create a set of Chunk of the specified EntityArchetype.
        /// </summary>
        /// <param name="archetype"></param>
        /// <param name="chunks"></param>
        /// <param name="entityCount"></param>
        public void CreateChunk(EntityArchetype archetype, NativeArray<ArchetypeChunk> chunks, int entityCount)
        {
            CreateChunkInternal(archetype, (ArchetypeChunk*) chunks.GetUnsafePtr(), entityCount);
        }

        public ArchetypeChunk GetChunk(Entity entity)
        {
            var chunk = Entities->GetComponentChunk(entity);
            return new ArchetypeChunk {m_Chunk = chunk};
        }

        public Entity CreateEntity(EntityArchetype archetype)
        {
            Entity entity;
            CreateEntityInternal(archetype, &entity, 1);
            return entity;
        }

        public Entity CreateEntity(params ComponentType[] types)
        {
            return CreateEntity(CreateArchetype(types));
        }

        internal void CreateEntityInternal(EntityArchetype archetype, Entity* entities, int count)
        {
            BeforeStructuralChange();
            Entities->CreateEntities(ArchetypeManager, archetype.Archetype, entities, count);
        }

        internal void CreateChunkInternal(EntityArchetype archetype, ArchetypeChunk* chunks, int entityCount)
        {
            BeforeStructuralChange();
            Entities->CreateChunks(ArchetypeManager, archetype.Archetype, chunks, entityCount);
        }

        NativeArray<Entity> GetTempEntityArray(ComponentGroup group)
        {
            var entityGroupArray = group.GetEntityArray();
            var entityArray = new NativeArray<Entity>(entityGroupArray.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            entityGroupArray.CopyTo(entityArray);
            return entityArray;
        }

        public void DestroyEntity(ComponentGroup componentGroupFilter)
        {
            //@TODO: When destroying entities with componentGroupFilter we assume that any LinkedEntityGroup also get destroyed
            //       We should have some sort validation that everything is included, and either give an error message or have a fast enough path to handle it...

            Profiler.BeginSample("DestroyEntity(ComponentGroup componentGroupFilter)");

            Profiler.BeginSample("GetAllMatchingChunks");
            var chunks = componentGroupFilter.GetAllMatchingChunks(Allocator.Temp);
            Profiler.EndSample();

            Profiler.BeginSample("DeleteChunks");
            EntityDataManager.DeleteChunks(chunks, Entities, ArchetypeManager, m_SharedComponentManager);
            chunks.Dispose();
            Profiler.EndSample();

            Profiler.EndSample();
        }

        public void DestroyEntity(NativeArray<Entity> entities)
        {
            DestroyEntityInternal((Entity*) entities.GetUnsafeReadOnlyPtr(), entities.Length);
        }

        public void DestroyEntity(NativeSlice<Entity> entities)
        {
            DestroyEntityInternal((Entity*) entities.GetUnsafeReadOnlyPtr(), entities.Length);
        }

        public void DestroyEntity(Entity entity)
        {
            DestroyEntityInternal(&entity, 1);
        }

        private void DestroyEntityInternal(Entity* entities, int count)
        {
            BeforeStructuralChange();
            Entities->AssertEntitiesExist(entities, count);
            Entities->AssertChunksUnlocked(entities, count);
            EntityDataManager.TryRemoveEntityId(entities, count, Entities, ArchetypeManager, m_SharedComponentManager);
        }

#if UNITY_EDITOR
        public string GetName(Entity entity)
        {
            return Entities->GetName(entity);
        }

        public void SetName(Entity entity, string name)
        {
            Entities->SetName(entity, name);
        }
#endif

        public bool Exists(Entity entity)
        {
            return Entities->Exists(entity);
        }

        public bool HasComponent<T>(Entity entity)
        {
            return Entities->HasComponent(entity, ComponentType.Create<T>());
        }

        public bool HasComponent(Entity entity, ComponentType type)
        {
            return Entities->HasComponent(entity, type);
        }

        public Entity Instantiate(Entity srcEntity)
        {
            Entity entity;
            InstantiateInternal(srcEntity, &entity, 1);
            return entity;
        }

        public void Instantiate(Entity srcEntity, NativeArray<Entity> outputEntities)
        {
            InstantiateInternal(srcEntity, (Entity*) outputEntities.GetUnsafePtr(), outputEntities.Length);
        }

        internal void InstantiateInternal(Entity srcEntity, Entity* outputEntities, int count)
        {
            BeforeStructuralChange();
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (!Entities->Exists(srcEntity))
                throw new ArgumentException("srcEntity is not a valid entity");
#endif

            Entities->InstantiateEntities(ArchetypeManager, m_SharedComponentManager, srcEntity, outputEntities, count);
        }

        public void AddComponent(Entity entity, ComponentType type)
        {
            BeforeStructuralChange();
            Entities->AssertEntitiesExist(&entity, 1);
            Entities->AddComponent(entity, type, ArchetypeManager, m_SharedComponentManager, m_GroupManager);
        }

        //@TODO: optimize for batch
        public void AddComponent(ComponentGroup componentGroupFilter, ComponentType type)
        {
            // @TODO: Don't copy entity array,
            // take advantage of inherent chunk structure to do faster destruction
            if (componentGroupFilter.CalculateLength() == 0)
                return;

            var entityArray = GetTempEntityArray(componentGroupFilter);
            AddComponent(entityArray, type);
            entityArray.Dispose();
        }

        //@TODO: optimize for batch
        public void AddComponent(NativeArray<Entity> entities, ComponentType type)
        {
            for(int i =0;i != entities.Length;i++)
                AddComponent(entities[i], type);
        }

        public void AddComponents(Entity entity, ComponentTypes types)
        {
            BeforeStructuralChange();
            Entities->AssertEntitiesExist(&entity, 1);
            Entities->AddComponents(entity, types, ArchetypeManager, m_SharedComponentManager, m_GroupManager);
        }

        public void RemoveComponent(Entity entity, ComponentType type)
        {
            BeforeStructuralChange();
            Entities->AssertEntityHasComponent(entity, type);
            Entities->RemoveComponent(entity, type, ArchetypeManager, m_SharedComponentManager, m_GroupManager);

            var archetype = Entities->GetArchetype(entity);
            if (archetype->SystemStateCleanupComplete)
            {
                EntityDataManager.TryRemoveEntityId(&entity, 1, Entities, ArchetypeManager, m_SharedComponentManager);
            }
        }

        //@TODO: optimize for batch
        public void RemoveComponent(ComponentGroup componentGroupFilter, ComponentType type)
        {
            // @TODO: Don't copy entity array,
            // take advantage of inherent chunk structure to do faster destruction
            if (componentGroupFilter.CalculateLength() == 0)
                return;

            var entityArray = GetTempEntityArray(componentGroupFilter);
            RemoveComponent(entityArray, type);
            entityArray.Dispose();
        }

        //@TODO: optimize for batch
        public void RemoveComponent(NativeArray<Entity> entities, ComponentType type)
        {
            for(int i =0;i != entities.Length;i++)
                RemoveComponent(entities[i], type);
        }

        public void RemoveComponent<T>(Entity entity)
        {
            RemoveComponent(entity, ComponentType.Create<T>());
        }

        public void AddComponentData<T>(Entity entity, T componentData) where T : struct, IComponentData
        {
            var type = ComponentType.Create<T>();
            AddComponent(entity, type);
            if (!type.IsZeroSized)
                SetComponentData(entity, componentData);
        }

        public void RemoveChunkComponent<T>(Entity entity)
        {
            RemoveComponent(entity, ComponentType.ChunkComponent<T>());
        }

        public void AddChunkComponentData<T>(Entity entity) where T : struct, IComponentData
        {
            AddComponent(entity, ComponentType.ChunkComponent<T>());
        }

        public void RemoveChunkComponentData<T>(ArchetypeChunk archetypeChunk) where T : struct, IComponentData
        {
            var type = ComponentType.ChunkComponent<T>();
            Entities->RemoveChunkComponent(archetypeChunk, type, ArchetypeManager, m_GroupManager);
        }

        public void AddChunkComponentData<T>(ArchetypeChunk archetypeChunk, T componentData) where T : struct, IComponentData
        {
            var type = ComponentType.ChunkComponent<T>();
            Entities->AddChunkComponent(archetypeChunk, type, ArchetypeManager, m_GroupManager);
            if (!type.IsZeroSized)
                SetComponentData(archetypeChunk.m_Chunk->metaChunkEntity, componentData);
        }

        public DynamicBuffer<T> AddBuffer<T>(Entity entity) where T : struct, IBufferElementData
        {
            AddComponent(entity, ComponentType.Create<T>());
            return GetBuffer<T>(entity);
        }

        internal ComponentDataFromEntity<T> GetComponentDataFromEntity<T>(bool isReadOnly = false)
            where T : struct, IComponentData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            return GetComponentDataFromEntity<T>(typeIndex, isReadOnly);
        }

        internal ComponentDataFromEntity<T> GetComponentDataFromEntity<T>(int typeIndex, bool isReadOnly)
            where T : struct, IComponentData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new ComponentDataFromEntity<T>(typeIndex, Entities,
                ComponentJobSafetyManager.GetSafetyHandle(typeIndex, isReadOnly));
#else
            return new ComponentDataFromEntity<T>(typeIndex, m_Entities);
#endif
        }

        internal BufferFromEntity<T> GetBufferFromEntity<T>(bool isReadOnly = false)
            where T : struct, IBufferElementData
        {
            return GetBufferFromEntity<T>(TypeManager.GetTypeIndex<T>(), isReadOnly);
        }

        internal BufferFromEntity<T> GetBufferFromEntity<T>(int typeIndex, bool isReadOnly = false)
            where T : struct, IBufferElementData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new BufferFromEntity<T>(typeIndex, Entities, isReadOnly,
                ComponentJobSafetyManager.GetSafetyHandle(typeIndex, isReadOnly),
                ComponentJobSafetyManager.GetBufferSafetyHandle(typeIndex));
#else
            return new BufferFromEntity<T>(typeIndex, m_Entities, isReadOnly);
#endif
        }

        public T GetComponentData<T>(Entity entity) where T : struct, IComponentData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();

            Entities->AssertEntityHasComponent(entity, typeIndex);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (ComponentType.FromTypeIndex(typeIndex).IsZeroSized)
                throw new System.ArgumentException($"GetComponentData<{typeof(T)}> can not be called with a zero sized component.");
#endif

            ComponentJobSafetyManager.CompleteWriteDependency(typeIndex);

            var ptr = Entities->GetComponentDataWithTypeRO(entity, typeIndex);

            T value;
            UnsafeUtility.CopyPtrToStructure(ptr, out value);
            return value;
        }

        public void SetComponentData<T>(Entity entity, T componentData) where T : struct, IComponentData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();

            Entities->AssertEntityHasComponent(entity, typeIndex);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (ComponentType.FromTypeIndex(typeIndex).IsZeroSized)
                throw new System.ArgumentException($"SetComponentData<{typeof(T)}> can not be called with a zero sized component.");
#endif

            ComponentJobSafetyManager.CompleteReadAndWriteDependency(typeIndex);

            var ptr = Entities->GetComponentDataWithTypeRW(entity, typeIndex, Entities->GlobalSystemVersion);
            UnsafeUtility.CopyStructureToPtr(ref componentData, ptr);
        }

        public T GetChunkComponentData<T>(ArchetypeChunk chunk) where T : struct, IComponentData
        {
            var metaChunkEntity = chunk.m_Chunk->metaChunkEntity;
            return GetComponentData<T>(metaChunkEntity);
        }

        public T GetChunkComponentData<T>(Entity entity) where T : struct, IComponentData
        {
            Entities->AssertEntitiesExist(&entity, 1);
            var chunk = Entities->GetComponentChunk(entity);
            var metaChunkEntity = chunk->metaChunkEntity;
            return GetComponentData<T>(metaChunkEntity);
        }

        public void SetChunkComponentData<T>(ArchetypeChunk chunk, T componentValue) where T : struct, IComponentData
        {
            var metaChunkEntity = chunk.m_Chunk->metaChunkEntity;
            SetComponentData<T>(metaChunkEntity, componentValue);
        }

        internal void SetComponentObject(Entity entity, ComponentType componentType, object componentObject)
        {
            Entities->AssertEntityHasComponent(entity, componentType.TypeIndex);

            //@TODO
            Chunk* chunk;
            int chunkIndex;
            Entities->GetComponentChunk(entity, out chunk, out chunkIndex);
            ArchetypeManager.SetManagedObject(chunk, componentType, chunkIndex, componentObject);
        }

        public int GetSharedComponentCount()
        {
            return m_SharedComponentManager.GetSharedComponentCount();
        }

        public void GetAllUniqueSharedComponentData<T>(List<T> sharedComponentValues)
            where T : struct, ISharedComponentData
        {
            m_SharedComponentManager.GetAllUniqueSharedComponents(sharedComponentValues);
        }

        public void GetAllUniqueSharedComponentData<T>(List<T> sharedComponentValues, List<int> sharedComponentIndices)
            where T : struct, ISharedComponentData
        {
            m_SharedComponentManager.GetAllUniqueSharedComponents(sharedComponentValues, sharedComponentIndices);
        }

        public T GetSharedComponentData<T>(Entity entity) where T : struct, ISharedComponentData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            Entities->AssertEntityHasComponent(entity, typeIndex);

            var sharedComponentIndex = Entities->GetSharedComponentDataIndex(entity, typeIndex);
            return m_SharedComponentManager.GetSharedComponentData<T>(sharedComponentIndex);
        }

        public T GetSharedComponentData<T>(int sharedComponentIndex) where T : struct, ISharedComponentData
        {
            return m_SharedComponentManager.GetSharedComponentData<T>(sharedComponentIndex);
        }

        public void AddSharedComponentData<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
        {
            //TODO: optimize this (no need to move the entity to a new chunk twice)
            AddComponent(entity, ComponentType.Create<T>());
            SetSharedComponentData(entity, componentData);
        }

        internal void AddSharedComponentDataBoxed(Entity entity, int typeIndex, int hashCode, object componentData)
        {
            //TODO: optimize this (no need to move the entity to a new chunk twice)
            AddComponent(entity, ComponentType.FromTypeIndex(typeIndex));
            SetSharedComponentDataBoxed(entity, typeIndex, hashCode, componentData);
        }

        public void SetSharedComponentData<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
        {
            BeforeStructuralChange();

            var typeIndex = TypeManager.GetTypeIndex<T>();
            Entities->AssertEntityHasComponent(entity, typeIndex);

            var newSharedComponentDataIndex = m_SharedComponentManager.InsertSharedComponent(componentData);
            Entities->SetSharedComponentDataIndex(ArchetypeManager, m_SharedComponentManager, entity, typeIndex,
                newSharedComponentDataIndex);
            m_SharedComponentManager.RemoveReference(newSharedComponentDataIndex);
        }

        internal void SetSharedComponentDataBoxed(Entity entity, int typeIndex, object componentData)
        {
            var hashCode = SharedComponentDataManager.GetHashCodeFast(componentData, typeIndex);
            SetSharedComponentDataBoxed(entity, typeIndex, hashCode, componentData);
        }

        internal void SetSharedComponentDataBoxed(Entity entity, int typeIndex, int hashCode, object componentData)
        {
            BeforeStructuralChange();

            Entities->AssertEntityHasComponent(entity, typeIndex);

            var newSharedComponentDataIndex = 0;
            if (componentData != null) // null means default
                newSharedComponentDataIndex = m_SharedComponentManager.InsertSharedComponentAssumeNonDefault(typeIndex,
                    hashCode, componentData);

            Entities->SetSharedComponentDataIndex(ArchetypeManager, m_SharedComponentManager, entity, typeIndex,
                newSharedComponentDataIndex);
            m_SharedComponentManager.RemoveReference(newSharedComponentDataIndex);
        }

        public DynamicBuffer<T> GetBuffer<T>(Entity entity) where T : struct, IBufferElementData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            Entities->AssertEntityHasComponent(entity, typeIndex);
            if (TypeManager.GetTypeInfo<T>().Category != TypeManager.TypeCategory.BufferData)
                throw new ArgumentException(
                    $"GetBuffer<{typeof(T)}> may not be IComponentData or ISharedComponentData; currently {TypeManager.GetTypeInfo<T>().Category}");
#endif

            ComponentJobSafetyManager.CompleteReadAndWriteDependency(typeIndex);

            BufferHeader* header = (BufferHeader*) Entities->GetComponentDataWithTypeRW(entity, typeIndex, Entities->GlobalSystemVersion);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new DynamicBuffer<T>(header, ComponentJobSafetyManager.GetSafetyHandle(typeIndex, false), ComponentJobSafetyManager.GetBufferSafetyHandle(typeIndex), false);
#else
            return new DynamicBuffer<T>(header);
#endif
        }

        internal void* GetBufferRawRW(Entity entity, int typeIndex)
        {
            Entities->AssertEntityHasComponent(entity, typeIndex);

            ComponentJobSafetyManager.CompleteReadAndWriteDependency(typeIndex);

            BufferHeader* header = (BufferHeader*)Entities->GetComponentDataWithTypeRW(entity, typeIndex, Entities->GlobalSystemVersion);

            return BufferHeader.GetElementPointer(header);
        }

        internal int GetBufferLength(Entity entity, int typeIndex)
        {
            Entities->AssertEntityHasComponent(entity, typeIndex);

            ComponentJobSafetyManager.CompleteReadAndWriteDependency(typeIndex);

            BufferHeader* header = (BufferHeader*)Entities->GetComponentDataWithTypeRW(entity, typeIndex, Entities->GlobalSystemVersion);

            return header->Length;
        }

        internal uint GetChunkVersionHash(Entity entity)
        {
            var chunk = Entities->GetComponentChunk(entity);
            var typeCount = chunk->Archetype->TypesCount;
            return math.hash((void*)chunk->ChangeVersion, typeCount*UnsafeUtility.SizeOf<uint>());
        }

        public NativeArray<Entity> GetAllEntities(Allocator allocator = Allocator.Temp)
        {
            BeforeStructuralChange();

            //@TODO: Doesn't include prefab & disabled combined query
            var enabledQuery = new EntityArchetypeQuery
            {
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
                All = Array.Empty<ComponentType>(),
            };
            var disabledQuery = new EntityArchetypeQuery
            {
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
                All = new ComponentType[] {typeof(Disabled)}
            };
            var prefabQuery = new EntityArchetypeQuery
            {
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
                All = new ComponentType[] {typeof(Prefab)}
            };
            var archetypes = new NativeList<EntityArchetype>(Allocator.TempJob);

            AddMatchingArchetypes(enabledQuery, archetypes);
            AddMatchingArchetypes(disabledQuery, archetypes);
            AddMatchingArchetypes(prefabQuery, archetypes);

            var chunks = CreateArchetypeChunkArray(archetypes, Allocator.TempJob);
            var count = ArchetypeChunkArray.CalculateEntityCount(chunks);
            var array = new NativeArray<Entity>(count, allocator);
            var entityType = GetArchetypeChunkEntityType();
            var offset = 0;

            for (int i = 0; i < chunks.Length; i++)
            {
                var chunk = chunks[i];
                var entities = chunk.GetNativeArray(entityType);
                array.Slice(offset, entities.Length).CopyFrom(entities);
                offset += entities.Length;
            }

            chunks.Dispose();
            archetypes.Dispose();

            return array;
        }

        public NativeArray<ArchetypeChunk> GetAllChunks(Allocator allocator = Allocator.TempJob)
        {
            BeforeStructuralChange();

            var enabledQuery = new EntityArchetypeQuery
            {
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
                All = Array.Empty<ComponentType>(),
            };
            var disabledQuery = new EntityArchetypeQuery
            {
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
                All = new ComponentType[] {typeof(Disabled)}
            };
            var prefabQuery = new EntityArchetypeQuery
            {
                Any = Array.Empty<ComponentType>(),
                None = Array.Empty<ComponentType>(),
                All = new ComponentType[] {typeof(Prefab)}
            };
            var archetypes = new NativeList<EntityArchetype>(Allocator.TempJob);

            AddMatchingArchetypes(enabledQuery, archetypes);
            AddMatchingArchetypes(disabledQuery, archetypes);
            AddMatchingArchetypes(prefabQuery, archetypes);

            var chunks = CreateArchetypeChunkArray(archetypes, allocator);
            archetypes.Dispose();
            return chunks;
        }

        public NativeArray<ComponentType> GetComponentTypes(Entity entity, Allocator allocator = Allocator.Temp)
        {
            Entities->AssertEntitiesExist(&entity, 1);

            var archetype = Entities->GetArchetype(entity);

            var components = new NativeArray<ComponentType>(archetype->TypesCount - 1, allocator);

            for (var i = 1; i < archetype->TypesCount; i++)
                components[i - 1] = archetype->Types[i].ToComponentType();

            return components;
        }

        internal void SetBufferRaw(Entity entity, int componentTypeIndex, BufferHeader* tempBuffer, int sizeInChunk)
        {
            Entities->AssertEntityHasComponent(entity, componentTypeIndex);

            ComponentJobSafetyManager.CompleteReadAndWriteDependency(componentTypeIndex);

            var ptr = Entities->GetComponentDataWithTypeRW(entity, componentTypeIndex, Entities->GlobalSystemVersion);

            BufferHeader.Destroy((BufferHeader*)ptr);

            UnsafeUtility.MemCpy(ptr, tempBuffer, sizeInChunk);
        }

        public int GetComponentCount(Entity entity)
        {
            Entities->AssertEntitiesExist(&entity, 1);
            var archetype = Entities->GetArchetype(entity);
            return archetype->TypesCount - 1;
        }

        internal int GetComponentTypeIndex(Entity entity, int index)
        {
            Entities->AssertEntitiesExist(&entity, 1);
            var archetype = Entities->GetArchetype(entity);

            if ((uint) index >= archetype->TypesCount) return -1;

            return archetype->Types[index + 1].TypeIndex;
        }

        internal void SetComponentDataRaw(Entity entity, int typeIndex, void* data, int size)
        {
            Entities->AssertEntityHasComponent(entity, typeIndex);

            ComponentJobSafetyManager.CompleteReadAndWriteDependency(typeIndex);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (TypeManager.GetTypeInfo(typeIndex).SizeInChunk != size)
                throw new System.ArgumentException($"SetComponentDataRaw<{TypeManager.GetType(typeIndex)}> can not be called with a zero sized component and must have same size as sizeof(T).");
#endif

            var ptr = Entities->GetComponentDataWithTypeRW(entity, typeIndex, Entities->GlobalSystemVersion);
            UnsafeUtility.MemCpy(ptr, data, size);
        }

        internal void* GetComponentDataRawRW(Entity entity, int typeIndex)
        {
            Entities->AssertEntityHasComponent(entity, typeIndex);

            ComponentJobSafetyManager.CompleteReadAndWriteDependency(typeIndex);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (TypeManager.GetTypeInfo(typeIndex).IsZeroSized)
                throw new System.ArgumentException($"GetComponentDataRaw<{TypeManager.GetType(typeIndex)}> can not be called with a zero sized component.");
#endif


            var ptr = Entities->GetComponentDataWithTypeRW(entity, typeIndex, Entities->GlobalSystemVersion);
            return ptr;
        }
#if !UNITY_CSHARP_TINY
        internal object GetSharedComponentData(Entity entity, int typeIndex)
        {
            Entities->AssertEntityHasComponent(entity, typeIndex);

            var sharedComponentIndex = Entities->GetSharedComponentDataIndex(entity, typeIndex);
            return m_SharedComponentManager.GetSharedComponentDataBoxed(sharedComponentIndex, typeIndex);
        }
#endif
        public int GetComponentOrderVersion<T>()
        {
            return Entities->GetComponentTypeOrderVersion(TypeManager.GetTypeIndex<T>());
        }

        public int GetSharedComponentOrderVersion<T>(T sharedComponent) where T : struct, ISharedComponentData
        {
            return m_SharedComponentManager.GetSharedComponentVersion(sharedComponent);
        }

        public ExclusiveEntityTransaction BeginExclusiveEntityTransaction()
        {
            ComponentJobSafetyManager.BeginExclusiveTransaction();
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_ExclusiveEntityTransaction.SetAtomicSafetyHandle(ComponentJobSafetyManager.ExclusiveTransactionSafety);
#endif
            return m_ExclusiveEntityTransaction;
        }

        public void EndExclusiveEntityTransaction()
        {
            ComponentJobSafetyManager.EndExclusiveTransaction();
        }

        private void BeforeStructuralChange()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (ComponentJobSafetyManager.IsInTransaction)
                throw new InvalidOperationException(
                    "Access to EntityManager is not allowed after EntityManager.BeginExclusiveEntityTransaction(); has been called.");

            if (m_InsideForEach != 0)
                throw new InvalidOperationException("EntityManager.AddComponent/RemoveComponent/CreateEntity/DestroyEntity are not allowed during ForEach. Please use PostUpdateCommandBuffer to delay applying those changes until after ForEach.");

#endif
            ComponentJobSafetyManager.CompleteAllJobsAndInvalidateArrays();
        }

        //@TODO: Not clear to me what this method is really for...
        public void CompleteAllJobs()
        {
            ComponentJobSafetyManager.CompleteAllJobsAndInvalidateArrays();
        }

        public void MoveEntitiesFrom(EntityManager srcEntities)
        {
            var entityRemapping = srcEntities.CreateEntityRemapArray(Allocator.TempJob);
            try
            {
                MoveEntitiesFrom(srcEntities, entityRemapping);
            }
            finally
            {
                entityRemapping.Dispose();
            }
        }
        public void MoveEntitiesFrom(EntityManager srcEntities, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (srcEntities == this)
                throw new ArgumentException("srcEntities must not be the same as this EntityManager.");

            if (!srcEntities.m_SharedComponentManager.AllSharedComponentReferencesAreFromChunks(srcEntities.ArchetypeManager))
                throw new ArgumentException("EntityManager.MoveEntitiesFrom failed - All ISharedComponentData references must be from EntityManager. (For example ComponentGroup.SetFilter with a shared component type is not allowed during EntityManager.MoveEntitiesFrom)");
#endif

            BeforeStructuralChange();
            srcEntities.BeforeStructuralChange();

            ArchetypeManager.MoveChunks(srcEntities, ArchetypeManager, m_GroupManager, Entities, m_SharedComponentManager, entityRemapping);

            //@TODO: Need to incrmeent the component versions based the moved chunks...
        }

        public NativeArray<EntityRemapUtility.EntityRemapInfo> CreateEntityRemapArray(Allocator allocator)
        {
            return new NativeArray<EntityRemapUtility.EntityRemapInfo>(m_Entities->Capacity, allocator);
        }

        public void MoveEntitiesFrom(EntityManager srcEntities, ComponentGroup filter, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if(filter.ArchetypeManager != srcEntities.ArchetypeManager)
                throw new ArgumentException("EntityManager.MoveEntitiesFrom failed - srcEntities and filter must belong to the same World)");
#endif
            var chunks = filter.GetAllMatchingChunks(Allocator.TempJob);
            MoveEntitiesFrom(srcEntities, chunks, entityRemapping);
            chunks.Dispose();
        }

        internal void MoveEntitiesFrom(EntityManager srcEntities, NativeArray<ArchetypeChunk> chunks, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (srcEntities == this)
                throw new ArgumentException("srcEntities must not be the same as this EntityManager.");
#endif

            BeforeStructuralChange();
            srcEntities.BeforeStructuralChange();

            ArchetypeManager.MoveChunks(srcEntities, chunks, ArchetypeManager, m_GroupManager, Entities, m_SharedComponentManager, entityRemapping);
        }

        public void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities)
        {
            var entityRemapping = srcEntities.CreateEntityRemapArray(Allocator.TempJob);
            try
            {
                MoveEntitiesFrom(out output, srcEntities, entityRemapping);
            }
            finally
            {
                entityRemapping.Dispose();
            }
        }
        public void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (srcEntities == this)
                throw new ArgumentException("srcEntities must not be the same as this EntityManager.");

            if (!srcEntities.m_SharedComponentManager.AllSharedComponentReferencesAreFromChunks(srcEntities.ArchetypeManager))
                throw new ArgumentException("EntityManager.MoveEntitiesFrom failed - All ISharedComponentData references must be from EntityManager. (For example ComponentGroup.SetFilter with a shared component type is not allowed during EntityManager.MoveEntitiesFrom)");
#endif

            BeforeStructuralChange();
            srcEntities.BeforeStructuralChange();

            ArchetypeManager.MoveChunks(srcEntities, ArchetypeManager, m_GroupManager, Entities, m_SharedComponentManager, entityRemapping);
            EntityRemapUtility.GetTargets(out output, entityRemapping);

            //@TODO: Need to incrmeent the component versions based the moved chunks...
        }
        public void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities, ComponentGroup filter, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if(filter.ArchetypeManager != srcEntities.ArchetypeManager)
                throw new ArgumentException("EntityManager.MoveEntitiesFrom failed - srcEntities and filter must belong to the same World)");
#endif
            var chunks = filter.GetAllMatchingChunks(Allocator.TempJob);
            MoveEntitiesFrom(out output, srcEntities, chunks, entityRemapping);
            chunks.Dispose();
        }

        internal void MoveEntitiesFrom(out NativeArray<Entity> output, EntityManager srcEntities, NativeArray<ArchetypeChunk> chunks, NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (srcEntities == this)
                throw new ArgumentException("srcEntities must not be the same as this EntityManager.");
#endif

            BeforeStructuralChange();
            srcEntities.BeforeStructuralChange();

            ArchetypeManager.MoveChunks(srcEntities, chunks, ArchetypeManager, m_GroupManager, Entities, m_SharedComponentManager, entityRemapping);
            EntityRemapUtility.GetTargets(out output, entityRemapping);
        }

        public List<Type> GetAssignableComponentTypes(Type interfaceType)
        {
            // #todo Cache this. It only can change when TypeManager.GetTypeCount() changes
            var componentTypeCount = TypeManager.GetTypeCount();
            var assignableTypes = new List<Type>();
            for (var i = 0; i < componentTypeCount; i++)
            {
                var type = TypeManager.GetType(i);
                if (interfaceType.IsAssignableFrom(type)) assignableTypes.Add(type);
            }

            return assignableTypes;
        }

        private bool TestMatchingArchetypeAny(Archetype* archetype, ComponentType* anyTypes, int anyCount)
        {
            if (anyCount == 0) return true;

            var componentTypes = archetype->Types;
            var componentTypesCount = archetype->TypesCount;
            for (var i = 0; i < componentTypesCount; i++)
            {
                var componentTypeIndex = componentTypes[i].TypeIndex;
                for (var j = 0; j < anyCount; j++)
                {
                    var anyTypeIndex = anyTypes[j].TypeIndex;
                    if (componentTypeIndex == anyTypeIndex) return true;
                }
            }

            return false;
        }

        private bool TestMatchingArchetypeNone(Archetype* archetype, ComponentType* noneTypes, int noneCount)
        {
            var componentTypes = archetype->Types;
            var componentTypesCount = archetype->TypesCount;
            for (var i = 0; i < componentTypesCount; i++)
            {
                var componentTypeIndex = componentTypes[i].TypeIndex;
                for (var j = 0; j < noneCount; j++)
                {
                    var noneTypeIndex = noneTypes[j].TypeIndex;
                    if (componentTypeIndex == noneTypeIndex) return false;
                }
            }

            return true;
        }

        private bool TestMatchingArchetypeAll(Archetype* archetype, ComponentType* allTypes, int allCount)
        {
            var componentTypes = archetype->Types;
            var componentTypesCount = archetype->TypesCount;
            var foundCount = 0;
            var disabledTypeIndex = TypeManager.GetTypeIndex<Disabled>();
            var prefabTypeIndex = TypeManager.GetTypeIndex<Prefab>();
            var requestedDisabled = false;
            var requestedPrefab = false;
            for (var i = 0; i < componentTypesCount; i++)
            {
                var componentTypeIndex = componentTypes[i].TypeIndex;
                for (var j = 0; j < allCount; j++)
                {
                    var allTypeIndex = allTypes[j].TypeIndex;
                    if (allTypeIndex == disabledTypeIndex)
                        requestedDisabled = true;
                    if (allTypeIndex == prefabTypeIndex)
                        requestedPrefab = true;
                    if (componentTypeIndex == allTypeIndex) foundCount++;
                }
            }

            if (archetype->Disabled && (!requestedDisabled))
                return false;
            if (archetype->Prefab && (!requestedPrefab))
                return false;

            return foundCount == allCount;
        }

        public void AddMatchingArchetypes(EntityArchetypeQuery query, NativeList<EntityArchetype> foundArchetypes)
        {
            var anyCount = query.Any.Length;
            var noneCount = query.None.Length;
            var allCount = query.All.Length;

            fixed (ComponentType* any = query.Any)
            {
                fixed (ComponentType* none = query.None)
                {
                    fixed (ComponentType* all = query.All)
                    {
                        for (var i = ArchetypeManager.m_Archetypes.Count - 1; i >= 0; --i)
                        {
                            var archetype = ArchetypeManager.m_Archetypes.p[i];
                            if (archetype->EntityCount == 0)
                                continue;
                            if (!TestMatchingArchetypeAny(archetype, any, anyCount))
                                continue;
                            if (!TestMatchingArchetypeNone(archetype, none, noneCount))
                                continue;
                            if (!TestMatchingArchetypeAll(archetype, all, allCount))
                                continue;

                            var entityArchetype = new EntityArchetype {Archetype = archetype};
                            var found = foundArchetypes.Contains(entityArchetype);
                            if (!found)
                                foundArchetypes.Add(entityArchetype);
                        }
                    }
                }
            }
        }

        public void GetAllArchetypes(NativeList<EntityArchetype> allArchetypes)
        {
            for(var i = ArchetypeManager.m_Archetypes.Count - 1; i >= 0; --i)
            {
                var archetype = ArchetypeManager.m_Archetypes.p[i];
                var entityArchetype = new EntityArchetype() { Archetype = archetype };
                allArchetypes.Add(entityArchetype);
            }
        }

        public NativeArray<ArchetypeChunk> CreateArchetypeChunkArray(NativeList<EntityArchetype> archetypes,
            Allocator allocator)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var safetyHandle = AtomicSafetyHandle.Create();
            return ArchetypeChunkArray.Create(archetypes, allocator, safetyHandle);
#else
            return ArchetypeChunkArray.Create(archetypes, allocator);
#endif
        }

        public NativeArray<ArchetypeChunk> CreateArchetypeChunkArray(EntityArchetypeQuery query, Allocator allocator)
        {
            var foundArchetypes = new NativeList<EntityArchetype>(Allocator.TempJob);
            AddMatchingArchetypes(query, foundArchetypes);
            var chunkStream = CreateArchetypeChunkArray(foundArchetypes, allocator);
            foundArchetypes.Dispose();
            return chunkStream;
        }

        public ArchetypeChunkComponentType<T> GetArchetypeChunkComponentType<T>(bool isReadOnly)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var typeIndex = TypeManager.GetTypeIndex<T>();
            return new ArchetypeChunkComponentType<T>(
                ComponentJobSafetyManager.GetSafetyHandle(typeIndex, isReadOnly), isReadOnly,
                GlobalSystemVersion);
#else
            return new ArchetypeChunkComponentType<T>(isReadOnly, GlobalSystemVersion);
#endif
        }

        public ArchetypeChunkBufferType<T> GetArchetypeChunkBufferType<T>(bool isReadOnly)
            where T : struct, IBufferElementData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            var typeIndex = TypeManager.GetTypeIndex<T>();
            return new ArchetypeChunkBufferType<T>(
                ComponentJobSafetyManager.GetSafetyHandle(typeIndex, isReadOnly),
                ComponentJobSafetyManager.GetBufferSafetyHandle(typeIndex),
                isReadOnly, GlobalSystemVersion);
#else
            return new ArchetypeChunkBufferType<T>(isReadOnly,GlobalSystemVersion);
#endif
        }

        public ArchetypeChunkSharedComponentType<T> GetArchetypeChunkSharedComponentType<T>()
            where T : struct, ISharedComponentData
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new ArchetypeChunkSharedComponentType<T>(
                ComponentJobSafetyManager.GetSafetyHandle(TypeManager.GetTypeIndex<T>(), true));
#else
            return new ArchetypeChunkSharedComponentType<T>(false);
#endif
        }

        public ArchetypeChunkEntityType GetArchetypeChunkEntityType()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new ArchetypeChunkEntityType(
                ComponentJobSafetyManager.GetSafetyHandle(TypeManager.GetTypeIndex<Entity>(), true));
#else
            return new ArchetypeChunkEntityType(false);
#endif
        }


        public void SwapComponents(ArchetypeChunk leftChunk, int leftIndex, ArchetypeChunk rightChunk, int rightIndex)
        {
            BeforeStructuralChange();
            ChunkDataUtility.SwapComponents(leftChunk.m_Chunk,leftIndex,rightChunk.m_Chunk,rightIndex,1);
        }

        public World World { get { return m_World; } }

        public void PrepareForDeserialize()
        {
            Assert.AreEqual(0, Debug.EntityCount);
            m_SharedComponentManager.PrepareForDeserialize();
        }

        public class EntityManagerDebug
        {
            private readonly EntityManager m_Manager;

            public EntityManagerDebug(EntityManager entityManager)
            {
                m_Manager = entityManager;
            }

            public void PoisonUnusedDataInAllChunks(EntityArchetype archetype, byte value)
            {
                for(var i = 0; i < archetype.Archetype->Chunks.Count; ++i)
                {
                    var chunk = archetype.Archetype->Chunks.p[i];
                    ChunkDataUtility.PoisonUnusedChunkData(chunk, value);
                }
            }

            public void SetGlobalSystemVersion(uint version)
            {
                m_Manager.Entities->GlobalSystemVersion = version;
            }

            public bool IsSharedComponentManagerEmpty()
            {
                return m_Manager.m_SharedComponentManager.IsEmpty();
            }

#if !UNITY_CSHARP_TINY
            internal static string GetArchetypeDebugString(Archetype* a)
            {
                var buf = new System.Text.StringBuilder();
                buf.Append("(");

                for (var i = 0; i < a->TypesCount; i++)
                {
                    var componentTypeInArchetype = a->Types[i];
                    if (i > 0)
                        buf.Append(", ");
                    buf.Append(componentTypeInArchetype.ToString());
                }

                buf.Append(")");
                return buf.ToString();
            }
#endif

            public int EntityCount
            {
                get
                {
                    var allEntities = m_Manager.GetAllEntities();
                    var count = allEntities.Length;
                    allEntities.Dispose();
                    return count;
                }
            }

            public void LogEntityInfo(Entity entity)
            {
                Unity.Debug.Log(GetEntityInfo(entity));
            }

            public string GetEntityInfo(Entity entity)
            {
                var archetype = m_Manager.Entities->GetArchetype(entity);
                #if !UNITY_CSHARP_TINY
                    var str = new System.Text.StringBuilder();
                    str.Append($"Entity {entity.Index}.{entity.Version}");
                    for (var i = 0; i < archetype->TypesCount; i++)
                    {
                        var componentTypeInArchetype = archetype->Types[i];
                        str.AppendFormat("  - {0}", componentTypeInArchetype.ToString());
                    }

                    return str.ToString();
                #else
                    // @TODO Tiny really needs a proper string/stringutils implementation
                    string str = $"Entity {entity.Index}.{entity.Version}";
                    for (var i = 0; i < archetype->TypesCount; i++)
                    {
                        var componentTypeInArchetype = archetype->Types[i];
                        str += "  - {0}" + componentTypeInArchetype.ToString();
                    }

                    return str;
                #endif
            }

#if !UNITY_CSHARP_TINY
            public object GetComponentBoxed(Entity entity, ComponentType type)
            {
                m_Manager.Entities->AssertEntityHasComponent(entity, type);

                var typeInfo = TypeManager.GetTypeInfo(type.TypeIndex);
                if (typeInfo.Category == TypeManager.TypeCategory.ComponentData)
                {
                    var obj = Activator.CreateInstance(TypeManager.GetType(type.TypeIndex));
                    if (!typeInfo.IsZeroSized)
                    {
                        ulong handle;
                        var ptr = (byte*)UnsafeUtility.PinGCObjectAndGetAddress(obj, out handle);
                        ptr += TypeManager.ObjectOffset;
                        var src = m_Manager.Entities->GetComponentDataWithTypeRO(entity, type.TypeIndex);
                        UnsafeUtility.MemCpy(ptr, src, TypeManager.GetTypeInfo(type.TypeIndex).SizeInChunk);

                        UnsafeUtility.ReleaseGCObject(handle);
                    }
                    return obj;
                }
                else if (typeInfo.Category == TypeManager.TypeCategory.ISharedComponentData)
                {
                    return m_Manager.GetSharedComponentData(entity, type.TypeIndex);
                }
                else
                {
                    throw new System.NotImplementedException();
                }
            }
#else
            public object GetComponentBoxed(Entity entity, ComponentType type)
            {
                throw new System.NotImplementedException();
            }
#endif

            public void CheckInternalConsistency()
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS

                //@TODO: Validate from perspective of componentgroup...
                var entityCountEntityData = m_Manager.Entities->CheckInternalConsistency();
                var entityCountArchetypeManager = m_Manager.ArchetypeManager.CheckInternalConsistency();
                Assert.AreEqual(entityCountEntityData, entityCountArchetypeManager);

                Assert.IsTrue(m_Manager.m_SharedComponentManager.AllSharedComponentReferencesAreFromChunks(m_Manager.m_ArchetypeManager));
                m_Manager.m_SharedComponentManager.CheckRefcounts();
#endif
            }
        }

        internal EntityArchetype GetEntityOnlyArchetype()
        {
            return new EntityArchetype{Archetype = m_ArchetypeManager.GetEntityOnlyArchetype(m_GroupManager)};
        }
    }
}
