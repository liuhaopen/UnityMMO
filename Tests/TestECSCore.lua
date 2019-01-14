TestECSCore = BaseClass(require("TestBaseClass"))
	
function TestECSCore:TestChunk(  )
	local chunk = ECSCore.CreateChunk(32)
	local integer_size = ECS.CoreHelper.GetIntegerSize()
	local number_size = ECS.CoreHelper.GetNumberSize()
	lu.assertNotEquals(integer_size, 0)
	lu.assertNotEquals(number_size, 0)
	local pos = {0}
	local cur_pos = 0
	ECSCore.WriteNumber(chunk, pos[1], 123)
	pos[2] = pos[1] + number_size
	ECSCore.WriteNumber(chunk, pos[2], 456.789)
	pos[3] = pos[2] + number_size
	ECSCore.WriteInteger(chunk, pos[3], 999)
	pos[4] = pos[3] + integer_size
	ECSCore.WriteInteger(chunk, pos[4], 110)
	pos[5] = pos[4] + integer_size
	ECSCore.WriteNumber(chunk, pos[5], 123.456)

	-- ECSCore.PrintChunk(chunk, 32)
	local num = ECSCore.ReadNumber(chunk, pos[1])
	lu.assertEquals(num, 123)
	num = ECSCore.ReadNumber(chunk, pos[2])
	lu.assertEquals(num, 456.789)
	num = ECSCore.ReadInteger(chunk, pos[3])
	lu.assertEquals(num, 999)
	num = ECSCore.ReadInteger(chunk, pos[4])
	lu.assertEquals(num, 110)
	num = ECSCore.ReadNumber(chunk, pos[5])
	lu.assertEquals(num, 123.456)
end

function TestECSCore:TestSize(  )
	local number_size = ECSCore.GetNumberSize()
	lu.assertNotNil(number_size)
	local integer_size = ECSCore.GetIntegerSize()
	lu.assertNotNil(integer_size)

	local num_size_from_helper = ECS.CoreHelper.GetNativeTypeSize("number")
	lu.assertEquals(num_size_from_helper, number_size)
end