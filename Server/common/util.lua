function PrintTable( tbl, level, return_counter )
	if tbl == nil or type(tbl) ~= "table" then
		return
	end
	return_counter = return_counter or 7 --剩下多少层就返回,防止无限打印
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

function Trim(str)
	if str == nil or type(str) == "table" then return "" end
	str = string.gsub(str, "^[ \t\n\r]+", "")
    return string.gsub(str, "[ \t\n\r]+$", "")
end

function Round(number)
	local intNum = math.floor(number)
	if number >= (intNum + 0.5) then
		return intNum + 1
	else
		return intNum
	end
end

