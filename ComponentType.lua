local ComponentType = BaseClass()
ECS.ComponentType = ComponentType

ComponentType.AccessMode = {
	ReadWrite = 1,
	ReadOnly = 2,
	Subtractive = 3,
}

function ComponentType:Constructor(  )
	self.TypeIndex = 0
	self.AccessModeType = ComponentType.AccessMode.ReadWrite

end

function ComponentType.Create( type_name )
	return ComponentType.FromTypeIndex(ECS.TypeManager.GetTypeIndexByName(type_name))
end

function ComponentType.FromTypeIndex( typeIndex )
	local ct = ECS.TypeManager.GetTypeInfoByIndex(typeIndex)
    local type = {}
    type.TypeIndex = typeIndex
    type.AccessModeType = ComponentType.AccessMode.ReadWrite
    type.BufferCapacity = ct.BufferCapacity
    return type
end

-- local meta_tbl = getmetatable(ComponentType)

return ComponentType