local ECS = require "ECS"
TestECSCore = ECS.BaseClass(require("TestBaseClass"))
	
function TestECSCore:TestChunk(  )
	local chunk = ECS.Core.CreateChunk(32)
	local integer_size = ECS.CoreHelper.GetIntegerSize()
	local number_size = ECS.CoreHelper.GetNumberSize()
	lu.assertNotEquals(integer_size, 0)
	lu.assertNotEquals(number_size, 0)
	local pos = {0}
	local cur_pos = 0
	ECS.Core.WriteNumber(chunk, pos[1], 123)
	pos[2] = pos[1] + number_size
	ECS.Core.WriteNumber(chunk, pos[2], 456.789)
	pos[3] = pos[2] + number_size
	ECS.Core.WriteInteger(chunk, pos[3], 999)
	pos[4] = pos[3] + integer_size
	ECS.Core.WriteInteger(chunk, pos[4], 110)
	pos[5] = pos[4] + integer_size
	ECS.Core.WriteNumber(chunk, pos[5], 123.456)

	-- ECS.Core.PrintChunk(chunk, 32)
	local num = ECS.Core.ReadNumber(chunk, pos[1])
	lu.assertEquals(num, 123)
	num = ECS.Core.ReadNumber(chunk, pos[2])
	lu.assertEquals(num, 456.789)
	num = ECS.Core.ReadInteger(chunk, pos[3])
	lu.assertEquals(num, 999)
	num = ECS.Core.ReadInteger(chunk, pos[4])
	lu.assertEquals(num, 110)
	num = ECS.Core.ReadNumber(chunk, pos[5])
	lu.assertEquals(num, 123.456)
end

function TestECSCore:TestSize(  )
	local number_size = ECS.Core.GetNumberSize()
	lu.assertNotNil(number_size)
	local integer_size = ECS.Core.GetIntegerSize()
	lu.assertNotNil(integer_size)

	local num_size_from_helper = ECS.CoreHelper.GetNativeTypeSize("number")
	lu.assertEquals(num_size_from_helper, number_size)
end