local ECS = require "ECS"
local skynet = require "skynet"
local skill_cfg = require "game.config.scene.config_skill"
local math_random = math.random 
local FightHelper = {}

function FightHelper:Init( sceneMgr )
	self.sceneMgr = sceneMgr
	self.entityMgr = sceneMgr.entityMgr
end

local randomAddOrMinus = function (  )
	local isAdd = math_random(1,2)==1
	return isAdd and 1 or -1
end

--获取可攻击的坐标	
function FightHelper:GetAssailablePos( curPos, targetPos, minDistance, maxDistance )
	local xAddOrMinus = randomAddOrMinus()
	local xRandomDis = math_random(0, (maxDistance-minDistance))
	local pos_x = targetPos.x + minDistance*xAddOrMinus + xRandomDis*xAddOrMinus
	local pos_y = targetPos.y
	local zAddOrMinus = randomAddOrMinus()
	local zRandomDis = math_random(0, (maxDistance-minDistance))
	local pos_z = targetPos.z + minDistance*zAddOrMinus + zRandomDis*zAddOrMinus
	local newPos = {x=pos_x, y=pos_y, z=pos_z}
	return newPos
end

function FightHelper:IsSkillInCD( entity, skillID )
	local hasCD = self.entityMgr:HasComponent(entity, "UMO.CD")
	if hasCD then
		local cdData = self.entityMgr:GetComponentData(entity, "UMO.CD")
		local cdEndTime = cdData[skillID]
		return cdEndTime and Time.timeMS <= cdEndTime
	end
	return false
end

function FightHelper:GetSkillCD( skillID, lv )
	lv = lv or 1
	local cfg = skill_cfg[skillID]
	if cfg and cfg.detail[lv] then
		return cfg.detail[lv].cd
	end
	return 0
end

function FightHelper:ApplySkillCD( entity, skillID, lv )
	local hasCD = self.entityMgr:HasComponent(entity, "UMO.CD")
	local endTime = 0
	if hasCD then
		local cdData = self.entityMgr:GetComponentData(entity, "UMO.CD")
		local cd = self:GetSkillCD(skillID, lv)
		endTime = Time.timeMS + cd
		cdData[skillID] = endTime
	end
	return endTime
end

function FightHelper:IsLive( entity )
	if entity and self.entityMgr:Exists(entity) and self.entityMgr:HasComponent(entity, "UMO.HP") then
		local hpData = self.entityMgr:GetComponentData(entity, "UMO.HP")
		return hpData.cur > 0
	end
	return false
end

function FightHelper:ChangeHP( entity, hp, offsetValue, attacker )
	if hp.cur <= 0 then return end
	hp.cur = hp.cur - offsetValue
	if hp.cur <= 0 then
		hp.cur = 0
		hp.killedBy = attacker
		hp.deathTime = Time.time
	end
	local uid = self.entityMgr:GetComponentData(entity, "UMO.UID")
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

function FightHelper:ChangeSpeed( entity, victim_uid, caster_uid, bodName, isSet, speed )
	if not entity then return end
	local isExist = self.entityMgr:Exists(entity)
	if not isExist then return end
	local speedData = self.entityMgr:GetComponentData(entity, "UMO.MoveSpeed")
	speedData:ChangeSpeed(bodName, isSet, speed)
	local buffEvent = {
		key = SceneConst.InfoKey.Speed, 
		value = string.format("%s,%s,%s,%s", bodName, isSet and 1 or 0, speed and math.floor(speed) or 0, caster_uid),
	}
	self.sceneMgr.eventMgr:AddSceneEvent(victim_uid, buffEvent)
end

function FightHelper:ChangeTargetPos( entity, pos )
	local speed = self.entityMgr:GetComponentData(entity, "UMO.MoveSpeed")
	if speed.curSpeed <= 0 then
		return
	end
	self.entityMgr:SetComponentData(entity, "UMO.TargetPos", pos)
	local uid = self.entityMgr:GetComponentData(entity, "UMO.UID")
	local change_target_pos_event_info = {key=SceneConst.InfoKey.TargetPos, value=math.floor(pos.x)..","..math.floor(pos.z), time=Time.timeMS}
	self.sceneMgr.eventMgr:AddSceneEvent(uid, change_target_pos_event_info)
end

return FightHelper