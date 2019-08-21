local ECS = require "ECS"
local skynet = require "skynet"
local SceneConst = require "game.scene.SceneConst"
local Time = Time
local DamageSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.DamageSystem", DamageSystem)

function DamageSystem:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)
	self.fightMgr = self.sceneMgr.fightMgr

	self.group = self:GetComponentGroup({"UMO.DamageEvents", "UMO.AOIHandle", "UMO.HP"})
end

function DamageSystem:OnUpdate(  )
	local damages = self.group:ToComponentDataArray("UMO.DamageEvents")
	local hps = self.group:ToComponentDataArray("UMO.HP")
	local entities = self.group:ToEntityArray()
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
			if self.m_EntityManager:HasComponent(entity, "UMO.MonsterAI") then
				self.sceneMgr.monsterMgr:TriggerState(uid, "DeadState")
				local killer = self.sceneMgr:GetEntity(hp.killedBy)
				if self.m_EntityManager:HasComponent(killer, "UMO.MsgAgent") then
					local agent = self.m_EntityManager:GetComponentData(killer, "UMO.MsgAgent")
					local roleID = self.m_EntityManager:GetComponentData(killer, "UMO.TypeID")
					local monsterID = self.m_EntityManager:GetComponentData(entity, "UMO.TypeID")
					skynet.send(agent, "lua", "execute", "Task", "KillMonster", roleID, monsterID, 1)
				end
			end
			change_target_pos_event_info = {key=SceneConst.InfoKey.HPChange, value=math.floor(hp.cur)..",dead", time=Time.timeMS}
		end
		self.sceneMgr.eventMgr:AddSceneEvent(uid, change_target_pos_event_info)
	end
end

return DamageSystem