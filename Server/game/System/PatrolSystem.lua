local PatrolSystem = BaseClass(ECS.ComponentSystem)
ECS.Systems:Register(PatrolSystem, "UMMO.PatrolSystem")
PatrolSystem.UpdateAfter = {"LastSystem"}
PatrolSystem.UpdateBefore = {"FirstSystem"}
PatrolSystem.UpdateInGroup = {"GroupSystem"}

function PatrolSystem:Constructor(  )
	local data = {
		position = "ECS.Position:Array",--当前坐标
		born_pos = "UMMO.BornPosition:Array:ReadOnly",--出生点
		speed = "UMMO.MoveSpeed:Array:ReadOnly",--速度
		radius = "UMMO.PatrolRadius:Array:ReadOnly",--巡逻半径
		direction = "UMMO.Direction:Array",--方向
		length   = "Length",
	}
	self.group = {}
	self:Inject(self.group, data)
end

--Cat_Todo : 使用RecastNavigation读取地形信息，不然发给前端的y坐标肯定对不上地图的
function PatrolSystem:OnUpdate(  )
	local deltaTime = Time.deltaTime
	for i=1,self.group.length do
		-- local last_pos = self.group.position:get(i)
		local last_pos_x = self.group.position:get(i, "x")
		local last_pos_y = self.group.position:get(i, "y")
		local last_pos_z = self.group.position:get(i, "z")
		local last_dir_x = self.group.direction:get(i, "x")
		local last_dir_y = self.group.direction:get(i, "y")
		local last_dir_z = self.group.direction:get(i, "z")
		local speed = self.group.speed:get(i, "speed")
		local new_pos_x = last_pos_x+last_dir_x*speed*deltaTime
		local new_pos_y = last_pos_y+last_dir_y*speed*deltaTime
		local new_pos_z = last_pos_z+last_dir_z*speed*deltaTime
		self.group.position:set(i, "x", new_pos_x)
		self.group.position:set(i, "y", new_pos_y)
		self.group.position:set(i, "z", new_pos_z)
	end
end
