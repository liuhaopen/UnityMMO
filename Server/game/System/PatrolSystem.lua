local PatrolSystem = BaseClass(ECS.ComponentSystem)
ECS.Systems:Register(PatrolSystem, "UMMO.PatrolSystem")
PatrolSystem.UpdateAfter = {"LastSystem"}
PatrolSystem.UpdateBefore = {"FirstSystem"}
PatrolSystem.UpdateInGroup = {"GroupSystem"}

function PatrolSystem:Constructor(  )
	local data = {
		position = "ECS.Position:Array",
		born_pos = "UMMO.BornPosition:Array:ReadOnly",
		speed = "UMMO.Speed:Array:ReadOnly",
		length   = "Length",
	}
	self.group = {}
	self:Inject(self.group, data)
end

function PatrolSystem:OnUpdate(  )
	-- local curTime = Time.time
	for i=1,self.group.length do
		--Cat_Todo : 使用RecastNavigation读取地形信息，不然发给前端的y坐标肯定对不上地图的
		self.group.position[i].x = self.group.position[i].x+1
	end
end
