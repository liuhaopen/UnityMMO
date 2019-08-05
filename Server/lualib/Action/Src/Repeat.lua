local Repeat = Ac.OO.Class {
	type 	= "Repeat",
	__index = {
		Start = function(self, data)
			self.data = data
			self.loop = self[1]
			self.hadLoop = 0
			self.action = self[2]
			self.startedActionLoopIndex = 0
		end,
		IsDone = function(self)
			return self.hadLoop >= self.loop
		end,
		Update = function(self, deltaTime)
			local leftLoop = self.loop - self.hadLoop
			if leftLoop > 0 then
				if self.hadLoop == self.startedActionLoopIndex then
					self.startedActionLoopIndex = self.startedActionLoopIndex + 1
					self.action:Start(self.data)
				end
				self.action:Update(deltaTime)
				local isDone = self.action:IsDone()
				if isDone then
					self.hadLoop = self.hadLoop + 1
				end
			end
			-- for i=1,leftLoop do
			-- 	if self.hadLoop == self.startedActionLoopIndex then
			-- 		self.startedActionLoopIndex = self.startedActionLoopIndex + 1
			-- 		self.action:Start(self.data)
			-- 	end
			-- 	self.action:Update(deltaTime)
			-- 	local isDone = self.action:IsDone()
			-- 	if isDone then
			-- 		self.hadLoop = self.hadLoop + 1
			-- 	end
			-- 	break
			-- end
		end
	},
}
return Repeat