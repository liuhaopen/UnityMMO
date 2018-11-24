local TestSystem = BaseClass(ECS.ComponentSystem)
ECS:RegisterSystem(TestSystem, "TestSystem")
TestSystem.UpdateAfter = {"LastSystem"}
TestSystem.UpdateBefore = {"FirstSystem"}
TestSystem.UpdateInGroup = {"GroupSystem"}

function TestSystem:Constructor(  )
	local data = {
		position = "ECS.Position:r",
		rotation = "ECS.Rotation:subtractive",
		othersys = "ECS.TestSystem2",
		entities = "EntityArray",
	}
	self.group = {}
	ECS.Inject(self.group, data)

	local data2 = {
		asd = "ECS.ASD",
	}
	self.group2 = {}
	ECS.Inject(self.group2, data2)
end

function TestSystem:OnCreateManager( capacity )
	ECS.ComponentSystem.OnCreateManager(self, capacity)


    self.group_with_filter = self:GetComponentGroup({"ECS.Position:r", "ECS.CodeLOD"})
end

function TestSystem:OnUpdate(  )
	for i=1,#self.group do
		v.position[i].x = v.position[i].x+1
	end
	for i,v in ipairs(self.group2) do
        v.asd.x = v.asd.x+1
	end

	self.group_with_filter:SetFilter(ECS.CodeLOD.New(1))
	local positions = self.group_with_filter:GetComponentDataArray(ECS.Position)
	for i=1,#positions do
		positions[i].x = positions[i].x+1
	end
end
