local oo = require("bp.common.lua_oo")

local mt = {}
local class_template = {type="sequence", __index=mt}
local sequence = oo.class(class_template)

function mt:start(graph)
	assert(oo.type(graph)=="graph", "graph must be an bp.graph type! see graph.lua")
	self.graph = graph
	self.curActionIndex = 1
	self.startedActionIndex = 0
	self.isDone = false
end

function mt:is_done()
	return self.isDone
end

function mt:add_one( v )
	self[#self+1] = v
end

function mt:add_list( list )
	for i,v in ipairs(list) do
		self[#self+1] = v
	end
end

function mt:update(dt)
	local actionNum = #self
	local hasTimeAct = nil
	for i=self.curActionIndex,actionNum do
		self.curActionIndex = i
		local action = self[i]
		if i > self.startedActionIndex then
			assert(action.start, "cannot find start method in index : "..(i or "nil"))
			action:start(self.graph)
			self.startedActionIndex = i
		end
		action:update(dt)
		local isDone, isTimeAction = action:is_done()
		if isDone then
			if i == actionNum then
				self.isDone = true
			end
			if isTimeAction then
				--一次update里不让连着完成两次时间相关的action
				if hasTimeAct then
					break
				else
					hasTimeAct = true
				end
			end
		else
			break
		end
	end
end

function mt:clone()
	local ret = sequence {}
	for i,v in ipairs(self) do
		ret[#ret + 1] = v:clone()
	end
	return ret
end

return sequence