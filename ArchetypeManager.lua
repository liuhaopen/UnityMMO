ArchetypeManager = BaseClass()
ECS.ArchetypeManager = ArchetypeManager


function ArchetypeManager:Constructor(  )
	
end

local GetHash = function ( componentTypeInArchetype, count )
	
end

local AssertArchetypeComponents = function ( types )
	
end

local GetOrCreateArchetypeInternal = function ( types, groupManager )
	local type = self:GetExistingArchetype(types)
    if type ~= null then
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

    self.m_TypeLookup.Add(GetHash(types, count), type);
    groupManager.OnArchetypeAdded(type)
 	return type
end           


function ArchetypeManager:GetExistingArchetype( componentTypeInArchetype, count )
	
end

function ArchetypeManager:GetOrCreateArchetype( componentTypeInArchetype, count, groupManager )
	
end

function ArchetypeManager:AddExistingChunk( chunk )
	
end

function ArchetypeManager:ConstructChunk(  )
	
end

function ArchetypeManager:AllocateIntoChunk(  )
	
end