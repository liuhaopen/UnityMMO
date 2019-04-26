ECS.TypeManager.RegisterType("umo.monster_ai", {ai_id="integer"})
ECS.TypeManager.RegisterType("umo.monster_state", {state="integer", sub_state="integer"})

local monster_const = require "game.scene.monster_const"

local monster_ai_system = BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("umo.monster_ai_system", monster_ai_system)
--Cat_Todo : 有空再改成行为树实现吧，不过要先弄套lua版本的行为树
--怪物AI系统
function monster_ai_system:Constructor(  )
end

function monster_ai_system:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

	self.state_func = {
		[monster_const.monster_state.patrol] = UpdatePatrol,
	}
	self.group = self:GetComponentGroup({"umo.monster_ai", "umo.monster_state"})
end

function monster_ai_system:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local entities = self.group:GetEntityArray()
	local monster_states = self.group:GetComponentDataArray("umo.monster_state")
	print('Cat:monster_module.lua[25] #monster_states', #monster_states)
	for i=1,#monster_states do
		local state_data = monster_states[i]
		local func = self.state_func[state_data.state]
		if func then
			func(self, entities[i], state_data.sub_state)
		end
	end
end

function monster_ai_system:UpdatePatrol( entity, sub_state )
	if sub_state == monster_const.monster_sub_state.update then
		--[[巡逻还分几个子状态： 
			idle-站着看来看去,
			walk-前往下个盯哨点
		]]--

		--主动打人的怪需要经常判断附近有没人
		
	elseif sub_state == monster_const.monster_sub_state.enter then
	elseif sub_state == monster_const.monster_sub_state.leave then
	end
end

return monster_ai_system