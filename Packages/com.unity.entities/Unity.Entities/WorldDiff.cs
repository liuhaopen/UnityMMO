//#define LOG_DIFF_ALL
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace Unity.Entities
{
    public struct ComponentDiff
    {
        public int EntityIndex;
        public int TypeHashIndex;
    }

    [Serializable]
    public struct EntityGuid : IComponentData, IEquatable<EntityGuid>, IComparable<EntityGuid>
    {
        public ulong a;
        public ulong b;

        public static readonly EntityGuid Null = new EntityGuid();
        
        public bool Equals(EntityGuid other)
        {
            return a == other.a && b == other.b;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (a.GetHashCode() * 397) ^ b.GetHashCode();
            }
        }

        public int CompareTo(EntityGuid other)
        {
            if (a != other.a)
                return a > other.a ? 1 : -1;

            if (b != other.b)
                return b > other.b ? 1 : -1;

            return 0;
        }

        public override string ToString()
        {
            return $"{a:x16}{b:x16}";
        }
    }

    public struct DataDiff
    {
        public int EntityIndex;
        public int TypeHashIndex;
        public int Offset;
        public int SizeBytes;

        public override string ToString()
        {
            return $"{EntityIndex} {TypeHashIndex} {Offset} {SizeBytes}";
        }
    }

    public struct DiffEntityPatch
    {
        public int EntityIndex;
        public int TypeHashIndex;
        public EntityGuid Guid;
        public int Offset;
    }

    public struct SetSharedComponentDiff
    {
        public int EntityIndex;
        public int TypeHashIndex;
        public object BoxedSharedValue;
    }

    public static class DiffUtil
    {
        static unsafe void CreateEntityToGuidLookup(Chunk* c, NativeHashMap<EntityGuid, Entity> lookup)
        {
            var arch = c->Archetype;
            Entity* entities = (Entity*) (c->Buffer + arch->Offsets[0]);

            // Find EntityGuid type index
            int guidIndex = ChunkDataUtility.GetIndexInTypeArray(arch, TypeManager.GetTypeIndex<EntityGuid>());
            Assert.IsTrue(guidIndex > 0);
            EntityGuid* guids = (EntityGuid*) (c->Buffer + arch->Offsets[guidIndex]);

            for (int i = 0; i < c->Count; ++i)
            {
                lookup.TryAdd(guids[i], entities[i]);
            }
        }
        public unsafe static NativeHashMap<EntityGuid, Entity> GenerateEntityGuidToEntityHashMap(World source)
        {
            var allEntities = new NativeHashMap<EntityGuid, Entity>(4096, Allocator.TempJob);
            var smgr = source.GetOrCreateManager<EntityManager>();
            var schunks = GetAllChunks(smgr);

            for (int i = 0; i < schunks.Length; ++i)
            {
                Chunk* sourceChunk = schunks[i].m_Chunk;
                CreateEntityToGuidLookup(sourceChunk, allEntities);                
            }
            schunks.Dispose();
            return allEntities;
        }

        public static ComponentGroup CreateAllChunksGroup(EntityManager manager)
        {
            var guidQuery = new[]
            {
                new EntityArchetypeQuery
                {
                    All = new ComponentType[] {typeof(EntityGuid)},
                },
                new EntityArchetypeQuery
                {
                    All = new ComponentType[] {typeof(EntityGuid), typeof(Disabled)},
                },
                new EntityArchetypeQuery
                {
                    All = new ComponentType[] {typeof(EntityGuid), typeof(Prefab)},
                },
                new EntityArchetypeQuery
                {
                    All = new ComponentType[] {typeof(EntityGuid), typeof(Prefab), typeof(Disabled)},
                }
            };

            return manager.CreateComponentGroup(guidQuery);
        }


        public static NativeArray<ArchetypeChunk> GetAllChunks(EntityManager manager)
        {
            var group = CreateAllChunksGroup(manager);
            
            var chunks = group.CreateArchetypeChunkArray(Allocator.TempJob);
            
            group.Dispose();
            
            return chunks;
        }
    }
    
    public struct WorldDiff : IDisposable
    {
        public NativeArray<ulong> TypeHashes;
        public NativeArray<EntityGuid> Entities;
        public NativeArray<byte> ComponentPayload;

        // As consecutive counts in Entities
        public int NewEntityCount;
        public int DeletedEntityCount;

        public NativeArray<ComponentDiff> AddComponents;
        public NativeArray<ComponentDiff> RemoveComponents;
        public NativeArray<DataDiff> SetCommands;
        public NativeArray<DiffEntityPatch> EntityPatches;
        public SetSharedComponentDiff[] SharedSetCommands;

        public void Dispose()
        {
            TypeHashes.Dispose();
            Entities.Dispose();
            ComponentPayload.Dispose();
            AddComponents.Dispose();
            RemoveComponents.Dispose();
            SetCommands.Dispose();
            EntityPatches.Dispose();
        }

        
        public bool HasChanges
        {
            get { return NewEntityCount != 0 || DeletedEntityCount != 0 || AddComponents.Length != 0 || RemoveComponents.Length != 0 || SetCommands.Length != 0 || EntityPatches.Length != 0 || SharedSetCommands.Length != 0; }
        }
    }

    internal unsafe struct DiffEntityData
    {
        public Chunk* Chunk;
        public int Index;
    }

    internal unsafe struct DiffCreationData
    {
        public EntityGuid Guid;
        public Chunk* Chunk;
        public int Index;
    }

    internal unsafe struct DiffModificationData
    {
        public EntityGuid Guid;
        public Chunk* BeforeChunk;
        public int BeforeIndex;
        public Chunk* AfterChunk;
        public int AfterIndex;
    }

    public static unsafe class WorldDiffer
    {
        public static WorldDiff UpdateDiff(World Source, World ShadowWorld, Allocator resultAllocator)
        {
            if (Source == null)
                throw new ArgumentNullException("Source");

            if (ShadowWorld == null)
                throw new ArgumentNullException("ShadowWorld");

            if (resultAllocator == Allocator.Temp)
                throw new ArgumentException("Allocator can not be Allocator.Temp. Use Allocator.TempJob instead.");

            var smgr = Source.GetOrCreateManager<EntityManager>();
            var dmgr = ShadowWorld.GetOrCreateManager<EntityManager>();

            var schunks = DiffUtil.GetAllChunks(smgr);
            var dchunks = DiffUtil.GetAllChunks(dmgr);

            var visited = new NativeHashMap<uint, byte>(schunks.Length * 3, Allocator.TempJob);
            var cloneList = new NativeList<IntPtr>(schunks.Length, Allocator.TempJob);
            var dropList = new NativeList<IntPtr>(schunks.Length, Allocator.TempJob);

            //@TODO: Consistent naming...
            // beforeState == dchunk == ShadowWorld
            var beforeState = new NativeHashMap<EntityGuid, DiffEntityData>(4096, Allocator.TempJob);
            // afterState == schunk == Source
            var afterState = new NativeHashMap<EntityGuid, DiffEntityData>(4096, Allocator.TempJob);

            var allEntities = new NativeHashMap<Entity, EntityGuid>(4096, Allocator.TempJob);

            for (int i = 0; i < schunks.Length; ++i)
            {
                Chunk* sourceChunk = schunks[i].m_Chunk;
                Chunk* destChunk = dmgr.ArchetypeManager.GetChunkBySequenceNumber(sourceChunk->SequenceNumber);

                bool b = visited.TryAdd(sourceChunk->SequenceNumber, 1);
                UnityEngine.Assertions.Assert.IsTrue(b);

                CreateEntityToGuidLookup(sourceChunk, allEntities);

                if (destChunk == null)
                {
                    cloneList.Add((IntPtr) sourceChunk);
                    AddChunkEntitiesToState(sourceChunk, afterState);
                }
                else if (ChunkChanged(sourceChunk, destChunk))
                {
                    cloneList.Add((IntPtr) sourceChunk);
                    dropList.Add((IntPtr) destChunk);
                    AddChunkEntitiesToState(sourceChunk, afterState);
                    AddChunkEntitiesToState(destChunk, beforeState);
                }
            }

            for (int i = 0; i < dchunks.Length; ++i)
            {
                Chunk* destChunk = dchunks[i].m_Chunk;

                byte ignored;
                if (!visited.TryGetValue(destChunk->SequenceNumber, out ignored))
                {
                    dropList.Add((IntPtr) destChunk);
                    AddChunkEntitiesToState(destChunk, beforeState);
                }
            }

            var removedEntities = new NativeList<EntityGuid>(1, Allocator.TempJob);
            var createdEntities = new NativeList<DiffCreationData>(1, Allocator.TempJob);
            var modifiedEntities = new NativeList<DiffModificationData>(1, Allocator.TempJob);

            ComputeEntityDiff(beforeState, afterState, removedEntities, createdEntities, modifiedEntities);

            var typeHashes = new NativeList<ulong>(1, Allocator.TempJob);
            var typeHashLookup = new NativeHashMap<int, int>(1, Allocator.TempJob);

            var entityList = new NativeList<EntityGuid>(1, Allocator.TempJob);
            var entityLookup = new NativeHashMap<EntityGuid, int>(1, Allocator.TempJob);

            var removeComponents = new NativeList<ComponentDiff>(1, Allocator.TempJob);
            var addComponents = new NativeList<ComponentDiff>(1, Allocator.TempJob);
            var dataDiffs = new NativeList<DataDiff>(1, Allocator.TempJob);
            var byteDiffs = new NativeList<byte>(1, Allocator.TempJob);
            var sharedDiffs = new List<SetSharedComponentDiff>();
            var entityPatches = new NativeList<DiffEntityPatch>(1, Allocator.TempJob);

            BuildAddComponents(createdEntities, typeHashes, typeHashLookup, addComponents, dataDiffs, byteDiffs, entityList, entityLookup, sharedDiffs, smgr.m_SharedComponentManager, entityPatches, allEntities);

            BuildComponentDiff(modifiedEntities, typeHashes, typeHashLookup, removeComponents, addComponents, dataDiffs, sharedDiffs, byteDiffs, entityList, entityLookup, smgr.m_SharedComponentManager, dmgr.m_SharedComponentManager, entityPatches, allEntities);

            // Add removed entities to back of entity list
            entityList.AddRange(removedEntities);

            // Build output data
            WorldDiff result;
            result.TypeHashes = typeHashes.ToArray(resultAllocator);
            result.Entities = entityList.ToArray(resultAllocator);
            result.NewEntityCount = createdEntities.Length;
            result.DeletedEntityCount = removedEntities.Length;
            result.AddComponents = addComponents.ToArray(resultAllocator);
            result.RemoveComponents = removeComponents.ToArray(resultAllocator);
            result.SetCommands = dataDiffs.ToArray(resultAllocator);
            result.EntityPatches = entityPatches.ToArray(resultAllocator);
            result.ComponentPayload = byteDiffs.ToArray(resultAllocator);
            result.SharedSetCommands = sharedDiffs.ToArray();

            // Drop all unused and modified chunks
            for (int i = 0; i < dropList.Length; ++i)
            {
                dmgr.ArchetypeManager.DestroyChunkForDiffing((Chunk*)dropList[i], dmgr.Entities);
            }

            // Clone all new and modified chunks
            for (int i = 0; i < cloneList.Length; ++i)
            {
                dmgr.ArchetypeManager.CloneChunkForDiffing((Chunk*)cloneList[i], dmgr.Entities, dmgr.GroupManager, smgr.m_SharedComponentManager);
            }

            entityPatches.Dispose();
            allEntities.Dispose();
            entityLookup.Dispose();
            entityList.Dispose();
            byteDiffs.Dispose();
            dataDiffs.Dispose();
            addComponents.Dispose();
            removeComponents.Dispose();
            typeHashLookup.Dispose();
            typeHashes.Dispose();
            modifiedEntities.Dispose();
            createdEntities.Dispose();
            removedEntities.Dispose();
            afterState.Dispose();
            beforeState.Dispose();
            visited.Dispose();
            dropList.Dispose();
            cloneList.Dispose();
            dchunks.Dispose();
            schunks.Dispose();

            return result;
        }


        static void BuildComponentDiff(NativeList<DiffModificationData> modifiedEntities,
            NativeList<ulong> typeHashes,
            NativeHashMap<int, int> typeHashLookup,
            NativeList<ComponentDiff> removeComponents,
            NativeList<ComponentDiff> addComponents,
            NativeList<DataDiff> dataDiffs,
            List<SetSharedComponentDiff> sharedComponentDiffs,
            NativeList<byte> byteDiffs,
            NativeList<EntityGuid> guidList,
            NativeHashMap<EntityGuid, int> guidToIndex,
            SharedComponentDataManager ssharedComponentDataManager,
            SharedComponentDataManager dsharedComponentDataManager,
            NativeList<DiffEntityPatch> entityPatches, NativeHashMap<Entity, EntityGuid> allEntities)
        {
            for (int i = 0; i < modifiedEntities.Length; ++i)
            {
                var mod = modifiedEntities[i];

                var beforeArch = mod.BeforeChunk->Archetype;
                var afterArch = mod.AfterChunk->Archetype;

                
                var entityIndex = GetExternalIndex(mod.Guid, guidList, guidToIndex);

                for (int afterType = 1; afterType < afterArch->TypesCount; ++afterType)
                {
                    var at = afterArch->Types[afterType];

                    int targetTypeIndex = at.TypeIndex;
                    var typeHashIndex = GetTypeHashIndex(targetTypeIndex, typeHashes, typeHashLookup);
                    int beforeType = ChunkDataUtility.GetIndexInTypeArray(beforeArch, afterArch->Types[afterType].TypeIndex);
                    if (-1 == beforeType)
                    {
                        addComponents.Add(new ComponentDiff {EntityIndex = entityIndex, TypeHashIndex = typeHashIndex});

                        if (at.IsSharedComponent)
                        {
                            // Also push through initial data wholesale.
                            object afterObject = GetSharedComponentObject(ssharedComponentDataManager, mod.AfterChunk, afterType, targetTypeIndex);
                            sharedComponentDiffs.Add(new SetSharedComponentDiff
                            {
                                BoxedSharedValue = afterObject,
                                EntityIndex = entityIndex,
                                TypeHashIndex = typeHashIndex,
                            });
                        }
                        else if (at.IsBuffer)
                        {
                            // Also push through initial data wholesale.
                            var afterHeader = (BufferHeader*)(mod.AfterChunk->Buffer + afterArch->Offsets[afterType] + afterArch->SizeOfs[afterType] * mod.AfterIndex);
                            var afterElements = afterHeader->Length;
                            var bytesPerElement = TypeManager.GetTypeInfo(at.TypeIndex).ElementSize;
                            var afterBytes = bytesPerElement * afterElements;
                            dataDiffs.Add(new DataDiff
                            {
                                EntityIndex = entityIndex,
                                Offset = 0,
                                SizeBytes = afterBytes,
                                TypeHashIndex = typeHashIndex
                            });                            
                            var afterElementPointer = BufferHeader.GetElementPointer(afterHeader);
                            byteDiffs.AddRange(afterElementPointer, afterBytes);
                            ExtractGuidPatches(entityPatches, allEntities, at, afterElementPointer, afterElements, entityIndex, typeHashIndex);                            
                        }
                        else
                        {
                            // Also push through initial data wholesale.
                            dataDiffs.Add(new DataDiff
                            {
                                EntityIndex = entityIndex,
                                Offset = 0,
                                SizeBytes = afterArch->SizeOfs[afterType],
                                TypeHashIndex = typeHashIndex
                            });                            
                            byte* afterAddress = mod.AfterChunk->Buffer + afterArch->Offsets[afterType] + afterArch->SizeOfs[afterType] * mod.AfterIndex;                            
                            byteDiffs.AddRange(afterAddress, afterArch->SizeOfs[afterType]);

                            ExtractGuidPatches(entityPatches, allEntities, at, afterAddress, 1, entityIndex, typeHashIndex);
                        }

                        // Also check value
                        continue;
                    }

                    if (!at.IsSharedComponent && !at.IsBuffer && !at.IsZeroSized && !at.IsSystemStateComponent && !at.IsSystemStateSharedComponent)
                    {
                        Assert.AreEqual(beforeArch->SizeOfs[beforeType], afterArch->SizeOfs[afterType]);
                        
                        byte* beforeAddress = mod.BeforeChunk->Buffer + beforeArch->Offsets[beforeType] + beforeArch->SizeOfs[beforeType] * mod.BeforeIndex;
                        byte* afterAddress = mod.AfterChunk->Buffer + afterArch->Offsets[afterType] + afterArch->SizeOfs[afterType] * mod.AfterIndex;

                        if (!SharedComponentDataManager.FastEquality_ComparePtr(beforeAddress, afterAddress, targetTypeIndex))
                        {
                            // For now do full component replacement, because we need to dig into field information to make this work.
                            dataDiffs.Add(new DataDiff
                            {
                                EntityIndex = entityIndex,
                                Offset = 0,
                                SizeBytes = afterArch->SizeOfs[afterType],
                                TypeHashIndex = typeHashIndex
                            });

                            byteDiffs.AddRange(afterAddress, afterArch->SizeOfs[afterType]);

                            ExtractGuidPatches(entityPatches, allEntities, at, afterAddress, 1, entityIndex, typeHashIndex);
                        }

                        continue;
                    }

                    if (at.IsSharedComponent)
                    {
                        object beforeObject = GetSharedComponentObject(dsharedComponentDataManager, mod.BeforeChunk, beforeType, targetTypeIndex);
                        object afterObject = GetSharedComponentObject(ssharedComponentDataManager, mod.AfterChunk, afterType, targetTypeIndex);
                        
                        if (!SharedComponentDataManager.FastEquality_CompareBoxed(beforeObject, afterObject, targetTypeIndex))
                        {
                            sharedComponentDiffs.Add(new SetSharedComponentDiff
                            {
                                BoxedSharedValue = afterObject,
                                EntityIndex = entityIndex,
                                TypeHashIndex = typeHashIndex,
                            });
                        }

                        continue;
                    }

                    if (at.IsBuffer)
                    {
                        Assert.AreEqual(beforeArch->SizeOfs[beforeType], afterArch->SizeOfs[afterType]);
                        var beforeHeader = (BufferHeader*)(mod.BeforeChunk->Buffer + beforeArch->Offsets[beforeType] + beforeArch->SizeOfs[beforeType] * mod.BeforeIndex);
                        var afterHeader = (BufferHeader*)(mod.AfterChunk->Buffer + afterArch->Offsets[afterType] + afterArch->SizeOfs[afterType] * mod.AfterIndex);                        
                        byte* beforeElementPointer = BufferHeader.GetElementPointer(beforeHeader);
                        byte* afterElementPointer = BufferHeader.GetElementPointer(afterHeader);
                        var beforeElements = beforeHeader->Length;
                        var afterElements = afterHeader->Length;
                        var bytesPerElement = TypeManager.GetTypeInfo(at.TypeIndex).ElementSize;
                        var afterElementBytes = afterElements * bytesPerElement;
                        if (beforeElements != afterElements || !SharedComponentDataManager.FastEquality_CompareElements(beforeElementPointer, afterElementPointer, afterElements, at.TypeIndex))
                        {
                            dataDiffs.Add(new DataDiff
                            {
                                EntityIndex = entityIndex,
                                Offset = 0,
                                SizeBytes = afterElementBytes,
                                TypeHashIndex = typeHashIndex
                            });
                            byteDiffs.AddRange(afterElementPointer, afterElementBytes);
                            ExtractGuidPatches(entityPatches, allEntities, at, afterElementPointer, afterElements, entityIndex, typeHashIndex);                            
                        }
                        continue;                        
                    }
                }

                for (int beforeType = 1; beforeType < beforeArch->TypesCount; ++beforeType)
                {
                    int targetTypeIndex = beforeArch->Types[beforeType].TypeIndex;
                    if (-1 == ChunkDataUtility.GetIndexInTypeArray(afterArch, targetTypeIndex))
                    {
                        var typeHashIndex = GetTypeHashIndex(targetTypeIndex, typeHashes, typeHashLookup);
                        removeComponents.Add(new ComponentDiff {EntityIndex = entityIndex, TypeHashIndex = typeHashIndex});
                    }
                }
            }
        }

        private static void ExtractGuidPatches(NativeList<DiffEntityPatch> entityPatches, NativeHashMap<Entity, EntityGuid> allEntities, ComponentTypeInArchetype at, byte* afterAddress, int elementCount, int entityIndex, int typeHashIndex)
        {            
            var ft = TypeManager.GetTypeInfo(at.TypeIndex);

            if (ft.EntityOffsets == null)
                return;            
            
            int elementOffset = 0;
            for (var element = 0; element < elementCount; ++element)
            {
            #if !UNITY_CSHARP_TINY
                foreach (var eo in ft.EntityOffsets)
                {
            #else
                for(int i = 0; i < ft.EntityOffsetCount; ++i)
                {
                    var eo = UnsafeUtility.ReadArrayElement<TypeManager.EntityOffsetInfo>(ft.EntityOffsets, i);
            #endif

                    var offset = elementOffset + eo.Offset;
                    Entity* e = (Entity*) (afterAddress + offset);
                    EntityGuid guid;

                    if (!allEntities.TryGetValue(*e, out guid)) // if *e has no guid, then guid will be null (desired)
                        guid = EntityGuid.Null;
                    entityPatches.Add(new DiffEntityPatch
                    {
                        EntityIndex = entityIndex,
                        TypeHashIndex = typeHashIndex,
                        Offset = offset,
                        Guid = guid
                    });
                }

                elementOffset += ft.ElementSize;
            }
        }


#if !UNITY_CSHARP_TINY
        static object GetSharedComponentObject(SharedComponentDataManager sharedComponentDataManager, Chunk* chunk, int typeIndexInArchetype, int targetTypeIndex)
        {
            int off = chunk->Archetype->SharedComponentOffset[typeIndexInArchetype];
            Assert.AreNotEqual(-1, off);
            int sharedComponentIndex = chunk->SharedComponentValueArray[off];
            return sharedComponentDataManager.GetSharedComponentDataBoxed(sharedComponentIndex, targetTypeIndex);
        }
#else
        static object GetSharedComponentObject(SharedComponentDataManager sharedComponentDataManager, Chunk* chunk, int typeIndexInArchetype, int targetTypeIndex)
        {
            return null;
        }
#endif

        static private void BuildAddComponents(NativeList<DiffCreationData> newEntities,
            NativeList<ulong> typeHashes,
            NativeHashMap<int, int> typeHashLookup,
            NativeList<ComponentDiff> addComponents,
            NativeList<DataDiff> dataDiffs,
            NativeList<byte> byteDiffs,
            NativeList<EntityGuid> guidList,
            NativeHashMap<EntityGuid, int> guidToIndex,
            List<SetSharedComponentDiff> sharedDiffs,
            SharedComponentDataManager sharedComponentDataManager,
            NativeList<DiffEntityPatch> entityPatches,
            NativeHashMap<Entity, EntityGuid> allEntities)
        {
            for (int i = 0; i < newEntities.Length; ++i)
            {
                var ent = newEntities[i];

                var afterArch = ent.Chunk->Archetype;
                var entityIndex = GetExternalIndex(ent.Guid, guidList, guidToIndex);

                for (int afterType = 1; afterType < afterArch->TypesCount; ++afterType)
                {
                    var at = afterArch->Types[afterType];

                    if (at.IsSystemStateComponent || at.IsSystemStateSharedComponent)
                        continue;

                    int targetTypeIndex = at.TypeIndex;
                    var typeHashIndex = GetTypeHashIndex(targetTypeIndex, typeHashes, typeHashLookup);
                    addComponents.Add(new ComponentDiff {EntityIndex = entityIndex, TypeHashIndex = typeHashIndex});
                                       
                    //@TODO: BuildComponentDiff is copy pasta..
                    if (!at.IsBuffer && !at.IsSharedComponent && !at.IsZeroSized)
                    {
                        // Also push through initial data wholesale.
                        dataDiffs.Add(new DataDiff
                        {
                            EntityIndex = entityIndex,
                            Offset = 0,
                            SizeBytes = afterArch->SizeOfs[afterType],
                            TypeHashIndex = typeHashIndex
                        });

                        byte* src = ent.Chunk->Buffer + afterArch->Offsets[afterType] + ent.Index * afterArch->SizeOfs[afterType];
                        byteDiffs.AddRange(src, afterArch->SizeOfs[afterType]);

                        ExtractGuidPatches(entityPatches, allEntities, at, src, 1, entityIndex, typeHashIndex);
                    }
                    else if (at.IsSharedComponent)
                    {
                        object o = GetSharedComponentObject(sharedComponentDataManager, ent.Chunk, afterType, targetTypeIndex);
                        sharedDiffs.Add(new SetSharedComponentDiff
                        {
                            EntityIndex = entityIndex,
                            TypeHashIndex = typeHashIndex,
                            BoxedSharedValue = o
                        });
                    }
                    else if (at.IsBuffer)
                    {
                        BufferHeader* afterHeader = (BufferHeader*)(ent.Chunk->Buffer + afterArch->Offsets[afterType] + ent.Index * afterArch->SizeOfs[afterType]);
                        var bytesPerElement = TypeManager.GetTypeInfo(at.TypeIndex).ElementSize;
                        var afterElements = afterHeader->Length;
                        var afterElementBytes = bytesPerElement * afterElements;
                        dataDiffs.Add(new DataDiff
                        {
                            EntityIndex = entityIndex,
                            Offset = 0,
                            SizeBytes = afterElementBytes,
                            TypeHashIndex = typeHashIndex
                        });
                        byte* afterElementPointer = BufferHeader.GetElementPointer(afterHeader);                       
                        byteDiffs.AddRange(afterElementPointer, afterElementBytes);
                        ExtractGuidPatches(entityPatches, allEntities, at, afterElementPointer, afterElements, entityIndex, typeHashIndex);                        
                    }
                }
            }
        }

        static int GetTypeHashIndex(int targetTypeIndex, NativeList<ulong> typeHashes, NativeHashMap<int, int> typeHashLookup)
        {
            int result;
            if (!typeHashLookup.TryGetValue(targetTypeIndex, out result))
            {
                result = typeHashes.Length;
                typeHashes.Add(TypeManager.GetTypeInfo(targetTypeIndex).StableTypeHash);
                typeHashLookup.TryAdd(targetTypeIndex, result);
            }
            return result;
        }

        static int GetExternalIndex(EntityGuid guid, NativeList<EntityGuid> guidList, NativeHashMap<EntityGuid, int> guidToIndex)
        {
            int result;
            if (!guidToIndex.TryGetValue(guid, out result))
            {
                result = guidList.Length;
                guidList.Add(guid);
                guidToIndex.TryAdd(guid, result);
            }
            return result;
        }

        static void ComputeEntityDiff(
            NativeHashMap<EntityGuid, DiffEntityData> beforeState,
            NativeHashMap<EntityGuid, DiffEntityData> afterState,
            NativeList<EntityGuid> removedEntities,
            NativeList<DiffCreationData> createdEntities,
            NativeList<DiffModificationData> modifiedEntities)
        {
            NativeArray<EntityGuid> beforeGuids = beforeState.GetKeyArray(Allocator.Temp);
            NativeArray<EntityGuid> afterGuids = afterState.GetKeyArray(Allocator.Temp);

            Assert.AreEqual(beforeGuids.Length, beforeState.Length);
            Assert.AreEqual(afterGuids.Length, afterState.Length);

            NativeSortExtension.Sort(beforeGuids);
            NativeSortExtension.Sort(afterGuids);

            int ai = 0;
            int bi = 0;

            while (ai < afterGuids.Length && bi < beforeGuids.Length)
            {
                EntityGuid aguid = afterGuids[ai];
                EntityGuid bguid = beforeGuids[bi];

                int c = bguid.CompareTo(aguid);
                if (c < 0)
                {
                    removedEntities.Add(bguid);
                    ++bi;
                }
                else if (c == 0)
                {
                    DiffEntityData afterData;
                    DiffEntityData beforeData;
                    bool b = afterState.TryGetValue(aguid, out afterData);
                    Assert.IsTrue(b);
                    b = beforeState.TryGetValue(bguid, out beforeData);
                    Assert.IsTrue(b);

                    modifiedEntities.Add(new DiffModificationData
                    {
                        Guid = bguid,
                        AfterChunk = afterData.Chunk,
                        AfterIndex = afterData.Index,
                        BeforeChunk = beforeData.Chunk,
                        BeforeIndex = beforeData.Index,
                    });
                    ++ai;
                    ++bi;
                }
                else
                {
                    DiffEntityData d;
                    bool b = afterState.TryGetValue(aguid, out d);
                    Assert.IsTrue(b);
                    createdEntities.Add(new DiffCreationData {Chunk = d.Chunk, Guid = aguid, Index = d.Index});
                    ++ai;
                }
            }

            while (bi < beforeGuids.Length)
            {
                removedEntities.Add(beforeGuids[bi]);
                ++bi;
            }

            while (ai < afterGuids.Length)
            {
                DiffEntityData d;
                bool b = afterState.TryGetValue(afterGuids[ai], out d);
                Assert.IsTrue(b);
                createdEntities.Add(new DiffCreationData {Chunk = d.Chunk, Guid = afterGuids[ai], Index = d.Index});
                ++ai;
            }

            afterGuids.Dispose();
            beforeGuids.Dispose();
        }

        static void CreateEntityToGuidLookup(Chunk* c, NativeHashMap<Entity, EntityGuid> lookup)
        {
            var arch = c->Archetype;
            Entity* entities = (Entity*) (c->Buffer + arch->Offsets[0]);

            // Find EntityGuid type index
            int guidTypeIndex = TypeManager.GetTypeIndex<EntityGuid>();
            int guidIndex = 0;

            for (int i = 1; i < arch->TypesCount; ++i)
            {
                if (arch->Types[i].TypeIndex == guidTypeIndex)
                {
                    guidIndex = i;
                    break;
                }
            }

            Assert.IsTrue(guidIndex > 0);
            EntityGuid* guids = (EntityGuid*) (c->Buffer + arch->Offsets[guidIndex]);

            for (int i = 0; i < c->Count; ++i)
            {
                lookup.TryAdd(entities[i], guids[i]);
            }
        }

        static void AddChunkEntitiesToState(Chunk* c, NativeHashMap<EntityGuid, DiffEntityData> state)
        {
            var arch = c->Archetype;

            // Find EntityGuid type index
            int guidTypeIndex = TypeManager.GetTypeIndex<EntityGuid>();
            int guidIndex = 0;

            for (int i = 1; i < arch->TypesCount; ++i)
            {
                if (arch->Types[i].TypeIndex == guidTypeIndex)
                {
                    guidIndex = i;
                    break;
                }
            }

            Assert.IsTrue(guidIndex > 0);

            EntityGuid* ptr = (EntityGuid*) (c->Buffer + arch->Offsets[guidIndex]);

            for (int i = 0; i < c->Count; ++i)
            {
                var item = new DiffEntityData { Chunk = c, Index = i };
                state.TryAdd(ptr[i], item);
            }
        }

        static bool ChunkChanged(Chunk* sourceChunk, Chunk* destChunk)
        {
            int archCount = sourceChunk->Archetype->TypesCount;

            for (int ti = 0; ti < archCount; ++ti)
            {
                if (sourceChunk->ChangeVersion[ti] != destChunk->ChangeVersion[ti])
                {
                    return true;
                }
            }

            return false;
        }

        [BurstCompile]
        struct MapDiffGUIDsToDiffIndices : IJobParallelFor
        {
            [ReadOnly] public NativeArray<EntityGuid> DiffGuid;
            public NativeHashMap<EntityGuid, int>.Concurrent DiffGuidToDiffIndex;

            public void Execute(int index)
            {
                DiffGuidToDiffIndex.TryAdd(DiffGuid[index], index);
            }
        }

        [BurstCompile]
        struct MapDiffIndicesToWorldEntities : IJobChunk
        {
            [ReadOnly] public NativeHashMap<EntityGuid, int> DiffGuidToDiffIndex;
            public NativeMultiHashMap<int,Entity>.Concurrent DiffIndexToWorldEntity;
            
            [ReadOnly] public ArchetypeChunkComponentType<EntityGuid> EntityGUID;
            [ReadOnly] public ArchetypeChunkEntityType                Entity;

            public void Execute(ArchetypeChunk chunk, int entityIndex, int chunkIndex)
            {
                var entityGuids = chunk.GetNativeArray(EntityGUID);
                var entitys = chunk.GetNativeArray(Entity);
                for (int i = 0; i != entityGuids.Length; i++)
                {
                    int DiffIndex;
                    if (DiffGuidToDiffIndex.TryGetValue(entityGuids[i], out DiffIndex))
                        DiffIndexToWorldEntity.Add(DiffIndex, entitys[i]);
                }
            }
        }

        struct DiffApplier : IDisposable
        {
            World dest; // World to which we apply the diff
            WorldDiff diff; // Diff which we apply to World dest
            EntityManager destManager; // EntityManager for the dest World
            NativeMultiHashMap<int, Entity> DiffToWorldEntities; // from diff index to multiple dest world entities
            NativeArray<ComponentType> DiffToWorldTypes; // from diff type index to dest world typeindex

            public DiffApplier(World dest_, WorldDiff diff_)
            {
                dest = dest_;
                diff = diff_;
                destManager = dest.GetOrCreateManager<EntityManager>();
                destManager.CompleteAllJobs();
                DiffToWorldTypes = new NativeArray<ComponentType>(diff.TypeHashes.Length, Allocator.Temp);
                DiffToWorldEntities = default(NativeMultiHashMap<int, Entity>);
            }
            
            public void Apply()
            {
                BuildDiffToWorldEntities();
                BuildDiffToWorldTypes();               
                CreateEntities();
                DestroyEntities();
                AddComponents();
                RemoveComponents();
                SetSharedComponents();
                SetComponents();
                PatchEntities();
            }

            // every diff entity has a GUID, and maybe some world entities have GUIDs too.
            // in every case where the same GUID appears in the diff and the world,
            // we need a way to know which world entities the diff entity refers to.
            // we do not have that information yet. so, we build a table where for each
            // diff entity index, there is possibly one or more world entities.

            // first, we don't even look at the dest world. we need to build a hashmap from diff->diff before that.
            // we need something O(1) to go from diff GUID -> diff Entity index. let's build a hashmap now.  

            void BuildDiffToWorldEntities()
            {
                using(var DiffGuidToDiffIndex = new NativeHashMap<EntityGuid, int>(diff.Entities.Length, Allocator.TempJob))
                {
                    var MapDiffGUIDsToDiffEntitiesJob = new MapDiffGUIDsToDiffIndices
                    {
                        DiffGuid = diff.Entities,
                        DiffGuidToDiffIndex = DiffGuidToDiffIndex.ToConcurrent()
                    };
                    var dependency = MapDiffGUIDsToDiffEntitiesJob.Schedule(diff.Entities.Length, 64);
    
                    // ok, now that we have the hashmap needed, we use it to create the array (top of function)
                    // that we need for the remainder of the function. but after we build the array,
                    // we can throw away the hashmap.

                    // now that we built the diff GUID -> diff Entity hash, we can use it to build a diff Entity -> world Entity array.
                    // now we are going to look at the dest world for the first time.
                    // using the above hashmap we just created, we can make an array with one entry for each diff Entity, which
                    // contains the corresponding world Entity, if any exists.


                    using (var group = DiffUtil.CreateAllChunksGroup(destManager))
                    {
                        DiffToWorldEntities = new NativeMultiHashMap<int, Entity>(group.CalculateLength(), Allocator.TempJob);

                        //@TODO: Changing this to Allocator.Temp gives a totally crazy job debugger error... 
                        // Make proper repro test case...
                        var MapDiffEntitiesToExistingEntitiesJob = new MapDiffIndicesToWorldEntities
                        {
                            DiffGuidToDiffIndex = DiffGuidToDiffIndex,
                            DiffIndexToWorldEntity = DiffToWorldEntities.ToConcurrent(),
                            Entity = destManager.GetArchetypeChunkEntityType(),
                            EntityGUID = destManager.GetArchetypeChunkComponentType<EntityGuid>(true)
                        };

                        dependency = MapDiffEntitiesToExistingEntitiesJob.Schedule(group, dependency);
                    }

                    dependency.Complete();
                }                
            }

            void BuildDiffToWorldTypes()
            {
                for (var i = 0; i < diff.TypeHashes.Length; ++i)
                {
                    var memoryOrdering = diff.TypeHashes[i];
                    var typeIndex = TypeManager.GetTypeIndexFromStableTypeHash(memoryOrdering);
                    var type = TypeManager.GetType(typeIndex);
                    DiffToWorldTypes[i] = new ComponentType(type);
                }                
            }
            
            void CreateEntities()
            {
                var EntityGuidArchetype = destManager.CreateArchetype();
                using (var NewEntities = new NativeArray<Entity>(diff.NewEntityCount, Allocator.Temp))
                {
                    destManager.CreateEntity(EntityGuidArchetype, NewEntities);
                    for (var i = 0; i < diff.NewEntityCount; ++i)
                    {
                        DiffToWorldEntities.Add(i, NewEntities[i]);
#if LOG_DIFF_ALL
                        Debug.Log($"CreateEntity({diff.Entities[i]})");
#endif
                    }
                }
            }

            void DestroyEntities()
            {
                for (var i = 0; i < diff.DeletedEntityCount; ++i)
                {
                    if (DiffToWorldEntities.TryGetFirstValue(diff.NewEntityCount + i, out var entity, out var iterator))
                    {
                        do
                        {
                            if (destManager.Entities->Exists(entity))
                            {
#if LOG_DIFF_ALL
                                Debug.Log($"DestroyEntity({diff.Entities[diff.NewEntityCount + i]})");
#endif
                                destManager.DestroyEntity(entity);
                            }
                            else
                            {
                                Debug.LogWarning($"DestroyEntity({diff.Entities[diff.NewEntityCount + i]}) but it does not exist.");
                            }
                        } while (DiffToWorldEntities.TryGetNextValue(out entity, ref iterator));
                    }
                }
            }

            void AddComponents()
            {
                foreach (var addition in diff.AddComponents)
                {
                    var componentType = DiffToWorldTypes[addition.TypeHashIndex];
                    if (DiffToWorldEntities.TryGetFirstValue(addition.EntityIndex, out var entity, out var iterator))
                    {
                        do
                        {
                            destManager.AddComponent(entity, componentType);
#if LOG_DIFF_ALL
                            Debug.Log($"AddComponent<{componentType}>({diff.Entities[addition.EntityIndex]}, {Manager.Debug.GetComponentBoxed(entity, componentType)})");
#endif
                        } while (DiffToWorldEntities.TryGetNextValue(out entity, ref iterator));
                    }
                }
            }

            void RemoveComponents()
            {
                foreach (var removal in diff.RemoveComponents)
                {
                    var componentType = DiffToWorldTypes[removal.TypeHashIndex];
                    if (DiffToWorldEntities.TryGetFirstValue(removal.EntityIndex, out var entity, out var iterator))
                    {
                        do
                        {
                            destManager.RemoveComponent(entity, componentType);
#if LOG_DIFF_ALL
                            Debug.Log($"AddComponent<{componentType}>({diff.Entities[removal.EntityIndex]}).");
#endif
                        } while (DiffToWorldEntities.TryGetNextValue(out entity, ref iterator));
                    }
                }
            }

            void SetSharedComponents()
            {
                foreach (var shared in diff.SharedSetCommands)
                {
                    var componentType = DiffToWorldTypes[shared.TypeHashIndex];
                    var componentData = shared.BoxedSharedValue;
                    if (DiffToWorldEntities.TryGetFirstValue(shared.EntityIndex, out var entity, out var iterator))
                    {
                        do
                        {
                            if (!destManager.Exists(entity))
                                Debug.LogWarning($"SetComponent<{componentType}>({diff.Entities[shared.EntityIndex]}) but entity does not exist.");
                            else if (!destManager.HasComponent(entity, componentType))
                                Debug.LogWarning($"SetComponent<{componentType}>({diff.Entities[shared.EntityIndex]}) but component does not exist.");
                            else
                            {
                                destManager.SetSharedComponentDataBoxed(entity, componentType.TypeIndex, componentData);
#if LOG_DIFF_ALL
                                Debug.Log($"SetComponent<{componentType}>({diff.Entities[shared.EntityIndex]}, {componentData})");
#endif
                            }
                        } while (DiffToWorldEntities.TryGetNextValue(out entity, ref iterator));
                    }
                }
            }

            void SetComponents()
            {
                long readOffset = 0;
                foreach (var setting in diff.SetCommands)
                {
                    var data = (byte*) diff.ComponentPayload.GetUnsafePtr() + readOffset;
                    var size = setting.SizeBytes;
                    var componentType = DiffToWorldTypes[setting.TypeHashIndex];
                    ComponentTypeInArchetype ctia = new ComponentTypeInArchetype(componentType);
                    if (DiffToWorldEntities.TryGetFirstValue(setting.EntityIndex, out var entity, out var iterator))
                    {
                        do
                        {
                            if (!destManager.Exists(entity))
                                Debug.LogWarning($"SetComponent<{componentType}>({diff.Entities[setting.EntityIndex]}) but entity does not exist.");
                            else if (!destManager.HasComponent(entity, componentType))
                                Debug.LogWarning($"SetComponent<{componentType}>({diff.Entities[setting.EntityIndex]}) but component does not exist.");
                            else
                            {
                                if (!ctia.IsBuffer)
                                {
                                    var target = (byte*)destManager.GetComponentDataRawRW(entity, componentType.TypeIndex);
                                    UnsafeUtility.MemCpy(target + setting.Offset, data, size);
                                }
                                else
                                {
                                    var typeInfo = TypeManager.GetTypeInfo(ctia.TypeIndex);
                                    var elementSize = typeInfo.ElementSize;
                                    var lengthInElements = size / elementSize;
                                    var header = (BufferHeader*)destManager.GetComponentDataRawRW(entity, componentType.TypeIndex);
                                    BufferHeader.Assign(header, data, lengthInElements, elementSize, 16);
                                }
#if LOG_DIFF_ALL
                                Debug.Log($"SetComponent<{componentType}>({diff.Entities[setting.EntityIndex]}, {Manager.Debug.GetComponentBoxed(entity, componentType)})");
#endif
                            }
                        } while (DiffToWorldEntities.TryGetNextValue(out entity, ref iterator));
                    }
                    readOffset += size;
                }
            }

            void PatchEntities()
            {
                if (diff.EntityPatches.Length > 0)
                {
                    using (var allEntities = DiffUtil.GenerateEntityGuidToEntityHashMap(dest)) // this runs fast, it takes only one line of code
                    {
                        foreach (var patch in diff.EntityPatches)
                        {
                            var componentType = DiffToWorldTypes[patch.TypeHashIndex];
                            var guid = patch.Guid;
                            var offset = patch.Offset;
                            Entity freshEntity;
                            if (guid.Equals(EntityGuid.Null))
                                freshEntity = Entity.Null;
                            else
                                allEntities.TryGetValue(guid, out freshEntity);
                            if (DiffToWorldEntities.TryGetFirstValue(patch.EntityIndex, out var entity, out var iterator))
                            {
                                do
                                {
                                    if (!destManager.Exists(entity))
                                        Debug.LogWarning($"SetComponent_EntityPatch<{componentType}>({diff.Entities[patch.EntityIndex]}) but entity does not exist.");
                                    else if (!destManager.HasComponent(entity, componentType))
                                        Debug.LogWarning($"SetComponent_EntityPatch<{componentType}>({diff.Entities[patch.EntityIndex]}) but component does not exist.");
                                    else
                                    {
                                        if (componentType.BufferCapacity > 0)
                                        {
                                            byte* pointer = (byte*) destManager.GetBufferRawRW(entity, componentType.TypeIndex);
                                            UnsafeUtility.MemCpy(pointer + offset, &freshEntity, sizeof(Entity));
                                        }
                                        else
                                        {
                                            byte* pointer = (byte*) destManager.GetComponentDataRawRW(entity, componentType.TypeIndex);
                                            UnsafeUtility.MemCpy(pointer + offset, &freshEntity, sizeof(Entity));
                                        }
#if LOG_DIFF_ALL
                                        Debug.Log($"SetComponent_EntityPatch<{componentType}>({diff.Entities[patch.EntityIndex]}, {Manager.Debug.GetComponentBoxed(entity, componentType)})");
#endif
                                    }
                                } while (DiffToWorldEntities.TryGetNextValue(out entity, ref iterator));
                            }
                        }
                    }
                }
            }

            public void Dispose()
            {
                DiffToWorldEntities.Dispose();
                DiffToWorldTypes.Dispose();
            }
        }

        public static void ApplyDiff(World dest, WorldDiff diff)
        {
#if LOG_DIFF_ALL
            Debug.Log("--- Begin Apply diff ---- ");
#endif
            using(var applier = new DiffApplier(dest, diff))
                applier.Apply();
#if LOG_DIFF_ALL
            Debug.Log("--- End Apply diff ---- ");
#endif
        }
    }
}

