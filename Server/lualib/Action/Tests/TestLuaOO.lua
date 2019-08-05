local Ac = require("Action")

TestLuaOO = {}

function TestLuaOO:setUp(  )
end

function TestLuaOO:tearDown(  )
end

function TestLuaOO:TestLuaOOClassKV(  )
	local Object = Ac.OO.Class {
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
	local Object = Ac.OO.Class {
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

function TestLuaOO:TestLuaOOType(  )
end

function TestLuaOO:TestLuaOOClassOver(  )
end