local EntityManager = ECS.BaseClass(ECS.ScriptBehaviourManager)
ECS.EntityManager = EntityManager
ECS.EntityManager.Name = "ECS.EntityManager"
ECS.TypeManager.RegisterScriptMgr(ECS.EntityManager.Name, EntityManager)

local table_insert = table.insert
function EntityManager:Constructor(  )
	self.entities_free_id = 0
end

function EntityManager:OnCreate( capacity )
	ECS.TypeManager.Initialize()
	self.Entities = ECS.EntityDataManager.New(capacity)
	self.m_SharedComponentManager = ECS.SharedComponentDataManager.New()
	self.ArchetypeManager = ECS.ArchetypeManager.New(self.m_SharedComponentManager)
	self.m_GroupManager = ECS.EntityGroupManager.New(self.ComponentJobSafetyManager)
	self.m_CachedComponentTypeArray = {}
	self.m_CachedComponentTypeInArchetypeArray = {}
end

function EntityManager:GetArchetypeManager(  )
    return self.ArchetypeManager
end

function EntityManager:GetEntityDataManager(  )
    return self.Entities
end

function EntityManager:GetGroupManager(  )
    return self.m_GroupManager
end

function EntityManager:CreateEntityByArcheType( archetype )
    local entities = self.Entities:CreateEntities(self.ArchetypeManager, archetype.Archetype, 1)
	return entities and entities[1]
end

function EntityManager:CreateEntitiesByArcheType( archetype, num )
    return self.Entities:CreateEntities(self.ArchetypeManager, archetype.Archetype, num or 1)
end

function EntityManager:CreateEntityByComponents( com_types, num )
	return self.Entities:CreateEntities(self.ArchetypeManager, self:CreateArchetype(com_types), num or 1)
end

function EntityManager:PopulatedCachedTypeInArchetypeArray( requiredComponents, count )
    self.m_CachedComponentTypeInArchetypeArray = {}
    self.m_CachedComponentTypeInArchetypeArray[1] = ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(ECS.Entity.Name))
    for i=1,count do
        ECS.SortingUtilities.InsertSorted(self.m_CachedComponentTypeInArchetypeArray, i + 1, ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(requiredComponents[i])))
    end
    return count + 1
end

--e.g. CreateArchetype({"ECS.Position", "OtherCompTypeName"})
function EntityManager:CreateArchetype( types )
    local cachedComponentCount = self:PopulatedCachedTypeInArchetypeArray(types, #types)

    local entityArchetype = {}
    entityArchetype.Archetype =
        self.ArchetypeManager:GetExistingArchetype(self.m_CachedComponentTypeInArchetypeArray, cachedComponentCount)
    if entityArchetype.Archetype ~= nil then
        return entityArchetype
    end
    -- self:BeforeStructuralChange()
    entityArchetype.Archetype = self.ArchetypeManager:GetOrCreateArchetype(self.m_CachedComponentTypeInArchetypeArray, cachedComponentCount, self.m_GroupManager)
    return entityArchetype
end

function EntityManager:Exists( entity )
    return self.Entities:Exists(entity)
end

function EntityManager:HasComponent( entity, comp_type_name )
    return self.Entities:HasComponent(entity, comp_type_name)
end

function EntityManager:Instantiate( srcEntity )
	-- self:BeforeStructuralChange()
    if not self.Entities:Exists(srcEntity) then
        assert(false, "srcEntity is not a valid entity")
    end

    self.Entities:InstantiateEntities(self.ArchetypeManager, self.m_SharedComponentManager, self.m_GroupManager, srcEntity, outputEntities,
        count, self.m_CachedComponentTypeInArchetypeArray)
end

function EntityManager:AddComponent( entity, comp_type_name )
    self.Entities:AddComponent(entity, comp_type_name, self.ArchetypeManager, self.m_SharedComponentManager, self.m_GroupManager,
        self.m_CachedComponentTypeInArchetypeArray)
end

function EntityManager:RemoveComponent( entity, comp_type_name )
    self.Entities:AssertEntityHasComponent(entity, comp_type_name)
    self.Entities:RemoveComponent(entity, comp_type_name, self.ArchetypeManager, self.m_SharedComponentManager, self.m_GroupManager)

    local archetype = self.Entities:GetArchetype(entity)
    if (archetype.SystemStateCleanupComplete) then
        self.Entities:TryRemoveEntityId(entity, 1, self.ArchetypeManager, self.m_SharedComponentManager, self.m_GroupManager, self.m_CachedComponentTypeInArchetypeArray)
    end
end

function EntityManager:AddComponentData( entity, componentTypeName, componentData )
	self:AddComponent(entity, componentTypeName)
    self:SetComponentData(entity, componentTypeName, componentData)
end

function EntityManager:SetComponentData( entity, componentTypeName, componentData )
    self.Entities:AssertEntityHasComponent(entity, componentTypeName)--做检查需要消耗多一倍时间
    self.Entities:SetComponentDataWithTypeNameRW(entity, componentTypeName, componentData)
end

function EntityManager:GetComponentData( entity, componentTypeName )
    self.Entities:AssertEntityHasComponent(entity, componentTypeName)
    return self.Entities:GetComponentDataWithTypeNameRO(entity, componentTypeName)
end

function EntityManager:GetAllEntities(  )
end

function EntityManager:GetComponentTypes( entity )
    self.Entities:Exists(entity)
    local archetype = self.Entities:GetArchetype(entity)
    local components = {}
    for i=2, archetype.TypesCount do
        components[i - 1] = archetype.Types[i].ToComponentType()
    end
    return components
end

function EntityManager:GetComponentCount( entity )
    self.Entities:Exists(entity)
    local archetype = self.Entities:GetArchetype(entity)
    return archetype.TypesCount - 1
end

function EntityManager:CreateComponentGroup( requiredComponents )
    return self.m_GroupManager:CreateEntityGroupByNames(self.ArchetypeManager, self.Entities, requiredComponents)
end

function EntityManager:DestroyEntity( entity )
    self.Entities:Exists(entity)
    -- self.Entities:AssertChunksUnlocked(entities, count)
    self.Entities:TryRemoveEntityId(entity, self.ArchetypeManager)
end

function EntityManager:GetArchetypeChunkComponentType( comp_type_name, isReadOnly )
    return ArchetypeChunkComponentType.New(comp_type_name, isReadOnly, self.GlobalSystemVersion)
end

return EntityManager