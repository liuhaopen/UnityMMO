local Or = Ac.OO.Class {
	type 	= "Or",
	__index = {
		Start = function(self, data)
			self.data = data
		end,
	},
	__call = function(self)
		local conditionA = self[1]
		if type(conditionA)=="table" and conditionA.Start then
			conditionA:Start(self.data)
		end
		if conditionA(self.data) then
			return true
		end
		local conditionB = self[2]
		if type(conditionB)=="table" and conditionB.Start then
			conditionB:Start(self.data)
		end
		return conditionB(self.data)
	end
}
return Or