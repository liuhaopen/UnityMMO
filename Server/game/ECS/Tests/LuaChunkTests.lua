local lu = require('luaunit')

TestLuaChunk = BaseClass(require("TestBaseClass"))
	
function TestLuaChunk:TestDllRequire(  )
	require("LuaChunk")
	print('Cat:LuaChunkTests.lua[7] LuaChunk', LuaChunk)
	print(LuaChunk.dir())
end
