local ComponentGroup = BaseClass()
ECS.ComponentGroup = ComponentGroup

function ComponentGroup:Constructor( groupData, safetyManager, typeManager, entityDataManager )
	self.m_GroupData = groupData
	self.m_EntityDataManager = entityDataManager
	self.m_Filter = nil
    self.ArchetypeManager = typeManager
    self.EntityDataManager = entityDataManager
end

function ComponentGroup:GetComponentDataArray( com_type )
    local typeIndex = TypeManager.GetTypeIndex(com_type)
    local iterator, length = self:GetComponentChunkIterator()
    local indexInComponentGroup = self:GetIndexInComponentGroup(typeIndex)
    local res = {}
    self:GetComponentDataArray(iterator, indexInComponentGroup, length, res)
    return res
end

function ComponentGroup:GetSharedComponentDataArray( shared_com_type )
    local iterator, length = self:GetComponentChunkIterator()
    local indexInComponentGroup = self:GetIndexInComponentGroup(TypeManager.GetTypeIndex(shared_com_type))
    local res = {}
    self:GetSharedComponentDataArray(iterator, indexInComponentGroup, length, res)
    return res
end

function ComponentGroup:GetEntityArray(  )
    local iterator, length = self:GetComponentChunkIterator()
    local res
    self:GetEntityArray(iterator, length, res)
    return res
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

return ComponentGroup