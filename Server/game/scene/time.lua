local skynet = require "skynet"

local Time = {
	time = 0,
	deltaTime = 0,
	lastUpdateTime = 0,
}

function Time:update( curTime, deltaTime )
	local skynetTime = skynet.time()
	self.time = skynetTime
	self.timeMS = math.floor(skynetTime*1000+0.5)
	self.deltaTime = (self.time-(self.lastUpdateTime or self.time))
	self.lastUpdateTime = self.time
end
	
return Time