local Ac = require "Action"
-- local FightHelper = require("game.scene.FightHelper")
-- local SceneConst = require "game.scene.SceneConst"
local Ability = Ac.OO.Class {
	type 	= "Ability",
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
			if not self.buffData.victim_entity then return end
			local isExist = self.entityMgr:Exists(self.buffData.victim_entity)
			if not isExist then return end
			local ability = self.entityMgr:GetComponentData(self.buffData.victim_entity, "UMO.Ability")
			ability.NormalAtk:Change(self.bod, self.is_set, self.value)
			ability.CastSkill:Change(self.bod, self.is_set, self.value)
		end,
	},
}
return Ability