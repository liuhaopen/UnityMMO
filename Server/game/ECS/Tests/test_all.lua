package.path = package.path ..';..\\?.lua;..\\..\\?.lua';
require "ECS"
--将 szFullString 对象拆分为一个子字符串表
function Split(szFullString, szSeparator, start_pos)
	local nFindStartIndex = start_pos or 1
	local nSplitIndex = 1
	local nSplitArray = {}
	while true do
	   local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
	   if not nFindLastIndex then
	    nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
	    break
	   end
	   table.insert(nSplitArray, string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1))
	   nFindStartIndex = nFindLastIndex + string.len(szSeparator)
	   nSplitIndex = nSplitIndex + 1
	end
	return nSplitArray
end

-- local s = io.popen("ls ./")--for linux
local s = io.popen("dir /b")--for windows
local fileNames = s:read("*all")
fileNames = Split(fileNames, "\n")
for k,v in pairs(fileNames or {}) do
	if v~="" and v~="test_all.lua" and v~="luaunit.lua" and v~="BaseClass.lua" then
		local dot_index = string.find(v, ".", 1, true)
    	local is_lua_file = string.find(v, ".lua", -4, true)
    	if dot_index ~= nil and is_lua_file then
	    	local name_without_ex = string.sub(v, 1, dot_index-1)
	    	-- print('Cat:test_all.lua[31] name_without_ex', name_without_ex)
			require(name_without_ex)
		end
	end
end

local lu = require('luaunit')
os.exit( lu.LuaUnit.run() )