ArchetypeManager = BaseClass()
ECS.ArchetypeManager = ArchetypeManager


function ArchetypeManager:Constructor(  )
	
end

local GetHash = function ( componentTypeInArchetype )
	
end

local AssertArchetypeComponents = function ( types )
	
end

local GetOrCreateArchetypeInternal = function ( types, groupManager )
	local type = self:GetExistingArchetype(types)
    if type ~= nil then
        return type
    end

    AssertArchetypeComponents(types)

    type = {
    	TypesCount = #types, 
    	Types = {},
    	EntityCount = 0,
    	ChunkCount = 0,
    	NumSharedComponents = 0,
    	SharedComponentOffset = 0,
    	PrevArchetype = self.m_LastArchetype, 
	}
    self.m_LastArchetype = type

    self.m_TypeLookup.Add(GetHash(types), type)
    groupManager.OnArchetypeAdded(type)
 	return type
end           


function ArchetypeManager:GetExistingArchetype( componentTypeInArchetype )
	
end

function ArchetypeManager:GetOrCreateArchetype( componentTypeInArchetype, groupManager )
    local srcArchetype = GetOrCreateArchetypeInternal(types, groupManager)
    local removedTypes = 0
    local prefabTypeIndex = TypeManager.GetTypeIndex("Prefab")
    for t=1,srcArchetype.TypesCount do
        local type = srcArchetype.Types[t]
        local skip = type.IsSystemStateComponent or type.IsSystemStateSharedComponent or (type.TypeIndex == prefabTypeIndex)
        if (skip) then
            removedTypes = removedTypes + 1
        else
            types[t - removedTypes] = srcArchetype.Types[t]
        end
    end

    srcArchetype.InstantiableArchetype = srcArchetype
    if removedTypes > 0 then
        local instantiableArchetype = GetOrCreateArchetypeInternal(types, count-removedTypes, groupManager)
        srcArchetype.InstantiableArchetype = instantiableArchetype
        instantiableArchetype.InstantiableArchetype = instantiableArchetype
    end
    return srcArchetype
end

function ArchetypeManager:AddExistingChunk( chunk )
	
end

function ArchetypeManager:ConstructChunk(  )
	
end

function ArchetypeManager:AllocateIntoChunk(  )
	
end

function ArchetypeManager:GetChunkWithEmptySlots( archetype, sharedComponentDataIndices )
	 if archetype.NumSharedComponents == 0 then
        if not archetype.ChunkListWithEmptySlots.IsEmpty then
            local chunk = self:GetChunkFromEmptySlotNode(archetype.ChunkListWithEmptySlots.Begin)
            -- Assert.AreNotEqual(chunk->Count, chunk->Capacity)
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
    if m_EmptyChunkPool.IsEmpty then
        -- Allocate new chunk
        newChunk = UnsafeUtility.Malloc(Chunk.kChunkSize, 64, Allocator.Persistent)
    else
        newChunk = self.m_EmptyChunkPool.Begin
        newChunk.ChunkListNode.Remove()
    end

    ConstructChunk(archetype, newChunk, sharedComponentDataIndices)

    if archetype.NumSharedComponents > 0 then
        self.lastChunkWithSharedComponentsAllocatedInto = newChunk
    end
    return newChunk
end