-- local lu = require('luaunit')
TestECSCore = BaseClass(require("TestBaseClass"))
-- require("ECSCore")
	
function TestECSCore:TestChunk(  )
	local chunk = ECSCore.CreateChunk(24)
	ECSCore.WriteNumber(chunk, 0, 123)
	local num = ECSCore.ReadNumber(chunk, 0)
	lu.assertEquals(num, 123)
	ECSCore.WriteNumber(chunk, 8, 456.789)
	num = ECSCore.ReadNumber(chunk, 8)
	lu.assertEquals(num, 456.789)

	-- print('Cat:TestECSCore.lua[14] chunk', chunk)
	-- print('Cat:TestECSCore.lua[14] tostring(chunk)', chunk+2)
end

function TestECSCore:TestSize(  )
	local number_size = ECSCore.GetNumberSize()
	lu.assertNotNil(number_size)
	local integer_size = ECSCore.GetIntegerSize()
	lu.assertNotNil(integer_size)
	-- print('Cat:ECSCoreTests.lua[16] number_size', number_size, integer_size)

	local num_size_from_helper = ECS.CoreHelper.GetNativeTypeSize("number")
	lu.assertEquals(num_size_from_helper, number_size)
end