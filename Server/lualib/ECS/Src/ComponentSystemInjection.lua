local ComponentSystemInjection = {}

local function Split(szFullString, szSeparator, start_pos)
	local nFindStartIndex = start_pos or 1
	local nSplitIndex = 1
	local nSplitArray = {}
	while true do
	    local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
	    if not nFindLastIndex then
	    	nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
	    	break
	   	end
	    table.insert(nSplitArray, string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1))
	    nFindStartIndex = nFindLastIndex + string.len(szSeparator)
	    nSplitIndex = nSplitIndex + 1
	end
	return nSplitArray
end

local IsFindInStrList = function ( str_list, find_str )
	if not str_list then return false end
	
	for k,v in pairs(str_list) do
		if v == find_str then
			return true
		end
	end
	return false
end

function ComponentSystemInjection.Inject( componentSystem, world, entityManager,
            outInjectGroups, outInjectFromEntityData )
	local inject_info_list = componentSystem:GetInjectInfoList()
	for i,v in ipairs(inject_info_list) do
		local inject_field_name = v[1]
		local inject_info = v[2]
		
		local group = ECS.InjectComponentGroupData.CreateInjection(inject_field_name, inject_info, componentSystem)
		table.insert(outInjectGroups, group)
	end
end

function ComponentSystemInjection:InjectConstructorDependencies( manager, world, field_info, inject_field_name )
	manager[inject_field_name] = world:GetOrCreateManager(field_info[2])	
end

return ComponentSystemInjection