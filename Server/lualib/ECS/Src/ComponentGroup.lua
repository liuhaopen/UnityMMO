local ComponentGroup = ECS.BaseClass()
ECS.ComponentGroup = ComponentGroup

function ComponentGroup:Constructor( groupData, safetyManager, typeManager, entityDataManager )
	self.m_GroupData = groupData
	self.m_EntityDataManager = entityDataManager
	self.m_Filter = {Type=ECS.FilterType.None, RequiredChangeVersion=0}
    self.ArchetypeManager = typeManager
    self.EntityDataManager = entityDataManager
end

function ComponentGroup:ToComponentDataArray( com_type )
    local typeIndex = ECS.TypeManager.GetTypeIndexByName(com_type)
    local iterator, length = self:GetComponentChunkIterator()
    local indexInComponentGroup = self:GetIndexInComponentGroup(typeIndex)
    local res = self:ToComponentDataArrayByIterator(iterator, indexInComponentGroup, length, com_type)
    return res
end

function ComponentGroup:GetIndexInComponentGroup( componentType )
    local componentIndex = 1
    while componentIndex <= self.m_GroupData.RequiredComponentsCount and self.m_GroupData.RequiredComponents[componentIndex].TypeIndex ~= componentType do
        componentIndex = componentIndex + 1
    end
    return componentIndex
end

function ComponentGroup:ToComponentDataArrayByIterator( iterator, indexInComponentGroup, length, com_type )
    iterator:SetIndexInComponentGroup(indexInComponentGroup)
    local data = ECS.ComponentDataArray.Create(iterator, length, com_type)
    return data
end

function ComponentGroup:GetSharedComponentDataArray( shared_com_type )
    local iterator, length = self:GetComponentChunkIterator()
    local indexInComponentGroup = self:GetIndexInComponentGroup(TypeManager.GetTypeIndex(shared_com_type))
    local res = {}
    self:GetSharedComponentDataArray(iterator, indexInComponentGroup, length, res)
    return res
end

function ComponentGroup:ToEntityArray(  )
    local iterator, length = self:GetComponentChunkIterator()
    iterator:SetIndexInComponentGroup(1)
    local data = ECS.EntityArray.Create(iterator, length)
    return data
end

function ComponentGroup:GetComponentChunkIterator(  )
    local length = ECS.ComponentChunkIterator.CalculateLength(self.m_GroupData.FirstMatchingArchetype, self.m_Filter)
    local iterator = ECS.ComponentChunkIterator.New(self.m_GroupData.FirstMatchingArchetype, self.m_EntityDataManager.GlobalSystemVersion, self.m_Filter)
    return iterator, length
end

function ComponentGroup:ResetFilter(  )
	if self.m_Filter.Type == ECS.FilterType.SharedComponent then
        local filteredCount = self.m_Filter.Shared.Count
        local sm = self.ArchetypeManager.GetSharedComponentDataManager()
        local sharedComponentIndexPtr = self.m_Filter.Shared.SharedComponentIndex
        for var=1,filteredCount do
            sm:RemoveReference(sharedComponentIndexPtr[i])
        end
    end
    self.m_Filter.Type = FilterType.None
end

function ComponentGroup:SetFilter(  )
end

function ComponentGroup:CompareComponents( componentTypes )
    return ECS.EntityGroupManager.CompareComponents(componentTypes, self.m_GroupData)
end

return ComponentGroup