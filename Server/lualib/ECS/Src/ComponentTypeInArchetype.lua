local ComponentTypeInArchetype = ECS.BaseClass()
ECS.ComponentTypeInArchetype = ComponentTypeInArchetype

function ComponentTypeInArchetype:Constructor( type )
	self.TypeIndex = type.TypeIndex
    self.BufferCapacity = type.BufferCapacity
end

function ComponentTypeInArchetype.Create( type )
	local arche = ComponentTypeInArchetype.New(type)
	ComponentTypeInArchetype.InitMetaTable(arche)
	return arche
end

local is_equal = function ( lhs, rhs )
	return lhs.TypeIndex == rhs.TypeIndex and lhs.BufferCapacity == rhs.BufferCapacity
end

local less_than = function ( lhs, rhs )
	return lhs.TypeIndex ~= rhs.TypeIndex and lhs.TypeIndex < rhs.TypeIndex  or lhs.BufferCapacity < rhs.BufferCapacity
end

local big_than = function ( lhs, rhs )
	return lhs.TypeIndex ~= rhs.TypeIndex and lhs.TypeIndex > rhs.TypeIndex or lhs.BufferCapacity > rhs.BufferCapacity
end

local less_equal = function ( lhs, rhs )
    return not big_than(lhs, rhs)
end

function ComponentTypeInArchetype.InitMetaTable( arche )
	local meta_tbl = getmetatable(arche)
	meta_tbl.__eq = is_equal
	meta_tbl.__lt = less_than
	meta_tbl.__le = less_equal
	setmetatable(arche, meta_tbl)
end

return ComponentTypeInArchetype