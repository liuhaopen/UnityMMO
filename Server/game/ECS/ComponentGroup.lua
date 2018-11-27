local ComponentGroup = BaseClass()
ECS.ComponentGroup = ComponentGroup

function ComponentGroup:Constructor(  )
	self.m_GroupData = nil
	self.m_EntityDataManager = nil
	self.m_Filter = nil
end

function ComponentGroup:GetComponentDataArray( com_type )
	
end

function ComponentGroup:GetSharedComponentDataArray( shared_com_type )
	
end

function ComponentGroup:GetEntityArray(  )
	
end

function ComponentGroup:ResetFilter(  )
	
end

function ComponentGroup:SetFilter(  )
	
end

return ComponentGroup