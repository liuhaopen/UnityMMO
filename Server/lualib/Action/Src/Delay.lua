local Delay = Ac.OO.Class {
	type 	= "Delay",
	__index = {
		Start = function(self)
			self.elapsed = 0
			-- self.updateNum = 0
		end,
		IsDone = function(self)
			return self.elapsed >= self[1], true-- and self.updateNum>1
		end,
		Update = function(self, deltaTime)
			self.elapsed = self.elapsed + deltaTime
			-- self.updateNum = self.updateNum + 1
		end
	},
}
return Delay