local Ac = require "Action"

local CalDamage = function ( self )
	--Cat_Todo : attr
	return math.random(100, 550)
end

local Update = function ( self )
	local hurtEvent = {
		attacker_uid = uid,
		defenders = {},
	}
	for uid,_ in pairs(self.skillData.targets) do
		local entity = self.sceneMgr:GetEntity(uid)
		if entity then
			local hp = self.entityMgr:GetComponentData(entity, "UMO.HP")
			local damage_value = CalDamage(self, entity)
			table.insert(hurtEvent.defenders, {uid=uid, cur_hp=hp.cur, change_num=damage_value, flag=math.random(0, 2)})
			-- local pos = self.entityMgr:GetComponentData(entity, "UMO.Position")
			-- local dEvents = self.entityMgr:GetComponentData(entity, "UMO.DamageEvents")
			-- attacker攻击者，damage伤害值，direction攻击方向，impulse推力
			-- local direction = Vector3.Sub(Vector3(self.skillData.attacker_pos_x, self.skillData.attacker_pos_y, self.skillData.attacker_pos_z), pos)
			-- table.insert(dEvents, {attacker=self.skillData.attacker_uid, damage=damage_value, direction=direction, impulse=0})
			--Cat_Todo : break other's skill
		end
	end
	self.sceneMgr.eventMgr:AddHurtEvent(self.skillData.caster_uid, hurtEvent)
end

local Hurt = Ac.OO.Class {
	type 	= "Hurt",
	__index = {
		Start = function(self, skillData)
			self.skillData = skillData
			self.sceneMgr = skillData.sceneMgr
			self.entityMgr = self.sceneMgr.entityMgr
		end,
		IsDone = function(self)
			return true
		end,
		Update = Update,
	},
}

return Hurt