ECS.TypeManager.RegisterType("umo.monster_nest", {type_id="integer", radius="integer"})

local monster_nest_system = BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("umo.monster_nest_system", monster_nest_system)
--怪物巢穴系统，负责生成怪物
function monster_nest_system:Constructor(  )
	local data = {
		position = "Array:UMO.Position",--当前坐标
		length   = "Length",
	}
	-- self.group = {}
	self:Inject("m_data", data)
end

function monster_nest_system:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

	self.group = self:GetComponentGroup({"UMO.Position"})
	print('Cat:monster_module.lua[19] self.group', self.group)
end

function monster_nest_system:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local positions = self.group:GetComponentDataArray("UMO.Position")
	for i=1,positions do
		local last_pos = self.m_data.position[i]

	end
end

return monster_nest_system