local InjectComponentGroupData = BaseClass()
ECS.InjectComponentGroupData = InjectComponentGroupData

function InjectComponentGroupData:Constructor( system, componentRequirements )
	self.system = system
	self.m_ComponentDataInjections = {}
	self.m_EntityGroup = system:GetComponentGroup(componentRequirements)
end

function InjectComponentGroupData:CreateInjection( injectedGroupType, groupField, system )
	self:CollectInjectedGroup(system, groupField, injectedGroupType)
	return InjectComponentGroupData.New(system, groupField, componentDataInjections)
end

function InjectComponentGroupData:UpdateInjection(  )
    local iterator, length = self.m_EntityGroup:GetComponentChunkIterator()

	for i,v in ipairs(self.m_ComponentDataInjections) do
		local data = ECS.ComponentDataArray.New()
        self.m_EntityGroup.GetComponentDataArray(iterator, self.m_ComponentDataInjections[i].IndexInComponentGroup,
            length, data)
        --system里inject的字段名
        self.system[self.m_ComponentDataInjections[i].InjectFieldName] = data
        -- UnsafeUtility.CopyStructureToPtr(data, groupStructPtr + self.m_ComponentDataInjections[i].FieldOffset)
	end
end

function InjectComponentGroupData:CollectInjectedGroup( system, groupField, injectedGroupType )
	for i,v in ipairs(groupField) do
		
	end
end