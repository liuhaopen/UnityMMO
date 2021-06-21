local class = require "common.class"

test_class = {}

function test_class:test_class_func()
    local class_a = class("class_a")
    class_a.common_num = 123
    local test_on_new_num = 0
    class_a.on_new = function(t)
        t.common_str = "haha"
        t.common_bool = true
        test_on_new_num = test_on_new_num + 1
    end
    local a = class_a:new()
    lu.assert_not_is_nil(a)
    a.test_num = 1
    a.test_bool = true
    lu.assert_equals(a.common_num, 123)
    lu.assert_equals(test_on_new_num, 1)
    lu.assert_equals(a.common_str, "haha")
    lu.assert_true(a.common_bool)
    lu.assert_equals(a.test_num, 1)
    lu.assert_true(a.test_bool)
    local a2 = class_a:new()
    lu.assert_not_is_nil(a2)
    lu.assert_equals(a2.common_num, 123)
    lu.assert_equals(a2.common_str, "haha")
    lu.assert_true(a2.common_bool)
    lu.assert_nil(a2.test_num, 1)
    lu.assert_nil(a2.test_bool)
    lu.assert_equals(test_on_new_num, 2)

    local class_b = class("class_b", class_a)
    class_b.on_new = function(t)
        t.common_str = "bbbbb"
    end
    local b = class_b:new()
    lu.assert_not_is_nil(b)
    lu.assert_equals(b.common_num, 123)
    lu.assert_equals(b.common_str, "bbbbb")
    lu.assert_true(b.common_bool)
    lu.assert_equals(a.common_str, "haha")
    lu.assert_equals(a2.common_str, "haha")
    lu.assert_true(a.common_bool)
    lu.assert_true(a2.common_bool)
    lu.assert_equals(test_on_new_num, 3)
end

