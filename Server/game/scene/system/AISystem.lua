local ECS = require "ECS"

local AISystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.AISystem", AISystem)

function AISystem:Constructor(  )
end

function AISystem:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

	self.monsterMgr = self.sceneMgr.monsterMgr
	
	self.group = self:GetComponentGroup({"UMO.MonsterAI", "UMO.UID"})
end

function AISystem:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local entities = self.group:GetEntityArray()
	local uids = self.group:GetComponentDataArray("UMO.UID")
	for i=1,uids.Length do
		local graphsowner = self.monsterMgr:get_graphs_owner(uids[i].value)
		graphsowner:Update(deltaTime)
	end
end

return AISystem