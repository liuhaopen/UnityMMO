--检查身上是否有某种 buff
local Ac = require "Action"

local HasBuff = Ac.OO.Class {
	type 	= "HasBuff",
	Start = function(self, data)
		self.data = data
		self.entityMgr = data.sceneMgr.entityMgr
	end,
	__call = function(self)
		local targetEntity = self.data.victim_entity
		local buffList = self.entityMgr:GetComponentData(targetEntity, "UMO.Buff")
		local buff_id = self[1]
		for i,v in ipairs(buffList) do
			if v.buff_id == buff_id then
				return true
			end
		end
		return false
	end
}
return HasBuff