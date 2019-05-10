local ECS = require "ECS"
-- local lu = require('luaunit')
TestComponentTypeInArchetype = ECS.BaseClass(require("TestBaseClass"))
	
function TestComponentTypeInArchetype:TestLess(  )
	local a = ECS.ComponentTypeInArchetype.Create({TypeIndex=1, BufferCapacity=0})
	local b = ECS.ComponentTypeInArchetype.Create({TypeIndex=2, BufferCapacity=0})
	lu.assertTrue(a < b)
end