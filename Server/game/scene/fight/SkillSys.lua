--handle skill lifetime
local ECS = require "ECS"
local skynet = require "skynet"
local SkillCfg = require "game.config.scene.config_skill"
local SkillSys = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.SkillSys", SkillSys)

function SkillSys:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)
	-- self.fightMgr = self.sceneMgr.fightMgr
	self.aoi = self.sceneMgr.aoi
	self.group = self:GetComponentGroup({"UMO.Skill"})
end

function SkillSys:OnUpdate(  )
	local skills = self.group:ToComponentDataArray("UMO.Skill")
	local entities = self.group:ToEntityArray()
	for i=1,skills.Length do
		self:HandleSkill(skills[i], entities[i])
	end
end

function SkillSys:HandleSkill( skillData, entity )
	print('Cat:SkillSys.lua[23] entity, skillData', entity, skillData)
	--选定目标
	local uid_defenders_map = self:CalTargetList(skillData)
	--产生效果：生成buff和（一次性或定时扣血）

	-- HP低于X%时，N秒内持续恢复Y%的HP，冷却M秒
	--[[时机：概率或某条件
	condition = {
		{"random",10000},
	},
	--mp大于1000点且hp小于万分之10
	condition = {
		{"attr","mp",">=",1000},
		{"attr","hp","<=",10,"%"},
	},
	--5秒内被打10次或hp少了10%
	--]]
end

--计算受击者列表
function SkillSys:CalTargetList( skillData )
	local attacker_aoi_handle = self.EntityManager:GetComponentData(skillData.caster_entity, "UMO.AOIHandle")
	local cfg = SkillCfg[skillData.skill_id]
	if not cfg or not attacker_aoi_handle then return end
	
	local uid_defenders_map
	if cfg.target_type == SceneConst.SkillTargetType.Enemy or cfg.target_type == SceneConst.SkillTargetType.Our then
		local skill_bomb = self.aoi:add()
		self.aoi:set_pos(skill_bomb, skillData.target_pos_x, skillData.target_pos_y, skillData.target_pos_z)

		local area = cfg.detail[skillData.skill_lv].area
		local around = self.aoi:get_around_offset(skill_bomb, area, area)
		if around then
			uid_defenders_map = {}
			--Cat_Todo : 区分我方和敌方阵营
			around[attacker_aoi_handle] = nil--把攻击者自己去掉
			for aoi_handle,v in pairs(around) do
				local uid = self.aoi:get_user_data(aoi_handle, "uid")
				local entity = self.sceneMgr:GetEntity(uid)
				local isBeatable = self.EntityManager:HasComponent(entity, "UMO.Beatable")
				if isBeatable then
					local hpData = self.EntityManager:GetComponentData(entity, "UMO.HP")
					if hpData.cur > 0 then
						uid_defenders_map[uid] = true
					end
				end
			end
		end
		self.aoi:remove(skill_bomb)
	elseif cfg.target_type == SceneConst.SkillTargetType.Me then
		uid_defenders_map[skillData.caster_uid] = true
	end
	return uid_defenders_map
end

return SkillSys
