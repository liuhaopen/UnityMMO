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
    self:IncrementComponentTypeOrderVersion()
end

function EntityDataManager:IncrementComponentTypeOrderVersion( archetype )
	for t=1,archetype.TypesCount do
		local typeIndex = archetype.Types[t].TypeIndex
        self.m_ComponentTypeOrderVersion[typeIndex] = self.m_ComponentTypeOrderVersion[typeIndex] + 1
	end
end

function EntityDataManager:AddComponent( entity, com_type, archetypeManager, sharedComponentDataManager
	, groupManager, componentTypeInArchetypeArray )
    local componentType = ECS.ComponentTypeInArchetype.New(type)
	local archetype = self:GetArchetype(entity)

    local t = 1
    componentTypeInArchetypeArray = {}--Cat_Todo : obj pool optimize
    while (t <= archetype.TypesCount and archetype.Types[t] < componentType) do
        componentTypeInArchetypeArray[t] = archetype.Types[t]
        t = t + 1
    end
    --按顺序把新的类型插入临时列表里
    componentTypeInArchetypeArray[t] = componentType
    while (t <= archetype.TypesCount) do
        componentTypeInArchetypeArray[t + 1] = archetype.Types[t]
        t = t + 1
    end

    local newType = archetypeManager:GetOrCreateArchetype(componentTypeInArchetypeArray,
        archetype.TypesCount + 1, groupManager)

    local sharedComponentDataIndices = nil
    if newType.NumSharedComponents > 0 then
        local oldSharedComponentDataIndices = self:GetComponentChunk(entity).SharedComponentValueArray
        if type.IsSharedComponent then
            -- local stackAlloced = stackalloc int[newType->NumSharedComponents]
            sharedComponentDataIndices = stackAlloced

            if archetype.SharedComponentOffset == nil then
                sharedComponentDataIndices[1] = 0
            else
                t = 1
                local sharedIndex = 1
                while t <= archetype.TypesCount and archetype.Types[t] < componentType do
                    if archetype.SharedComponentOffset[t] ~= -1 then
                        sharedComponentDataIndices[sharedIndex] = oldSharedComponentDataIndices[sharedIndex]
                        sharedIndex = sharedIndex + 1
                    end
                    t = t + 1
                end

                sharedComponentDataIndices[sharedIndex] = 0
                while (t <= archetype.TypesCount) do
                    if (archetype.SharedComponentOffset[t] ~= -1) then
                        sharedComponentDataIndices[sharedIndex + 1] =
                            oldSharedComponentDataIndices[sharedIndex]
                        sharedIndex = sharedIndex + 1
                    end
                    t = t + 1
                end
            end
        else
            -- reuse old sharedComponentDataIndices
            sharedComponentDataIndices = oldSharedComponentDataIndices
        end
    end

    self:SetArchetype(archetypeManager, entity, newType, sharedComponentDataIndices)
    self:IncrementComponentOrderVersion(newType, self:GetComponentChunk(entity), sharedComponentDataManager)
end