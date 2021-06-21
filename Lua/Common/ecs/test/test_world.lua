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

function test_world:test_update()
	local world = ecs.world:new()
	-- world:add_system()
end