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