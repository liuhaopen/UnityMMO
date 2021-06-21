local export_map = {}
local oo = require("bp.common.lua_oo")
local node_wrapper = require 'bp.src.action.node_wrapper'

local FLT_EPSILON = 1.192092896e-07

local action_interval_mt = {}
local action_interval = oo.class({type="action_interval", __index=action_interval_mt})
export_map["action_interval"] = action_interval

function action_interval_mt:start(graph)
	assert(self.duration, "must has duration!")
	self.graph = graph
	self.elapsed = 0
	self.first_tick = true
end

function action_interval_mt:is_done()
	return self.elapsed >= self.duration
end

function action_interval_mt:update(dt)
	if self.first_tick then
		self.first_tick = false
		self.elapsed = 0
	else
    	self.elapsed = self.elapsed + dt
    end
    local percent = math.max(0,math.min(1, self.elapsed / math.max(self.duration, FLT_EPSILON)))
    self:update_percent(percent)
end

----------------------move action start-----------------------
local move_by_mt = {}
local move_by = oo.class_over(action_interval){type="move_by", __index=move_by_mt}
export_map["move_by"] = move_by

local move_type_wrapper = {
	["anchored"] = {get_func="get_anchored_pos", set_func="set_anchored_pos"},
	["local"] = {get_func="get_local_pos", set_func="set_local_pos"},
	["abs"] = {get_func="get_abs_pos", set_func="set_anchored_pos"},
}
--[[用法：
bp.move_to {
	duration = 2,
	target = self.handler.item,
	anchored_y = 100,
}
--]]
function move_by_mt:start( graph )
	action_interval_mt.start(self, graph)

	self.move_type = ""
	if self.anchored_x or self.anchored_y or self.anchored_z then
		self.move_type = "anchored"
		self.delta_pos_x = self.anchored_x or 0
		self.delta_pos_y = self.anchored_y or 0
		self.delta_pos_z = self.anchored_z or 0
	elseif self.local_x or self.local_y or self.local_z then
		self.move_type = "local"
		self.delta_pos_x = self.local_x or 0
		self.delta_pos_y = self.local_y or 0
		self.delta_pos_z = self.local_z or 0
	elseif self.abs_x or self.abs_y or self.abs_z then
		self.move_type = "abs"
		self.delta_pos_x = self.abs_x or 0
		self.delta_pos_y = self.abs_y or 0
		self.delta_pos_z = self.abs_z or 0
	end
	assert(self.move_type, "unknow move type!")
	self.wrapper = move_type_wrapper[self.move_type]
	local x, y, z = node_wrapper:get_wrapper()[self.wrapper.get_func](self.target)
	self.start_pos_x = x
	self.start_pos_y = y
	self.start_pos_z = z or 0
end

function move_by_mt:update_percent(t)
	local new_x = self.start_pos_x + (self.delta_pos_x * t)
    local new_y = self.start_pos_y + (self.delta_pos_y * t)
    local new_z = self.start_pos_z + (self.delta_pos_z * t)
	node_wrapper:get_wrapper()[self.wrapper.set_func](self.target, new_x, new_y, new_z)
end

local move_to_mt = {}
local move_to = oo.class_over(move_by){type="move_to", __index=move_to_mt}
export_map["move_to"] = move_to
export_map["move_to_mt"] = move_to_mt

function move_to_mt:start( graph )
	move_by_mt.start(self, graph)
	if self.move_type == "anchored" then
		self.delta_pos_x = self.anchored_x and (self.anchored_x-self.start_pos_x) or 0
		self.delta_pos_y = self.anchored_y and (self.anchored_y-self.start_pos_y) or 0
		self.delta_pos_z = self.anchored_z and (self.anchored_z-self.start_pos_z) or 0
	elseif self.move_type == "local" then
		self.delta_pos_x = self.local_x and (self.local_x-self.start_pos_x) or 0
		self.delta_pos_y = self.local_y and (self.local_y-self.start_pos_y) or 0
		self.delta_pos_z = self.local_z and (self.local_z-self.start_pos_z) or 0
	elseif self.move_type == "abs" then
		self.delta_pos_x = self.abs_x and (self.abs_x-self.start_pos_x) or 0
		self.delta_pos_y = self.abs_y and (self.abs_y-self.start_pos_y) or 0
		self.delta_pos_z = self.abs_z and (self.abs_z-self.start_pos_z) or 0
	end
end

----------------------move action end-----------------------

return export_map