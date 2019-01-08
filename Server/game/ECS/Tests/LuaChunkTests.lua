local lu = require('luaunit')

TestLuaChunk = BaseClass(require("TestBaseClass"))
	
function TestLuaChunk:TestDllRequire(  )
	require("ECSCore")
	local chunk = ECSCore.CreateChunk(24)
	ECSCore.WriteNumber(chunk, 0, 123)
	ECSCore.WriteNumber(chunk, 8, 456)
	local num = ECSCore.ReadNumber(chunk, 8)
	print('Cat:ECSCoreTests.lua[13] num', num)
	local number_size = ECSCore.GetNumberSize()
	local integer_size = ECSCore.GetIntegerSize()
	print('Cat:LuaChunkTests.lua[16] number_size', number_size, integer_size)
end
