local TypeManager = {}
ECS.TypeManager = TypeManager
local this = TypeManager
TypeManager.TypeCategory = {
	ComponentData = 1,
	BufferData = 2,
	ISharedComponentData = 3,
	EntityData = 4,
	Class = 5,
}
TypeManager.s_Types = {}
TypeManager.s_Count = 0
TypeManager.StaticTypeLookup = {}
function TypeManager.Initialize(  )
	-- self.ObjectOffset = UnsafeUtility.SizeOf<ObjectOffsetType>();
    -- self.s_CreateTypeLock = new SpinLock()
    TypeManager.s_Types = {}
    TypeManager.s_Count = 0

	TypeManager.s_Types[TypeManager.s_Count] = {
		Type=nil, SizeInChunk=0, Category=TypeManager.TypeCategory.ComponentData,
		-- FastEqualityTypeInfo = FastEquality.TypeInfo.Null,
		EntityOffsets = nil, MemoryOrdering=0, BufferCapacity=-1, ElementSize = 0
	}
    TypeManager.s_Count = TypeManager.s_Count + 1
    TypeManager.s_Types[TypeManager.s_Count] = {
		Type="ECS.Entity", SizeInChunk=ECS.Entity.Size, Category=TypeManager.TypeCategory.EntityData,
		-- FastEqualityTypeInfo = FastEquality.CreateTypeInfo(typeof(Entity)),
		-- EntityOffsets = EntityRemapUtility.CalculateEntityOffsets(typeof(Entity)), 
		MemoryOrdering=0, BufferCapacity=-1, ElementSize = ECS.Entity.Size
	}
	TypeManager.StaticTypeLookup["ECS.Entity"] = TypeManager.s_Count
    TypeManager.s_Count = TypeManager.s_Count + 1
end

local CalculateFieldInfo = function ( type_desc )
	local field_names = {}
	for k,v in pairs(type_desc) do
		table.insert(field_names, tostring(k))
	end
	table.sort(field_names)
	local field_offset = {}

	return field_offset
end

function TypeManager.RegisterType( name, type_desc )
	if TypeManager.StaticTypeLookup[name] then
		return
	end
	TypeManager.s_Count = TypeManager.s_Count + 1
	local type_info = {
		Name = name,
		Type = type_desc,
		TypeIndex = TypeManager.s_Count,
		BufferCapacity = -1,
		SizeInChunk = 4 --Cat_Todo : size in chunk
	}
	TypeManager.s_Types[name] = type_info
	TypeManager.StaticTypeLookup[name] = TypeManager.s_Count
end

local CreateTypeIndexThreadSafe = function ( type_name )
	TypeManager.s_Count = TypeManager.s_Count + 1
	local type_info = {
		Name = type_name,
		Type = type,
		TypeIndex = TypeManager.s_Count,
		BufferCapacity = -1,
	}
	TypeManager.s_Types[TypeManager.s_Count] = type_info
	return TypeManager.s_Count
end

function TypeManager.GetTypeIndexByName( type_name )
	assert(type_name and type_name ~= "", "wrong type name!")
	local index = TypeManager.StaticTypeLookup[type_name]
	assert(index, "had no register type : "..type_name)
	if index then
		return index
	end
	-- index = CreateTypeIndexThreadSafe(type_name)
	-- TypeManager.StaticTypeLookup[type_name] = index
	-- return index
end

function TypeManager.GetTypeInfoByIndex( typeIndex )
	return TypeManager.s_Types[typeIndex]
end

function TypeManager.GetTypeInfoByName( typeName )
	return TypeManager.s_Types[TypeManager.GetTypeIndexByName(typeName)]
end

return TypeManager

-- local TypeInfo = BaseClass()
-- ECS.TypeInfo = TypeInfo
-- function TypeManager:Constructor( type, size, category, typeInfo, entityOffsets, memoryOrdering, bufferCapacity, elementSize )
-- 	self.Type = type
--     self.SizeInChunk = size
--     self.Category = category
--     self.FastEqualityTypeInfo = typeInfo
--     self.EntityOffsets = entityOffsets
--     self.MemoryOrdering = memoryOrdering
--     self.BufferCapacity = bufferCapacity
--     self.ElementSize = elementSize
--     -- self.IsSystemStateSharedComponent = typeof(ISystemStateSharedComponentData).IsAssignableFrom(type)
--     -- self.IsSystemStateComponent = typeof(ISystemStateComponentData).IsAssignableFrom(type)
-- end