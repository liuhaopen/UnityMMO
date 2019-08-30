local SpeedData = BaseClass()
--因为速度会有多方同时改变到，所以获取当前速度时应该要考虑各方的设置，比如 A 状态下设置速度为0，状态结束后设回原来速度，但如果 A 状态期间又发生 B 状况需要把速度改成其它值，这时是不让改的，所以必须记录各种状态的速度设置。
function SpeedData:Constructor( baseSpeed, curSpeed )
	self.baseSpeed = baseSpeed
	self.curSpeed = curSpeed or baseSpeed
	self.jury = {}
	self.frozenJury = {}
end

function SpeedData:ChangeSpeed( juryName, offset )
	self.jury[juryName] = offset
end

function SpeedData:SetFrozen( juryName, isFrozen )
	if isFrozen then
		self.frozenJury[juryName] = isFrozen
	else
		table.remove(self.frozenJury, juryName)
	end
	self:UpdateSpeed()
end

function SpeedData:UpdateSpeed(  )
	local hasFrozen = self:HasFrozen()
	if hasFrozen then
		self.curSpeed = 0
	else
		local factor = 0
		for k,v in pairs(self.jury) do
			factor = factor + v
		end
		self.curSpeed = self.baseSpeed + self.baseSpeed*factor
	end
end

function SpeedData:HasFrozen(  )
	return next(self.frozenJury)
end

return SpeedData