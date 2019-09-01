local SpeedData = BaseClass()
--因为速度会有多方同时改变到，所以获取当前速度时应该要考虑各方的设置，比如 A 状态下设置速度为0，状态结束后设回原来速度，但如果 A 状态期间又发生 B 状况需要把速度改成其它值，这时是不让改的，所以必须记录各种状态的速度设置。
function SpeedData:Constructor( baseSpeed, curSpeed )
	self.baseSpeed = baseSpeed
	self.curSpeed = curSpeed or baseSpeed
	self.bod = {}--董事会缩写:board of directors
end

--bodName : 每次更改速度都要指定是哪个董事会成员的意见, 
--offset：变更万分比，0时即不动
--isSet: true 时即发表意见，false 时为撤消意见
function SpeedData:ChangeSpeed( bodName, offset, isSet )
	if isSet then
		self.bod[bodName] = offset
	else
		table.remove(self.bod, bodName)
	end
	self:UpdateSpeed()
end

function SpeedData:UpdateSpeed(  )
	local hasFrozen = self:HasFrozen()
	if hasFrozen then
		self.curSpeed = 0
	else
		local factor = 0
		for k,v in pairs(self.bod) do
			factor = factor + v
		end
		self.curSpeed = self.baseSpeed + self.baseSpeed*factor
	end
end

function SpeedData:HasFrozen(  )
	local hasFrozen = false
	for k,v in pairs(self.bod) do
		if v == 0 then
			hasFrozen = true
			break
		end
	end
	return hasFrozen
end

return SpeedData