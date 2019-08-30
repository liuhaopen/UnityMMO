local Ac = require "Action"
local skill_cfg = require "game.config.scene.config_skill"
local FightHelper = require("game.scene.FightHelper")
local SkillActions = require("game.scene.fight.SkillActions")
local BuffActions = require("game.scene.fight.BuffActions")
local FightMgr = BaseClass()

function FightMgr:Init( scene )
	self.sceneMgr = scene
	self.entityMgr = scene.entityMgr
	self.aoi = scene.aoi
	self:InitArchetype()
	SkillActions:Init()
	BuffActions:Init()
end

function FightMgr:InitArchetype(  )
	self.skillArchetype = self.entityMgr:CreateArchetype({
		"UMO.Skill"
	})
end

function FightMgr:CastSkill( uid, req_data )
	--检查施法者状态（技能CD,是否麻痹、中毒、加班等）
	-- local role_info = self.sceneMgr.roleMgr.roleList[user_info.cur_role_id]
	-- local sceneObj = self.sceneMgr:GetSceneObjByUID(uid)
	local cfg = skill_cfg[req_data.skill_id]
	local skillLv = 1--Cat_Todo : get role skill level
	local skillCfg = cfg and cfg.detail[skillLv]
	if not cfg or not skillCfg then 
		return ErrorCode.SkillCfgNotFind
	end

	local entity = self.sceneMgr:GetEntity(uid)
	if not entity then 
		return ErrorCode.UIDErrorOnCastSkill
	end
	
	local aoi_handle = self.entityMgr:GetComponentData(entity, "UMO.AOIHandle")
	local isSkillInCD = FightHelper:IsSkillInCD(entity, req_data.skill_id)
	-- print('Cat:FightMgr.lua[21] isSkillInCD', isSkillInCD)
	local is_can_cast = not isSkillInCD
	local cdEndTime = 0
	local fight_event = nil
	local errorCode = ErrorCode.SkillCastFail
	if is_can_cast then
		-- local skillEntity = self.entityMgr:CreateEntityByArcheType(self.skillArchetype)
		local skillData = TablePool:Get("SkillComData") or {}
		skillData.caster_uid = uid
		skillData.caster_entity = entity
		skillData.cast_time = Time.timeMS
		skillData.skill_id = req_data.skill_id
		skillData.skill_lv = skillLv
		skillData.target_pos_x = req_data.target_pos_x
		skillData.target_pos_y = req_data.target_pos_y
		skillData.target_pos_z = req_data.target_pos_z
		skillData.direction = req_data.direction
		skillData.targets = req_data.targets
		skillData.sceneMgr = self.sceneMgr
		skillData.cfg = cfg
		-- skillData.max_target_num = skillCfg.attack_max_num --每次选目标时再取配置，加了相关的buff后再改此字段
		local skillActionCreator = SkillActions:GetActionCreator(req_data.skill_id)
		local skillAction = skillActionCreator(skillCfg)
		skillAction:Start(skillData)
		self.sceneMgr.actionMgr:AutoUpdate(skillAction)
		-- skillData.action = skillAction
		-- self.entityMgr:SetComponentData(skillEntity, "UMO.Skill", skillData)
		-- print('Cat:FightMgr.lua[64] skillEntity', skillEntity, req_data.skill_id)
		cdEndTime = FightHelper:ApplySkillCD(entity, req_data.skill_id, skillLv)

		errorCode = ErrorCode.Succeed
		fight_event = {
			attacker_uid = uid,
			skill_id = req_data.skill_id,
			skill_lv = skillLv,
			attacker_pos_x = req_data.cur_pos_x,--Cat_Todo : 记得做校验
			attacker_pos_y = req_data.cur_pos_y,
			attacker_pos_z = req_data.cur_pos_z,
			target_pos_x = req_data.target_pos_x,
			target_pos_y = req_data.target_pos_y,
			target_pos_z = req_data.target_pos_z,
			direction = req_data.direction,
			time = Time.timeMS,
			flag = 0,
		}
		self.sceneMgr.eventMgr:AddSkillEvent(uid, fight_event)
		-- local uid_defenders_map = req_data.uid_defenders_map or self:CalDefenderList(fight_event, aoi_handle)
		-- self:AddDamageEventForDefenders(fight_event, uid_defenders_map)
	elseif isSkillInCD then
		errorCode = ErrorCode.SkillInCD
	end
	-- print('Cat:FightMgr.lua[cast skill] errorCode', errorCode)
	return errorCode, cdEndTime, fight_event
end

--计算受击者列表
function FightMgr:CalDefenderList( fight_info, attacker_aoi_handle )
	local cfg = skill_cfg[fight_info.skill_id]
	if not cfg or not attacker_aoi_handle then return end
	
	local skill_bomb = self.aoi:add()
	self.aoi:set_pos(skill_bomb, fight_info.target_pos_x, fight_info.target_pos_y, fight_info.target_pos_z)

	local area = cfg.detail[fight_info.skill_lv].area
	local around = self.aoi:get_around_offset(skill_bomb, area, area)
	local uid_defenders_map
	if around then
		uid_defenders_map = {}
		around[attacker_aoi_handle] = nil--把攻击者自己去掉
		for aoi_handle,v in pairs(around) do
			local uid = self.aoi:get_user_data(aoi_handle, "uid")
			local entity = self.sceneMgr:GetEntity(uid)
			local isBeatable = self.entityMgr:HasComponent(entity, "UMO.Beatable")
			if isBeatable then
				local hpData = self.entityMgr:GetComponentData(entity, "UMO.HP")
				if hpData.cur > 0 then
					uid_defenders_map[uid] = true
				end
			end
		end
	end
	self.aoi:remove(skill_bomb)
	return uid_defenders_map
end

function FightMgr:AddDamageEventForDefenders( fight_event, uid_defenders_map )
	if not uid_defenders_map then return end
	fight_event.defenders = {}
	for uid,v in pairs(uid_defenders_map) do
		-- for aoi_handle,v in pairs(around) do
		-- local uid = self.aoi:get_user_data(aoi_handle, "uid")
		local entity = self.sceneMgr:GetEntity(uid)
		if entity then
			local hp = self.entityMgr:GetComponentData(entity, "UMO.HP")
			local damage_value = self:CalDamage(fight_info, entity)
			table.insert(fight_event.defenders, {uid=uid, cur_hp=hp.cur, damage=damage_value, flag=math.random(0, 2)})
			local pos = self.entityMgr:GetComponentData(entity, "UMO.Position")
			local dEvents = self.entityMgr:GetComponentData(entity, "UMO.DamageEvents")
			-- attacker攻击者，damage伤害值，direction攻击方向，impulse推力
			local direction = Vector3.Sub(Vector3(fight_event.attacker_pos_x, fight_event.attacker_pos_y, fight_event.attacker_pos_z), pos)
			table.insert(dEvents, {attacker=fight_event.attacker_uid, damage=damage_value, direction=direction, impulse=0})
		end
	end
end

function FightMgr:CalDamage( fight_info, entity )
	return math.random(100, 550)
end

return FightMgr