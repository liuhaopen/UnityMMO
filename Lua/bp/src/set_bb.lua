local oo = require("bp.common.lua_oo")
local slot_mgr = require("bp.src.slot_mgr")
local bt_helper = require 'bp.src.bt_helper'

local mt = {}
local class_template = {type="set_bb", __index=mt}
local set_bb = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
end

function mt:is_done()
	return true
end

function mt:update(dt)
	local arge1 = bt_helper.get_input(self, 1, self[1])
	local arge2 = bt_helper.get_input(self, 2, self[2])
	self.graph:set_bb(arge1, arge2)
end

return set_bb