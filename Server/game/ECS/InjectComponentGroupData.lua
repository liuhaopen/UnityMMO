local InjectComponentGroupData = BaseClass()
ECS.InjectComponentGroupData = InjectComponentGroupData

function InjectComponentGroupData:Constructor( system, componentRequirements )
	self.system = system
	self.m_ComponentDataInjections = {}
	self.m_EntityGroup = system:GetComponentGroup(componentRequirements)
end

function InjectComponentGroupData.CreateInjection( injectedGroupType, groupField, system )
	InjectComponentGroupData.CollectInjectedGroup(system, groupField, injectedGroupType)
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

function InjectComponentGroupData.CollectInjectedGroup( system, groupField, injectedGroupType )
	for i,v in ipairs(groupField) do
		local field_info = Split(v, ":")
		print('Cat:InjectComponentGroupData.lua[31] field_info', field_info)
		if not field_info then return end

		local field_type = field_info and field_info[1]
		if field_type == "ComponentDataArray" then
			local comp_type_name = field_info[2]
		elseif field_type == "SubtractiveComponent" then
		elseif field_type == "BufferArray" then
		elseif field_type == "SharedComponentDataArray" then
		elseif field_type == "EntityArray" then
		elseif field_type == "Length" then
		end
	end
end