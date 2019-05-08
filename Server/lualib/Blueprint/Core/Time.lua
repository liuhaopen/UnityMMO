local Time = {}

--每帧更新最新时间，秒为单位
function Time:Update( curTime )
	self.time = curTime
	self.deltaTime = (self.time-(self.lastUpdateTime or self.time))
	self.lastUpdateTime = self.time
end

return Time