-- local lu = require('luaunit')
TestComponentTypeInArchetypeTests = BaseClass(require("TestBaseClass"))
	
function TestComponentTypeInArchetypeTests:TestLess(  )
	local a = ECS.ComponentTypeInArchetype.Create({TypeIndex=1, BufferCapacity=0})
	local b = ECS.ComponentTypeInArchetype.Create({TypeIndex=2, BufferCapacity=0})
	lu.assertTrue(a < b)
end