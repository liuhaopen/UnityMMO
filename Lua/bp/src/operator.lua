local oo = require("bp.common.lua_oo")
local bt_helper = require 'bp.src.bt_helper'

local mt = {}
local class_template = {type="operator", __index=mt}
local operator = oo.class(class_template)

local op_name_map = {
	["not"] = "op_not",
	["not nil"] = "op_not_nil",
	["=="] = "op_equal",
	["~="] = "op_not_equal",
	[">"] = "op_bigger",
	["<"] = "op_smaller",
	[">="] = "op_bigger_equal",
	["<="] = "op_smaller_equal",
}

function mt:start(graph)
	self.graph = graph
end

function class_template:__call()
	local op = self[1]
	local op_name = op_name_map[op]
	local op_func = mt[op_name or ""]
	assert(op_func, "unknow operator : "..(op or "nil"))
	local arge1 = bt_helper.get_input(self, 1, self[2])
	local arge2 = bt_helper.get_input(self, 2, self[3])
	if self.log then
		slog.cat('Cat:operator : ', op, arge1, arge2)
	end
	return op_func(self, arge1, arge2)
end

function mt:op_not(arge)
	return not arge
end

function mt:op_not_nil( arge )
	return arge ~= nil
end

function mt:op_equal(arge1, arge2)
	return arge1 == arge2
end

function mt:op_not_equal(arge1, arge2)
	return arge1 ~= arge2
end

function mt:op_bigger(arge1, arge2)
	return arge1 > arge2
end

function mt:op_smaller(arge1, arge2)
	return arge1 < arge2
end

function mt:op_bigger_equal(arge1, arge2)
	return arge1 >= arge2
end

function mt:op_smaller_equal(arge1, arge2)
	-- slog.cat('Cat:operator.lua[53] arge1, arge2', arge1, arge2)
	return arge1 <= arge2
end

return operator