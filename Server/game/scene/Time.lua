local skynet = require "skynet"

local Time = {
	time = 0,
	deltaTime = 0,
	lastUpdateTime = 0,
}

function Time:update( curTime, deltaTime )
	local skynetTime = skynet.time()
	self.time = skynetTime
	self.timeMS = math.floor(skynetTime*1000+0.5)--Cat_Todo : 应该加一个每次都取最新值的，这样更加精确
	self.deltaTime = (self.time-(self.lastUpdateTime or self.time))
	self.deltaTimeMS = math.floor(self.deltaTime*1000+0.5)
	self.lastUpdateTime = self.time
end
	
return Time