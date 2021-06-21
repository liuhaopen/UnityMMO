local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="loop", __index=mt}
local loop = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
	self.loop = self[1]
	self.hadLoop = 0
	self.action = self[2]
	self.break_func = self.break_func or self[3]
	self.startedActionLoopIndex = 0
end

function mt:is_done()
	if self.break_func then
		local conditionFunctor = self.break_func
		if oo.has_func(conditionFunctor, "start") then
			conditionFunctor:start(self.graph)
		end
		local is_break
		if oo.has_func(conditionFunctor, "update") then
			is_break = conditionFunctor:update()
		else
			is_break = conditionFunctor(self.graph)
		end
		return is_break
	end
	if self.loop == -1 then
		--永远循环
		return false
	end
	return self.hadLoop >= self.loop
end

function mt:update(dt)
	local need_update = self.loop == -1
	if not need_update then
		local leftLoop = self.loop - self.hadLoop
		need_update = leftLoop > 0
	end
	if self.log then
		slog.ailoop('Cat:loop ', need_update, self.action)
	end
	if need_update then
		if self.hadLoop == self.startedActionLoopIndex then
			self.startedActionLoopIndex = self.startedActionLoopIndex + 1
			self.action:start(self.graph)
		end
		self.action:update(dt)
		local isDone = self.action:is_done()
		if isDone then
			self.hadLoop = self.hadLoop + 1
		end
	end
end

return loop