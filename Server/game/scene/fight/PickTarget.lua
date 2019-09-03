local Ac = require "Action"
local ECS = require "ECS"
local SkillCfg = require "game.config.scene.config_skill"
local SceneConst = require "game.scene.SceneConst"

--计算受击者列表
local Update = function ( self )
	local cfg = SkillCfg[self.skillData.skill_id]
	if not cfg then return end
	
	local target_type = self[1] or cfg.target_type
	local maxTargetNum = self[2] or cfg.detail[self.skillData.skill_lv].attack_max_num
	self.skillData.targets = nil
	local isPickEnemy = target_type == SceneConst.SkillTargetType.Enemy
	if isPickEnemy or target_type == SceneConst.SkillTargetType.Our then
		local skill_bomb = self.aoi:add()
		self.aoi:set_pos(skill_bomb, self.skillData.target_pos_x, self.skillData.target_pos_y, self.skillData.target_pos_z)

		local area = cfg.detail[self.skillData.skill_lv].area
		local around = self.aoi:get_around_offset(skill_bomb, area, area)
		if around then
			self.skillData.targets = {}
			--Cat_Todo : 区分我方和敌方阵营
			if isPickEnemy then
				local isExist = self.EntityManager:Exists(self.skillData.caster_entity)
				if isExist then
					local attacker_aoi_handle = self.EntityManager:GetComponentData(self.skillData.caster_entity, "UMO.AOIHandle")
					around[attacker_aoi_handle.value] = nil--把攻击者自己去掉
				else
					print("caster entity unexist on pick target "..tostring(self.skillData.caster_entity).." skill id : "..self.skillData.skill_id)
				end
			end
			local targetNums = 0
			for aoi_handle,v in pairs(around) do
				local uid = self.aoi:get_user_data(aoi_handle, "uid")
				local entity = self.sceneMgr:GetEntity(uid)
				local isBeatable = self.EntityManager:HasComponent(entity, "UMO.Beatable")
				-- print('Cat:PickTarget.lua[36] uid, entity, isBeatable', uid, entity, isBeatable)
				if isBeatable then
					local hpData = self.EntityManager:GetComponentData(entity, "UMO.HP")
					if hpData.cur > 0 then
						self.skillData.targets[uid] = true
						targetNums = targetNums + 1
						if targetNums >= maxTargetNum then
							break
						end
					end
				end
			end
		end
		self.aoi:remove(skill_bomb)
	elseif target_type == SceneConst.SkillTargetType.Me then
		self.skillData.targets[self.skillData.caster_uid] = true
	end
	--Cat_Todo : 控制最大受击数量
end

local PickTarget = Ac.OO.Class {
	type 	= "PickTarget",
	__index = {
		Start = function(self, skillData)
			self.skillData = skillData
			self.sceneMgr = skillData.sceneMgr
			self.EntityManager = self.sceneMgr.entityMgr
			self.aoi = self.sceneMgr.aoi
		end,
		IsDone = function(self)
			return true
		end,
		Update = Update,
	},
}

return PickTarget