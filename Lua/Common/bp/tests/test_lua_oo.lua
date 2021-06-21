local bp = require("bp")

TestLuaOO = {}

function TestLuaOO:setUp(  )
end

function TestLuaOO:tearDown(  )
end

function TestLuaOO:TestLuaOOClassKV(  )
	local Object = bp.oo.class {
		type = "Object",
		__tostring = function (o) return "("..o.x..","..o.y..","..o.z..")" end,
		__index    = {
			x=1, y=2, z=3
		},
	}
	local obj = Object {}
    lu.assertNotNil(obj)
	lu.assertEquals(obj.x, 1)
	lu.assertEquals(obj.y, 2)
	lu.assertEquals(obj.z, 3)
	lu.assertNil(obj.nothing)
	lu.assertEquals(tostring(obj), "(1,2,3)")

	local obj2 = Object {x=11, other=22}
    lu.assertNotNil(obj2)
	lu.assertEquals(obj2.x, 11)
	lu.assertEquals(obj.x, 1)
	lu.assertEquals(obj2.y, 2)
	lu.assertEquals(obj2.z, 3)
	lu.assertEquals(obj2.other, 22)
	lu.assertNil(obj2.nothing)
	lu.assertEquals(tostring(obj2), "(11,2,3)")

	local obj3 = Object {x=1111, other=444}
    lu.assertNotNil(obj3)
	lu.assertEquals(obj3.x, 1111)
	lu.assertEquals(obj.x, 1)
	lu.assertEquals(obj2.x, 11)
	lu.assertEquals(obj3.y, 2)
	lu.assertEquals(obj3.z, 3)
	lu.assertEquals(obj3.other, 444)
	lu.assertEquals(obj2.other, 22)
	lu.assertEquals(tostring(obj3), "(1111,2,3)")

end

function TestLuaOO:TestLuaOOClassIV(  )
	local Object = bp.oo.class {
		type = "Object",
		__tostring = function (o) return "("..o[1].." "..o[2].." "..o[3]..")" end,
		__index    = {
			11, 22, 33
		},
	}
	local obj = Object {}
    lu.assertNotNil(obj)
	lu.assertEquals(obj[1], 11)
	lu.assertEquals(obj[2], 22)
	lu.assertEquals(obj[3], 33)
    lu.assertNil(obj[4])
	lu.assertEquals(tostring(obj), "(11 22 33)")

	local obj2 = Object {[2]=2, [3]=333, [5]=-1}
    lu.assertNotNil(obj2)
	lu.assertEquals(obj2[1], 11)
	lu.assertEquals(obj2[2], 2)
	lu.assertEquals(obj[2], 22)
	lu.assertEquals(obj2[3], 333)
	lu.assertEquals(obj[3], 33)
    lu.assertNil(obj2[4])
	lu.assertEquals(obj2[5], -1)
	lu.assertEquals(tostring(obj2), "(11 2 333)")

	local obj3 = Object {nil, 22222, nil, 44444}
    lu.assertNotNil(obj3)
	lu.assertEquals(obj3[1], 11)
	lu.assertEquals(obj3[2], 22222)
	lu.assertEquals(obj3[3], 33)
	lu.assertEquals(obj3[4], 44444)
	lu.assertEquals(tostring(obj3), "(11 22222 33)")
end

-- function TestLuaOO:TestLuaOOName(  )
-- 	local cls = bp.oo.class_by_name("cls")
-- 	cls.int = 1

-- 	local obj = cls{}
--     lu.assertNotNil(obj)
-- 	lu.assertEquals(obj.int, 1)
-- end

function TestLuaOO:TestLuaOOClassOver(  )
	local parent_mt = {
		parent_value = 1, common_val = "common_val_parent",
	}
	local parent = bp.oo.class({type="parent", __index=parent_mt})

	local child_mt = {
		child_value = 2, common_val = "common_val_child"
	}
	local child = bp.oo.class_over(parent)({type="child", __index=child_mt})
	local obj = child {}
    lu.assertNotNil(obj)
	lu.assertEquals(bp.oo.type(obj), "child")
	lu.assertEquals(obj.parent_value, 1)
	lu.assertEquals(obj.child_value, 2)
	lu.assertEquals(obj.common_val, "common_val_child")
	obj.new_child_val = 3
	lu.assertEquals(obj.new_child_val, 3)

	local obj2 = child {}
	lu.assertEquals(bp.oo.type(obj2), "child")
	lu.assertNil(obj2.new_child_val, nil)

	local obj_parent = parent {}
    lu.assertNotNil(obj_parent)
	lu.assertEquals(bp.oo.type(obj_parent), "parent")
	lu.assertEquals(obj_parent.parent_value, 1)
	lu.assertEquals(obj_parent.common_val, "common_val_parent")
	lu.assertNil(obj_parent.new_child_val, nil)
end