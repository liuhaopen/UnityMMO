local ECS = require "ECS"
local SceneConst = require "game.scene.SceneConst"
local Time = Time
local DamageSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.DamageSystem", DamageSystem)

function DamageSystem:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)
	self.fightMgr = self.sceneMgr.fightMgr

	self.group = self:GetComponentGroup({"UMO.DamageEvents", "UMO.AOIHandle", "UMO.HP", "UMO.DeadState"})
end

function DamageSystem:OnUpdate(  )
	local damages = self.group:GetComponentDataArray("UMO.DamageEvents")
	local hps = self.group:GetComponentDataArray("UMO.HP")
	local entities = self.group:GetEntityArray()
	for i=1,damages.Length do
		self:HandleDamage(entities[i], damages[i], hps[i])
	end
end

function DamageSystem:HandleDamage( entity, dEvents, hp )
	if not dEvents or #dEvents <= 0 or hp.cur <= 0 then
		return
	end
	local isDamaged = false
	local sumDamage = 0
	for i,damageEvent in ipairs(dEvents) do
		isDamaged = true
		if hp.cur > 0 then
			hp.cur = hp.cur - damageEvent.damage
			if hp.cur <= 0 then
				hp.cur = 0
				hp.killedBy = damageEvent.attacker
				hp.deathTime = Time.time
			end
		end
		sumDamage = sumDamage + damageEvent.damage
		dEvents[i] = nil--逐个置nil，不需要重新分配table
	end
	if isDamaged then
		local uid = self.m_EntityManager:GetComponentData(entity, "UMO.UID")
		local change_target_pos_event_info
		if hp.cur > 0 then
			change_target_pos_event_info = {key=SceneConst.InfoKey.HPChange, value=math.floor(hp.cur), time=Time.timeMS}
		else
			--enter dead state
			self.m_EntityManager:SetComponentData(entity, "UMO.DeadState", 1)
			if self.m_EntityManager:HasComponent(entity, "UMO.MonsterAI") then
				self.sceneMgr.monsterMgr:TriggerState(uid, "Dead")
			end
			change_target_pos_event_info = {key=SceneConst.InfoKey.HPChange, value=math.floor(hp.cur), time=Time.timeMS}
		end
		self.sceneMgr.eventMgr:AddSceneEvent(uid, change_target_pos_event_info)
	end
end

return DamageSystem