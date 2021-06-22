# ecs usage
ecs.world = require "ecs.world"
-- ecs.entity_mgr = require "ecs.entity_mgr"
ecs.entity_mgr = require "ecs.entity_mgr"

ecs.world.active = ecs.world {
	name = "normal_world"
}
local move_sys = require "move_system"
ecs.world.active:add_system(move_sys)
ecs.world.active:update()
ecs.world.active:destroy_system(move_sys)

local entity_mgr = ecs.world.active.entity_mgr
local entity = entity_mgr:create_entity("com.move_info", "com.speed", "com.height")
entity_mgr:add_component(entity, "com.move_info", {x=1, y=2, z=3})
entity_mgr:set_component(entity, "com.move_info", {x=1, y=2, z=3})
entity_mgr:remove_component(entity, "com.move_info")
entity_mgr:destroy_entity(entity)
local move_info = entity_mgr:get_component(entity, "com.move_info")

local move_sys = base_class(ecs.system)

function move_sys:on_update()
	self.filter = self.filter or ecs.all("com1", "com2")
	--or complecate version:
	self.filter = ecs.all(ecs.any("com1", "com2"), ecs.any("com3", "com4"), ecs.no("com5", "com6"))
	self:foreach(self.filter, function(e)
		e.com1.x = 3 
		e.com2 = 4
    end)
end

-- movement_system
-- aoi_system