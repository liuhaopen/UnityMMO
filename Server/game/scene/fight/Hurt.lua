local Ac = require "Action"
local skynet = require "skynet"
local SceneConst = require "game.scene.SceneConst"
local FightHelper = require("game.scene.FightHelper")

local CalDamage = function( self, targetEntity )
	local targetFightAttr = self.entityMgr:GetComponentData(targetEntity, "UMO.FightAttr")
	local attackerAtt = self.attackerFightAttr[SceneConst.Attr.Att] or 0
	local defenderDef = targetFightAttr[SceneConst.Attr.Def] or 0
	local baseDamage = math.max(attackerAtt-defenderDef, 0)*self.damageRate/10000
	local attackerCrit = self.attackerFightAttr[SceneConst.Attr.Crit] or 0
	local critDamage = attackerCrit*math.random(1,100)/100
	return math.floor(baseDamage+critDamage)
end

local Update = function ( self )
	local hurtEvent = {
		attacker_uid = self.skillData.caster_uid,
		time = Time.timeMS,
		defenders = {},
	}
	local attackerEntity = self.sceneMgr:GetEntity(self.skillData.caster_uid)
	-- print('Cat:Hurt.lua[23] self.skillData.caster_uid, attackerEntity', self.skillData.caster_uid, attackerEntity)
	if not attackerEntity then 
		print('Cat:Hurt.lua attackerEntity no exist uid : ', self.skillData.caster_uid)
		return 
	end
	
	local isExist = self.entityMgr:Exists(attackerEntity)
	if not isExist then
		print('Cat:Hurt.lua attackerEntity no exist')
		return
	end
	self.attackerFightAttr = self.entityMgr:GetComponentData(attackerEntity, "UMO.FightAttr")

	for uid,_ in pairs(self.skillData.targets) do
		local entity = self.sceneMgr:GetEntity(uid)
		if entity then
			local hp = self.entityMgr:GetComponentData(entity, "UMO.HP")
			local damage_value = CalDamage(self, entity)
			FightHelper:ChangeHP(entity, hp, damage_value, self.skillData.caster_uid)
			table.insert(hurtEvent.defenders, {uid=uid, cur_hp=hp.cur, change_num=damage_value, flag=math.random(0, 2)})
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
			self.damageRate = self[1] or self.skillData.cfg.detail[self.skillData.skill_lv].damage_rate
		end,
		IsDone = function(self)
			return true
		end,
		Update = Update,
	},
}

return Hurt