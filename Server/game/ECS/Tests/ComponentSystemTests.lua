local lu = require('luaunit')

local TestSystem = BaseClass(ECS.ComponentSystem)

function TestSystem:Constructor(  )
	self.Created = false
end

function TestSystem:OnCreateManager( )
	self.Created = true
end

function TestSystem:OnDestroyManager(  )
	self.Created = false
end

TestComponentSystem = BaseClass(require("TestBaseClass"))
	
function TestComponentSystem:TestCreate(  )
	-- local system = ECS.World.Active:CreateManager(TestSystem)
 --    lu.assertEquals(system, ECS.World.Active:GetExistingManager(TestSystem))
    -- lu.assertTrue(system.Created)
end

