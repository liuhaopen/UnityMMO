ECS.TypeManager.RegisterType("umo.monster_ai", {ai_id="integer"})
ECS.TypeManager.RegisterType("umo.monster_state", {state="integer", sub_state="integer"})

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
	-- self.state_func = {
	-- 	[monster_const.monster_state.patrol] = UpdatePatrol,
	-- }
	self.group = self:GetComponentGroup({"umo.monster_ai", "umo.uid"})
end

function monster_ai_system:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local entities = self.group:GetEntityArray()
	local uids = self.group:GetComponentDataArray("umo.uid")
	print('Cat:monster_module.lua[25] #uids', #uids)
	for i=1,#uids do
		local graphsowner = self.monster_mgr:get_graphs_owner(uids[i].value)
		graphsowner:Update(deltaTime)
		-- local state_data = uids[i]
		-- local func = self.state_func[state_data.state]
		-- if func then
		-- 	func(self, entities[i], state_data.sub_state)
		-- end
	end
end

-- function monster_ai_system:UpdatePatrol( entity, sub_state )
-- 	if sub_state == monster_const.monster_sub_state.update then
-- 		--[[巡逻还分几个子状态： 
-- 			idle-站着看来看去,
-- 			walk-前往下个盯哨点
-- 		]]--

-- 		--主动打人的怪需要经常判断附近有没人
		
-- 	elseif sub_state == monster_const.monster_sub_state.enter then
-- 	elseif sub_state == monster_const.monster_sub_state.leave then
-- 	end
-- end

return monster_ai_system