EntityDataManager = BaseClass()
ECS.EntityDataManager = EntityDataManager

local EntityData = {
	Version=0, 
	Archetype = nil,
	Chunk = nil,
	IndexInChunk = 0
}
function EntityDataManager:Constructor( capacity )
	self.m_Entities = {}
	self.m_EntitiesCapacity = capacity
	self.m_EntitiesFreeIndex = 0
    self.GlobalSystemVersion = ECS.ChangeVersionUtility.InitialGlobalSystemVersion

	self:InitializeAdditionalCapacity(1)
	-- const int componentTypeOrderVersionSize = sizeof(int) * TypeManager.MaximumTypesCount;
 --    m_ComponentTypeOrderVersion = (int*) UnsafeUtility.Malloc(componentTypeOrderVersionSize,
 --        UnsafeUtility.AlignOf<int>(), Allocator.Persistent);
 --    UnsafeUtility.MemClear(m_ComponentTypeOrderVersion, componentTypeOrderVersionSize);
end

function EntityDataManager:InitializeAdditionalCapacity( start )
	for i=start,self.m_EntitiesCapacity do
		self.m_Entities[i] = {}
		self.m_Entities[i].IndexInChunk = i
        self.m_Entities[i].Version = 1
        self.m_Entities[i].Chunk = nil
        self.m_Entities[i].Archetype = nil
	end
    --Last entity indexInChunk identifies that we ran out of space...
    self.m_Entities[self.m_EntitiesCapacity].IndexInChunk = -1;
end

function EntityDataManager:HasComponent( entity, com_type_name )
	if not self:Exit(entity) then 
		return false
	end
	local archetype = self.m_Entities[entity.Index].Archetype
    -- return ChunkDataUtility.GetIndexInTypeArray(archetype, type.TypeIndex) ~= -1;
end

function EntityDataManager:GetComponentDataWithTypeRO( entity, typeIndex )
	-- local entityData = self.m_Entities[entity.Index]
    -- return ChunkDataUtility.GetComponentDataWithTypeRO(entityData.Chunk, entityData.IndexInChunk, typeIndex)
end

function EntityDataManager:CreateEntities( archetypeManager, archetype, entities, count )
	self:IncrementComponentTypeOrderVersion()
	if count == 1 then
		local entity = {Index=self.entities_free_id, }
		self.entities_free_id = self.entities_free_id + 1
		return entity
	else
		local entities = {}
		for i=1,count do
			local entity = {Index=self.entities_free_id, }
			table_insert(entities, entity)
		end
		self.entities_free_id = self.entities_free_id + count
		return entities
	end
end

function EntityDataManager:IncrementComponentTypeOrderVersion( archetype )
	for t=1,archetype.TypesCount do
		local typeIndex = archetype.Types[t].TypeIndex
        self.m_ComponentTypeOrderVersion[typeIndex] = self.m_ComponentTypeOrderVersion[typeIndex] + 1
	end
end