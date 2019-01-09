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


return CoreHelper