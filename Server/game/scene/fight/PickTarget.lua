local Ac = require "Action"
local ECS = require "ECS"
local SkillCfg = require "game.config.scene.config_skill"
local SceneConst = require "game.scene.SceneConst"

--计算受击者列表
local Update = function ( self )
	local attacker_aoi_handle = self.EntityManager:GetComponentData(skillData.caster_entity, "UMO.AOIHandle")
	local cfg = SkillCfg[skillData.skill_id]
	if not cfg or not attacker_aoi_handle then return end
	
	local target_type = self[1] or cfg.target_type
	self.skillData.targets = nil
	if cfg.target_type == SceneConst.SkillTargetType.Enemy or cfg.target_type == SceneConst.SkillTargetType.Our then
		local skill_bomb = self.aoi:add()
		self.aoi:set_pos(skill_bomb, skillData.target_pos_x, skillData.target_pos_y, skillData.target_pos_z)

		local area = cfg.detail[skillData.skill_lv].area
		local around = self.aoi:get_around_offset(skill_bomb, area, area)
		if around then
			self.skillData.targets = {}
			--Cat_Todo : 区分我方和敌方阵营
			around[attacker_aoi_handle] = nil--把攻击者自己去掉
			for aoi_handle,v in pairs(around) do
				local uid = self.aoi:get_user_data(aoi_handle, "uid")
				local entity = self.sceneMgr:GetEntity(uid)
				local isBeatable = self.EntityManager:HasComponent(entity, "UMO.Beatable")
				if isBeatable then
					local hpData = self.EntityManager:GetComponentData(entity, "UMO.HP")
					if hpData.cur > 0 then
						self.skillData.targets[uid] = true
					end
				end
			end
		end
		self.aoi:remove(skill_bomb)
	elseif cfg.target_type == SceneConst.SkillTargetType.Me then
		self.skillData.targets[skillData.caster_uid] = true
	end
	--Cat_Todo : 控制最大受击数量
end

local PickTarget = Ac.OO.Class {
	type 	= "PickTarget",
	__index = {
		Start = function(self, skillData)
			self.skillData = skillData
			self.EntityManager = skillData.sceneMgr.entityMgr
			self.aoi = skillData.sceneMgr.aoi
			print('Cat:PickTarget.lua[50] self.EntityManager', self.aoi, self.EntityManager)
		end,
		IsDone = function(self)
			return true
		end,
		Update = Update,
	},
}

return PickTarget