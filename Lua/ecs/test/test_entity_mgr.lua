local ecs = require "ecs.ecs"

test_entity_mgr = {}

function test_entity_mgr:test_create_entity()
	local mgr = ecs.entity_mgr:new()
	local e = mgr:create_entity()
    lu.assert_not_is_nil(e)

	local e2 = mgr:create_entity("com1", "com2")
    lu.assert_not_is_nil(e2)
    lu.assert_not_equals(e, e2)

    local archetype = mgr:get_archetype("com1", "com2", "com3")
	local e3 = mgr:create_entity_by_archetype(archetype)
    lu.assert_not_is_nil(e3)
end

function test_entity_mgr:test_get_set_component()
	local mgr = ecs.entity_mgr:new()
	local e = mgr:create_entity("com1")
    lu.assert_not_is_nil(e)

    mgr:set_component(e, "com1", 1)
    lu.assert_equals(mgr:get_component(e, "com1"), 1)

	local e2 = mgr:create_entity("com1")
    lu.assert_not_is_nil(e2)
    lu.assert_nil(mgr:get_component(e2, "com1"))
    mgr:set_component(e2, "com1", 2)
    lu.assert_equals(mgr:get_component(e2, "com1"), 2)
    lu.assert_equals(mgr:get_component(e, "com1"), 1)

    mgr:set_component(e2, "com2", 3)
    lu.assert_equals(mgr:get_component(e2, "com2"), 3)
    lu.assert_equals(mgr:get_component(e2, "com1"), 2)
    lu.assert_nil(mgr:get_component(e, "com2"))
    lu.assert_equals(mgr:get_component(e, "com1"), 1)
end

function test_entity_mgr:test_foreach_all()
	local mgr = ecs.entity_mgr:new()
	local e1 = mgr:create_entity("com1")
	local e2 = mgr:create_entity("com1", "com2")
	local e3 = mgr:create_entity("com1", "com2", "com3")
	mgr:foreach(ecs.all("com1", "com2"), function(ed)
    	lu.assert_nil(ed.com1)
    	lu.assert_nil(ed.com2)
		ed.com1 = 1
		ed.com2 = {
			x = 2, y = 3,
		}
    end)
	lu.assert_nil(mgr:get_component(e1, "com1"))
    lu.assert_equals(mgr:get_component(e2, "com1"), 1)
    local e2_com2_val = mgr:get_component(e2, "com2")
    lu.assert_equals(e2_com2_val.x, 2)
    lu.assert_equals(e2_com2_val.y, 3)

    lu.assert_equals(mgr:get_component(e3, "com1"), 1)
    local e3_com2_val = mgr:get_component(e3, "com2")
    lu.assert_equals(e3_com2_val.x, 2)
    lu.assert_equals(e3_com2_val.y, 3)
	lu.assert_nil(mgr:get_component(e3, "com3"))
end

function test_entity_mgr:test_remove_component()
	local mgr = ecs.entity_mgr:new()
	local e1 = mgr:create_entity("com1")
    lu.assert_true(mgr:has_component(e1, "com1"))
    mgr:set_component(e1, "com1", 1)
    lu.assert_equals(mgr:get_component(e1, "com1"), 1)
    mgr:remove_component(e1, "com1")
    lu.assert_false(mgr:has_component(e1, "com1"))
    lu.assert_nil(mgr:get_component(e1, "com1"), 1)

	local e2 = mgr:create_entity("com1", "com2", "com3")
    mgr:set_component(e2, "com1", 1)
    mgr:set_component(e2, "com2", 2)
    mgr:set_component(e2, "com3", 3)
    mgr:remove_component(e2, "com2")
    lu.assert_false(mgr:has_component(e2, "com2"))
    lu.assert_true(mgr:has_component(e2, "com1"))
    lu.assert_true(mgr:has_component(e2, "com3"))
end

function test_entity_mgr:test_foreach_any()
	local mgr = ecs.entity_mgr:new()
	local e1 = mgr:create_entity("com1")
	local e2 = mgr:create_entity("__", "com2")
	local e3 = mgr:create_entity("com2", "com3")
	mgr:foreach(ecs.any("com1", "com3"), function(ed)
		if ed.com1 then
		end
    end)
end

function test_entity_mgr:test_destroy_entity()
    local mgr = ecs.entity_mgr:new()
    local e1 = mgr:create_entity("com1")
    lu.assert_not_is_nil(e1)
    lu.assert_true(mgr:is_entity_exist(e1))
    mgr:destroy_entity(e1)
    lu.assert_false(mgr:is_entity_exist(e1))
end

function test_entity_mgr:test_destroy_entity_in_foreach()
    local mgr = ecs.entity_mgr:new()
    local e1 = mgr:create_entity("com1")
    local e2 = mgr:create_entity("com1")
    lu.assert_not_is_nil(e1)
    lu.assert_true(mgr:is_entity_exist(e1))
    local run_times = 0
    mgr:foreach(ecs.any("com1", "com3"), function(ed)
        run_times = run_times + 1
        mgr:destroy_entity(ed.entity)
    end)
    lu.assert_equals(run_times, 2)
    lu.assert_false(mgr:is_entity_exist(e1))
    lu.assert_false(mgr:is_entity_exist(e2))
end

function test_entity_mgr:test_foreach_no()
end

function test_entity_mgr:test_clone_entity()
end