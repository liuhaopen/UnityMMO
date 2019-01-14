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