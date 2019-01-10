local CoreHelper = {}
ECS.CoreHelper = CoreHelper

function CoreHelper.Init(  )
	if not CoreHelper.NativeTypeSize then
		CoreHelper.NativeTypeSize = {
		["number"] = ECSCore.GetNumberSize(),
		["integer"] = ECSCore.GetIntegerSize()
	}
	end
end

function CoreHelper.GetNativeTypeSize( type_name )
	return CoreHelper.NativeTypeSize[type_name] or 0
end

function CoreHelper.GetIntegerSize(  )
	return CoreHelper.NativeTypeSize["integer"]
end

function CoreHelper.WriteFieldValueInChunk( chunk_ptr, offset, field_value, field_type )
	if not field_type then return end
	
	if field_type == "number" then
        ECSCore.WriteNumber(chunk_ptr, offset, field_value)
    else
    	assert(false, "wrong field type : "..field_type)
    end
end

return CoreHelper