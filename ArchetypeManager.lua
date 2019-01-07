local ArchetypeManager = BaseClass()
ECS.ArchetypeManager = ArchetypeManager

function ArchetypeManager:Constructor( sharedComponentManager )
	self.m_SharedComponentManager = sharedComponentManager
    self.m_TypeLookup = {}
    self.m_EmptyChunkPool = {}
    -- ECS.UnsafeLinkedListNode.InitializeList(self.m_EmptyChunkPool)
end

-- local GetHash = function ( componentTypeInArchetypes )
-- 	local hash = HashUtility.Fletcher32(componentTypeInArchetypes,
--         #componentTypeInArchetypes * sizeof(ComponentTypeInArchetype) / sizeof(ushort))
--     return hash
-- end

local GetTypesStr = function ( types )
    local names = {}
    for k,v in pairs(types) do
        table.insert(names, v.Name)
    end
    table.sort(names)
    return table.concat(names)
end

function ArchetypeManager:GetOrCreateArchetypeInternal( types, count, groupManager )
	local type = self:GetExistingArchetype(types)
    return type~=nil and type or self:CreateArchetypeInternal(types, count, groupManager)
end           

function ArchetypeManager:GetExistingArchetype( types )
    local type_str = GetTypesStr(types)
    return self.m_TypeLookup[type_str]
    -- if not self.m_TypeLookup[type_str] then
    --     return nil
    -- end
    -- repeat
    --     local type = typePtr
    --     if (ComponentTypeInArchetype.CompareArray(type.Types, type.TypesCount, types, #types)) then
    --         return type
    --     end
    -- until (not (self.m_TypeLookup:TryGetNextValue(typePtr, it)))
    -- return nil
end

function ArchetypeManager:GetOrCreateArchetype( types, count, groupManager )
    local srcArchetype = self:GetOrCreateArchetypeInternal(types, count, groupManager)
    local removedTypes = 0
    -- local prefabTypeIndex = ECS.TypeManager.GetTypeIndexByName("Prefab")
    for t=1,srcArchetype.TypesCount do
        local type = srcArchetype.Types[t]
        -- local skip = type.IsSystemStateComponent or type.IsSystemStateSharedComponent or (type.TypeIndex == prefabTypeIndex)
        -- if skip then
        --     removedTypes = removedTypes + 1
        -- else
            types[t - removedTypes] = srcArchetype.Types[t]
        -- end
    end

    srcArchetype.InstantiableArchetype = srcArchetype
    if removedTypes > 0 then
        local instantiableArchetype = self:GetOrCreateArchetypeInternal(types, count-removedTypes, groupManager)
        srcArchetype.InstantiableArchetype = instantiableArchetype
        instantiableArchetype.InstantiableArchetype = instantiableArchetype
    end
    return srcArchetype
end

function ArchetypeManager:CreateArchetypeInternal( types, count, groupManager )
    local type = {}
    type.TypesCount = count
    type.Types = {}
    type.EntityCount = 0
    type.ChunkCount = 0

    type.NumSharedComponents = 0
    type.SharedComponentOffset = nil

    -- local disabledTypeIndex = ECS.TypeManager.GetTypeIndex("Disabled")
    -- local prefabTypeIndex = ECS.TypeManager.GetTypeIndex("Prefab>")
    type.Disabled = false
    type.Prefab = false
    -- for i=1,count do
    --     if (ECS.TypeManager.GetTypeInfo(types[i].TypeIndex).Category == ECS.TypeManager.TypeCategory.ISharedComponentData) then
    --         type.NumSharedComponents = type.NumSharedComponents + 1
    --     end
    --     if (types[i].TypeIndex == disabledTypeIndex) then
    --         type.Disabled = true
    --     end
    --     if (types[i].TypeIndex == prefabTypeIndex) then
    --         type.Prefab = true
    --     end
    -- end

    local scalarEntityPatchCount = 0
    local bufferEntityPatchCount = 0
    for i=1,count do
        local ct = ECS.TypeManager.GetTypeInfoByIndex(types[i].TypeIndex)
        local entityOffsets = ct.EntityOffsets
        if (entityOffsets ~= nil) then
            if (ct.BufferCapacity >= 0) then
                bufferEntityPatchCount = bufferEntityPatchCount + entityOffsets.Length
            else
                scalarEntityPatchCount = scalarEntityPatchCount + entityOffsets.Length
            end
        end
    end

    local chunkDataSize = ECS.Chunk.GetChunkBufferSize(type.TypesCount, type.NumSharedComponents)

    type.Offsets = {}
    type.SizeOfs = {}
    type.TypeMemoryOrder = {}
    type.ScalarEntityPatches = {}
    type.ScalarEntityPatchCount = scalarEntityPatchCount
    type.BufferEntityPatches = {}
    type.BufferEntityPatchCount = bufferEntityPatchCount

    local bytesPerInstance = 0
    for i=1,count do
        local cType = ECS.TypeManager.GetTypeInfoByIndex(types[i].TypeIndex)
        local sizeOf = cType.SizeInChunk 
        type.SizeOfs[i] = sizeOf

        bytesPerInstance = bytesPerInstance + sizeOf
    end

    type.ChunkCapacity = chunkDataSize / bytesPerInstance

    local memoryOrderings = {}
    for i=1,count do
        memoryOrderings[i] = ECS.TypeManager.GetTypeInfoByIndex(types[i].TypeIndex).MemoryOrdering
    end
    for i=1,count do
        local index = i
        while (index > 1 and memoryOrderings[i] < memoryOrderings[type.TypeMemoryOrder[index - 1]]) do
            type.TypeMemoryOrder[index] = type.TypeMemoryOrder[index - 1]
            index = index - 1
        end
        type.TypeMemoryOrder[index] = i
    end

    local usedBytes = 0
    for i=1,count do
        local index = type.TypeMemoryOrder[i]
        local sizeOf = type.SizeOfs[index]
        type.Offsets[index] = usedBytes
        usedBytes = usedBytes + sizeOf * type.ChunkCapacity
    end

    type.NumManagedArrays = 0
    type.ManagedArrayOffset = null

    for i=1,count do
        if (ECS.TypeManager.GetTypeInfoByIndex(types[i].TypeIndex).Category == ECS.TypeManager.TypeCategory.Class) then
            type.NumManagedArrays = type.NumManagedArrays + 1
        end
    end

    if (type.NumManagedArrays > 0) then
        type.ManagedArrayOffset = self.m_ArchetypeChunkAllocator.Allocate(sizeof(int) * count, 4)
        local mi = 0
        for i=1,count do
            local index = type.TypeMemoryOrder[i]
            local cType = ECS.TypeManager.GetTypeInfoByIndex(types[index].TypeIndex)
            if (cType.Category == ECS.TypeManager.TypeCategory.Class) then
                type.ManagedArrayOffset[index] = mi
                mi = mi + 1
            else
                type.ManagedArrayOffset[index] = -1
            end
        end
    end

    if (type.NumSharedComponents > 0) then
        type.SharedComponentOffset = self.m_ArchetypeChunkAllocator.Allocate(sizeof(int) * count, 4)
        local mi = 0
        for i=1,count do
            local index = type.TypeMemoryOrder[i]
            local cType = ECS.TypeManager.GetTypeInfoByIndex(types[index].TypeIndex)
            if (cType.Category == ECS.TypeManager.TypeCategory.ISharedComponentData) then
                type.SharedComponentOffset[index] = mi
                mi = mi + 1
            else
                type.SharedComponentOffset[index] = -1
            end
        end
    end

    -- local scalarPatchInfo = type.ScalarEntityPatches
    -- local bufferPatchInfo = type.BufferEntityPatches
    -- for i=1,count do
    --     local ct = ECS.TypeManager.GetTypeInfoByIndex(types[i].TypeIndex)
    --     local offsets = ct.EntityOffsets
    --     if (ct.BufferCapacity >= 0) then
    --         bufferPatchInfo = EntityRemapUtility.AppendBufferEntityPatches(bufferPatchInfo, offsets, type.Offsets[i], type.SizeOfs[i], ct.ElementSize);
    --     else
    --         scalarPatchInfo = EntityRemapUtility.AppendEntityPatches(scalarPatchInfo, offsets, type.Offsets[i], type.SizeOfs[i]);
    --     end
    -- end
    -- type.ScalarEntityPatchCount = scalarEntityPatchCount
    -- type.BufferEntityPatchCount = bufferEntityPatchCount

    type.PrevArchetype = self.m_LastArchetype
    m_LastArchetype = type

    type.ChunkList = ECS.UnsafeLinkedListNode.New()
    type.ChunkListWithEmptySlots = ECS.UnsafeLinkedListNode.New()
    ECS.UnsafeLinkedListNode.InitializeList(type.ChunkList)
    ECS.UnsafeLinkedListNode.InitializeList(type.ChunkListWithEmptySlots)
    -- type.FreeChunksBySharedComponents.Init(8)

    -- m_TypeLookup.Add(GetHash(types, count), type)
    local type_str = GetTypesStr(types)
    self.m_TypeLookup[type_str] = type

    -- type.SystemStateCleanupComplete = ArchetypeSystemStateCleanupComplete(type)
    -- type.SystemStateCleanupNeeded = ArchetypeSystemStateCleanupNeeded(type)

    groupManager.AddArchetypeIfMatching(type)
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
    chunk.ChunkListWithEmptySlotsNode = ECS.UnsafeLinkedListNode.New()

    local numSharedComponents = archetype.NumSharedComponents
    local numTypes = archetype.TypesCount
    local sharedComponentOffset = ECS.Chunk.GetSharedComponentOffset(numSharedComponents)
    local changeVersionOffset = ECS.Chunk.GetChangedComponentOffset(numTypes, numSharedComponents)

    chunk.SharedComponentValueArray = (chunk + sharedComponentOffset)
    chunk.ChangeVersion = (chunk + changeVersionOffset)

    archetype.ChunkList.Add(chunk.ChunkListNode)
    archetype.ChunkCount = archetype.ChunkCount+1
    -- Assert.IsTrue(!archetype.ChunkList.IsEmpty)
    -- Assert.IsTrue(chunk == (Chunk*) archetype.ChunkList.Back)
    if (numSharedComponents == 0) then
        archetype.ChunkListWithEmptySlots.Add(chunk.ChunkListWithEmptySlotsNode)
        -- Assert.IsTrue(chunk == GetChunkFromEmptySlotNode(archetype.ChunkListWithEmptySlots.Back))
        -- Assert.IsTrue(!archetype.ChunkListWithEmptySlots.IsEmpty)
    else
        local sharedComponentValueArray = chunk.SharedComponentValueArray
        UnsafeUtility.MemCpy(sharedComponentValueArray, sharedComponentDataIndices, archetype.NumSharedComponents*sizeof(int))

        for i=1,archetype.NumSharedComponents do
            local sharedComponentIndex = sharedComponentValueArray[i]
            m_SharedComponentManager.AddReference(sharedComponentIndex)
        end

        archetype.FreeChunksBySharedComponents.Add(chunk)
        -- Assert.IsTrue(archetype.FreeChunksBySharedComponents.GetChunkWithEmptySlots(sharedComponentDataIndices, archetype.NumSharedComponents) ~= nil)
    end

    if archetype.NumManagedArrays > 0 then
        chunk.ManagedArrayIndex = AllocateManagedArrayStorage(archetype.NumManagedArrays * chunk.Capacity)
    else
        chunk.ManagedArrayIndex = -1
    end

    for i=1,archetype.TypesCount do
        chunk.ChangeVersion[i] = 0
    end
end

function ArchetypeManager:AllocateIntoChunk( chunk, count )
    count = count or 1
	local allocatedCount = math.min(chunk.Capacity - chunk.Count, count)
    local newChunkIndex = chunk.Count
    SetChunkCount(chunk, chunk.Count + allocatedCount)
    chunk.Archetype.EntityCount = chunk.Archetype.EntityCount + allocatedCount
    return allocatedCount, newChunkIndex
end

function ArchetypeManager:GetChunkWithEmptySlots( archetype, sharedComponentDataIndices )
	if archetype.NumSharedComponents == 0 then
        if not archetype.ChunkListWithEmptySlots.IsEmpty then
            local chunk = self:GetChunkFromEmptySlotNode(archetype.ChunkListWithEmptySlots.Begin)
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
            local chunk = self:GetChunkFromEmptySlotNode(archetype.ChunkListWithEmptySlots.Begin)
            -- Assert.AreNotEqual(chunk.Count, chunk.Capacity)
            return chunk
        end
    end

    local newChunk
    -- Try empty chunk pool
    if not self.m_EmptyChunkPool or self.m_EmptyChunkPool.IsEmpty then
        -- Allocate new chunk
        newChunk = UnsafeUtility.Malloc(Chunk.kChunkSize, 64, Allocator.Persistent)
    else
        newChunk = self.m_EmptyChunkPool.Begin
        newChunk.ChunkListNode.Remove()
    end

    self:ConstructChunk(archetype, newChunk, sharedComponentDataIndices)

    if archetype.NumSharedComponents > 0 then
        self.lastChunkWithSharedComponentsAllocatedInto = newChunk
    end
    return newChunk
end

return ArchetypeManager