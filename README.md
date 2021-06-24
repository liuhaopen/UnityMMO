# usage
```
local ecs = require "ecs.ecs"

local world = ecs.world:new()
local entity_mgr = world.entity_mgr

local entity = entity_mgr:create_entity("move_info", "speed", "height")
entity_mgr:set_component(entity, "move_info", {x=1, y=2, z=3})
local move_info = entity_mgr:get_component(entity, "move_info")
entity_mgr:remove_component(entity, "move_info")
entity_mgr:destroy_entity(entity)

local move_sys = ecs.class("move_sys", ecs.system)
function move_sys:on_update()
    self.filter = self.filter or ecs.all("move_info", "speed")
    --complex version: ecs.all(ecs.any("com1", "com2"), ecs.any("com3", "com4"), ecs.no("com5", "com6"))
    self:foreach(self.filter, function(ed)
	ed.move_info.x = 3 
	ed.speed = 4
    end)
end
world:add_system(move_sys)

world:update()
```
