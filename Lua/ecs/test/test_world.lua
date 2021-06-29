local ecs = require "ecs.ecs"

test_world = {}

function test_world:test_create_world()
	local world = ecs.world:new()
    lu.assert_not_is_nil(world)
    lu.assert_equals(world, ecs.world.active)

	local world2 = ecs.world:new()
    lu.assert_not_is_nil(world2)
    lu.assert_equals(world, ecs.world.active)
end

local sys_a = {}
function sys_a:on_update()
	self.num_a = self.num_a or 0
	self.num_a = self.num_a + 1
end
local sys_b = {}
function sys_b:on_update()
	self.num_b = self.num_b or 0
	self.num_b = self.num_b + 1
end
function test_world:test_update()
	local world = ecs.world:new()
	world:add_system(sys_a)
	world:add_system(sys_b)
	world:update(0.02)
    lu.assert_equals(sys_a.num_a, 1)
    lu.assert_equals(sys_b.num_b, 1)
	world:update(0.02)
	lu.assert_equals(sys_a.num_a, 2)
    lu.assert_equals(sys_b.num_b, 2)
end