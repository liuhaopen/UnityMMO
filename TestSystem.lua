local TestSystem = BaseClass(ECS.ComponentSystem)
ECS:RegisterSystem(TestSystem, "TestSystem")
TestSystem.UpdateAfter = {"LastSystem"}
TestSystem.UpdateBefore = {"FirstSystem"}
TestSystem.UpdateInGroup = {"GroupSystem"}
TestSystem.AlwaysUpdateSystem = true
function TestSystem:Constructor(  )
	local data = {
		position = "ECS.Position:Array:ReadOnly",
		rotation = "ECS.Rotation:Subtractive",
		othersys = "ECS.TestSystem2:ScriptMgr",
		entities = "EntityArray",
		length   = "Length",
	}
	self.group = {}
	self:Inject(self.group, data)

end

function TestSystem:OnCreateManager( capacity )
	ECS.ComponentSystem.OnCreateManager(self, capacity)
    self.group_with_filter = self:GetComponentGroup({"ECS.Position:ReadOnly", "ECS.CodeLOD"})
end

function TestSystem:OnUpdate(  )
	for i=1,self.group.length do
		v.position[i].x = v.position[i].x+1
	end
	-- for i,v in ipairs(self.group2) do
 --        v.asd.x = v.asd.x+1
	-- end

	self.group_with_filter:SetFilter(ECS.CodeLOD.New(1))
	local positions = self.group_with_filter:GetComponentDataArray(ECS.Position)
	for i=1,#positions do
		positions[i].x = positions[i].x+1
	end
end
