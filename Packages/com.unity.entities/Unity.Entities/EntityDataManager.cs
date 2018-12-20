//#define USE_BURST_DESTROY

using System;
using System.Diagnostics;
using Unity.Assertions;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Unity.Entities
{
    public unsafe struct ComponentTypes
    {
        ResizableArray64Byte<int> m_sorted;

        public struct Masks
        {
            public UInt16 m_BufferMask;
            public UInt16 m_SystemStateComponentMask;
            public UInt16 m_SystemStateSharedComponentMask;
            public UInt16 m_SharedComponentMask;
            public UInt16 m_ZeroSizedMask;

            public bool IsSharedComponent(int index)
            {
                return (m_SharedComponentMask & (1 << index)) != 0;
            }

            public bool IsZeroSized(int index)
            {
                return (m_ZeroSizedMask & (1 << index)) != 0;
            }

            public int Buffers => math.countbits((UInt32)m_BufferMask);
            public int SystemStateComponents => math.countbits((UInt32)m_SystemStateComponentMask);
            public int SystemStateSharedComponents => math.countbits((UInt32)m_SystemStateSharedComponentMask);
            public int SharedComponents => math.countbits((UInt32)m_SharedComponentMask);
            public int ZeroSizeds => math.countbits((UInt32)m_ZeroSizedMask);
        }
        public Masks m_masks;

        private void ComputeMasks()
        {
            for (var i = 0; i < m_sorted.Length; ++i)
            {
                var typeIndex = m_sorted[i];
                var typeInfo = TypeManager.GetTypeInfo(typeIndex);
                var mask = (UInt16)(1 << i);
                if (typeInfo.BufferCapacity >= 0)
                    m_masks.m_BufferMask |= mask;
                if (typeInfo.IsSystemStateComponent)
                    m_masks.m_SystemStateComponentMask |= mask;
                if (typeInfo.IsSystemStateSharedComponent)
                    m_masks.m_SystemStateSharedComponentMask |= mask;
                if (TypeManager.TypeCategory.ISharedComponentData == typeInfo.Category)
                    m_masks.m_SharedComponentMask |= mask;
                if (typeInfo.IsZeroSized)
                    m_masks.m_ZeroSizedMask |= mask;
            }
        }

        public int Length
        {
            get => m_sorted.Length;
        }

        public int GetTypeIndex(int index)
        {
            return m_sorted[index];
        }
        public ComponentType GetComponentType(int index)
        {
            return TypeManager.GetType(m_sorted[index]);
        }

        public ComponentTypes(ComponentType a)
        {
            m_sorted = new ResizableArray64Byte<int>();
            m_masks = new Masks();
            m_sorted.Length = 1;
            var pointer = (int*)m_sorted.GetUnsafePointer();
            SortingUtilities.InsertSorted(pointer, 0, a.TypeIndex);
            ComputeMasks();
        }

        public ComponentTypes(ComponentType a, ComponentType b)
        {
            m_sorted = new ResizableArray64Byte<int>();
            m_masks = new Masks();
            m_sorted.Length = 2;
            var pointer = (int*)m_sorted.GetUnsafePointer();
            SortingUtilities.InsertSorted(pointer, 0, a.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 1, b.TypeIndex);
            ComputeMasks();
        }

        public ComponentTypes(ComponentType a, ComponentType b, ComponentType c)
        {
            m_sorted = new ResizableArray64Byte<int>();
            m_masks = new Masks();
            m_sorted.Length = 3;
            var pointer = (int*)m_sorted.GetUnsafePointer();
            SortingUtilities.InsertSorted(pointer, 0, a.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 1, b.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 2, c.TypeIndex);
            ComputeMasks();
        }

        public ComponentTypes(ComponentType a, ComponentType b, ComponentType c, ComponentType d)
        {
            m_sorted = new ResizableArray64Byte<int>();
            m_masks = new Masks();
            m_sorted.Length = 4;
            var pointer = (int*)m_sorted.GetUnsafePointer();
            SortingUtilities.InsertSorted(pointer, 0, a.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 1, b.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 2, c.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 3, d.TypeIndex);
            ComputeMasks();
        }

        public ComponentTypes(ComponentType a, ComponentType b, ComponentType c, ComponentType d, ComponentType e)
        {
            m_sorted = new ResizableArray64Byte<int>();
            m_masks = new Masks();
            m_sorted.Length = 5;
            var pointer = (int*)m_sorted.GetUnsafePointer();
            SortingUtilities.InsertSorted(pointer, 0, a.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 1, b.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 2, c.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 3, d.TypeIndex);
            SortingUtilities.InsertSorted(pointer, 4, e.TypeIndex);
            ComputeMasks();
        }

        public ComponentTypes(ComponentType[] componentType)
        {
            m_sorted = new ResizableArray64Byte<int>();
            m_masks = new Masks();
            m_sorted.Length = componentType.Length;
            var pointer = (int*)m_sorted.GetUnsafePointer();
            for(var i = 0; i < componentType.Length; ++i)
                SortingUtilities.InsertSorted(pointer, i, componentType[i].TypeIndex);
            ComputeMasks();
        }

    }

    internal struct CleanupEntity : IComponentData
    {
    }

    internal unsafe struct EntityDataManager
    {
#if USE_BURST_DESTROY
        private delegate Chunk* DeallocateDataEntitiesInChunkDelegate(EntityDataManager* entityDataManager, Entity* entities, int count, out int indexInChunk, out int batchCount);
        static DeallocateDataEntitiesInChunkDelegate ms_DeallocateDataEntitiesInChunkDelegate;
#endif

        private struct EntityChunkData
        {
            public Chunk* Chunk;
            public int IndexInChunk;
        }
        
        private struct EntityData
        {
            public int* Version;
            public Archetype** Archetype;
            public EntityChunkData* ChunkData;
        }

        private EntityData m_Entities;
        private int m_EntitiesCapacity;
        private int m_EntitiesFreeIndex;

        private int* m_ComponentTypeOrderVersion;
        public uint GlobalSystemVersion;

        public int Version => GetComponentTypeOrderVersion(TypeManager.GetTypeIndex<Entity>());

        public void IncrementGlobalSystemVersion()
        {
            ChangeVersionUtility.IncrementGlobalSystemVersion(ref GlobalSystemVersion);
        }

        private EntityData CreateEntityData(int newCapacity)
        {
            EntityData entities = new EntityData();

            var versionBytes   = (newCapacity * sizeof(int)             + 63) & ~63;
            var archetypeBytes = (newCapacity * sizeof(Archetype*)      + 63) & ~63;
            var chunkDataBytes = (newCapacity * sizeof(EntityChunkData) + 63) & ~63;

            var bytesToAllocate = versionBytes + archetypeBytes + chunkDataBytes;

            var bytes = (byte*)UnsafeUtility.Malloc(bytesToAllocate, 64, Allocator.Persistent);

            entities.Version   = (int*            )(bytes                                );
            entities.Archetype = (Archetype**     )(bytes + versionBytes                 );
            entities.ChunkData = (EntityChunkData*)(bytes + versionBytes + archetypeBytes);

            return entities;
        }

        private void FreeEntityData(ref EntityData entities)
        {
            UnsafeUtility.Free(entities.Version, Allocator.Persistent);
 
            entities.Version = null;
            entities.Archetype = null;
            entities.ChunkData = null;
        }

        private void CopyEntityData(ref EntityData dstEntityData, EntityData srcEntityData, long copySize)
        {
            UnsafeUtility.MemCpy(dstEntityData.Version,      srcEntityData.Version,      copySize * sizeof(int));
            UnsafeUtility.MemCpy(dstEntityData.Archetype,    srcEntityData.Archetype,    copySize * sizeof(Archetype*));
            UnsafeUtility.MemCpy(dstEntityData.ChunkData,    srcEntityData.ChunkData,    copySize * sizeof(EntityChunkData));
        }

        public void OnCreate()
        {
            m_EntitiesCapacity = 10;
            m_Entities = CreateEntityData(m_EntitiesCapacity);
            m_EntitiesFreeIndex = 0;
            GlobalSystemVersion = ChangeVersionUtility.InitialGlobalSystemVersion;
            InitializeAdditionalCapacity(0);

#if USE_BURST_DESTROY
            if (ms_DeallocateDataEntitiesInChunkDelegate == null)
            {
                ms_DeallocateDataEntitiesInChunkDelegate = DeallocateDataEntitiesInChunk;
                ms_DeallocateDataEntitiesInChunkDelegate =
 Burst.BurstDelegateCompiler.CompileDelegate(ms_DeallocateDataEntitiesInChunkDelegate);
            }
#endif

            const int componentTypeOrderVersionSize = sizeof(int) * TypeManager.MaximumTypesCount;
            m_ComponentTypeOrderVersion = (int*) UnsafeUtility.Malloc(componentTypeOrderVersionSize,
                UnsafeUtility.AlignOf<int>(), Allocator.Persistent);
            UnsafeUtility.MemClear(m_ComponentTypeOrderVersion, componentTypeOrderVersionSize);
        }

        public void OnDestroy()
        {
            FreeEntityData(ref m_Entities);
            m_EntitiesCapacity = 0;

            UnsafeUtility.Free(m_ComponentTypeOrderVersion, Allocator.Persistent);
            m_ComponentTypeOrderVersion = null;
        }

        private void InitializeAdditionalCapacity(int start)
        {
            for (var i = start; i != m_EntitiesCapacity; i++)
            {
                m_Entities.ChunkData[i].IndexInChunk = i + 1;
                m_Entities.Version[i] = 1;
                m_Entities.ChunkData[i].Chunk = null;
            }

            // Last entity indexInChunk identifies that we ran out of space...
            m_Entities.ChunkData[m_EntitiesCapacity - 1].IndexInChunk = -1;
        }

        private void IncreaseCapacity()
        {
            Capacity = 2 * Capacity;
        }

        public int Capacity
        {
            get { return m_EntitiesCapacity; }
            set
            {
                if (value <= m_EntitiesCapacity)
                    return;

                var newEntities = CreateEntityData(value);
                CopyEntityData(ref newEntities, m_Entities, m_EntitiesCapacity);
                FreeEntityData(ref m_Entities);
                
                var startNdx = m_EntitiesCapacity - 1;
                m_Entities = newEntities;
                m_EntitiesCapacity = value;

                InitializeAdditionalCapacity(startNdx);
            }
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void ValidateEntity(Entity entity)
        {
            if ((uint)entity.Index >= (uint)m_EntitiesCapacity)
                throw new ArgumentException(
                    "All entities passed to EntityManager must exist. One of the entities has already been destroyed or was never created.");
        }

        public bool Exists(Entity entity)
        {
            int index = entity.Index;

            ValidateEntity(entity);

            var versionMatches = m_Entities.Version[index] == entity.Version;
            var hasChunk = m_Entities.ChunkData[index].Chunk != null;

            return versionMatches && hasChunk;
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public void AssertEntitiesExist(Entity* entities, int count)
        {
            for (var i = 0; i != count; i++)
            {
                var entity = entities + i;
                int index = entity->Index;

                if ((uint)index >= (uint)m_EntitiesCapacity)
                    throw new ArgumentException(
                        "All entities passed to EntityManager must exist. One of the entities has already been destroyed or was never created.");

                var exists = m_Entities.Version[index] == entity->Version;
                if (!exists)
                    throw new ArgumentException(
                        "All entities passed to EntityManager must exist. One of the entities has already been destroyed or was never created.");
            }
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public void AssertEntityHasComponent(Entity entity, ComponentType componentType)
        {
            if (HasComponent(entity, componentType))
                return;

            if (!Exists(entity))
                throw new ArgumentException("The Entity does not exist");

            if (HasComponent(entity, componentType.TypeIndex))
                throw new ArgumentException(
                    $"The component typeof({componentType.GetManagedType()}) exists on the entity but the exact type {componentType} does not");

            throw new ArgumentException($"{componentType} component has not been added to the entity.");
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        public void AssertEntityHasComponent(Entity entity, int componentType)
        {
            if (HasComponent(entity, componentType))
                return;

            if (!Exists(entity))
                throw new ArgumentException("The entity does not exist");

            throw new ArgumentException("The component has not been added to the entity.");
        }

        private static Chunk* EntityChunkBatch(EntityDataManager* entityDataManager, Entity* entities, int count,
            out int indexInChunk, out int batchCount)
        {
            /// This is optimized for the case where the array of entities are allocated contigously in the chunk
            /// Thus the compacting of other elements can be batched

            // Calculate baseEntityIndex & chunk
            var baseEntityIndex = entities[0].Index;

            var chunk = entityDataManager->m_Entities.ChunkData[baseEntityIndex].Chunk;
            indexInChunk = entityDataManager->m_Entities.ChunkData[baseEntityIndex].IndexInChunk;
            batchCount = 0;

            while (batchCount < count)
            {
                var entityIndex = entities[batchCount].Index;
                var batchChunk = entityDataManager->m_Entities.ChunkData[entityIndex].Chunk;
                var batchIndexInChunk = entityDataManager->m_Entities.ChunkData[entityIndex].IndexInChunk;
                

                if (batchChunk != chunk || batchIndexInChunk != indexInChunk + batchCount)
                    break;

                batchCount++;
            }

            return chunk;
        }

        private static void DeallocateDataEntitiesInChunk(EntityDataManager* entityDataManager, Entity* entities,
            Chunk* chunk, int indexInChunk, int batchCount)
        {
            DeallocateBuffers(entityDataManager, entities, chunk, batchCount);

            var freeIndex = entityDataManager->m_EntitiesFreeIndex;

            for (var i = batchCount - 1; i >= 0; --i)
            {
                var entityIndex = entities[i].Index;

                entityDataManager->m_Entities.ChunkData[entityIndex].Chunk = null;
                entityDataManager->m_Entities.Version[entityIndex]++;
                entityDataManager->m_Entities.ChunkData[entityIndex].IndexInChunk = freeIndex;
                freeIndex = entityIndex;
            }

            entityDataManager->m_EntitiesFreeIndex = freeIndex;

            // Compute the number of things that need to moved and patched.
            int patchCount = Math.Min(batchCount, chunk->Count - indexInChunk - batchCount);

            if (0 == patchCount)
                return;

            // updates EntitityData->indexInChunk to point to where the components will be moved to
            //Assert.IsTrue(chunk->archetype->sizeOfs[0] == sizeof(Entity) && chunk->archetype->offsets[0] == 0);
            var movedEntities = (Entity*) chunk->Buffer + (chunk->Count - patchCount);
            for (var i = 0; i != patchCount; i++)
                entityDataManager->m_Entities.ChunkData[movedEntities[i].Index].IndexInChunk = indexInChunk + i;

            // Move component data from the end to where we deleted components
            ChunkDataUtility.Copy(chunk, chunk->Count - patchCount, chunk, indexInChunk, patchCount);
        }

        private static void DeallocateBuffers(EntityDataManager* entityDataManager, Entity* entities, Chunk* chunk, int batchCount)
        {
            var archetype = chunk->Archetype;

            for (var ti = 0; ti < archetype->TypesCount; ++ti)
            {
                var type = archetype->Types[ti];

                if (!type.IsBuffer)
                    continue;

                var basePtr = chunk->Buffer + archetype->Offsets[ti];
                var stride = archetype->SizeOfs[ti];

                for (int i = 0; i < batchCount; ++i)
                {
                    Entity e = entities[i];
                    int indexInChunk = entityDataManager->m_Entities.ChunkData[e.Index].IndexInChunk;
                    byte* bufferPtr = basePtr + stride * indexInChunk;
                    BufferHeader.Destroy((BufferHeader*)bufferPtr);
                }
            }
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        public int CheckInternalConsistency()
        {
            var aliveEntities = 0;
            var entityType = TypeManager.GetTypeIndex<Entity>();

            for (var i = 0; i != m_EntitiesCapacity; i++)
            {
                if (m_Entities.ChunkData[i].Chunk == null)
                    continue;

                aliveEntities++;
                var archetype = m_Entities.Archetype[i];
                Assert.AreEqual(entityType, archetype->Types[0].TypeIndex);
                var entity =
                    *(Entity*) ChunkDataUtility.GetComponentDataRO(m_Entities.ChunkData[i].Chunk, m_Entities.ChunkData[i].IndexInChunk, 0);
                Assert.AreEqual(i, entity.Index);
                Assert.AreEqual(m_Entities.Version[i], entity.Version);

                Assert.IsTrue(Exists(entity));
            }

            return aliveEntities;
        }
#endif

        public void AllocateConsecutiveEntitiesForLoading(int count)
        {
            int newCapacity = count + 1; // make room for Entity.Null
            Capacity = newCapacity;
            m_EntitiesFreeIndex = Capacity == newCapacity ? -1 : newCapacity;
            for (int i = 1; i < newCapacity; ++i)
            {
                if (m_Entities.ChunkData[i].Chunk != null)
                {
                    throw new ArgumentException("loading into non-empty entity manager is not supported");
                }

                m_Entities.ChunkData[i].IndexInChunk = 0;
                m_Entities.Version[i] = 0;
            }
        }

        internal void AddExistingChunk(Chunk* chunk)
        {
            for (int iEntity = 0; iEntity < chunk->Count; ++iEntity)
            {
                var entity = (Entity*)ChunkDataUtility.GetComponentDataRO(chunk, iEntity, 0);
                m_Entities.ChunkData[entity->Index].Chunk = chunk;
                m_Entities.ChunkData[entity->Index].IndexInChunk = iEntity;
                m_Entities.Archetype[entity->Index] = chunk->Archetype;
            }
        }

        public void AllocateEntities(Archetype* arch, Chunk* chunk, int baseIndex, int count, Entity* outputEntities)
        {
            Assert.AreEqual(chunk->Archetype->Offsets[0], 0);
            Assert.AreEqual(chunk->Archetype->SizeOfs[0], sizeof(Entity));

            var entityInChunkStart = (Entity*) chunk->Buffer + baseIndex;

            for (var i = 0; i != count; i++)
            {
                var entityIndexInChunk = m_Entities.ChunkData[m_EntitiesFreeIndex].IndexInChunk;
                if (entityIndexInChunk == -1)
                {
                    IncreaseCapacity();
                    entityIndexInChunk = m_Entities.ChunkData[m_EntitiesFreeIndex].IndexInChunk;
                }
                
                var entityVersion = m_Entities.Version[m_EntitiesFreeIndex];

                outputEntities[i].Index = m_EntitiesFreeIndex;
                outputEntities[i].Version = entityVersion;

                var entityInChunk = entityInChunkStart + i;

                entityInChunk->Index = m_EntitiesFreeIndex;
                entityInChunk->Version = entityVersion;

                m_Entities.ChunkData[m_EntitiesFreeIndex].IndexInChunk = baseIndex + i;
                m_Entities.Archetype[m_EntitiesFreeIndex] = arch;
                m_Entities.ChunkData[m_EntitiesFreeIndex].Chunk = chunk;
                
                m_EntitiesFreeIndex = entityIndexInChunk;
            }
        }

        public void AllocateEntitiesForRemapping(EntityDataManager * srcEntityDataManager, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
            var srcEntityData = srcEntityDataManager->m_Entities;
            var count = srcEntityDataManager->m_EntitiesCapacity;
            for (var i = 0; i != count; i++)
            {
                if (srcEntityData.ChunkData[i].Chunk != null)
                {
                    var entityIndexInChunk = m_Entities.ChunkData[m_EntitiesFreeIndex].IndexInChunk;
                    if (entityIndexInChunk == -1)
                    {
                        IncreaseCapacity();
                        entityIndexInChunk = m_Entities.ChunkData[m_EntitiesFreeIndex].IndexInChunk;
                    }
                    
                    var entityVersion = m_Entities.Version[m_EntitiesFreeIndex];
                    
                    EntityRemapUtility.AddEntityRemapping(ref entityRemapping, new Entity { Version = srcEntityData.Version[i], Index = i }, new Entity { Version = entityVersion, Index = m_EntitiesFreeIndex });
                    m_EntitiesFreeIndex = entityIndexInChunk;
                }
            }
        }

        public void AllocateEntitiesForRemapping(EntityDataManager * srcEntityDataManager, Chunk* chunk, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
            var count = chunk->Count;
            var entities = (Entity*)chunk->Buffer;
            for (var i = 0; i != count; i++)
            {
                var entityIndexInChunk = m_Entities.ChunkData[m_EntitiesFreeIndex].IndexInChunk;
                if (entityIndexInChunk == -1)
                {
                    IncreaseCapacity();
                    entityIndexInChunk = m_Entities.ChunkData[m_EntitiesFreeIndex].IndexInChunk;
                }

                var entityVersion = m_Entities.Version[m_EntitiesFreeIndex];

                EntityRemapUtility.AddEntityRemapping(ref entityRemapping, new Entity { Version = entities[i].Version, Index = entities[i].Index }, new Entity { Version = entityVersion, Index = m_EntitiesFreeIndex });
                m_EntitiesFreeIndex = entityIndexInChunk;
            }
        }

        public void RemapChunk(Archetype* arch, Chunk* chunk, int baseIndex, int count, ref NativeArray<EntityRemapUtility.EntityRemapInfo> entityRemapping)
        {
            Assert.AreEqual(chunk->Archetype->Offsets[0], 0);
            Assert.AreEqual(chunk->Archetype->SizeOfs[0], sizeof(Entity));

            var entityInChunkStart = (Entity*)(chunk->Buffer) + baseIndex;

            for (var i = 0; i != count; i++)
            {
                var entityInChunk = entityInChunkStart + i;
                var target = EntityRemapUtility.RemapEntity(ref entityRemapping, *entityInChunk);
                var entityVersion = m_Entities.Version[target.Index];

                Assert.AreEqual(entityVersion, target.Version);

                entityInChunk->Index = target.Index;
                entityInChunk->Version = entityVersion;
                m_Entities.ChunkData[target.Index].IndexInChunk = baseIndex + i;
                m_Entities.Archetype[target.Index] = arch;
                m_Entities.ChunkData[target.Index].Chunk = chunk;
            }
        }

        public void FreeAllEntities()
        {
            for (var i = 0; i != m_EntitiesCapacity; i++)
            {
                m_Entities.ChunkData[i].IndexInChunk = i + 1;
                m_Entities.Version[i] += 1;
                m_Entities.ChunkData[i].Chunk = null;
            }

            // Last entity indexInChunk identifies that we ran out of space...
            m_Entities.ChunkData[m_EntitiesCapacity - 1].IndexInChunk = -1;

            m_EntitiesFreeIndex = 0;
        }

        public void FreeEntities(Chunk* chunk)
        {
            var count = chunk->Count;
            var entities = (Entity*)chunk->Buffer;
            int freeIndex = m_EntitiesFreeIndex;
            for (var i = 0; i != count; i++)
            {
                int index = entities[i].Index;
                m_Entities.Version[index] += 1;
                m_Entities.ChunkData[index].Chunk = null;
                m_Entities.ChunkData[index].IndexInChunk = freeIndex;
                freeIndex = index;
            }

            m_EntitiesFreeIndex = freeIndex;
        }

        public bool HasComponent(Entity entity, int type)
        {
            if (!Exists(entity))
                return false;

            var archetype = m_Entities.Archetype[entity.Index];
            return ChunkDataUtility.GetIndexInTypeArray(archetype, type) != -1;
        }

        public bool HasComponent(Entity entity, ComponentType type)
        {
            if (!Exists(entity))
                return false;

            var archetype = m_Entities.Archetype[entity.Index];

            return ChunkDataUtility.GetIndexInTypeArray(archetype, type.TypeIndex) != -1;
        }

        public int GetSizeInChunk(Entity entity, int typeIndex, ref int typeLookupCache)
        {
            var entityChunk = m_Entities.ChunkData[entity.Index].Chunk;
            return ChunkDataUtility.GetSizeInChunk(entityChunk, typeIndex, ref typeLookupCache);
        }

        public byte* GetComponentDataWithTypeRO(Entity entity, int typeIndex)
        {
            var entityChunk = m_Entities.ChunkData[entity.Index].Chunk;
            var entityIndexInChunk = m_Entities.ChunkData[entity.Index].IndexInChunk;

            return ChunkDataUtility.GetComponentDataWithTypeRO(entityChunk, entityIndexInChunk, typeIndex);
        }

        public byte* GetComponentDataWithTypeRW(Entity entity, int typeIndex, uint globalVersion)
        {
            var entityChunk = m_Entities.ChunkData[entity.Index].Chunk;
            var entityIndexInChunk = m_Entities.ChunkData[entity.Index].IndexInChunk;
            
            return ChunkDataUtility.GetComponentDataWithTypeRW(entityChunk, entityIndexInChunk, typeIndex,
                globalVersion);
        }

        public byte* GetComponentDataWithTypeRO(Entity entity, int typeIndex, ref int typeLookupCache)
        {
            var entityChunk = m_Entities.ChunkData[entity.Index].Chunk;
            var entityIndexInChunk = m_Entities.ChunkData[entity.Index].IndexInChunk;
            
            return ChunkDataUtility.GetComponentDataWithTypeRO(entityChunk, entityIndexInChunk, typeIndex,
                ref typeLookupCache);
        }

        public byte* GetComponentDataWithTypeRW(Entity entity, int typeIndex, uint globalVersion,
            ref int typeLookupCache)
        {
            var entityChunk = m_Entities.ChunkData[entity.Index].Chunk;
            var entityIndexInChunk = m_Entities.ChunkData[entity.Index].IndexInChunk;
            
            return ChunkDataUtility.GetComponentDataWithTypeRW(entityChunk, entityIndexInChunk, typeIndex,
                globalVersion, ref typeLookupCache);
        }

        public Chunk* GetComponentChunk(Entity entity)
        {
            var entityChunk = m_Entities.ChunkData[entity.Index].Chunk;

            return entityChunk;
        }

        public void GetComponentChunk(Entity entity, out Chunk* chunk, out int chunkIndex)
        {
            var entityChunk = m_Entities.ChunkData[entity.Index].Chunk;
            var entityIndexInChunk = m_Entities.ChunkData[entity.Index].IndexInChunk;
            
            chunk = entityChunk;
            chunkIndex = entityIndexInChunk;
        }

        public Archetype* GetArchetype(Entity entity)
        {
            return m_Entities.Archetype[entity.Index];
        }

        public Archetype* GetInstantiableArchetype(Entity entity, ArchetypeManager archetypeManager, EntityGroupManager groupManager, ComponentTypeInArchetype* componentTypeInArchetypeArray)
        {
            var srcArchetype = GetArchetype(entity);
            return srcArchetype->InstantiableArchetype;
        }

        public void SetArchetype(ArchetypeManager typeMan, Entity entity, Archetype* archetype,
            int* sharedComponentDataIndices)
        {
            var chunk = typeMan.GetChunkWithEmptySlots(archetype, sharedComponentDataIndices);
            var chunkIndex = typeMan.AllocateIntoChunk(chunk);

            var oldArchetype = m_Entities.Archetype[entity.Index];
            var oldChunk = m_Entities.ChunkData[entity.Index].Chunk;
            var oldChunkIndex = m_Entities.ChunkData[entity.Index].IndexInChunk;
            ChunkDataUtility.Convert(oldChunk, oldChunkIndex, chunk, chunkIndex);
            if (chunk->ManagedArrayIndex >= 0 && oldChunk->ManagedArrayIndex >= 0)
                ChunkDataUtility.CopyManagedObjects(typeMan, oldChunk, oldChunkIndex, chunk, chunkIndex, 1);

            m_Entities.Archetype[entity.Index] = archetype;
            m_Entities.ChunkData[entity.Index].Chunk = chunk;
            m_Entities.ChunkData[entity.Index].IndexInChunk = chunkIndex;

            var lastIndex = oldChunk->Count - 1;
            // No need to replace with ourselves
            if (lastIndex != oldChunkIndex)
            {
                var lastEntity = (Entity*) ChunkDataUtility.GetComponentDataRO(oldChunk, lastIndex, 0);
                m_Entities.ChunkData[lastEntity->Index].IndexInChunk = oldChunkIndex;

                ChunkDataUtility.Copy(oldChunk, lastIndex, oldChunk, oldChunkIndex, 1);
                if (oldChunk->ManagedArrayIndex >= 0)
                    ChunkDataUtility.CopyManagedObjects(typeMan, oldChunk, lastIndex, oldChunk, oldChunkIndex, 1);
            }

            if (oldChunk->ManagedArrayIndex >= 0)
                ChunkDataUtility.ClearManagedObjects(typeMan, oldChunk, lastIndex, 1);

            --oldArchetype->EntityCount;
            typeMan.SetChunkCount(oldChunk, lastIndex);
        }

        public void AddComponents(Entity entity, ComponentTypes types, ArchetypeManager archetypeManager,
            SharedComponentDataManager sharedComponentDataManager,
            EntityGroupManager groupManager, ComponentTypeInArchetype* componentTypeInArchetypeArray)
        {
            var oldArchetype = GetArchetype(entity);

            var indexOfNewTypeInNewArchetype = stackalloc int[types.Length];

            // zipper the two sorted arrays "type" and "componentTypeInArchetype" into "componentTypeInArchetype"
            // because this is done in-place, it must be done backwards so as not to disturb the existing contents.

            {
                var oldThings = oldArchetype->TypesCount;
                var newThings = types.Length;
                var mixedThings = oldThings + newThings;
                while (oldThings > 0 && newThings > 0) // while both are still zippering,
                {
                    var oldThing = componentTypeInArchetypeArray[oldThings - 1];
                    var newThing = types.GetComponentType(newThings - 1);
                    if (oldThing.TypeIndex > newThing.TypeIndex) // put whichever is bigger at the end of the array
                    {
                        componentTypeInArchetypeArray[--mixedThings] = oldThing;
                        --oldThings;
                    }
                    else
                    {
                        var componentTypeInArchetype = new ComponentTypeInArchetype(newThing);
                        componentTypeInArchetypeArray[--mixedThings] = componentTypeInArchetype;
                        --newThings;
                        indexOfNewTypeInNewArchetype[newThings] = mixedThings; // "this new thing ended up HERE"
                    }
                }
                while (newThings > 0) // if smallest new things < smallest old things, copy them here
                {
                    var newThing = types.GetComponentType(newThings - 1);
                    var componentTypeInArchetype = new ComponentTypeInArchetype(newThing);
                    componentTypeInArchetypeArray[--mixedThings] = componentTypeInArchetype;
                    --newThings;
                    indexOfNewTypeInNewArchetype[newThings] = mixedThings; // "this new thing ended up HERE"
                }
                Assert.AreEqual(newThings, 0); // must not be any new things to copy remaining
                Assert.AreEqual(oldThings, mixedThings); // all things we didn't copy must be old
            }

            var newTypesCount = oldArchetype->TypesCount + (int)types.Length;
            var newArchetype = archetypeManager.GetOrCreateArchetype(componentTypeInArchetypeArray,
                newTypesCount, groupManager);

            int* sharedComponentDataIndices = GetComponentChunk(entity)->SharedComponentValueArray;
            if (types.m_masks.m_SharedComponentMask != 0)
            {
                int* alloc2 = stackalloc int[newArchetype->NumSharedComponents];
                var oldSharedComponentDataIndices = sharedComponentDataIndices;
                sharedComponentDataIndices = alloc2;

                UInt64 sharedComponentInNewArchetypeIsNew = 0;
                for (var i = 0; i < types.Length; ++i)
                    if (types.m_masks.IsSharedComponent(i))
                    {
                        var indexInNewArchetype = indexOfNewTypeInNewArchetype[i];
                        Assert.IsTrue((indexInNewArchetype >= 0) && (indexInNewArchetype < newArchetype->TypesCount));
                        var sharedComponentOffset = newArchetype->SharedComponentOffset[indexInNewArchetype];
                        Assert.IsTrue((sharedComponentOffset >= 0) && (sharedComponentOffset < newArchetype->NumSharedComponents));
                        sharedComponentInNewArchetypeIsNew |= 1UL << sharedComponentOffset;
                    }
                Assert.AreEqual(math.countbits(sharedComponentInNewArchetypeIsNew), types.m_masks.SharedComponents);
                var j = 0;
                for (var i = 0; i < newArchetype->NumSharedComponents; ++i)
                    if ((sharedComponentInNewArchetypeIsNew & (1UL << i)) != 0)
                        sharedComponentDataIndices[i] = 0;
                    else
                    {
                        Assert.IsTrue(j < oldArchetype->NumSharedComponents);
                        sharedComponentDataIndices[i] = oldSharedComponentDataIndices[j++];
                    }
                Assert.AreEqual(j, oldArchetype->NumSharedComponents);
            }

            SetArchetype(archetypeManager, entity, newArchetype, sharedComponentDataIndices);
            IncrementComponentOrderVersion(newArchetype, GetComponentChunk(entity), sharedComponentDataManager);
        }

        public void AddComponent(Entity entity, ComponentType type, ArchetypeManager archetypeManager,
            SharedComponentDataManager sharedComponentDataManager,
            EntityGroupManager groupManager, ComponentTypeInArchetype* componentTypeInArchetypeArray)
        {
            var componentType = new ComponentTypeInArchetype(type);
            var archetype = GetArchetype(entity);

            var t = 0;
            while (t < archetype->TypesCount && archetype->Types[t] < componentType)
            {
                componentTypeInArchetypeArray[t] = archetype->Types[t];
                ++t;
            }

            var indexInTypeArray = t;
            componentTypeInArchetypeArray[t] = componentType;
            while (t < archetype->TypesCount)
            {
                componentTypeInArchetypeArray[t + 1] = archetype->Types[t];
                ++t;
            }

            var newType = archetypeManager.GetOrCreateArchetype(componentTypeInArchetypeArray,
                archetype->TypesCount + 1, groupManager);

            int* sharedComponentDataIndices = GetComponentChunk(entity)->SharedComponentValueArray;
            if ((newType->NumSharedComponents > 0) && (newType->NumSharedComponents != archetype->NumSharedComponents))
            {
                var oldSharedComponentDataIndices = sharedComponentDataIndices;
                int* stackAlloced = stackalloc int[newType->NumSharedComponents];
                sharedComponentDataIndices = stackAlloced;

                int indexOfNewSharedComponent = newType->SharedComponentOffset[indexInTypeArray];
                UnsafeUtility.MemCpy(sharedComponentDataIndices, oldSharedComponentDataIndices, indexOfNewSharedComponent*sizeof(int));
                sharedComponentDataIndices[indexOfNewSharedComponent] = 0;
                UnsafeUtility.MemCpy(sharedComponentDataIndices + indexOfNewSharedComponent + 1, oldSharedComponentDataIndices + indexOfNewSharedComponent,
                    (archetype->NumSharedComponents-indexOfNewSharedComponent)*sizeof(int));
            }

            SetArchetype(archetypeManager, entity, newType, sharedComponentDataIndices);
            IncrementComponentOrderVersion(newType, GetComponentChunk(entity), sharedComponentDataManager);
        }

        public void TryRemoveEntityId(Entity* entities, int count, ArchetypeManager archetypeManager,
            SharedComponentDataManager sharedComponentDataManager,
            EntityGroupManager groupManager, ComponentTypeInArchetype* componentTypeInArchetypeArray)
        {
            var entityIndex = 0;
            while (entityIndex != count)
            {
                int indexInChunk;
                int batchCount;
                fixed (EntityDataManager* manager = &this)
                {
                    var chunk = EntityChunkBatch(manager, entities + entityIndex, count - entityIndex, out indexInChunk,
                        out batchCount);
                    var archetype = GetArchetype(entities[entityIndex]);
                    if (!archetype->SystemStateCleanupNeeded)
                    {
                        DeallocateDataEntitiesInChunk(manager, entities + entityIndex, chunk, indexInChunk, batchCount);
                        IncrementComponentOrderVersion(chunk->Archetype, chunk, sharedComponentDataManager);

                        if (chunk->ManagedArrayIndex >= 0)
                        {
                            // We can just chop-off the end, no need to copy anything
                            if (chunk->Count != indexInChunk + batchCount)
                                ChunkDataUtility.CopyManagedObjects(archetypeManager, chunk, chunk->Count - batchCount,
                                    chunk,
                                    indexInChunk, batchCount);

                            ChunkDataUtility.ClearManagedObjects(archetypeManager, chunk, chunk->Count - batchCount,
                                batchCount);
                        }

                        chunk->Archetype->EntityCount -= batchCount;
                        archetypeManager.SetChunkCount(chunk, chunk->Count - batchCount);
                    }
                    else
                    {
                        var newType = archetype->SystemStateResidueArchetype;

                        int* sharedComponentDataIndices = chunk->SharedComponentValueArray;
                        if ((newType->NumSharedComponents > 0) && (newType->NumSharedComponents != archetype->NumSharedComponents))
                        {
                            var oldSharedComponentDataIndices = sharedComponentDataIndices;
                            int* tempAlloc = stackalloc int[newType->NumSharedComponents];
                            sharedComponentDataIndices = tempAlloc;

                            int oldIndex = 0;
                            for (int newIndex = 0; newIndex < newType->TypesCount; ++newIndex, ++oldIndex)
                            {
                                if (newType->SharedComponentOffset[newIndex] != -1)
                                {
                                    var t = newType->Types[newIndex];
                                    while (t != archetype->Types[oldIndex])
                                        ++oldIndex;
                                    var newOffset = newType->SharedComponentOffset[newIndex];
                                    var oldOffset = archetype->SharedComponentOffset[oldIndex];
                                    sharedComponentDataIndices[newOffset] = oldSharedComponentDataIndices[oldOffset];
                                }
                            }
                        }

                        for (var batchEntityIndex = 0; batchEntityIndex < batchCount; batchEntityIndex++)
                        {
                            var entity = entities[entityIndex + batchEntityIndex];
                            IncrementComponentOrderVersion(archetype, GetComponentChunk(entity),
                                sharedComponentDataManager);
                            SetArchetype(archetypeManager, entity, newType, sharedComponentDataIndices);
                        }
                    }
                }

                entityIndex += batchCount;
            }
        }

        public void RemoveComponent(Entity entity, ComponentType type, ArchetypeManager archetypeManager,
            SharedComponentDataManager sharedComponentDataManager,
            EntityGroupManager groupManager, ComponentTypeInArchetype* componentTypeInArchetypeArray)
        {
            var componentType = new ComponentTypeInArchetype(type);

            var archetype = GetArchetype(entity);

            var removedTypes = 0;
            int indexInOldTypeArray = -1;
            for (var t = 0; t < archetype->TypesCount; ++t)
                if (archetype->Types[t].TypeIndex == componentType.TypeIndex)
                {
                    indexInOldTypeArray = t;
                    ++removedTypes;
                }
                else
                    componentTypeInArchetypeArray[t - removedTypes] = archetype->Types[t];

            var newType = archetypeManager.GetOrCreateArchetype(componentTypeInArchetypeArray,
                archetype->TypesCount - removedTypes, groupManager);

            int* sharedComponentDataIndices = GetComponentChunk(entity)->SharedComponentValueArray;
            if ((newType->NumSharedComponents > 0) && (newType->NumSharedComponents != archetype->NumSharedComponents))
            {
                var oldSharedComponentDataIndices = sharedComponentDataIndices;
                int* tempAlloc = stackalloc int[newType->NumSharedComponents];
                sharedComponentDataIndices = tempAlloc;

                int indexOfRemovedSharedComponent = archetype->SharedComponentOffset[indexInOldTypeArray];
                UnsafeUtility.MemCpy(sharedComponentDataIndices, oldSharedComponentDataIndices, indexOfRemovedSharedComponent*sizeof(int));
                UnsafeUtility.MemCpy(sharedComponentDataIndices + indexOfRemovedSharedComponent, oldSharedComponentDataIndices + indexOfRemovedSharedComponent + 1, (newType->NumSharedComponents-indexOfRemovedSharedComponent)*sizeof(int));
            }

            IncrementComponentOrderVersion(archetype, GetComponentChunk(entity), sharedComponentDataManager);

            SetArchetype(archetypeManager, entity, newType, sharedComponentDataIndices);
        }

        public void MoveEntityToChunk(ArchetypeManager typeMan, Entity entity, Chunk* newChunk, int newChunkIndex)
        {
            var oldChunk = m_Entities.ChunkData[entity.Index].Chunk;
            Assert.IsTrue(oldChunk->Archetype == newChunk->Archetype);

            var oldChunkIndex = m_Entities.ChunkData[entity.Index].IndexInChunk;

            ChunkDataUtility.Copy(oldChunk, oldChunkIndex, newChunk, newChunkIndex, 1);

            if (oldChunk->ManagedArrayIndex >= 0)
                ChunkDataUtility.CopyManagedObjects(typeMan, oldChunk, oldChunkIndex, newChunk, newChunkIndex, 1);

            m_Entities.ChunkData[entity.Index].Chunk = newChunk;
            m_Entities.ChunkData[entity.Index].IndexInChunk = newChunkIndex;

            var lastIndex = oldChunk->Count - 1;
            // No need to replace with ourselves
            if (lastIndex != oldChunkIndex)
            {
                var lastEntity = (Entity*) ChunkDataUtility.GetComponentDataRO(oldChunk, lastIndex, 0);
                m_Entities.ChunkData[lastEntity->Index].IndexInChunk = oldChunkIndex;

                ChunkDataUtility.Copy(oldChunk, lastIndex, oldChunk, oldChunkIndex, 1);
                if (oldChunk->ManagedArrayIndex >= 0)
                    ChunkDataUtility.CopyManagedObjects(typeMan, oldChunk, lastIndex, oldChunk, oldChunkIndex, 1);
            }

            if (oldChunk->ManagedArrayIndex >= 0)
                ChunkDataUtility.ClearManagedObjects(typeMan, oldChunk, lastIndex, 1);

            newChunk->Archetype->EntityCount--;
            typeMan.SetChunkCount(oldChunk, oldChunk->Count - 1);
        }

        public void CreateEntities(ArchetypeManager archetypeManager, Archetype* archetype, Entity* entities, int count)
        {
            int* sharedComponentDataIndices = stackalloc int[archetype->NumSharedComponents];
            UnsafeUtility.MemClear(sharedComponentDataIndices, archetype->NumSharedComponents*sizeof(int));

            while (count != 0)
            {
                var chunk = archetypeManager.GetChunkWithEmptySlots(archetype, sharedComponentDataIndices);
                int allocatedIndex;
                var allocatedCount = archetypeManager.AllocateIntoChunk(chunk, count, out allocatedIndex);
                AllocateEntities(archetype, chunk, allocatedIndex, allocatedCount, entities);
                ChunkDataUtility.InitializeComponents(chunk, allocatedIndex, allocatedCount);

                entities += allocatedCount;
                count -= allocatedCount;
            }

            IncrementComponentTypeOrderVersion(archetype);
        }

        public void InstantiateEntities(ArchetypeManager archetypeManager,
            SharedComponentDataManager sharedComponentDataManager, EntityGroupManager groupManager, Entity srcEntity, Entity* outputEntities, int count, ComponentTypeInArchetype* componentTypeInArchetypeArray)
        {
            var srcIndex = m_Entities.ChunkData[srcEntity.Index].IndexInChunk;
            var srcChunk = m_Entities.ChunkData[srcEntity.Index].Chunk;
            var dstArchetype = GetInstantiableArchetype(srcEntity,archetypeManager, groupManager, componentTypeInArchetypeArray);
            var srcSharedComponentDataIndices = GetComponentChunk(srcEntity)->SharedComponentValueArray;

            while (count != 0)
            {
                var chunk = archetypeManager.GetChunkWithEmptySlots(dstArchetype, srcSharedComponentDataIndices);
                int indexInChunk;
                var allocatedCount = archetypeManager.AllocateIntoChunk(chunk, count, out indexInChunk);

                ChunkDataUtility.ReplicateComponents(srcChunk, srcIndex, chunk, indexInChunk, allocatedCount);

                AllocateEntities(dstArchetype, chunk, indexInChunk, allocatedCount, outputEntities);

                outputEntities += allocatedCount;
                count -= allocatedCount;
            }

            IncrementComponentOrderVersion(dstArchetype, srcChunk, sharedComponentDataManager);
        }

        public int GetSharedComponentDataIndex(Entity entity, int typeIndex)
        {
            var archetype = GetArchetype(entity);
            var indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);

            var chunk = m_Entities.ChunkData[entity.Index].Chunk;
            var sharedComponentValueArray = chunk->SharedComponentValueArray;
            var sharedComponentOffset = m_Entities.Archetype[entity.Index]->SharedComponentOffset[indexInTypeArray];
            return sharedComponentValueArray[sharedComponentOffset];
        }

        public void SetSharedComponentDataIndex(ArchetypeManager archetypeManager,
            SharedComponentDataManager sharedComponentDataManager, Entity entity, int typeIndex,
            int newSharedComponentDataIndex)
        {
            var archetype = GetArchetype(entity);

            var indexInTypeArray = ChunkDataUtility.GetIndexInTypeArray(archetype, typeIndex);

            var srcChunk = GetComponentChunk(entity);
            var srcSharedComponentValueArray = srcChunk->SharedComponentValueArray;
            var sharedComponentOffset = archetype->SharedComponentOffset[indexInTypeArray];
            var oldSharedComponentDataIndex = srcSharedComponentValueArray[sharedComponentOffset];

            if (newSharedComponentDataIndex == oldSharedComponentDataIndex)
                return;

            var sharedComponentIndices = stackalloc int[archetype->NumSharedComponents];
            var srcSharedComponentDataIndices = srcChunk->SharedComponentValueArray;

            UnsafeUtility.MemCpy(sharedComponentIndices, srcSharedComponentDataIndices, archetype->NumSharedComponents*sizeof(int));

            sharedComponentIndices[sharedComponentOffset] = newSharedComponentDataIndex;

            var newChunk = archetypeManager.GetChunkWithEmptySlots(archetype, sharedComponentIndices);
            var newChunkIndex = archetypeManager.AllocateIntoChunk(newChunk);

            IncrementComponentOrderVersion(archetype, srcChunk, sharedComponentDataManager);

            MoveEntityToChunk(archetypeManager, entity, newChunk, newChunkIndex);
        }

        internal void IncrementComponentOrderVersion(Archetype* archetype, Chunk* chunk,
            SharedComponentDataManager sharedComponentDataManager)
        {
            // Increment shared component version
            var sharedComponentDataIndices = chunk->SharedComponentValueArray;
            for (var i = 0; i < archetype->NumSharedComponents; i++)
                sharedComponentDataManager.IncrementSharedComponentVersion(sharedComponentDataIndices[i]);

            IncrementComponentTypeOrderVersion(archetype);
        }

        internal void IncrementComponentTypeOrderVersion(Archetype* archetype)
        {
            // Increment type component version
            for (var t = 0; t < archetype->TypesCount; ++t)
            {
                var typeIndex = archetype->Types[t].TypeIndex;
                m_ComponentTypeOrderVersion[typeIndex]++;
            }
        }

        public int GetComponentTypeOrderVersion(int typeIndex)
        {
            return m_ComponentTypeOrderVersion[typeIndex];
        }
    }
}
