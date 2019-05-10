local ECS = require "ECS"
local monster_const = require "game.scene.monster_const"

local AISystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.AISystem", AISystem)

function AISystem:Constructor(  )
end

function AISystem:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

	local blueprint_register = require "game.scene.ai.blueprint_register"
	--注册所有的蓝图类
	blueprint_register:register_all()
	self.monster_mgr = require "game.scene.monster_mgr"
	self.monster_mgr = self.monster_mgr:getInstance()
	
	self.group = self:GetComponentGroup({"umo.monster_ai", "umo.uid"})
end

function AISystem:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local entities = self.group:GetEntityArray()
	local uids = self.group:GetComponentDataArray("umo.uid")
	for i=1,uids.Length do
		local graphsowner = self.monster_mgr:get_graphs_owner(uids[i].value)
		graphsowner:Update(deltaTime)
	end
end

return AISystem