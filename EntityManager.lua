ECS = ECS or {}
ECS.Entity = {
	Index=0, Version=0
}
EntityManager = BaseClass()
ECS.EntityManager = EntityManager

function EntityManager:Constructor(  )
	self.entities_free_id = 0
end

function EntityManager:CreateEntity( com_types )
	local entity = ECS.Entity.New()
	entity.Index = self.entities_free_id
	self.entities_free_id = self.entities_free_id + 1
	return entity
end

function EntityManager:DestroyEntity( entity )
	
end

function EntityManager:CreateArchetype( com_types )
	
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

return EntityManager