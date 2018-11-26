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
	
end

function TypeManager.RegisterType( type, name )
	if not TypeManager.s_Types[name] then
		TypeManager.s_Count = TypeManager.s_Count + 1
	end
	local type_info = {
		Type = type,
	}
	TypeManager.s_Types[name] = type_info
end

local CreateTypeIndexThreadSafe = function ( type_name )
	
end

function TypeManager.GetTypeIndexByName( type_name )
	assert(type_name and type_name ~= "", "wrong type name!")
	local index = TypeManager.StaticTypeLookup[type_name]
	if index then
		return index
	end
	index = CreateTypeIndexThreadSafe(type_name)
	TypeManager.StaticTypeLookup[type_name] = index
	return index
end