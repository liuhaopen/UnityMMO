ECS.TypeManager.RegisterType("umo.monster_nest", {type_id="integer", radius="integer"})

local monster_nest_system = BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("umo.monster_nest_system", monster_nest_system)
--怪物巢穴系统，负责生成怪物
function monster_nest_system:Constructor(  )
end

function monster_nest_system:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

	-- self.group = self:GetComponentGroup({"umo.position", "umo.monster_nest"})
	-- print('Cat:monster_module.lua[19] self.group', self.group)
end

function monster_nest_system:OnUpdate(  )
	local deltaTime = Time.deltaTime
	-- local positions = self.group:GetComponentDataArray("umo.position")
	-- -- print('Cat:monster_module.lua[25] #positions', #positions)
	-- for i=1,#positions do
	-- 	local pos = positions[i]

	-- end
end

return monster_nest_system