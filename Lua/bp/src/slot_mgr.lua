local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="slot_mgr", __index=mt}
local slot_mgr = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
	self.slot_data = {}
	self.slot_names = {}
end

--注册孔是为了在 action 内部的孔是按顺序的数字作 key，只需要调用 self.graph:get_slot_value(第几个孔)
function mt:register_slot(act, type, index, name)
	self.slot_names[act] = self.slot_names[act] or {}
	self.slot_names[act][type] = self.slot_names[act][type] or {}
	self.slot_names[act][type][index] = name
end

function mt:get_slot_name(act, type, index, skip_err)
	if self.slot_names[act] and self.slot_names[act][type] and self.slot_names[act][type][index] then
		return self.slot_names[act][type][index]
	end
	-- if not skip_err then
	-- 	local err_str = string.format("cannot find slot : %s, type:%s, index:%s", act or "nil", type or "nil", index or "nil")
	-- 	error(err_str)
	-- end
	return nil
end

function mt:get_slot_value(act, index, type)
	type = type or "in"
	local name = self:get_slot_name(act, type, index, true)
	-- print('Cat:slot_mgr.lua[get_slot_value] name, act, index, type', name, act, index, type)
	return name and self.slot_data[name]
end

function mt:set_slot_value(act, index, value, type)
	type = type or "out"
	local name = self:get_slot_name(act, type, index)
	-- print('Cat:slot_mgr.lua[set_slot_value] name, act, index, value, type', name, act, index, value, type)
	if name then
		self.slot_data[name] = value
	end
end

return slot_mgr