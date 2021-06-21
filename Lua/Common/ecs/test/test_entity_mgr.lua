local ecs = require "ecs.ecs"

test_entity_mgr = {}

function test_entity_mgr:test_create_entity()
	local mgr = ecs.entity_mgr:new()
	local entity = mgr:create_entity()
    -- lu.assert_not_is_nil(entity)
end

