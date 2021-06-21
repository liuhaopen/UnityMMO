local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="random", __index=mt}
local random = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
end

function class_template:__call()
	local ret = math.random(1, 100)
	return ret <= self[1]
end

return random