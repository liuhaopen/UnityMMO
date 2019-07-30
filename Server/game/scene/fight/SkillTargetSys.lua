--用于决定技能的攻击目标
local ECS = require "ECS"
local skynet = require "skynet"
local SkillTargetFilterSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.SkillTargetFilterSystem", SkillTargetFilterSystem)

function SkillTargetFilterSystem:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)
	self.fightMgr = self.sceneMgr.fightMgr

	self.group = self:GetComponentGroup({"UMO.Skill"})
end

function SkillTargetFilterSystem:OnUpdate(  )
	local skills = self.group:ToComponentDataArray("UMO.Skill")
	local entities = self.group:ToEntityArray()
	for i=1,skills.Length do
		
	end
end

return SkillTargetFilterSystem
