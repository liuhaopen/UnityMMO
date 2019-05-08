ECS.TypeManager.RegisterType("umo.monster_ai", {ai_id="integer"})
ECS.TypeManager.RegisterType("umo.monster_state", {state="integer", sub_state="integer"})
local Time = require "game.scene.time"

local monster_const = require "game.scene.monster_const"


local monster_ai_system = BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("umo.monster_ai_system", monster_ai_system)

function monster_ai_system:Constructor(  )
end

function monster_ai_system:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

	local blueprint_register = require "game.scene.ai.blueprint_register"
	--注册所有的蓝图类
	blueprint_register:register_all()
	self.monster_mgr = require "game.scene.monster_mgr"
	self.monster_mgr = self.monster_mgr:getInstance()
	
	self.group = self:GetComponentGroup({"umo.monster_ai", "umo.uid"})
end

function monster_ai_system:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local entities = self.group:GetEntityArray()
	local uids = self.group:GetComponentDataArray("umo.uid")
	for i=1,uids.Length do
		local graphsowner = self.monster_mgr:get_graphs_owner(uids[i].value)
		graphsowner:Update(deltaTime)
	end
end

return monster_ai_system