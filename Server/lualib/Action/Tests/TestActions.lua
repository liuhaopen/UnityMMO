local Ac = require("Action")

TestActions = {}

function TestActions:TestCallFunc(  )
	local testValue = 1
	local action = Ac.CallFunc {function(data)
		testValue = data.value
	end}
	action:Start({value=1234})
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testValue, 1234)
end

function TestActions:TestDelay(  )
	local action = Ac.Delay{5000}
	action:Start()
	lu.assertFalse(action:IsDone())
	action:Update(1000)
	lu.assertFalse(action:IsDone())
	action:Update(4000)
	lu.assertTrue(action:IsDone())

	local action = Ac.Delay{5000}
	action:Start()
	lu.assertFalse(action:IsDone())
	action:Update(6000)
	lu.assertTrue(action:IsDone())
end

function TestActions:TestIf(  )
	local check = function()
		return false
	end
	local trueAction = Ac.CallFunc {function(data)
		data.value = 22222
	end}
	local FalseAction = Ac.OO.Class {
		__index = {
			Start = function(self, data)
				self.data = data
			end,
			IsDone = function(self)
				return true
			end,
			Update = function(self)
				self.data.value = 333333
			end
		}
	}
	local action = Ac.If {check, trueAction, FalseAction{}}
	local testTbl = { value = 1 }
	action:Start(testTbl)
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, 333333)

	local action = Ac.If {check, trueAction}
	local testTbl = { value = 1 }
	action:Start(testTbl)
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, 1)
end

function TestActions:TestSequence(  )
	local actionA = Ac.CallFunc {function(data)
		data.value = "a"
	end}
	local actionB = Ac.CallFunc {function(data)
		data.value = "b"
	end}
	local actionC = Ac.CallFunc {function(data)
		data.value = "c"
	end}
	local action = Ac.Sequence{ actionA, Ac.Delay{1000}, actionB, actionC }
	local testTbl = {value = "0"}
	action:Start(testTbl)
	lu.assertFalse(action:IsDone())
	action:Update(500)
	lu.assertFalse(action:IsDone())
	lu.assertEquals(testTbl.value, "a")
	--在Sequence里的Delay,就算是时间到了，也要下次Update才会运行Delay后面的Action
	action:Update(500)
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, "c")

	local action = Ac.Sequence{ actionA, Ac.Delay{1000}, actionB, Ac.Delay{1000}, actionC }
	local testTbl = {value = "0"}
	action:Start(testTbl)
	action:Update(1000)
	lu.assertEquals(testTbl.value, "b")
	lu.assertFalse(action:IsDone())
	action:Update(1000)
	lu.assertEquals(testTbl.value, "c")
	lu.assertTrue(action:IsDone())

	--最后一个是Delay
	local action = Ac.Sequence{ actionA, Ac.Delay{1000} }
	local testTbl = {value = "0"}
	action:Start(testTbl)
	action:Update(500)
	lu.assertEquals(testTbl.value, "a")
	lu.assertFalse(action:IsDone())
	action:Update(500)
	lu.assertTrue(action:IsDone())

	--测试Delay被If包裹后会不会被漏掉
	local action = Ac.Sequence{ actionA, Ac.If{ Ac.Random{10000}, Ac.Delay{1000}, Ac.Delay{1000}}, actionB, Ac.Delay{1000}, actionC }
	local testTbl = {value = "0"}
	action:Start(testTbl)
	action:Update(1000)
	lu.assertEquals(testTbl.value, "b")
	action:Update(1000)
	lu.assertEquals(testTbl.value, "c")
end

function TestActions:TestRandom(  )
	local trueAction = Ac.CallFunc {function(data)
		data.value = "true"
	end}
	local falseAction = Ac.CallFunc {function(data)
		data.value = "false"
	end}
	local testTbl = {value = 1}
	local ifAction = Ac.If {Ac.Random{10000}, trueAction, falseAction}
	ifAction:Start(testTbl)
	ifAction:Update()
	lu.assertTrue(ifAction:IsDone())
	lu.assertEquals(testTbl.value, "true")

	testTbl.value = 1
	local ifAction = Ac.If {Ac.Random{0}, trueAction, falseAction}
	ifAction:Start(testTbl)
	ifAction:Update()
	lu.assertTrue(ifAction:IsDone())
	lu.assertEquals(testTbl.value, "false")
end

function TestActions:TestAnd(  )
	local conditionFuncA = function(data)
		local isTrue = data.value == 1
		data.value = 2
		return isTrue
	end
	local conditionFuncB = function(data)
		local isTrue = data.value == 2
		data.value = 3
		return isTrue
	end
	local trueAction = Ac.CallFunc {function(data)
		data.result = "true"
	end}
	local falseAction = Ac.CallFunc {function(data)
		data.result = "false"
	end}
	local action = Ac.If {Ac.And{conditionFuncA, conditionFuncB}, trueAction, falseAction}
	local testTbl = { value = 1 }
	action:Start(testTbl)
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, 3)
	lu.assertEquals(testTbl.result, "true")

	local action = Ac.If {Ac.And{conditionFuncA, conditionFuncB}, trueAction, falseAction}
	local testTbl = { value = 1111 }
	action:Start(testTbl)
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, 2)
	lu.assertEquals(testTbl.result, "false")
end

function TestActions:TestOr(  )
	local conditionFuncA = function(data)
		data.value = 2
		return data.conA
	end
	local conditionFuncB = function(data)
		data.value = 3
		return data.conB
	end
	local trueAction = Ac.CallFunc {function(data)
		data.result = "true"
	end}
	local falseAction = Ac.CallFunc {function(data)
		data.result = "false"
	end}
	local action = Ac.If {Ac.Or{conditionFuncA, conditionFuncB}, trueAction, falseAction}
	local testTbl = { value=1, conA=true, conB=false }
	action:Start(testTbl)
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, 2)
	lu.assertEquals(testTbl.result, "true")

	local action = Ac.If {Ac.Or{conditionFuncA, conditionFuncB}, trueAction, falseAction}
	local testTbl = { value=1, conA=false, conB=true }
	action:Start(testTbl)
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, 3)
	lu.assertEquals(testTbl.result, "true")

	local action = Ac.If {Ac.Or{conditionFuncA, conditionFuncB}, trueAction, falseAction}
	local testTbl = { value=1, conA=false, conB=false }
	action:Start(testTbl)
	action:Update()
	lu.assertTrue(action:IsDone())
	lu.assertEquals(testTbl.value, 3)
	lu.assertEquals(testTbl.result, "false")
end

function TestActions:TestRepeat(  )
	local funcAction = Ac.CallFunc {function(data)
		data.loop = data.loop + 1
	end}
	local action = Ac.Repeat {5, funcAction}
	local testTbl = {loop=0}
	action:Start(testTbl)
	lu.assertFalse(action:IsDone())
	for i=1,5 do
		lu.assertFalse(action:IsDone())
		action:Update()
		lu.assertEquals(testTbl.loop, i)
		if i ~= 5 then
			lu.assertFalse(action:IsDone())
		else
			lu.assertTrue(action:IsDone())
		end
	end

	local action = Ac.Repeat {4, Ac.Sequence { funcAction, Ac.Delay{100}} }
	local testTbl = {loop=0}
	action:Start(testTbl)
	action:Update(99)
	lu.assertEquals(testTbl.loop, 1)
	lu.assertFalse(action:IsDone())

	action:Update(1)
	lu.assertEquals(testTbl.loop, 1)
	lu.assertFalse(action:IsDone())

	action:Update(55)
	lu.assertEquals(testTbl.loop, 2)
	lu.assertFalse(action:IsDone())
	action:Update(2)
	lu.assertEquals(testTbl.loop, 2)
	lu.assertFalse(action:IsDone())
	action:Update(155)
	lu.assertEquals(testTbl.loop, 2)
	lu.assertFalse(action:IsDone())

	action:Update(555)
	lu.assertEquals(testTbl.loop, 3)
	lu.assertFalse(action:IsDone())

	action:Update(100)
	lu.assertEquals(testTbl.loop, 4)
	lu.assertTrue(action:IsDone())
end