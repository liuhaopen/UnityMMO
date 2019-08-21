--handle skill lifetime
local ECS = require "ECS"
local skynet = require "skynet"
-- local SkillCfg = require "game.config.scene.config_skill"
local SkillSys = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.SkillSys", SkillSys)

function SkillSys:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)
	self.ecsSystemMgr = self.sceneMgr.ecsSystemMgr
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
	skillData.action:Update(Time.deltaTimeMS)
	local isDone = skillData.action:IsDone()
	if isDone then
		print('Cat:SkillSys.lua[29] isDone', isDone, entity)
		self.ecsSystemMgr:AddDestroyEntity(entity)
	end
end

return SkillSys
