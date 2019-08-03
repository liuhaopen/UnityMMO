--此buff控制玩家发出的技能最大攻击数量，比如本来该技能最多攻击4个，有此buff就能攻击6个了
local ECS = require "ECS"
local skynet = require "skynet"
local SkillMaxTargetNumBuffSys = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.SkillMaxTargetNumBuffSys", SkillMaxTargetNumBuffSys)

function SkillMaxTargetNumBuffSys:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)
	self.fightMgr = self.sceneMgr.fightMgr

	self.group = self:GetComponentGroup({"UMO.Skill"})
end

function SkillMaxTargetNumBuffSys:OnUpdate(  )
	local skills = self.group:ToComponentDataArray("UMO.Skill")
	local entities = self.group:ToEntityArray()
	for i=1,skills.Length do
		
	end
end

return SkillMaxTargetNumBuffSys
