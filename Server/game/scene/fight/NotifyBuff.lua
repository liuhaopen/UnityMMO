local Ac = require "Action"
local SceneConst = require "game.scene.SceneConst"

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
			local buffID = self[1]
			local duration = self[2]
			local endTime = Time.timeMS + duration
			local buffEvent = {
				key = SceneConst.InfoKey.Buff, 
				value = string.format("%s,%s,%s", buffID, endTime, self.buffData.caster_uid),
			}
			self.sceneMgr.eventMgr:AddSceneEvent(self.buffData.victim_uid, buffEvent)
		end,
	},
}
return NotifyBuff