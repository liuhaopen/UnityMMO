local Ac = require "Action"
local skynet = require "skynet"

local CalDamage = function( self, targetEntity )
	--Cat_Todo : attr
	local damage = self.baseDamage
	return math.random(100, 550)
end

local ApplyDamage = function( self, entity, hp, damage, attacker )
	if hp.cur <= 0 then return end
	hp.cur = hp.cur - damage
	if hp.cur <= 0 then
		hp.cur = 0
		hp.killedBy = attacker
		hp.deathTime = Time.time
	end
		
	local uid = self.entityMgr:GetComponentData(entity, "UMO.UID")
	-- local change_target_pos_event_info
	if hp.cur <= 0 then
		--enter dead state
		if self.entityMgr:HasComponent(entity, "UMO.MonsterAI") then
			self.sceneMgr.monsterMgr:TriggerState(uid, "DeadState")
			local killer = self.sceneMgr:GetEntity(hp.killedBy)
			if self.entityMgr:HasComponent(killer, "UMO.MsgAgent") then
				local agent = self.entityMgr:GetComponentData(killer, "UMO.MsgAgent")
				local roleID = self.entityMgr:GetComponentData(killer, "UMO.TypeID")
				local monsterID = self.entityMgr:GetComponentData(entity, "UMO.TypeID")
				skynet.send(agent, "lua", "execute", "Task", "KillMonster", roleID, monsterID, 1)
			end
		end
	end
end

local Update = function ( self )
	local hurtEvent = {
		attacker_uid = self.skillData.caster_uid,
		time = Time.timeMS,
		defenders = {},
	}
	for uid,_ in pairs(self.skillData.targets) do
		local entity = self.sceneMgr:GetEntity(uid)
		if entity then
			local hp = self.entityMgr:GetComponentData(entity, "UMO.HP")
			local damage_value = CalDamage(self, entity)
			ApplyDamage(self, entity, hp, damage_value, self.skillData.caster_uid)
			table.insert(hurtEvent.defenders, {uid=uid, cur_hp=hp.cur, change_num=damage_value, flag=math.random(0, 2)})
			--Cat_Todo : break other's skill
		end
	end
	if hurtEvent.defenders and #hurtEvent.defenders > 0 then
		self.sceneMgr.eventMgr:AddHurtEvent(self.skillData.caster_uid, hurtEvent)
	end
end

local Hurt = Ac.OO.Class {
	type 	= "Hurt",
	__index = {
		Start = function(self, skillData)
			self.skillData = skillData
			self.sceneMgr = skillData.sceneMgr
			self.entityMgr = self.sceneMgr.entityMgr
			self.baseDamage = self[1] or self.skillData.cfg.detail[self.skillData.skill_lv].damage_rate
		end,
		IsDone = function(self)
			return true
		end,
		Update = Update,
	},
}

return Hurt