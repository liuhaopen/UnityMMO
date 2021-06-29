local class = require "ecs.common.class"

test_class = {}

function test_class:test_class_func()
    local class_a = class("class_a")
    class_a.common_num = 123
    local test_on_new_num = 0
    class_a.on_new = function(t)
        t.common_str = "haha"
        t.must_has_a = true
        test_on_new_num = test_on_new_num + 1
    end
    local a = class_a:new()
    lu.assert_not_is_nil(a)
    a.test_num = 1
    a.test_bool = true
    lu.assert_equals(a.common_num, 123)
    lu.assert_equals(test_on_new_num, 1)
    lu.assert_equals(a.common_str, "haha")
    lu.assert_equals(a.test_num, 1)
    lu.assert_true(a.test_bool)
    local a2 = class_a:new()
    lu.assert_not_is_nil(a2)
    lu.assert_equals(a2.common_num, 123)
    lu.assert_equals(a2.common_str, "haha")
    lu.assert_nil(a2.test_num, 1)
    lu.assert_nil(a2.test_bool)
    lu.assert_equals(test_on_new_num, 2)
    lu.assert_true(a.must_has_a)
    lu.assert_true(a2.must_has_a)
    lu.assert_nil(a.must_has_b)
    lu.assert_nil(a.must_has_c)

    local class_b = class("class_b", class_a)
    class_b.on_new = function(t)
        t.common_str = "bbbbb"
        t.must_has_b = true
    end
    local b = class_b:new()
    lu.assert_not_is_nil(b)
    lu.assert_equals(b.common_num, 123)
    lu.assert_equals(b.common_str, "bbbbb")
    lu.assert_equals(a.common_str, "haha")
    lu.assert_equals(a2.common_str, "haha")
    lu.assert_equals(test_on_new_num, 3)
    lu.assert_true(b.must_has_a)
    lu.assert_true(b.must_has_b)
    lu.assert_nil(b.must_has_c)

    local class_c = class("class_c", class_b)
    class_c.on_new = function(t)
        t.common_str = "ccccc"
        t.must_has_c = true
    end
    local c = class_c:new()
    lu.assert_not_is_nil(c)
    lu.assert_equals(c.common_num, 123)
    lu.assert_equals(c.common_str, "ccccc")
    lu.assert_true(c.must_has_a)
    lu.assert_true(c.must_has_b)
    lu.assert_true(c.must_has_c)
    lu.assert_equals(test_on_new_num, 4)

    local class_d = class("class_d", class_c)
    class_d.on_new = function(t, arge)
        t.d_val = arge
    end
    local d = class_d:new(666)
    lu.assert_not_is_nil(d)
    lu.assert_equals(d.d_val, 666)
end

