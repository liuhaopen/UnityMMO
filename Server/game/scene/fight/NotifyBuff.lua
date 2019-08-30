local Ac = require "Action"
local NotifyBuff = Ac.OO.Class {
	type 	= "NotifyBuff",
	__index = {
		Start = function(self, buffData)
			self.buffData = buffData
			self.sceneMgr = buffData.sceneMgr
		end,
		IsDone = function(self)
			return true
		end,
		Update = function(self, deltaTime)
			
		end,
	},
}
return NotifyBuff