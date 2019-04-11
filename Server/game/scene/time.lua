local skynet = require "skynet"

local time = {}

function time:get_cur_time(  )
	return math.floor(skynet.time()*1000+0.5)
end
	
return time