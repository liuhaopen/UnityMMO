ECS.Entity = {
	Index=0, Version=0
}
EntityManager = BaseClass()
ECS.EntityManager = EntityManager

function EntityManager:Constructor(  )
	self.entities_free_id = 0
end

function EntityManager:OnCreateManager( capacity )
	
end

local CreateEntities = function ( archetype, num )
	local entity = ECS.Entity.New()
	entity.Index = self.entities_free_id
	self.entities_free_id = self.entities_free_id + 1
	return entity
end

function EntityManager:CreateEntityByArcheType( archetype, num )
	return CreateEntities(archetype, num or 1)
end

function EntityManager:CreateEntityByComponents( com_types )
	return CreateEntities(self:CreateArchetype(com_types), num or 1)
end

local PopulatedCachedTypeInArchetypeArray = function ( requiredComponents )
	
end

function EntityManager:CreateArchetype( com_types )
	local cachedComponentCount = PopulatedCachedTypeInArchetypeArray(com_types);

    local entityArchetype = {}
    entityArchetype.Archetype =
        ArchetypeManager.GetExistingArchetype(self.m_CachedComponentTypeInArchetypeArray, cachedComponentCount);
    if entityArchetype.Archetype ~= nil then
        return entityArchetype
    end

    self:BeforeStructuralChange()

    entityArchetype.Archetype = ArchetypeManager.GetOrCreateArchetype(self.m_CachedComponentTypeInArchetypeArray,
        cachedComponentCount, self.m_GroupManager)
    return entityArchetype
end

function EntityManager:Exists( entity )
	
end

function EntityManager:HasComponent( entity, com_type )
	
end

function EntityManager:Instantiate( srcEntity )
	
end

function EntityManager:AddComponent( entity, com_type )
	
end

function EntityManager:RemoveComponent( entity, com_type )
	
end

function EntityManager:AddComponentData( entity, com_data )
	
end

function EntityManager:SetComponentData( entity, com_data )
	
end

function EntityManager:GetComponentData( entity )
	
end

function EntityManager:GetAllEntities(  )
	
end

function EntityManager:GetComponentTypes( entity )
	
end

function EntityManager:GetComponentCount( entity )
	
end

function EntityManager:DestroyEntity( entity )
	
end

local EntityArchetypeQuery = {
	Any = {}, None = {}, All = {}, 
}

local EntityArchetype = {
	
}

return EntityManager