local If = Ac.OO.Class {
	type 	= "If",
	__index = {
		Start = function(self, data)
			self.data = data
			self.isTrue = nil
		end,
		IsDone = function(self)
			return self.isTrue and self[2]:IsDone() or self[3]:IsDone()
		end,
		Update = function(self, deltaTime)
			if self.isTrue == nil then
				local conditionFunctor = self[1] 
				if type(conditionFunctor)=="table" and conditionFunctor.Start then
					conditionFunctor:Start(self.data)
				end
				self.isTrue = conditionFunctor(self.data)
				if self.isTrue then
					self[2]:Start(self.data) 
				else
					self[3]:Start(self.data)
				end
			end
			if self.isTrue then
				self[2]:Update(deltaTime)
			else
				self[3]:Update(deltaTime)
			end
		end
	},
}
return If