package.path = package.path ..';../?.lua;../../?.lua;Tests/?.lua';
local BP = require('Blueprint')
lu = require('Tests.luaunit')

--在上级目录运行本文件即可：lua Tests/test_all.lua
--目前只支持lua5.2及以上版本，如果你在windows系统的话可以自己编译最新的lua库并生成exe用来运行本测试

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

-- 检查table是否为空
function IsTableEmpty(tbl)
    return not tbl or _G.next( tbl ) == nil
end

-- 获取table长度，当数据不连续时不能用#
function TableSize(tbl)
	if IsTableEmpty(tbl) then
		return 0
	end

	local len = 0
	for _ in pairs(tbl) do
		len = len + 1
	end
	return len
end

function PrintTable( tbl, level, return_counter )
	if tbl == nil or type(tbl) ~= "table" then
		return
	end
	return_counter = return_counter or 5 --剩下多少层就返回,防止无限打印
	if return_counter <= 0 then
		-- print('Cat:util.lua PrintTable return_counter empty')
		return 
	end
	return_counter = return_counter - 1
	level = level or 1

	local indent_str = ""
	for i = 1, level do
		indent_str = indent_str.."	"
	end
	print(indent_str .. "{")
	for k,v in pairs(tbl) do

		local item_str = string.format("%s%s = %s", indent_str .. "	",tostring(k), tostring(v))
		print(item_str)
		if type(v) == "table" then
			PrintTable(v, level + 1, return_counter)
		end
	end
	print(indent_str .. "}")
	
end

local ignore_files = {
	["test_blueprint_all.lua"]=true, 
	["luaunit.lua"]=true,
	["FSMSampleState.lua"]=true,
}
-- local s = io.popen("ls ./Tests")--for linux
local s = io.popen("dir /b Tests")--for windows
local fileNames = s:read("*all")
fileNames = Split(fileNames, "\n")
for k,v in pairs(fileNames or {}) do
	if v~="" and not ignore_files[v] then
		local dot_index = string.find(v, ".", 1, true)
    	local is_lua_file = string.find(v, ".lua", -4, true)
    	if dot_index ~= nil and is_lua_file then
	    	local name_without_ex = string.sub(v, 1, dot_index-1)
	    	-- print('test_blueprint_all.lua init test file name : ', name_without_ex)
			require(name_without_ex)
		end
	end
end

os.exit( lu.LuaUnit.run() )