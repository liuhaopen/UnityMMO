local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="parallel", __index=mt}
local parallel = oo.class(class_template)

local State = {
	Updating = 1,
	Done = 2,
}
function mt:start(graph)
	self.graph = graph
	self.done_actions = {}
	self._is_done = false
end

function mt:is_done()
	return self._is_done
end

local function get_done_num(done_actions)
	local ret = 0
	for k,v in pairs(done_actions) do
		if v == State.Done then
			ret = ret + 1
		end
	end
	return ret
end

function mt:update(dt)
	local action_num = #self
	for i=1, action_num do
		local action = self[i]
		if not self.done_actions[i] then
			self.done_actions[i] = State.Updating
			action:start(self.graph)
		end
		if self.done_actions[i] == State.Updating then
			action:update(dt)
			local isDone, isTimeAction = action:is_done()
			if isDone then
				self.done_actions[i] = State.Done
				local done_num = get_done_num(self.done_actions)
				self._is_done = done_num>=action_num
			end
		end
	end
end

return parallel