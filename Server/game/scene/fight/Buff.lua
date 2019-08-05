local Ac = require "Action"
local BuffActions = require("game.scene.fight.BuffActions")

local Buff = Ac.OO.Class {
	type 	= "Buff",
	__index = {
		Start = function(self, skillData)
			self.skillData = skillData
		end,
		IsDone = function(self)
			return true
		end,
		Update = function(self, deltaTime)
			local sceneMgr = self.skillData.sceneMgr
			local entityMgr = sceneMgr.entityMgr
			local buff_id = self[1]
			local buff_arge = self[2]
			for k,uid in pairs(self.skillData.targets) do
				local buffActionCreator = BuffActions:GetActionCreator(buff_id)
				local buffAction = buffActionCreator(buff_arge)
				local targetEntity = sceneMgr:GetEntity(uid)
				local buffList = entityMgr:GetComponentData(targetEntity, "UMO.Buff")
				local buffData = TablePool:Get("BuffComData") or {}
				buffData.buff_id = buff_id
				buffData.target_uid = uid
				buffData.come_from_uid = self.skillData.caster_uid
				buffData.arge = buff_arge
				buffData.action = buffAction
				table.insert(buffList, buffData)
			end
		end
	},
}
return Buff