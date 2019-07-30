local ArchetypeManager = ECS.BaseClass()
ECS.ArchetypeManager = ArchetypeManager

function ArchetypeManager:Constructor( sharedComponentManager )
	self.m_SharedComponentManager = sharedComponentManager
    self.m_TypeLookup = {}
    self.m_EmptyChunkPool = ECS.UnsafeLinkedListNode.New()
    ECS.UnsafeLinkedListNode.InitializeList(self.m_EmptyChunkPool)
end

local GetTypesStr = function ( types, count )
    local names = {}
    for i=1,count do
        table.insert(names, ECS.TypeManager.GetTypeNameByIndex(types[i].TypeIndex))
    end
    table.sort(names)
    return table.concat(names, ":")
end
ArchetypeManager.GetTypesStr = GetTypesStr

function ArchetypeManager:GetOrCreateArchetype( types, count, groupManager )
	local type = self:GetExistingArchetype(types, count)
    return type~=nil and type or self:CreateArchetypeInternal(types, count, groupManager)
end           

function ArchetypeManager:GetExistingArchetype( types, count )
    local type_str = GetTypesStr(types, count)
    return self.m_TypeLookup[type_str]
end

function ArchetypeManager:CreateArchetypeInternal( types, count, groupManager )
    local type = {}
    type.TypesCount = count
    type.Types = types--should be deepcopy
    type.EntityCount = 0
    type.ChunkCount = 0
    type.TypesMap = {}
    for k,v in pairs(types) do
        local typeName = ECS.TypeManager.GetTypeNameByIndex(v.TypeIndex)
        type.TypesMap[typeName] = true
    end
    type.NumSharedComponents = 0
    type.SharedComponentOffset = nil

    --交给lua的table自动扩充
    type.ChunkCapacity = 2^50

    type.NumManagedArrays = 0
    type.ManagedArrayOffset = nil

    type.PrevArchetype = self.m_LastArchetype
    self.m_LastArchetype = type

    type.ChunkList = ECS.UnsafeLinkedListNode.New()
    type.ChunkListWithEmptySlots = ECS.UnsafeLinkedListNode.New()
    ECS.UnsafeLinkedListNode.InitializeList(type.ChunkList)
    ECS.UnsafeLinkedListNode.InitializeList(type.ChunkListWithEmptySlots)

    local type_str = GetTypesStr(types, count)
    self.m_TypeLookup[type_str] = type

    groupManager:AddArchetypeIfMatching(type)
    return type
end

function ArchetypeManager:AddExistingChunk( chunk )
	local archetype = chunk.Archetype
    archetype.ChunkList.Add(chunk.ChunkListNode)
    archetype.ChunkCount = archetype.ChunkCount + 1
    archetype.EntityCount = archetype.EntityCount + chunk.Count
    for var=1,archetype.NumSharedComponents do
        m_SharedComponentManager.AddReference(chunk.SharedComponentValueArray[i])
    end
    if (chunk.Count < chunk.Capacity) then
        if (archetype.NumSharedComponents == 0) then
            archetype.ChunkListWithEmptySlots.Add(chunk.ChunkListWithEmptySlotsNode)
        else
            archetype.FreeChunksBySharedComponents.Add(chunk)
        end
    end
end

function ArchetypeManager:ConstructChunk( archetype, chunk, sharedComponentDataIndices )
	chunk.Archetype = archetype

    chunk.Count = 0
    chunk.Capacity = archetype.ChunkCapacity
    chunk.ChunkListNode = ECS.UnsafeLinkedListNode.New()
    chunk.ChunkListNode:SetChunk(chunk)
    chunk.ChunkListWithEmptySlotsNode = ECS.UnsafeLinkedListNode.New()
    chunk.ChunkListWithEmptySlotsNode:SetChunk(chunk)

    for k,v in pairs(archetype.Types) do
        local componentName = ECS.TypeManager.GetTypeNameByIndex(v.TypeIndex)
        chunk.Buffer[componentName] = {}
    end

    local numSharedComponents = archetype.NumSharedComponents
    local numTypes = archetype.TypesCount
    local sharedComponentOffset = ECS.Chunk.GetSharedComponentOffset(numSharedComponents)
    local changeVersionOffset = ECS.Chunk.GetChangedComponentOffset(numTypes, numSharedComponents)

    -- chunk.SharedComponentValueArray = (chunk + sharedComponentOffset)
    -- chunk.ChangeVersion = (chunk + changeVersionOffset)

    archetype.ChunkList:Add(chunk.ChunkListNode)
    archetype.ChunkCount = archetype.ChunkCount+1
    if (numSharedComponents == 0) then
        archetype.ChunkListWithEmptySlots:Add(chunk.ChunkListWithEmptySlotsNode)
    end
end

function ArchetypeManager:AllocateIntoChunk( chunk, count )
    count = count or 1
	local allocatedCount = math.min(chunk.Capacity - chunk.Count, count)
    local newChunkIndex = chunk.Count+1
    self:SetChunkCount(chunk, chunk.Count + allocatedCount)
    chunk.Archetype.EntityCount = chunk.Archetype.EntityCount + allocatedCount
    return allocatedCount, newChunkIndex
end

function ArchetypeManager:SetChunkCount( chunk, newCount )
    -- Assert.AreNotEqual(newCount, chunk.Count);
    local capacity = chunk.Capacity
    -- Chunk released to empty chunk pool
    if newCount == 0 then
        chunk.Archetype.ChunkCount = chunk.Archetype.ChunkCount - 1
        chunk.Archetype = nil
        chunk.ChunkListNode:Remove()
        chunk.ChunkListWithEmptySlotsNode:Remove()

        self.m_EmptyChunkPool:Add(chunk.ChunkListNode)
    -- Chunk is now full
    elseif (newCount == capacity) then
        chunk.ChunkListWithEmptySlotsNode:Remove()
    -- Chunk is no longer full
    elseif (chunk.Count == capacity) then
        -- Assert.IsTrue(newCount < chunk.Count)
        if (chunk.Archetype.NumSharedComponents == 0) then
            chunk.Archetype.ChunkListWithEmptySlots:Add(chunk.ChunkListWithEmptySlotsNode)
        else
            chunk.Archetype.FreeChunksBySharedComponents:Add(chunk)
        end
    end
    chunk.Count = newCount
end

function ArchetypeManager:GetChunkFromEmptySlotNode( node )
    return node and node:GetChunk()
end

function ArchetypeManager:GetChunkWithEmptySlots( archetype, sharedComponentDataIndices )
	if archetype.NumSharedComponents == 0 then
        if not archetype.ChunkListWithEmptySlots:IsEmpty() then
            local chunk = self:GetChunkFromEmptySlotNode(archetype.ChunkListWithEmptySlots:Begin())
            -- Assert.AreNotEqual(chunk.Count, chunk.Capacity)
            return chunk
        end
    else
        local chunk = archetype.FreeChunksBySharedComponents:GetChunkWithEmptySlots(sharedComponentDataIndices,
            archetype.NumSharedComponents)
        if chunk ~= nil then
            return chunk
        end
    end

    -- Try existing archetype chunks
    if not archetype.ChunkListWithEmptySlots.IsEmpty then
        if self.lastChunkWithSharedComponentsAllocatedInto ~= nil 
        	and self.lastChunkWithSharedComponentsAllocatedInto.Archetype == archetype 
        	and self.lastChunkWithSharedComponentsAllocatedInto.Count < self.lastChunkWithSharedComponentsAllocatedInto.Capacity then
            if self:ChunkHasSharedComponents(self.lastChunkWithSharedComponentsAllocatedInto, sharedComponentDataIndices) then
                return self.lastChunkWithSharedComponentsAllocatedInto
            end
        end

        if archetype.NumSharedComponents == 0 then
            local chunk = self:GetChunkFromEmptySlotNode(archetype.ChunkListWithEmptySlots:Begin())
            -- Assert.AreNotEqual(chunk.Count, chunk.Capacity)
            return chunk
        end
    end

    local newChunk
    -- Try empty chunk pool
    if not self.m_EmptyChunkPool or self.m_EmptyChunkPool:IsEmpty() then
        -- Allocate new chunk
        newChunk = ECS.Chunk.New()
    else
        newChunk = self.m_EmptyChunkPool:Begin()
        newChunk = newChunk:GetChunk()
        newChunk.ChunkListNode:Remove()
    end

    self:ConstructChunk(archetype, newChunk, sharedComponentDataIndices)

    if archetype.NumSharedComponents > 0 then
        self.lastChunkWithSharedComponentsAllocatedInto = newChunk
    end
    return newChunk
end

return ArchetypeManager