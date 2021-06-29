local oo = require("bp.common.lua_oo")
local slot_mgr = require("bp.src.slot_mgr")

local mt = {}
local class_template = {type="get_bb", __index=mt}
local get_bb = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
end

function mt:is_done()
	return true
end

function mt:update(dt)
	local value = self.graph:get_bb(self[1])
	if value == nil then
		value = self[2]
	end
	self.graph:set_slot_value(self, 1, value)
	-- slog.cat('Cat:get_bb.lua[22] value', value, self)
	return value
end

return get_bb