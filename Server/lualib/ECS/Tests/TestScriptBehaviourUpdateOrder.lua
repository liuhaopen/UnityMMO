TestScriptBehaviourUpdateOrder = BaseClass(require("TestBaseClass"))

local get_index = function ( list, system )
	for i,v in ipairs(list) do
		if v == system then
			return i
		end
	end
	return 999999
end

function TestScriptBehaviourUpdateOrder:TestSortSystemList(  )
	local system_map = {}
	local SystemA = {
		UpdateBefore={"SystemB"},
	}
	local SystemB = {
		UpdateBefore={"SystemC"},
	}
	local SystemC = {
	}
	local SystemD = {
		UpdateAfter={"SystemC"},
	}
	local SystemE = {
		UpdateAfter={"SystemD"},
	}
	system_map["SystemA"] = SystemA
	system_map["SystemB"] = SystemB
	system_map["SystemC"] = SystemC
	system_map["SystemD"] = SystemD
	system_map["SystemE"] = SystemE
	system_map["SystemF"] = SystemF
	local sorted_list = ECS.ScriptBehaviourUpdateOrder.SortSystemList( system_map )
	local index_for_a = get_index(sorted_list, SystemA)
	local index_for_b = get_index(sorted_list, SystemB)
	local index_for_c = get_index(sorted_list, SystemC)
	local index_for_d = get_index(sorted_list, SystemD)
	local index_for_e = get_index(sorted_list, SystemE)
	local index_for_f = get_index(sorted_list, SystemF)
	lu.assertEquals(index_for_a, 1)
end
