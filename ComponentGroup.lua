local ComponentGroup = BaseClass()
ECS.ComponentGroup = ComponentGroup

function ComponentGroup:Constructor(  )
	self.m_GroupData = nil
	self.m_EntityDataManager = nil
	self.m_Filter = nil
end

function ComponentGroup:GetComponentDataArray( com_type )
    local typeIndex = TypeManager.GetTypeIndex(com_type)
	local length
    local iterator
    self:GetComponentChunkIterator(length, iterator)
    local indexInComponentGroup = self:GetIndexInComponentGroup(typeIndex)

    local res = {}
    self:GetComponentDataArray(iterator, indexInComponentGroup, length, res)
    return res;
end

function ComponentGroup:GetSharedComponentDataArray( shared_com_type )
	local length
    local iterator
    self:GetComponentChunkIterator(length, iterator)
    local indexInComponentGroup = self:GetIndexInComponentGroup(TypeManager.GetTypeIndex(shared_com_type))
    local res = {}
    self:GetSharedComponentDataArray(iterator, indexInComponentGroup, length, res)
    return res;
end

function ComponentGroup:GetEntityArray(  )
	
end

function ComponentGroup:ResetFilter(  )
	
end

function ComponentGroup:SetFilter(  )
	
end

return ComponentGroup