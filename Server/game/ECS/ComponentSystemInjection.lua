local ComponentSystemInjection = {}
ECS.ComponentSystemInjection = ComponentSystemInjection

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
		local inject_target_tl = v[1]
		local inject_info = v[2]
		for component_name,inject_component_info in pairs(inject_info) do
			local info_parts = Split(inject_component_info)
			if #info_parts > 0 then
				if IsFindInStrList(info_parts, "ScriptMgr") then
					self:InjectConstructorDependencies(componentSystem, world, info_parts)
				else
					if InjectFromEntityData.SupportsInjections(info_parts) then
						--Cat_Todo : inject from entity data
						InjectFromEntityData.CreateInjection(info_parts, entityManager, injectFromEntity,
                            injectFromFixedArray)
					else
						local group = InjectComponentGroupData.CreateInjection(info_parts, componentSystem)
						table.insert(outInjectGroups, group)
					end
				end
			end
		end
	end
	--生成ComponentSystem.OnUpdate的环境table，只有Inject的才可以访问

end

--Cat_Todo : inject ScriptMgr class
function ComponentSystemInjection:InjectConstructorDependencies( manager, world, field )
end