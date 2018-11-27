ECS.Entity = {
	Index=0, Version=0
}
EntityManager = BaseClass()
ECS.EntityManager = EntityManager
local table_insert = table.insert
function EntityManager:Constructor(  )
	self.entities_free_id = 0
end

function EntityManager:OnCreateManager( capacity )
	ECS.TypeManager.Initialize()
	self.Entities = ECS.EntityDataManager.New(capacity)
	self.m_SharedComponentManager = ECS.SharedComponentDataManager.New()
	self.ArchetypeManager = ECS.ArchetypeManager.New(self.m_SharedComponentManager)
	-- self.ComponentJobSafetyManager = ECS.ComponentJobSafetyManager.New()
	self.m_GroupManager = ECS.EntityGroupManager.New(self.ComponentJobSafetyManager)
	self.m_CachedComponentTypeArray = {}
	self.m_CachedComponentTypeInArchetypeArray = {}
end

local CreateEntities = function ( archetype, num )
	if num == 1 then
		local entity = {Index=self.entities_free_id, }
		self.entities_free_id = self.entities_free_id + 1
		return entity
	else
		local entities = {}
		for i=1,num do
			local entity = {Index=self.entities_free_id, }
			table_insert(entities, entity)
		end
		self.entities_free_id = self.entities_free_id + num
		return entities
	end
end

function EntityManager:CreateEntityByArcheType( archetype, num )
	return CreateEntities(archetype, num or 1)
end

function EntityManager:CreateEntityByComponents( com_types )
	return CreateEntities(self:CreateArchetype(com_types), num or 1)
end

function EntityManager:CreateArchetype( com_types )
	-- local cachedComponentCount = PopulatedCachedTypeInArchetypeArray(com_types);

    local entityArchetype = {}
    entityArchetype.Archetype =
        ArchetypeManager.GetExistingArchetype(self.m_CachedComponentTypeInArchetypeArray)
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

function EntityManager:CreateComponentGroup( requiredComponents )
    return self.m_GroupManager:CreateEntityGroup(self.ArchetypeManager, self.Entities, requiredComponents)
end

function EntityManager:DestroyEntity( entity )
	
end

local EntityArchetypeQuery = {
	Any = {}, None = {}, All = {}, 
}

local EntityArchetype = {
	
}

return EntityManager