TestChunkDataUtility = BaseClass(require("TestBaseClass"))

function TestChunkDataUtility:TestReadWriteComponentInChunk()
	local test_comp_name = "TestComponent"
	local type_info = ECS.TypeManager.RegisterType(test_comp_name, {n="number", i="integer", b="boolean", b2="boolean"})

	local chunk = ECSCore.CreateChunk(32)
	local comp_data = {}
	ECS.ChunkDataUtility.ReadComponentFromChunk(chunk, test_comp_name, comp_data)
	lu.assertEquals(comp_data.n, 0)
	lu.assertEquals(comp_data.i, 0)
	lu.assertEquals(comp_data.b, false)
	lu.assertEquals(comp_data.b2, false)

	comp_data = {n=1.23, i=999, b=true}
	ECS.ChunkDataUtility.WriteComponentInChunk(chunk, test_comp_name, comp_data)
	ECS.ChunkDataUtility.ReadComponentFromChunk(chunk, test_comp_name, comp_data)
	lu.assertEquals(comp_data.n, 1.23)
	lu.assertEquals(comp_data.i, 999)
	lu.assertEquals(comp_data.b, true)
	lu.assertEquals(comp_data.b2, false)
end

-- function TestChunkDataUtility:TestConvert()
-- 	local type_info_one = ECS.TypeManager.RegisterType("EcsTestDataOne", {x="number", y="boolean"})
--     local type_info_two = ECS.TypeManager.RegisterType("EcsTestDataTwo", {x="integer", y="number", z="boolean"})
--     -- local type_info_three = ECS.TypeManager.RegisterType("EcsTestDataThree", {x="boolean", y="integer"})
--     local archetype = self.m_Manager:CreateArchetype({"EcsTestDataOne"})
--     local archetype2 = self.m_Manager:CreateArchetype({"EcsTestDataOne", "EcsTestDataTwo"})

--     local entity1 = self.m_Manager:CreateEntityByArcheType(archetype)
--     local entity2 = self.m_Manager:CreateEntityByArcheType(archetype2)

--     self.m_Manager:SetComponentData(entity1, "EcsTestDataOne", {x=1.2, y=true})
--     self.m_Manager:SetComponentData(entity1, "EcsTestDataThree", {x=false, y=3})
--     self.m_Manager:SetComponentData(entity2, "EcsTestDataTwo", {x=6, y=4.5, z=true})

--     local _dataMgr = self.m_Manager:GetEntityDataManager()
--     local chunk1 = _dataMgr:GetComponentChunk(entity1)
--     local chunk2 = _dataMgr:GetComponentChunk(entity2)
-- 	ECS.ChunkDataUtility.Convert(chunk1, 1, chunk2, 1)

-- 	local comp_data1 = self.m_Manager:GetComponentData(entity1, "EcsTestDataOne")
--     lu.assertEquals(comp_data1.x, 1.2)
--     lu.assertEquals(comp_data1.y, true)
-- end
