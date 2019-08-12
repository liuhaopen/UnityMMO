local Update = function(self, deltaTime)
	local actionNum = #self
	local hasTimeAct = nil
	for i=self.curActionIndex,actionNum do
		self.curActionIndex = i
		local action = self[i]
		if i > self.startedActionIndex then
			action:Start(self.data)
			self.startedActionIndex = i
		end
		action:Update(deltaTime)
		local isDone, isTimeAction = action:IsDone()
		if isDone then
			if i == actionNum then
				self.isDone = true
			end
			if isTimeAction then
				--一次Update里不让连着完成两次时间相关的action
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

local Sequence = Ac.OO.Class {
	type 	= "Sequence",
	__index = {
		Start = function(self, data)
			self.data = data
			self.curActionIndex = 1
			self.startedActionIndex = 0
			self.isDone = false
		end,
		IsDone = function(self)
			return self.isDone
		end,
		Update = Update,
	},
}

return Sequence