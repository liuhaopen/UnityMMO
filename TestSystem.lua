local TestSystem = BaseClass(ECS.ComponentSystem)
ECS.Systems:Register(TestSystem, "TestSystem")
TestSystem.UpdateAfter = {"LastSystem"}
TestSystem.UpdateBefore = {"FirstSystem"}
TestSystem.UpdateInGroup = {"GroupSystem"}

function TestSystem:Constructor(  )
	local data = {
		position = {ECS.Position, Access="r"},
		rotation = {ECS.Rotation, SubtractiveComponent},
		othersys = {ECS.TestSystem2}
	}
	self.group = {}
	ECS.Inject(self.group, data)
end

function TestSystem:OnUpdate(  )
	for i,v in ipairs(self.group) do
        v.position.x = v.position.x+1;
	end
end
