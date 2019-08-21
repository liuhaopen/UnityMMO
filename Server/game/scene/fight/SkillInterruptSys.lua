--处理技能的打断逻辑
--handle skill lifetime
local ECS = require "ECS"
local skynet = require "skynet"
local SkillInterruptSys = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.SkillInterruptSys", SkillInterruptSys)

function SkillInterruptSys:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)
	self.fightMgr = self.sceneMgr.fightMgr

	self.group = self:GetComponentGroup({"UMO.Skill"})
end

function SkillInterruptSys:OnUpdate(  )
	local skills = self.group:ToComponentDataArray("UMO.Skill")
	local entities = self.group:ToEntityArray()
	for i=1,skills.Length do
		self:HandleSkill(entities[i], skills[i])
	end
end

function SkillInterruptSys:HandleSkill( entity, skill )
	print('Cat:SkillInterruptSys.lua[23] entity, skill', entity, skill)

	--打断比自己后发且正在可打断期间的技能

	--如果技能被打断的话不算CD
	
end

return SkillInterruptSys
