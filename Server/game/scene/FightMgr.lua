local skill_cfg = require "game.config.scene.config_skill"

local FightMgr = BaseClass()

function FightMgr:Init( scene )
	self.sceneMgr = scene
	self.entityMgr = scene.entityMgr
	self.aoi = scene.aoi
	-- self.damage_events = {}
end

function FightMgr:CastSkill( uid, req_data )
	--检查施法者状态（技能CD,是否麻痹、中毒、加班等）
	-- local role_info = self.sceneMgr.roleMgr.roleList[user_info.cur_role_id]
	-- local sceneObj = self.sceneMgr:GetSceneObjByUID(uid)
	local entity = self.sceneMgr:GetEntity(uid)
	if not entity then return end
	
	local aoi_handle = self.entityMgr:GetComponentData(entity, "UMO.AOIHandle")
	local is_can_cast = true
	local fight_event = nil
	if is_can_cast then
		fight_event = {
			attacker_uid = uid,
			skill_id = req_data.skill_id,
			skill_lv = 1,
			attacker_pos_x = req_data.cur_pos_x,--Cat_Todo : 记得做校验
			attacker_pos_y = req_data.cur_pos_y,
			attacker_pos_z = req_data.cur_pos_z,
			target_pos_x = req_data.target_pos_x,
			target_pos_y = req_data.target_pos_y,
			target_pos_z = req_data.target_pos_z,
			direction = req_data.direction,
			time = Time.timeMS,
			defenders = nil,
		}
		local uid_defenders_map = req_data.uid_defenders_map or self:CalDefenderList(fight_event, aoi_handle)

		self:AddDamageEventForDefenders(fight_event, uid_defenders_map)

		self.sceneMgr.eventMgr:AddFightEvent(uid, fight_event)
	end
	return is_can_cast and 0 or 1, fight_event
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
			local hpData = self.entityMgr:GetComponentData(entity, "UMO.HP")
			local isBeatable = self.entityMgr:HasComponent(entity, "UMO.Beatable")
			if hpData.cur > 0 and isBeatable then
				uid_defenders_map[uid] = true
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