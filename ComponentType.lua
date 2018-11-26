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
	return ComponentType.FromTypeIndex(TypeManager.GetTypeIndexByName(type_name))
end

function ComponentType.FromTypeIndex( typeIndex )
	local ct = TypeManager.GetTypeInfo(typeIndex)
    local type = {}
    type.TypeIndex = typeIndex
    type.AccessModeType = AccessMode.ReadWrite
    type.BufferCapacity = ct.BufferCapacity
    return type
end