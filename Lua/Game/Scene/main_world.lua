local ecs = require "ecs.ecs"

local mt = {}

function mt:init()
	self.world = ecs.world:new("main_world")
	self.entity_mgr = self.world.entity_mgr
end

function mt:start_game()
	local systems = {
		"systems.movement_system",
		"systems.aoi_system",
		"systems.visual_system",
		"systems.aoi_system",
	}
	for i,sys_path in ipairs(systems) do
		local sys = require(sys_path):new()
		self.world:add_system(sys)
	end
end

return mt