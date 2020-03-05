--[[
-- added by wsh @ 2017-12-11
-- table扩展工具类，对table不支持的功能执行扩展
-- 注意：
-- 1、所有参数带hashtable的函数，将把table当做哈希表对待
-- 2、所有参数带array的函数，将把table当做可空值数组对待
-- 3、所有参数带tb的函数，对表通用，不管是哈希表还是数组
--]]

-- 计算哈希表长度
local function count(hashtable)
	local count = 0
	for _,_ in pairs(hashtable) do
		count = count + 1
	end
	return count
end

-- 计算数据长度
local function length(array)
	if array.n ~= nil then
		return array.n
	end
	
	local count = 0
	for i,_ in pairs(array) do
		if count < i then
			count = i
		end		
	end
	return count
end

-- 设置数组长度
local function setlen(array, n)
	array.n = n
end

-- 获取哈希表所有键
local function keys(hashtable)
    local keys = {}
    for k, v in pairs(hashtable) do
        keys[#keys + 1] = k
    end
    return keys
end

-- 获取哈希表所有值
local function values(hashtable)
    local values = {}
    for k, v in pairs(hashtable) do
        values[#values + 1] = v
    end
    return values
end

-- 合并哈希表：将src_hashtable表合并到dest_hashtable表，相同键值执行覆盖
local function merge(dest_hashtable, src_hashtable)
    for k, v in pairs(src_hashtable) do
        dest_hashtable[k] = v
    end
end

-- 合并数组：将src_array数组从begin位置开始插入到dest_array数组
-- 注意：begin <= 0被认为没有指定起始位置，则将两个数组执行拼接
local function insertto(dest_array, src_array, begin)
	assert(begin == nil or type(begin) == "number")
	if begin == nil or begin <= 0 then
		begin = #dest_array + 1
	end

	local src_len = #src_array
	for i = 0, src_len - 1 do
		dest_array[i + begin] = src_array[i + 1]
	end
end

-- 从数组中查找指定值，返回其索引，没找到返回false
local function indexof(array, value, begin)
    for i = begin or 1, #array do
        if array[i] == value then 
			return i 
		end
    end
	return false
end

-- 从哈希表查找指定值，返回其键，没找到返回nil
-- 注意：
-- 1、containskey用hashtable[key] ~= nil快速判断
-- 2、containsvalue由本函数返回结果是否为nil判断
local function keyof(hashtable, value)
    for k, v in pairs(hashtable) do
        if v == value then 
			return k 
		end
    end
    return nil
end

-- 从数组中删除指定值，返回删除的值的个数
function table.removebyvalue(array, value, removeall)
    local remove_count = 0
	for i = #array, 1, -1 do
		if array[i] == value then
			table.remove(array, i)
			remove_count = remove_count + 1
            if not removeall then 
				break 
			end
		end
	end
	return remove_count
end

-- 遍历写：用函数返回值更新表格内容
local function map(tb, func)
    for k, v in pairs(tb) do
        tb[k] = func(k, v)
    end
end

-- 遍历读：不修改表格
local function walk(tb, func)
    for k,v in pairs(tb) do
        func(k, v)
    end
end

-- 按指定的排序方式遍历：不修改表格
local function walksort(tb, sort_func, walk_func)
	local keys = table.keys(tb)
	table.sort(keys, function(lkey, rkey)
		return sort_func(lkey, rkey)
	end)
	for i = 1, table.length(keys) do
		walk_func(keys[i], tb[keys[i]])
	end
end

-- 过滤掉不符合条件的项：不对原表执行操作
local function filter(tb, func)
	local filter = {}
    for k, v in pairs(tb) do
        if not func(k, v) then 
			filter[k] = v
		end
    end
	return filter
end

-- 筛选出符合条件的项：不对原表执行操作
local function choose(tb, func)
	local choose = {}
    for k, v in pairs(tb) do
        if func(k, v) then 
			choose[k] = v
		end
    end
	return choose
end

-- 获取数据循环器：用于循环数组遍历，每次调用走一步，到数组末尾从新从头开始
local function circulator(array)
	local i = 1
	local iter = function()
		i = i >= #array and 1 or i + 1
		return array[i]
	end
	return iter
end

-- dump表
local function dump(tb, dump_metatable, max_level)
	local lookup_table = {}
	local level = 0
	local rep = string.rep
	local dump_metatable = dump_metatable
	local max_level = max_level or 1

	local function _dump(tb, level)
		local str = "\n" .. rep("\t", level) .. "{\n"
		for k,v in pairs(tb) do
			local k_is_str = type(k) == "string" and 1 or 0
			local v_is_str = type(v) == "string" and 1 or 0
			str = str..rep("\t", level + 1).."["..rep("\"", k_is_str)..(tostring(k) or type(k))..rep("\"", k_is_str).."]".." = "
			if type(v) == "table" then
				if not lookup_table[v] and ((not max_level) or level < max_level) then
					lookup_table[v] = true
					str = str.._dump(v, level + 1, dump_metatable).."\n"
				else
					str = str..(tostring(v) or type(v))..",\n"
				end
			else
				str = str..rep("\"", v_is_str)..(tostring(v) or type(v))..rep("\"", v_is_str)..",\n"
			end
		end
		if dump_metatable then
			local mt = getmetatable(tb)
			if mt ~= nil and type(mt) == "table" then
				str = str..rep("\t", level + 1).."[\"__metatable\"]".." = "
				if not lookup_table[mt] and ((not max_level) or level < max_level) then
					lookup_table[mt] = true
					str = str.._dump(mt, level + 1, dump_metatable).."\n"
				else
					str = str..(tostring(v) or type(v))..",\n"
				end
			end
		end
		str = str..rep("\t", level) .. "},"
		return str
	end
	
	return _dump(tb, level)
end

local function get_value_in_array(array, field, fieldValue)
	if not array then return nil end
	for i,v in ipairs(array) do
		if v[field] == fieldValue then
			return v
		end
	end	
	return nil
end

local function remove_value_in_array(array, field, fieldValue)
	if not array then return nil end
	for i,v in ipairs(array) do
		if v[field] == fieldValue then
			table.remove(array, i)
			return
		end
	end	
	return
end

local function deep_copy(object)
	-- @param object 需要深拷贝的对象
	-- @return 深拷贝完成的对象

	local lookup_table = {}
	local function _copy(object)
		if type(object) ~= "table" then
			return object
		elseif lookup_table[object] then
			return lookup_table[object]
		end

		local new_table = {}
		lookup_table[object] = new_table
		for index, value in pairs(object) do
			new_table[_copy(index)] = _copy(value)
		end

		return setmetatable(new_table, getmetatable(object))
	end

	return _copy(object)
end

table.count = count
table.length = length
table.setlen = setlen
table.keys = keys
table.values = values
table.merge = merge
table.insertto = insertto
table.indexof = indexof
table.keyof = keyof
table.map = map
table.walk = walk
table.walksort = walksort
table.filter = filter
table.choose = choose
table.circulator = circulator
table.dump = dump
table.get_value_in_array = get_value_in_array
table.remove_value_in_array = remove_value_in_array
table.deep_copy = deep_copy
