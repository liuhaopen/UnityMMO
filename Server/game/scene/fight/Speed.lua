local Ac = require "Action"
local FightHelper = require("game.scene.FightHelper")
local SceneConst = require "game.scene.SceneConst"

local Speed = Ac.OO.Class {
	type 	= "Speed",
	__index = {
		Start = function(self, buffData)
			self.buffData = buffData
			self.sceneMgr = buffData.sceneMgr
			self.entityMgr = self.sceneMgr.entityMgr
		end,
		IsDone = function(self)
			return true
		end,
		Update = function(self, deltaTime)
			FightHelper:ChangeSpeed(self.buffData.victim_entity, self.buffData.victim_uid, self.buffData.caster_uid, self.bod, self.is_set, self.value)
		end,
	},
}
return Speed