local oo = require("bp.common.lua_oo")
local bt_helper = require("bp.src.bt_helper")

local mt = {}
local class_template = {type="wait", __index=mt}
local wait = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
	self.elapsed = 0
	self.duration = bt_helper.get_input(self, 1, self[1])
end

function mt:is_done()
	return self.elapsed >= self.duration, true
end

function mt:update(dt)
	self.elapsed = self.elapsed + dt
end

function mt:clone(  )
	local duration = self[1]
	local type_d = type(duration)
	--如果duration也是节点，比如ai.get_bb，这里也应该支持clone的
	assert(type_d=="number", "unspport type for clone:"..(type_d or "nil"))
	return wait {duration}
end

return wait