local TypeManager = {}
ECS.TypeManager = TypeManager

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

local CalculateFieldInfo, CalculateMemoryOrdering

function TypeManager.Initialize(  )
	-- self.ObjectOffset = UnsafeUtility.SizeOf<ObjectOffsetType>();
    -- self.s_CreateTypeLock = new SpinLock()
    TypeManager.s_Types = {}
    TypeManager.StaticTypeLookup = {}
    TypeManager.s_Count = 0

	TypeManager.s_Types[TypeManager.s_Count] = {
		Type=nil, SizeInChunk=0, Category=TypeManager.TypeCategory.ComponentData,
		-- FastEqualityTypeInfo = FastEquality.TypeInfo.Null,
		EntityOffsets = nil, MemoryOrdering=0, BufferCapacity=-1, ElementSize = 0
	}
    TypeManager.s_Count = TypeManager.s_Count + 1

    TypeManager.RegisterType(ECS.Entity.Name, {Index="integer", Version="integer"})
end

function TypeManager.BuildComponentType( name, type_desc )
	local field_info_list, size = CalculateFieldInfo(type_desc)
	local memoryOrdering = CalculateMemoryOrdering(name)
	local type_info = {
		Name = name,
		Type = type_desc,
		TypeIndex = TypeManager.s_Count,
		BufferCapacity = -1,
		SizeInChunk = size,
		FieldInfoList = field_info_list,
		MemoryOrdering = memoryOrdering,
	}
	return type_info
end

function TypeManager.RegisterType( name, type_desc )
	if TypeManager.StaticTypeLookup[name] then
		return
	end
	local type_info = TypeManager.BuildComponentType(name, type_desc)
	TypeManager.s_Types[TypeManager.s_Count] = type_info
	TypeManager.StaticTypeLookup[name] = TypeManager.s_Count
	TypeManager.s_Count = TypeManager.s_Count + 1
	return type_info
end

CalculateFieldInfo = function ( type_desc )
	local field_names = {}
	for k,v in pairs(type_desc) do
		assert(type(k)=="string", "key type must be string!")
		table.insert(field_names, k)
	end
	table.sort(field_names)
	--Cat_Todo : 考虑字节对齐，提高读取性能
	local sum_size = 0
	local field_info_list = {}
	for i,v in ipairs(field_names) do
		local field_type = type_desc[v]
		local field_desc_type = type(field_type)
		if field_desc_type == "string" then
			local field_size = ECS.CoreHelper.GetNativeTypeSize(field_type)
			table.insert(field_info_list, {FieldName=v, FieldType=field_type, FieldSize=field_size, Offset=sum_size})
			sum_size = sum_size + field_size
		elseif field_desc_type == "table" then
			local out_field_info_list, out_field_size = CalculateFieldInfo(field_type)
			table.insert(field_info_list, {FieldName=v, FieldType="table", FieldSize=out_field_size, Offset=sum_size, ChildFieldList=out_field_info_list})
			sum_size = sum_size + out_field_size
		else
			assert(false, "wrong type : "..field_desc_type)
		end
	end
	return field_info_list, sum_size
end

CalculateMemoryOrdering = function ( type_name )
	if type_name == ECS.Entity.Name then
		return 0
	end
	return 1
end

function TypeManager.GetTypeIndexByName( type_name )
	assert(type_name and type_name ~= "", "wrong type name!")
	local index = TypeManager.StaticTypeLookup[type_name]
	assert(index, "had no register type : "..type_name)
	if index then
		return index
	end
end

function TypeManager.GetTypeInfoByIndex( typeIndex )
	return TypeManager.s_Types[typeIndex]
end

function TypeManager.GetTypeInfoByName( typeName )
	local index = TypeManager.GetTypeIndexByName(typeName)
	return TypeManager.s_Types[index]
end

function TypeManager.GetTypeNameByIndex( typeIndex )
	local info = TypeManager.s_Types[typeIndex]
	return info and info.Name or "UnkownTypeName"
end

return TypeManager