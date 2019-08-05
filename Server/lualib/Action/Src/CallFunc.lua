local CallFunc = Ac.OO.Class {
	type 	= "CallFunc",
	__index = {
		Start = function(self, data)
			self.data = data
		end,
		IsDone = function(self)
			return true
		end,
		Update = function(self, deltaTime)
			self[1](self.data)
		end
	},
}
return CallFunc