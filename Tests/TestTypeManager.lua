TestTypeManager = BaseClass(require("TestBaseClass"))
	
function TestTypeManager:TestEntityTypeInfo(  )
	local type_index = ECS.TypeManager.GetTypeIndexByName("ECS.Entity")
	lu.assertNotNil(type_index)
	local type_info1 = ECS.TypeManager.GetTypeInfoByName("ECS.Entity")
	lu.assertNotNil(type_info1)
	local type_info2 = ECS.TypeManager.GetTypeInfoByIndex(type_index)
	lu.assertNotNil(type_info2)
	lu.assertEquals(type_info1, type_info2)
end

function TestTypeManager:TestTypeInfo(  )
	local test_comp_name = "ECS.Position"
	ECS.TypeManager.RegisterType(test_comp_name, {pos_x="number", pos_y="number", pos_z="number"})
	local type_index = ECS.TypeManager.GetTypeIndexByName(test_comp_name)
	lu.assertNotNil(type_index)
	local type_info1 = ECS.TypeManager.GetTypeInfoByName(test_comp_name)
	local type_info2 = ECS.TypeManager.GetTypeInfoByIndex(type_index)
	lu.assertNotNil(type_info1)
	lu.assertNotNil(type_info2)
	lu.assertEquals(type_info1, type_info2)
end