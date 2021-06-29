local class = require "ecs.common.class"

local mt = class("system")

function mt:add_to_world(world)
	self.world = world
	self.entity_mgr = self.world.entity_mgr
end

function mt:foreach(filter, cb)
	assert(self.world, "has not add to world!")
	self.world.entity_mgr:foreach(filter, cb)
end

function mt:on_update()
	--override me
end

return mt