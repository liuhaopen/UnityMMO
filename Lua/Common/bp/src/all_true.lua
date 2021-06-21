local oo = require("bp.common.lua_oo")
local bt_helper = require 'bp.src.bt_helper'

local mt = {}
local class_template = {type="all_true", __index=mt}
local all_true = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
end

-- function class_template:__call()
function mt:update()
	for i,condition in ipairs(self) do
		local isTrue = bt_helper.get_value_from_action(self, condition)
		-- slog.cat('Cat:all_true.lua[19] isTrue, condition, self', isTrue, condition)
		if not isTrue then
			return false
		end
	end
	return true
end

return all_true