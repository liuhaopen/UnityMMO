local And = Ac.OO.Class {
	type 	= "And",
	__index = {
		Start = function(self, data)
			self.data = data
		end,
	},
	__call = function(self)
		local conditionA = self[1]
		local conditionB = self[2]
		if type(conditionA)=="table" and conditionA.Start then
			conditionA:Start(self.data)
		end
		if type(conditionB)=="table" and conditionB.Start then
			conditionB:Start(self.data)
		end
		return conditionA(self.data) and conditionB(self.data)
	end
}
return And