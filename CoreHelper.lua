local CoreHelper = {}
ECS.CoreHelper = CoreHelper

function CoreHelper.Init(  )
	if not CoreHelper.NativeTypeSize then
		CoreHelper.NativeTypeSize = {
		["number"] = ECS.Core.GetNumberSize(),
		["integer"] = ECS.Core.GetIntegerSize(),
		["boolean"] = ECS.Core.GetBooleanSize(),
	}
	end
end

function CoreHelper.GetNativeTypeSize( type_name )
	return CoreHelper.NativeTypeSize[type_name] or 0
end

function CoreHelper.GetIntegerSize()
	return CoreHelper.NativeTypeSize["integer"]
end

function CoreHelper.GetNumberSize()
	return CoreHelper.NativeTypeSize["number"]
end

function CoreHelper.GetBooleanSize()
	return CoreHelper.NativeTypeSize["boolean"]
end

function CoreHelper.WriteFieldValueInChunk( chunk_ptr, offset, field_value, field_type )
	if not field_type then return end
	
	if field_type == "number" then
        ECS.Core.WriteNumber(chunk_ptr, offset, field_value)
    elseif field_type == "integer" then
        ECS.Core.WriteInteger(chunk_ptr, offset, field_value)
    elseif field_type == "boolean" then
        ECS.Core.WriteBoolean(chunk_ptr, offset, field_value and 1 or 0)
    else
    	assert(false, "wrong field type : "..field_type)
    end
end

function CoreHelper.ReadFieldValueInChunk( chunk_ptr, offset, field_type )
	if not field_type then return end
	
	local value = nil
	if field_type == "number" then
        value = ECS.Core.ReadNumber(chunk_ptr, offset)
    elseif field_type == "integer" then
        value = ECS.Core.ReadInteger(chunk_ptr, offset)
    elseif field_type == "boolean" then
        value = ECS.Core.ReadBoolean(chunk_ptr, offset)
    else
    	assert(false, "wrong field type : "..field_type)
    end
    return value
end

ECS.CoreHelper.Init()

return CoreHelper