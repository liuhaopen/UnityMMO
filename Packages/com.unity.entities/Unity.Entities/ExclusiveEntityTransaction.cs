using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
    [NativeContainer]
    public unsafe struct ExclusiveEntityTransaction
    {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private AtomicSafetyHandle m_Safety;
#endif
        [NativeDisableUnsafePtrRestriction] private GCHandle m_ArchetypeManager;

        [NativeDisableUnsafePtrRestriction] private GCHandle m_EntityGroupManager;

        [NativeDisableUnsafePtrRestriction] private GCHandle m_SharedComponentDataManager;

        [NativeDisableUnsafePtrRestriction] private EntityDataManager* m_Entities;

        internal SharedComponentDataManager SharedComponentDataManager
        {
            get
            {
                return (SharedComponentDataManager) m_SharedComponentDataManager.Target;
            }
        }

        internal ArchetypeManager ArchetypeManager
        {
            get
            {
                return (ArchetypeManager) m_ArchetypeManager.Target;
            }
        }

        
        
        internal ExclusiveEntityTransaction(ArchetypeManager archetypes, EntityGroupManager entityGroupManager,
            SharedComponentDataManager sharedComponentDataManager, EntityDataManager* data)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            m_Safety = new AtomicSafetyHandle();
#endif
            m_Entities = data;
            m_ArchetypeManager = GCHandle.Alloc(archetypes, GCHandleType.Weak);
            m_EntityGroupManager = GCHandle.Alloc(entityGroupManager, GCHandleType.Weak);
            m_SharedComponentDataManager = GCHandle.Alloc(sharedComponentDataManager, GCHandleType.Weak);
        }

        internal void OnDestroyManager()
        {
            m_ArchetypeManager.Free();
            m_EntityGroupManager.Free();
            m_SharedComponentDataManager.Free();
            m_Entities = null;
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal void SetAtomicSafetyHandle(AtomicSafetyHandle safety)
        {
            m_Safety = safety;
        }
#endif

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public void CheckAccess()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
#endif
        }

        internal EntityArchetype CreateArchetype(ComponentType* types, int count)
        {
            CheckAccess();

            var groupManager = (EntityGroupManager) m_EntityGroupManager.Target;

            ComponentTypeInArchetype* typesInArchetype = stackalloc ComponentTypeInArchetype[count + 1];
            var componentCount = EntityManager.FillSortedArchetypeArray(typesInArchetype, types, count);

            EntityArchetype type;
            type.Archetype = ArchetypeManager.GetOrCreateArchetype(typesInArchetype, componentCount, groupManager);

            return type;
        }

        public EntityArchetype CreateArchetype(params ComponentType[] types)
        {
            fixed (ComponentType* typesPtr = types)
            {
                return CreateArchetype(typesPtr, types.Length);
            }
        }

        public Entity CreateEntity(EntityArchetype archetype)
        {
            CheckAccess();

            Entity entity;
            CreateEntityInternal(archetype, &entity, 1);
            return entity;
        }

        public void CreateEntity(EntityArchetype archetype, NativeArray<Entity> entities)
        {
            CreateEntityInternal(archetype, (Entity*) entities.GetUnsafePtr(), entities.Length);
        }

        public Entity CreateEntity(params ComponentType[] types)
        {
            return CreateEntity(CreateArchetype(types));
        }

        private void CreateEntityInternal(EntityArchetype archetype, Entity* entities, int count)
        {
            CheckAccess();
            m_Entities->CreateEntities(ArchetypeManager, archetype.Archetype, entities, count);
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

        private void InstantiateInternal(Entity srcEntity, Entity* outputEntities, int count)
        {
            CheckAccess();

            if (!m_Entities->Exists(srcEntity))
                throw new ArgumentException("srcEntity is not a valid entity");

            m_Entities->InstantiateEntities(ArchetypeManager, SharedComponentDataManager, srcEntity, outputEntities, count);
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
            CheckAccess();
            m_Entities->AssertEntitiesExist(entities, count);

            EntityDataManager.TryRemoveEntityId(entities, count, m_Entities, ArchetypeManager, SharedComponentDataManager);
        }

        public void AddComponent(Entity entity, ComponentType type)
        {
            CheckAccess();

            var groupManager = (EntityGroupManager) m_EntityGroupManager.Target;

            m_Entities->AssertEntitiesExist(&entity, 1);
            m_Entities->AddComponent(entity, type, ArchetypeManager, SharedComponentDataManager, groupManager);
        }

        public void RemoveComponent(Entity entity, ComponentType type)
        {
            CheckAccess();

            var groupManager = (EntityGroupManager) m_EntityGroupManager.Target;

            m_Entities->AssertEntityHasComponent(entity, type);
            m_Entities->RemoveComponent(entity, type, ArchetypeManager, SharedComponentDataManager, groupManager);
        }

        public bool Exists(Entity entity)
        {
            CheckAccess();

            return m_Entities->Exists(entity);
        }

        public T GetComponentData<T>(Entity entity) where T : struct, IComponentData
        {
            CheckAccess();

            var typeIndex = TypeManager.GetTypeIndex<T>();
            m_Entities->AssertEntityHasComponent(entity, typeIndex);

            var ptr = m_Entities->GetComponentDataWithTypeRO(entity, typeIndex);

            T data;
            UnsafeUtility.CopyPtrToStructure(ptr, out data);
            return data;
        }

        public void SetComponentData<T>(Entity entity, T componentData) where T : struct, IComponentData
        {
            CheckAccess();

            var typeIndex = TypeManager.GetTypeIndex<T>();
            m_Entities->AssertEntityHasComponent(entity, typeIndex);

            var ptr = m_Entities->GetComponentDataWithTypeRW(entity, typeIndex, m_Entities->GlobalSystemVersion);
            UnsafeUtility.CopyStructureToPtr(ref componentData, ptr);
        }

        public T GetSharedComponentData<T>(Entity entity) where T : struct, ISharedComponentData
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            m_Entities->AssertEntityHasComponent(entity, typeIndex);

            var sharedComponentIndex = m_Entities->GetSharedComponentDataIndex(entity, typeIndex);
            return SharedComponentDataManager.GetSharedComponentData<T>(sharedComponentIndex);
        }

        public void SetSharedComponentData<T>(Entity entity, T componentData) where T : struct, ISharedComponentData
        {
            CheckAccess();

            var typeIndex = TypeManager.GetTypeIndex<T>();
            m_Entities->AssertEntityHasComponent(entity, typeIndex);

            var archetypeManager = ArchetypeManager;
            var sharedComponentDataManager = SharedComponentDataManager;

            var newSharedComponentDataIndex = sharedComponentDataManager.InsertSharedComponent(componentData);
            m_Entities->SetSharedComponentDataIndex(archetypeManager, sharedComponentDataManager, entity, typeIndex,
                newSharedComponentDataIndex);
            sharedComponentDataManager.RemoveReference(newSharedComponentDataIndex);
        }

        public DynamicBuffer<T> GetBuffer<T>(Entity entity) where T : struct, IBufferElementData
        {
            CheckAccess();

            var typeIndex = TypeManager.GetTypeIndex<T>();

            m_Entities->AssertEntityHasComponent(entity, typeIndex);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (TypeManager.GetTypeInfo<T>().Category != TypeManager.TypeCategory.BufferData)
                throw new ArgumentException(
                    $"GetBuffer<{typeof(T)}> may not be IComponentData or ISharedComponentData; currently {TypeManager.GetTypeInfo<T>().Category}");
#endif

            BufferHeader* header = (BufferHeader*) m_Entities->GetComponentDataWithTypeRW(entity, typeIndex, m_Entities->GlobalSystemVersion);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new DynamicBuffer<T>(header, m_Safety, m_Safety, false);
#else
            return new DynamicBuffer<T>(header);
#endif
        }

        internal void AllocateConsecutiveEntitiesForLoading(int count)
        {
            m_Entities->AllocateConsecutiveEntitiesForLoading(count);
        }


        internal void AddExistingChunk(Chunk* chunk)
        {
            ArchetypeManager.AddExistingChunk(chunk);
            m_Entities->AddExistingChunk(chunk);
        }
        
        
        public void SwapComponents(ArchetypeChunk leftChunk, int leftIndex, ArchetypeChunk rightChunk, int rightIndex)
        {
            CheckAccess();
            ChunkDataUtility.SwapComponents(leftChunk.m_Chunk,leftIndex,rightChunk.m_Chunk,rightIndex,1);
        }
    }
}
