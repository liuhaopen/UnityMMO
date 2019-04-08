local EntityDataManager = BaseClass()
ECS.EntityDataManager = EntityDataManager

local EntityData = {
	Version=0, 
	Archetype = nil,
	Chunk = nil,
	IndexInChunk = 0
}
function EntityDataManager:Constructor( )
	self.m_EntitiesCapacity = 10
    self.m_Entities = self:CreateEntityData(self.m_EntitiesCapacity)
	self.m_EntitiesFreeIndex = 1
    -- self.GlobalSystemVersion = ECS.ChangeVersionUtility.InitialGlobalSystemVersion
    self.GlobalSystemVersion = 1

	self:InitializeAdditionalCapacity(1)
    self.m_ComponentTypeOrderVersion = {}
	-- const int componentTypeOrderVersionSize = sizeof(int) * TypeManager.MaximumTypesCount;
 --    m_ComponentTypeOrderVersion = (int*) UnsafeUtility.Malloc(componentTypeOrderVersionSize,
 --        UnsafeUtility.AlignOf<int>(), Allocator.Persistent);
 --    UnsafeUtility.MemClear(m_ComponentTypeOrderVersion, componentTypeOrderVersionSize);
end

function EntityDataManager:InitializeAdditionalCapacity( start )
	for i=start,self.m_EntitiesCapacity do
        self.m_Entities.ChunkData[i] = {}
		self.m_Entities.ChunkData[i].IndexInChunk = i+1
        self.m_Entities.Version[i] = 1
        self.m_Entities.ChunkData[i].Chunk = nil
        -- self.m_Entities[i].Archetype = nil
	end
    --Last entity indexInChunk identifies that we ran out of space...
    self.m_Entities.ChunkData[self.m_EntitiesCapacity].IndexInChunk = -1;
end

function EntityDataManager:HasComponent( entity, comp_type_name )
	if not self:Exists(entity) then 
		return false
	end
	local archetype = self.m_Entities.Archetype[entity.Index]
    local type_index = ECS.TypeManager.GetTypeIndexByName(comp_type_name)
    return ECS.ChunkDataUtility.GetIndexInTypeArray(archetype, type_index) ~= -1;
end

function EntityDataManager:ValidateEntity( entity )
    if (entity.Index >= self.m_EntitiesCapacity) then
        assert(false, "All entities passed to EntityManager must exist. One of the entities has already been destroyed or was never created.")
    end
end

function EntityDataManager:Exists( entity )
    local index = entity.Index
    self:ValidateEntity(entity)
    local versionMatches = self.m_Entities.Version[index] == entity.Version
    local hasChunk = self.m_Entities.ChunkData[index] and self.m_Entities.ChunkData[index].Chunk ~= nil
    return versionMatches and hasChunk
end

function EntityDataManager:AssertEntityHasComponent( entity, com_type_name )
    if not self:Exists(entity) then
        assert(false, "The Entity does not exist")
    end
    if self:HasComponent(entity, com_type_name) then
        return
    end
    -- if not self:HasComponent(entity, com_type_name) then
        -- assert(false, "The component exists on the entity but the exact type {componentType} does not")
    -- end
    assert(false, "component has not been added to the entity.")
end
            
function EntityDataManager:GetComponentDataWithTypeRO( entity, typeIndex )
	local entityChunk = self.m_Entities.ChunkData[entity.Index].Chunk
    local entityIndexInChunk = self.m_Entities.ChunkData[entity.Index].IndexInChunk
    return ECS.ChunkDataUtility.GetComponentDataWithTypeRO(entityChunk, entityIndexInChunk, typeIndex)
end

function EntityDataManager:CreateEntities( archetypeManager, archetype, count )
    -- local sharedComponentDataIndices = stackalloc int[archetype.NumSharedComponents]
    -- UnsafeUtility.MemClear(sharedComponentDataIndices, archetype.NumSharedComponents*sizeof(int))
    local entities = {}
    local sharedComponentDataIndices = {}
    local num = 0
    while count ~= 0 do
        local chunk = archetypeManager:GetChunkWithEmptySlots(archetype, sharedComponentDataIndices)
        local allocatedIndex
        local allocatedCount, allocatedIndex = archetypeManager:AllocateIntoChunk(chunk, count)
        num = num + allocatedCount
        local tmp_entities = {}
        self:AllocateEntities(archetype, chunk, allocatedIndex, allocatedCount, tmp_entities)
        ECS.ChunkDataUtility.InitializeComponents(chunk, allocatedIndex, allocatedCount)
        for i,v in ipairs(tmp_entities) do
            table.insert(entities, v)
        end
        count = count - allocatedCount
    end
    self:IncrementComponentTypeOrderVersion(archetype)
    return entities
end

function EntityDataManager:IncrementComponentTypeOrderVersion( archetype )
	for t=1,archetype.TypesCount do
		local typeIndex = archetype.Types[t].TypeIndex
        if not self.m_ComponentTypeOrderVersion[typeIndex] then
            self.m_ComponentTypeOrderVersion[typeIndex] = 0
        end
        self.m_ComponentTypeOrderVersion[typeIndex] = self.m_ComponentTypeOrderVersion[typeIndex] + 1
	end
end

function EntityDataManager:GetComponentTypeOrderVersion( typeIndex )
    return self.m_ComponentTypeOrderVersion[typeIndex]
end

function EntityDataManager:GetArchetype( entity )
    return self.m_Entities.Archetype[entity.Index]
end

function EntityDataManager:AddComponent( entity, comp_type_name, archetypeManager, sharedComponentDataManager, groupManager, componentTypeInArchetypeArray )
    local componentType = ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(comp_type_name))
	local archetype = self:GetArchetype(entity)

    local t = 1
    componentTypeInArchetypeArray = {}--Cat_Todo : obj pool optimize
    while (t <= archetype.TypesCount and archetype.Types[t] < componentType) do
        componentTypeInArchetypeArray[t] = archetype.Types[t]
        t = t + 1
    end
    --按顺序把新的类型插入临时列表里
    componentTypeInArchetypeArray[t] = componentType
    while (t <= archetype.TypesCount) do
        componentTypeInArchetypeArray[t + 1] = archetype.Types[t]
        t = t + 1
    end
    local newType = archetypeManager:GetOrCreateArchetype(componentTypeInArchetypeArray,
        archetype.TypesCount + 1, groupManager)
    local sharedComponentDataIndices = nil
    if newType.NumSharedComponents > 0 then
        local oldSharedComponentDataIndices = self:GetComponentChunk(entity).SharedComponentValueArray
        if type.IsSharedComponent then
            -- local stackAlloced = stackalloc int[newType.NumSharedComponents]
            sharedComponentDataIndices = stackAlloced

            if archetype.SharedComponentOffset == nil then
                sharedComponentDataIndices[1] = 0
            else
                t = 1
                local sharedIndex = 1
                while t <= archetype.TypesCount and archetype.Types[t] < componentType do
                    if archetype.SharedComponentOffset[t] ~= -1 then
                        sharedComponentDataIndices[sharedIndex] = oldSharedComponentDataIndices[sharedIndex]
                        sharedIndex = sharedIndex + 1
                    end
                    t = t + 1
                end

                sharedComponentDataIndices[sharedIndex] = 0
                while (t <= archetype.TypesCount) do
                    if (archetype.SharedComponentOffset[t] ~= -1) then
                        sharedComponentDataIndices[sharedIndex + 1] =
                            oldSharedComponentDataIndices[sharedIndex]
                        sharedIndex = sharedIndex + 1
                    end
                    t = t + 1
                end
            end
        else
            -- reuse old sharedComponentDataIndices
            sharedComponentDataIndices = oldSharedComponentDataIndices
        end
    end

    self:SetArchetype(archetypeManager, entity, newType, sharedComponentDataIndices)
    self:IncrementComponentOrderVersion(newType, self:GetComponentChunk(entity), sharedComponentDataManager)
end

function EntityDataManager:RemoveComponent( entity, comp_type_name, archetypeManager, sharedComponentDataManager, groupManager, componentTypeInArchetypeArray )
    local componentType = ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(comp_type_name))
    local archetype = self:GetArchetype(entity)

    local removedTypes = 0
    local indexInOldTypeArray = -1
    for t=1,archetype.TypesCount do
        if archetype.Types[t].TypeIndex == componentType.TypeIndex then
            indexInOldTypeArray = t
            removedTypes = removedTypes + 1
        else
            componentTypeInArchetypeArray[t - removedTypes] = archetype.Types[t]
        end
    end
    local newType = archetypeManager:GetOrCreateArchetype(componentTypeInArchetypeArray,
        archetype.TypesCount - removedTypes, groupManager)

    local sharedComponentDataIndices = self:GetComponentChunk(entity).SharedComponentValueArray
    if ((newType.NumSharedComponents > 0) and (newType.NumSharedComponents ~= archetype.NumSharedComponents)) then
        local oldSharedComponentDataIndices = sharedComponentDataIndices
        -- local tempAlloc = stackalloc int[newType.NumSharedComponents]
        local tempAlloc = {}
        sharedComponentDataIndices = tempAlloc

        local indexOfRemovedSharedComponent = archetype.SharedComponentOffset[indexInOldTypeArray]
        UnsafeUtility.MemCpy(sharedComponentDataIndices, oldSharedComponentDataIndices, indexOfRemovedSharedComponent*sizeof(int))
        UnsafeUtility.MemCpy(sharedComponentDataIndices + indexOfRemovedSharedComponent, oldSharedComponentDataIndices + indexOfRemovedSharedComponent + 1, (newType.NumSharedComponents-indexOfRemovedSharedComponent)*sizeof(int))
    end

    self:IncrementComponentOrderVersion(archetype, self:GetComponentChunk(entity), sharedComponentDataManager)
    self:SetArchetype(archetypeManager, entity, newType, sharedComponentDataIndices)
end

function EntityDataManager:IncrementComponentOrderVersion(  )
    
end

function EntityDataManager:GetComponentChunk( entity )
    return self.m_Entities.ChunkData[entity.Index].Chunk
end

function EntityDataManager:TryRemoveEntityId( entities, count, archetypeManager, sharedComponentDataManager, groupManager, componentTypeInArchetypeArray )
    local entityIndex = 0;
    while (entityIndex ~= count) do
        local indexInChunk
        local batchCount
        local manager = self
        local chunk = EntityChunkBatch(manager, entities + entityIndex, count - entityIndex, indexInChunk,
            batchCount);
        local archetype = GetArchetype(entities[entityIndex])
        if (not archetype.SystemStateCleanupNeeded) then
            DeallocateDataEntitiesInChunk(manager, entities + entityIndex, chunk, indexInChunk, batchCount)
            self:IncrementComponentOrderVersion(chunk.Archetype, chunk, sharedComponentDataManager)

            if (chunk.ManagedArrayIndex >= 0) then
                -- We can just chop-off the end, no need to copy anything
                if (chunk.Count ~= indexInChunk + batchCount) then
                    ChunkDataUtility.CopyManagedObjects(archetypeManager, chunk, chunk.Count - batchCount,
                        chunk,
                        indexInChunk, batchCount)
                end

                ChunkDataUtility.ClearManagedObjects(archetypeManager, chunk, chunk.Count - batchCount,
                    batchCount)
            end

            chunk.Archetype.EntityCount = chunk.Archetype.EntityCount - batchCount
            archetypeManager.SetChunkCount(chunk, chunk.Count - batchCount)
        else
            for batchEntityIndex=1,batchCount do
                local entity = entities[entityIndex + batchEntityIndex]
                local removedTypes = 0
                local removedComponentIsShared = false
                for t=2,archetype.TypesCount do
                    local type = archetype.Types[t]
                    
                    if (not (type.IsSystemStateComponent or type.IsSystemStateSharedComponent)) then
                        removedTypes = removedTypes + 1
                        -- removedComponentIsShared |= type.IsSharedComponent
                    else
                        componentTypeInArchetypeArray[t - removedTypes] = archetype.Types[t]
                    end
                end

                componentTypeInArchetypeArray[archetype.TypesCount - removedTypes] =
                    new ComponentTypeInArchetype(ComponentType.Create("CleanupEntity"))

                local newType = archetypeManager.GetOrCreateArchetype(componentTypeInArchetypeArray,
                    archetype.TypesCount - removedTypes + 1, groupManager)

                local sharedComponentDataIndices = nil
                if (newType.NumSharedComponents > 0) then
                    local oldSharedComponentDataIndices =
                        GetComponentChunk(entity).SharedComponentValueArray
                    if (removedComponentIsShared) then
                        local tempAlloc = {}
                        sharedComponentDataIndices = tempAlloc

                        local srcIndex = 0
                        local dstIndex = 0
                        for t=1,archetype.TypesCount do
                            if (archetype.SharedComponentOffset[t] ~= -1) then
                                local typeIndex = archetype.Types[t].TypeIndex
                                local systemStateType = typeof(ISystemStateComponentData).IsAssignableFrom(TypeManager.GetType(typeIndex))
                                local systemStateSharedType = typeof(ISystemStateSharedComponentData).IsAssignableFrom(TypeManager.GetType(typeIndex))
                                if (not (systemStateType or systemStateSharedType)) then
                                    srcIndex = srcIndex + 1
                                else
                                    sharedComponentDataIndices[dstIndex] =
                                        oldSharedComponentDataIndices[srcIndex]
                                    srcIndex = srcIndex + 1
                                    dstIndex = dstIndex + 1
                                end
                            end
                        end
                    else
                        -- reuse old sharedComponentDataIndices
                        sharedComponentDataIndices = oldSharedComponentDataIndices;
                    end
                end
                self:IncrementComponentOrderVersion(archetype, GetComponentChunk(entity),
                    sharedComponentDataManager);
                self:SetArchetype(archetypeManager, entity, newType, sharedComponentDataIndices);
            end
        end
    end
    entityIndex = entityIndex + batchCount
end

function EntityDataManager:SetArchetype( typeMan, entity, archetype, sharedComponentDataIndices )
    local chunk = typeMan:GetChunkWithEmptySlots(archetype, sharedComponentDataIndices)
    local allocatedCount, chunkIndex = typeMan:AllocateIntoChunk(chunk)

    local oldArchetype = self.m_Entities.Archetype[entity.Index]
    local oldChunk = self.m_Entities.ChunkData[entity.Index].Chunk
    local oldChunkIndex = self.m_Entities.ChunkData[entity.Index].IndexInChunk
    ECS.ChunkDataUtility.Convert(oldChunk, oldChunkIndex, chunk, chunkIndex)
    -- if chunk.ManagedArrayIndex >= 0 and oldChunk.ManagedArrayIndex >= 0 then
    --     ECS.ChunkDataUtility.CopyManagedObjects(typeMan, oldChunk, oldChunkIndex, chunk, chunkIndex, 1)
    -- end

    self.m_Entities.Archetype[entity.Index] = archetype
    self.m_Entities.ChunkData[entity.Index].Chunk = chunk
    self.m_Entities.ChunkData[entity.Index].IndexInChunk = chunkIndex

    local lastIndex = oldChunk.Count
    if (lastIndex ~= oldChunkIndex) then
        local lastEntity = ECS.ChunkDataUtility.GetComponentDataRO(oldChunk, lastIndex, 1)
        lastEntity = ECS.ChunkDataUtility.ReadComponentFromChunk(lastEntity, ECS.Entity.Name)
        self.m_Entities.ChunkData[lastEntity.Index].IndexInChunk = oldChunkIndex

        ECS.ChunkDataUtility.Copy(oldChunk, lastIndex, oldChunk, oldChunkIndex, 1)
        if (oldChunk.ManagedArrayIndex >= 0) then
            ChunkDataUtility.CopyManagedObjects(typeMan, oldChunk, lastIndex, oldChunk, oldChunkIndex, 1)
        end
    end

    if (oldChunk.ManagedArrayIndex >= 0) then
        ChunkDataUtility.ClearManagedObjects(typeMan, oldChunk, lastIndex, 1)
    end
    --Entity归新的Archetype了，所以旧的EnityCount要减1
    oldArchetype.EntityCount = oldArchetype.EntityCount - 1
    typeMan:SetChunkCount(oldChunk, lastIndex)
end

function EntityDataManager:AllocateEntities( arch, chunk, baseIndex, count, outputEntities )
    if not self.entity_size_in_chunk then 
        self.entity_size_in_chunk = ECS.TypeManager.GetTypeInfoByName(ECS.Entity.Name).SizeInChunk
    end
    -- local entityInChunkStart = chunk.Buffer + baseIndex
    for i=1,count do
        local entityIndexInChunk = self.m_Entities.ChunkData[self.m_EntitiesFreeIndex].IndexInChunk
        if entityIndexInChunk == -1 then
            self:IncreaseCapacity()
            entityIndexInChunk = self.m_Entities.ChunkData[self.m_EntitiesFreeIndex].IndexInChunk
        end
        
        local entityVersion = self.m_Entities.Version[self.m_EntitiesFreeIndex]
        outputEntities[i] = {}
        outputEntities[i].Index = self.m_EntitiesFreeIndex
        outputEntities[i].Version = entityVersion

        ECS.ChunkDataUtility.WriteComponentInChunk(chunk.Buffer + (baseIndex + i - 1)*self.entity_size_in_chunk, ECS.Entity.Name, {Index=self.m_EntitiesFreeIndex, Version=entityVersion})

        self.m_Entities.ChunkData[self.m_EntitiesFreeIndex].IndexInChunk = baseIndex + i - 1
        self.m_Entities.Archetype[self.m_EntitiesFreeIndex] = arch
        self.m_Entities.ChunkData[self.m_EntitiesFreeIndex].Chunk = chunk
        
        self.m_EntitiesFreeIndex = entityIndexInChunk
    end
end

function EntityDataManager:IncreaseCapacity(  )
    self:SetCapacity(self.m_EntitiesCapacity*2)
end

function EntityDataManager:GetCapacity( )
    return self.m_EntitiesCapacity
end

local CopyEntityData = function ( dstEntityData, srcEntityData, copySize )
    if not srcEntityData or #srcEntityData <= 0 then return end
    
    for i,v in ipairs(srcEntityData) do
        for ii,vv in ipairs(dstEntityData) do
            vv.Version = v.Version
            vv.Archetype = v.Archetype
            vv.ChunkData = v.ChunkData
        end
    end
end

function EntityDataManager:SetCapacity( value )
    if value <= self.m_EntitiesCapacity then
        return
    end

    -- local newEntities = self:CreateEntityData(value)
    -- CopyEntityData(newEntities, self.m_Entities, self.m_EntitiesCapacity)
    -- self.m_Entities = nil
    
    -- self.m_Entities = newEntities
    local startNdx = self.m_EntitiesCapacity
    self.m_EntitiesCapacity = value
    self:InitializeAdditionalCapacity(startNdx)
    -- self.Capacity = value
end

function EntityDataManager:CreateEntityData( newCapacity )
    local entities = {}
    entities.Version   = {}
    entities.Archetype = {}
    entities.ChunkData = {}
    return entities
end

function EntityDataManager:GetComponentDataWithTypeRW( entity, typeIndex, globalVersion )
    local entityChunk = self.m_Entities.ChunkData[entity.Index].Chunk
    local entityIndexInChunk = self.m_Entities.ChunkData[entity.Index].IndexInChunk
    return ECS.ChunkDataUtility.GetComponentDataWithTypeRW(entityChunk, entityIndexInChunk, typeIndex,
        globalVersion);
end
            
return EntityDataManager