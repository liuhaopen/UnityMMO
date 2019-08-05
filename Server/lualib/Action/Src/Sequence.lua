local Update = function(self, deltaTime)
	local actionNum = #self
	if self.curActionIndex >= actionNum then
		self.isDone = true
		return
	end
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