local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="call_func", __index=mt}
local call_func = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
end

function mt:is_done()
	return true
end

function mt:update(dt)
	local cb = self[1]
	if self.bb_name then
		cb = self.graph:get_bb(self.bb_name)
	end
	if cb then
		return cb(self.graph)
	end
end

return call_func