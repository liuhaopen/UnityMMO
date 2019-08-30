local Ac = require "Action"
local FightHelper = require("game.scene.FightHelper")
local SceneConst = require "game.scene.SceneConst"

local SuckHP = Ac.OO.Class {
	type 	= "SuckHP",
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
			local hp = self.entityMgr:GetComponentData(self.buffData.victim_entity, "UMO.HP")
			local suckHP = 0
			if self.is_percent then
				local baseAttr = self.entityMgr:GetComponentData(self.buffData.victim_entity, "UMO.BaseAttr")
				local maxHP = baseAttr[SceneConst.Attr.HP] or 0
				suckHP = maxHP * self.value / 10000
			else
				suckHP = self.value
			end
			suckHP = math.min(hp.cur, suckHP)
			FightHelper:ChangeHP(self.buffData.victim_entity, hp, suckHP, self.buffData.caster_uid)
			local buffEvent = {
				key = SceneConst.InfoKey.Buff, 
				value = string.format("%s,%s,%s", SceneConst.Buff.SuckHP, math.floor(suckHP), self.buffData.caster_uid),
			}
			self.sceneMgr.eventMgr:AddSceneEvent(self.buffData.victim_uid, buffEvent)
		end,
	},
}
return SuckHP