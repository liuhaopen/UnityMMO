local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="any_true", __index=mt}
local any_true = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
end

function class_template:__call()
	for i,v in ipairs(self) do
		local condition = v
		if type(condition)=="table" and condition.start then
			condition:start(self.graph)
		end
		if condition(self.graph) then
			return true
		end
	end
	return false
end

return any_true