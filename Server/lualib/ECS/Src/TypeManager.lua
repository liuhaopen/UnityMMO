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
TypeManager.s_Systems = {}
TypeManager.s_Count = 0
TypeManager.StaticTypeLookup = {}

local CalculateFieldInfo, CalculateMemoryOrdering

function TypeManager.Initialize(  )
    TypeManager.RegisterType(ECS.Entity.Name, {Index=0, Version=0})
end

function TypeManager.BuildComponentType( name, type_desc )
	local memoryOrdering = CalculateMemoryOrdering(name)
	local type_info = {
		Name = name,
		Prototype = type_desc,
		TypeIndex = TypeManager.s_Count,
		BufferCapacity = -1,
		MemoryOrdering = memoryOrdering,
	}
	return type_info
end

function TypeManager.RegisterType( name, type_desc )
	if TypeManager.StaticTypeLookup[name] then
		return TypeManager.s_Types[TypeManager.StaticTypeLookup[name]]
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

function TypeManager.RegisterScriptMgr( name, system )
	assert(TypeManager.s_Systems[name]==nil, "had register system :"..name)
	TypeManager.s_Systems[name] = system
end

function TypeManager.GetScriptMgr( name )
	return TypeManager.s_Systems[name]
end

function TypeManager.GetScriptMgrMap( )
	return TypeManager.s_Systems
end

return TypeManager