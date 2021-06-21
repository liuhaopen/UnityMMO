local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="if_else", __index=mt}
local if_else = oo.class(class_template)

function mt:start(graph)
	assert(oo.type(graph)=="graph", "graph must be an bp.graph type! see graph.lua")
	self.graph = graph
	self.action = nil
end

function mt:is_done()
	local isDone, isTimeAction
	if self.action then
		isDone, isTimeAction = self.action:is_done()
	else
		isDone = true
	end
	return isDone, isTimeAction
end

function mt:update(dt)
	if self.action == nil then
		local conditionFunctor = self[1] 
		if oo.has_func(conditionFunctor, "start") then
			conditionFunctor:start(self.graph)
		end
		local isTrue
		if oo.has_func(conditionFunctor, "update") then
			isTrue = conditionFunctor:update()
		else
			isTrue = conditionFunctor(self.graph)
		end
		if isTrue then
			self.action  = self[2]
		else
			self.action  = self[3]
		end
		if self.action then
			self.action:start(self.graph)
		end
		if self.log then
			slog.if_else('Cat:if_else ', isTrue, self.action, self[2], self[3])
		end
	end
	if self.action then
		self.action:update(dt)
	end
end

return if_else