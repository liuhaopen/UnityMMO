local InjectComponentGroupData = ECS.BaseClass()
ECS.InjectComponentGroupData = InjectComponentGroupData

function InjectComponentGroupData:Constructor( system, injectGroupName, componentRequirements, componentDataInjections, lengthFieldInfo )
	self.system = system
	self.m_InjectGroupName = injectGroupName
	self.m_ComponentDataInjections = componentDataInjections
	self.m_EntityGroup = system:GetComponentGroup(componentRequirements)
	self.m_LengthFieldInfo = lengthFieldInfo

	self:PatchGetIndexInComponentGroup(self.m_ComponentDataInjections)
    -- self:PatchGetIndexInComponentGroup(m_BufferArrayInjections)
    -- self:PatchGetIndexInComponentGroup(m_SharedComponentInjections)
end

function InjectComponentGroupData.CreateInjection( injectGroupName, groupField, system )
	local componentRequirements = {}
	local componentDataInjections = {}
	local lengthFieldInfo = {}
	InjectComponentGroupData.CollectInjectedGroup(system, groupField, componentRequirements, componentDataInjections, lengthFieldInfo)
	return InjectComponentGroupData.New(system, injectGroupName, componentRequirements, componentDataInjections, lengthFieldInfo)
end

function InjectComponentGroupData:UpdateInjection(  )
    local origin_iterator, length = self.m_EntityGroup:GetComponentChunkIterator()
    self.system[self.m_InjectGroupName] = {}
	for i,v in ipairs(self.m_ComponentDataInjections) do
    	local iterator = ECS.ComponentChunkIterator.Clone(origin_iterator)
	    iterator:SetIndexInComponentGroup(self.m_ComponentDataInjections[i].IndexInComponentGroup)
        local data = ECS.ComponentDataArray.Create(iterator, length, self.m_ComponentDataInjections[i].ComponentTypeName)
        self.system[self.m_InjectGroupName][self.m_ComponentDataInjections[i].InjectFieldName] = data
	end
	if self.m_LengthFieldInfo and self.m_LengthFieldInfo.InjectFieldName then
        self.system[self.m_InjectGroupName][self.m_LengthFieldInfo.InjectFieldName] = length
	end
end

function InjectComponentGroupData.CollectInjectedGroup( system, groupField, componentRequirements, componentDataInjections, lengthFieldInfo )
	local field_info
	for field_name,v in pairs(groupField) do
		local field_info = Split(v, ":")
		if not field_info then return end

		local field_type = field_info and field_info[1]
		if field_type == "ComponentDataArray" or field_type == "Array" then
			local comp_type_name = field_info[2]
			table.insert(componentRequirements, comp_type_name)
			table.insert(componentDataInjections, {InjectFieldName=field_name, IndexInComponentGroup=0, ComponentTypeName=comp_type_name})
		elseif field_type == "SubtractiveComponent" then
		elseif field_type == "BufferArray" then
		elseif field_type == "SharedComponentDataArray" then
		elseif field_type == "EntityArray" then
		elseif field_type == "Length" then
			lengthFieldInfo.InjectFieldName = field_name
		end
	end
end

function InjectComponentGroupData:PatchGetIndexInComponentGroup( componentInjections )
	for i=1,#componentInjections do
		local type_index = ECS.TypeManager.GetTypeIndexByName(componentInjections[i].ComponentTypeName)
        componentInjections[i].IndexInComponentGroup = self.m_EntityGroup:GetIndexInComponentGroup(type_index)
    end
end

return InjectComponentGroupData