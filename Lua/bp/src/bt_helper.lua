local oo = require("bp.common.lua_oo")
local mt = {}

function mt.get_input(act, slot_index, arge)
	--最优先从输入管道取值
	local ret
	if slot_index then
		ret = mt.get_value_from_slot(act, slot_index)
	end
	local is_val_act = false
	if not ret and arge then
		ret, is_val_act = mt.get_value_from_action(act, arge)
	end
	if not ret and not is_val_act then
		ret = arge
	end
	return ret
end

function mt.get_value_from_slot(act, slot_index)
	return act.graph and act.graph:get_slot_value(act, slot_index)
end

function mt.get_value_from_action(act, arge)
	if type(arge) == "function" then
		return arge(act.graph)
	elseif oo.has_func(arge, "update") then
		arge:start(act.graph)
		return arge:update(), true
	elseif oo.callable(arge) then
		arge:start(act.graph)
		return arge(act, act.graph), true
	end
	return nil
end

return mt