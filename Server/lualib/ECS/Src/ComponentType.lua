local ComponentType = ECS.BaseClass()
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
	local ctype = ComponentType.FromTypeIndex(ECS.TypeManager.GetTypeIndexByName(type_name))
	ComponentType.InitMetaTable(ctype)
	return ctype
end

function ComponentType.FromTypeIndex( typeIndex )
	local ct = ECS.TypeManager.GetTypeInfoByIndex(typeIndex)
    local type = ComponentType.New()
    type.TypeIndex = typeIndex
    type.AccessModeType = ComponentType.AccessMode.ReadWrite
    type.BufferCapacity = ct.BufferCapacity
    return type
end

local is_equal = function ( lhs, rhs )
	return lhs.TypeIndex == rhs.TypeIndex and lhs.BufferCapacity == rhs.BufferCapacity and lhs.AccessModeType == rhs.AccessModeType
end

local less_than = function ( lhs, rhs )
	if lhs.TypeIndex == rhs.TypeIndex then
        return lhs.BufferCapacity ~= rhs.BufferCapacity
            and lhs.BufferCapacity < rhs.BufferCapacity
            or lhs.AccessModeType < rhs.AccessModeType
    end
    return lhs.TypeIndex < rhs.TypeIndex
end

local big_than = function ( lhs, rhs )
	return less_than(rhs, lhs)
end

local less_equal = function ( lhs, rhs )
    return not big_than(lhs, rhs)
end

function ComponentType.InitMetaTable( ctype )
	local meta_tbl = getmetatable(ctype)
	meta_tbl.__eq = is_equal
	meta_tbl.__lt = less_than
	meta_tbl.__le = less_equal
	setmetatable(ctype, meta_tbl)
end

return ComponentType