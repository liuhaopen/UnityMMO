local class = require "ecs.common.class"
local entity_mgr = require "ecs.src.entity_mgr"

local mt = class("entity_mgr")

function mt:on_new(name)
	if mt.active == nil then
		mt.active = self
	end
	self.name = name
	self.systems = {}
	self.entity_mgr = entity_mgr:new()
end

function mt:add_system(sys)
	assert(sys.on_update, "has not defind 'on_update' method in system!")
	self.systems[#self.systems + 1] = sys
	if sys.add_to_world then
		sys:add_to_world(self)
	end
end

function mt:update(dt)
	for i,sys in ipairs(self.systems) do
		sys:on_update(dt)
	end
end

return mt