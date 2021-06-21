local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="bb_mgr", __index=mt}
local bb_mgr = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
	self.bd_data = {}
end

function mt:set_bb(name, value)
	self.bd_data[name] = value
end

function mt:get_bb(name)
	return self.bd_data[name]
end

return bb_mgr