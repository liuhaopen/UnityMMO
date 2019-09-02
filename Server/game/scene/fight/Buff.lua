local Ac = require "Action"
local BuffActions = require("game.scene.fight.BuffActions")

local GetBuffs = function (  )
	
end
local Buff = Ac.OO.Class {
	type 	= "Buff",
	__index = {
		Start = function(self, skillData)
			self.skillData = skillData
			self.sceneMgr = self.skillData.sceneMgr
			self.entityMgr = self.sceneMgr.entityMgr
		end,
		IsDone = function(self)
			return true
		end,
		Update = function(self, deltaTime)
			local buff_id = self[1]
			local buff_arge = self[2] or self.skillData.cfg.detail[self.skillData.skill_lv].buff
			local curTime = Time.timeMS
			for uid,_ in pairs(self.skillData.targets) do
				local targetEntity = self.sceneMgr:GetEntity(uid)
				local buffList = self.entityMgr:GetComponentData(targetEntity, "UMO.Buff")
				local buffActionCreator = BuffActions:GetActionCreator(buff_id)
				local buffAction = buffActionCreator(buff_arge)
				local buffData = TablePool:Get("BuffComData") or {}
				buffData.buff_id = buff_id
				buffData.victim_uid = uid
				buffData.victim_entity = targetEntity
				buffData.caster_uid = self.skillData.caster_uid
				buffData.caster_entity = self.skillData.caster_entity
				buffData.cast_time = curTime
				buffData.sceneMgr = self.skillData.sceneMgr
				buffData.arge = buff_arge
				buffData.action = buffAction
				buffAction:Start(buffData)
				table.insert(buffList, buffData)
				self.sceneMgr.actionMgr:AutoUpdate(buffAction)
			end
		end
	},
}
return Buff