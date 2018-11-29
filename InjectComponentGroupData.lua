local InjectComponentGroupData = BaseClass()
ECS.InjectComponentGroupData = InjectComponentGroupData

function InjectComponentGroupData:Constructor(  )
	self.m_ComponentDataInjections = {}
end

function InjectComponentGroupData:CreateInjection( injectedGroupType, groupField, system )
	self:CollectInjectedGroup(system, groupField, injectedGroupType)
	return InjectComponentGroupData.New(system, groupField, componentDataInjections)
end

function InjectComponentGroupData:UpdateInjection(  )
	for i,v in ipairs(self.m_ComponentDataInjections) do
		
	end
end

function InjectComponentGroupData:CollectInjectedGroup( system, groupField, injectedGroupType )
	for i,v in ipairs(groupField) do
		
	end
end