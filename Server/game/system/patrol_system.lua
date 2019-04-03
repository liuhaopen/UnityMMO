local patrol_system = BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("umo.patrol_system", patrol_system)

function patrol_system:Constructor(  )
	local data = {
		position = "Array:UMO.Position",--当前坐标
		-- born_pos = "Array:UMO.BornPosition:ReadOnly",--出生点
		-- speed = "Array:UMO.MoveSpeed:ReadOnly",--速度
		-- radius = "Array:UMO.PatrolRadius:ReadOnly",--巡逻半径
		-- direction = "Array:UMO.Direction",--方向
		length   = "Length",
	}
	-- self.group = {}
	self:Inject("m_data", data)
end

--Cat_Todo : 使用RecastNavigation读取地形信息，不然发给前端的y坐标肯定对不上地图的
function patrol_system:OnUpdate(  )
	local deltaTime = Time.deltaTime
	-- print('Cat:patrol_system.lua[OnUpdate]', deltaTime)
	for i=1,self.m_data.length do
		local last_pos = self.m_data.position[i]
		-- print("Cat:patrol_system [start:23] last_pos:", last_pos)
		-- PrintTable(last_pos)
		-- print("Cat:patrol_system [end]")
		
		-- self.m_data.position:set(i, last_pos)
		-- local last_pos_x = self.m_data.position:get_field(i, "x")
		-- local last_pos_y = self.m_data.position:get_field(i, "y")
		-- local last_pos_z = self.m_data.position:get_field(i, "z")
		-- local last_dir_x = self.m_data.direction:get_field(i, "x")
		-- local last_dir_y = self.m_data.direction:get_field(i, "y")
		-- local last_dir_z = self.m_data.direction:get_field(i, "z")
		-- local speed = self.m_data.speed:get_field(i, "speed")
		-- local new_pos_x = last_pos_x+last_dir_x*speed*deltaTime
		-- local new_pos_y = last_pos_y+last_dir_y*speed*deltaTime
		-- local new_pos_z = last_pos_z+last_dir_z*speed*deltaTime
		-- self.m_data.position:set(i, "x", new_pos_x)
		-- self.m_data.position:set(i, "y", new_pos_y)
		-- self.m_data.position:set(i, "z", new_pos_z)
	end
end

return patrol_system