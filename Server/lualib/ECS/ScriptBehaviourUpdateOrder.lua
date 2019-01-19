local ScriptBehaviourUpdateOrder = {}
ECS.ScriptBehaviourUpdateOrder = ScriptBehaviourUpdateOrder

function ScriptBehaviourUpdateOrder.SortSystemList( system_map )
	local result = {}
	for k,v in pairs(system_map) do
		v.__name_for_sort__ = k
		table.insert(result, v)
	end
	return result
end