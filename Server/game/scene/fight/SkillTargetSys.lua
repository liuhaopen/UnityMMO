--calculate targets for skill
local ECS = require "ECS"
local skynet = require "skynet"
local SkillTargetSys = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.SkillTargetSys", SkillTargetSys)

function SkillTargetSys:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)
	self.fightMgr = self.sceneMgr.fightMgr

	self.group = self:GetComponentGroup({"UMO.Skill"})
end

function SkillTargetSys:OnUpdate(  )
	local skills = self.group:ToComponentDataArray("UMO.Skill")
	local entities = self.group:ToEntityArray()
	for i=1,skills.Length do
		
	end
end

return SkillTargetSys
