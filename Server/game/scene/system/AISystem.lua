local ECS = require "ECS"

local AISystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.AISystem", AISystem)

function AISystem:Constructor(  )
end

function AISystem:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)

	self.monsterMgr = self.sceneMgr.monsterMgr
	
	self.group = self:GetComponentGroup({"UMO.MonsterAI", "UMO.UID"})
end

function AISystem:OnUpdate(  )
	local deltaTime = Time.deltaTime
	-- local entities = self.group:ToEntityArray()
	local uids = self.group:ToComponentDataArray("UMO.UID")
	for i=1,uids.Length do
		local graphsowner = self.monsterMgr:GetGraphsOwner(uids[i])
		graphsowner:Update(deltaTime)
	end
end

return AISystem