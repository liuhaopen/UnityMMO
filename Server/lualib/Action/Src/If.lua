local If = Ac.OO.Class {
	type 	= "If",
	__index = {
		Start = function(self, data)
			self.data = data
			self.action = nil
		end,
		IsDone = function(self)
			local isDone, isTimeAction
			if self.action then
				isDone, isTimeAction = self.action:IsDone()
			else
				isDone = true
			end
			return isDone, isTimeAction
		end,
		Update = function(self, deltaTime)
			if self.action == nil then
				local conditionFunctor = self[1] 
				if type(conditionFunctor)=="table" and conditionFunctor.Start then
					conditionFunctor:Start(self.data)
				end
				local isTrue = conditionFunctor(self.data)
				if isTrue then
					self.action  = self[2]
				else
					self.action  = self[3]
				end
				if self.action then
					self.action:Start(self.data)
				end
			end
			if self.action then
				self.action:Update(deltaTime)
			end
		end
	},
}
return If