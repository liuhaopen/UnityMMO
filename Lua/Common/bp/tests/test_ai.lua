local bp = require("bp")

test_ai = {}

function test_ai:test_call_func(  )
	local testValue = 1
	local action = bp.call_func {function(graph)
		testValue = graph:get_data().value
	end}
	local graph = bp.graph {}
	graph:set_data({value=1234})
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testValue, 1234)
end

function test_ai:test_wait(  )
	local action = bp.wait{5000}
	action:start()
	lu.assertFalse(action:is_done())
	action:update(1000)
	lu.assertFalse(action:is_done())
	action:update(4000)
	lu.assertTrue(action:is_done())

	local action = bp.wait{5000}
	action:start()
	lu.assertFalse(action:is_done())
	action:update(6000)
	lu.assertTrue(action:is_done())
end

function test_ai:test_if_else(  )
	--测试：if_else进入true
	local check_ret_true = function()
		return true
	end
	local trueAction = bp.call_func {function(graph)
		graph:get_data().value = 2
	end}
	local action = bp.if_else {check_ret_true, trueAction}
	local testTbl = { value = 1 }
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, 2)

	--测试：if_else进入false
	local check_ret_false = function()
		return false
	end
	local FalseAction = bp.oo.class {
		__index = {
			start = function(self, graph)
				self.graph = graph
			end,
			is_done = function(self)
				return true
			end,
			update = function(self)
				self.graph:get_data().value = 333333
			end
		}
	}
	local action = bp.if_else {check_ret_false, trueAction, FalseAction{}}
	local testTbl = { value = 1 }
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, 333333)
end

local CheckerCallable = bp.oo.class {
	type 	= "checker",
	__index = {
		start = function(self, graph)
			self.graph = graph
		end,
	},
	__call = function(self)
		return self[1]
	end
}

local Checker = bp.oo.class {
	type 	= "checker",
	__index = {
		start = function(self, graph)
			self.graph = graph
		end,
		update = function(self)
			return self[1]
		end,
	},
}

function test_ai:test_if_else_with_checker()
	--测试：if_else的是否会调用条件节点的start函数
	local actionA = bp.call_func {function(graph)
		graph:get_data().value = "a"
	end}
	local action = bp.if_else {CheckerCallable{true}, actionA}
	local testTbl = {  }
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, "a")

	local action = bp.if_else {Checker{true}, actionA}
	local testTbl = {  }
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, "a")
end

function test_ai:test_sequence(  )
	local actionA = bp.call_func {function(graph)
		graph:get_data().value = "a"
	end}
	local actionB = bp.call_func {function(graph)
		graph:get_data().value = "b"
	end}
	local actionC = bp.call_func {function(graph)
		graph:get_data().value = "c"
	end}
	local action = bp.sequence{ actionA, bp.wait{1000}, actionB, actionC }
	local testTbl = {value = "0"}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	lu.assertFalse(action:is_done())
	action:update(500)
	lu.assertFalse(action:is_done())
	lu.assertEquals(testTbl.value, "a")
	--在sequence里的delay,就算是时间到了，也要下次Update才会运行delay后面的Action
	action:update(500)
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, "c")

	local action = bp.sequence{ actionA, bp.wait{1000}, actionB, bp.wait{1000}, actionC }
	local testTbl = {value = "0"}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update(1000)
	lu.assertEquals(testTbl.value, "b")
	lu.assertFalse(action:is_done())
	action:update(1000)
	lu.assertEquals(testTbl.value, "c")
	lu.assertTrue(action:is_done())

	--最后一个是delay
	local action = bp.sequence{ actionA, bp.wait{1000} }
	local testTbl = {value = "0"}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update(500)
	lu.assertEquals(testTbl.value, "a")
	lu.assertFalse(action:is_done())
	action:update(500)
	lu.assertTrue(action:is_done())

	--测试delay被If包裹后会不会被漏掉
	local action = bp.sequence{ actionA, bp.if_else{ bp.random{10000}, bp.wait{1000}, bp.wait{1000}}, actionB, bp.wait{1000}, actionC }
	local testTbl = {value = "0"}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update(1000)
	lu.assertEquals(testTbl.value, "b")
	action:update(1000)
	lu.assertEquals(testTbl.value, "c")
end

function test_ai:test_random(  )
	local trueAction = bp.call_func {function(graph)
		graph:get_data().value = "true"
	end}
	local falseAction = bp.call_func {function(graph)
		graph:get_data().value = "false"
	end}
	local testTbl = {value = 1}
	local ifAction = bp.if_else {bp.random{10000}, trueAction, falseAction}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	ifAction:start(graph)
	ifAction:update()
	lu.assertTrue(ifAction:is_done())
	lu.assertEquals(testTbl.value, "true")

	testTbl.value = 1
	local ifAction = bp.if_else {bp.random{0}, trueAction, falseAction}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	ifAction:start(graph)
	ifAction:update()
	lu.assertTrue(ifAction:is_done())
	lu.assertEquals(testTbl.value, "false")
end

function test_ai:test_all_true(  )
	local is_value_1_and_set_2 = function(graph)
		local data = graph:get_data()
		local isTrue = data.value == 1
		data.value = 2
		return isTrue
	end
	local is_value_2_and_set_3 = function(graph)
		local data = graph:get_data()
		local isTrue = data.value == 2
		data.value = 3
		return isTrue
	end
	local set_result_true = bp.call_func {function(graph)
		local data = graph:get_data()
		data.result = "true"
	end}
	local set_result_false = bp.call_func {function(graph)
		local data = graph:get_data()
		data.result = "false"
	end}
	local action = bp.if_else {bp.all_true{is_value_1_and_set_2, is_value_2_and_set_3}, set_result_true, set_result_false}
	local testTbl = { value = 1 }
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, 3)
	lu.assertEquals(testTbl.result, "true")

	local action = bp.if_else {bp.all_true{is_value_1_and_set_2, is_value_2_and_set_3}, set_result_true, set_result_false}
	local testTbl = { value = 1111 }
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, 2)
	lu.assertEquals(testTbl.result, "false")

	local action = bp.if_else {bp.all_true{Checker{true}, bp.get_bb{"bb_value"}}, set_result_true, set_result_false}
	local testTbl = { value = 1 }
	local graph = bp.graph {}
	graph:set_bb("bb_value", true)
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.value, 1)
	lu.assertEquals(testTbl.result, "true")
end

function test_ai:test_any_true(  )
	local conditionTrue = function(graph)
		return true
	end
	local conditionFalse = function(graph)
		return false
	end
	local conditionNil = function(graph)
		return nil
	end
	local trueAction = bp.call_func {function(graph)
		graph:get_data().result = "true"
	end}
	local falseAction = bp.call_func {function(graph)
		graph:get_data().result = "false"
	end}
	local action = bp.if_else {bp.any_true{conditionTrue, conditionFalse}, trueAction, falseAction}
	local testTbl = {}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.result, "true")

	action = bp.if_else {bp.any_true{conditionFalse, conditionTrue}, trueAction, falseAction}
	testTbl = {}
	graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.result, "true")

	action = bp.if_else {bp.any_true{conditionFalse, conditionNil, conditionFalse}, trueAction, falseAction}
	testTbl = {}
	graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update()
	lu.assertTrue(action:is_done())
	lu.assertEquals(testTbl.result, "false")
end

function test_ai:test_loop(  )
	local funcAction = bp.call_func {function(graph)
		local data = graph:get_data()
		data.loop = data.loop + 1
	end}
	local action = bp.loop {5, funcAction}
	local testTbl = {loop=0}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	lu.assertFalse(action:is_done())
	for i=1,5 do
		lu.assertFalse(action:is_done())
		action:update()
		lu.assertEquals(testTbl.loop, i)
		if i ~= 5 then
			lu.assertFalse(action:is_done())
		else
			lu.assertTrue(action:is_done())
		end
	end

	local action = bp.loop {4, bp.sequence { funcAction, bp.wait{100}} }
	local testTbl = {loop=0}
	local graph = bp.graph {}
	graph:set_data(testTbl)
	action:start(graph)
	action:update(99)
	lu.assertEquals(testTbl.loop, 1)
	lu.assertFalse(action:is_done())

	action:update(1)
	lu.assertEquals(testTbl.loop, 1)
	lu.assertFalse(action:is_done())

	action:update(55)
	lu.assertEquals(testTbl.loop, 2)
	lu.assertFalse(action:is_done())
	action:update(2)
	lu.assertEquals(testTbl.loop, 2)
	lu.assertFalse(action:is_done())
	action:update(155)
	lu.assertEquals(testTbl.loop, 2)
	lu.assertFalse(action:is_done())

	action:update(555)
	lu.assertEquals(testTbl.loop, 3)
	lu.assertFalse(action:is_done())

	action:update(100)
	lu.assertEquals(testTbl.loop, 4)
	lu.assertTrue(action:is_done())
end

function test_ai:test_loop_infinite(  )
	local funcAction = bp.call_func {function(graph)
		local data = graph:get_data()
		data.loop = data.loop + 1
	end}
	local act = bp.loop {-1, bp.sequence { funcAction, bp.wait{2}}}
	local data = {loop=0}
	local graph = bp.graph {}
	graph:set_data(data)
	act:start(graph)
	for i=1,10 do
		act:update(1)
		lu.assertEquals(data.loop, i)
		act:update(1)
	end
end

function test_ai:test_parallel()
	--测试：同时跑几个 action
	local run_num = 4
	local acts = {}
	for i=1,run_num do
		local funcAction = bp.call_func {function(graph)
			local data = graph:get_data()
			data.test_num = data.test_num + 1
		end}
		acts[#acts + 1] = funcAction
	end
	local act = bp.parallel(acts)
	local data = {test_num=0}
	local graph = bp.graph {}
	graph:set_data(data)
	act:start(graph)
	act:update(1)
	lu.assertEquals(data.test_num, run_num)
	lu.assertTrue(act:is_done())
end

function test_ai:test_parallel_with_wait()
	--测试：同时跑几个 action，其中有一个等待节点，需要等该节点完成才算真正完成
	local run_num = 4
	local acts = {}
	for i=1,run_num do
		local funcAction = bp.call_func {function(graph)
			local data = graph:get_data()
			data.test_num = data.test_num + 1
		end}
		acts[#acts + 1] = funcAction
	end
	table.insert(acts, 2, bp.wait {100})
	local act = bp.parallel(acts)
	local data = {test_num=0}
	local graph = bp.graph {}
	graph:set_data(data)
	act:start(graph)
	act:update(10)
	lu.assertEquals(data.test_num, run_num)
	lu.assertFalse(act:is_done())
	act:update(50)
	lu.assertEquals(data.test_num, run_num)
	lu.assertFalse(act:is_done())
	act:update(40)
	lu.assertEquals(data.test_num, run_num)
	lu.assertTrue(act:is_done())
end

function test_ai:test_bd()
	local act = bp.sequence { 
		bp.set_bb {"test_bd_1", 1},
		bp.set_bb {"test_bd_2", true},
		bp.set_bb {"test_bd_3", "test_bd_3"},
	}
	local graph = bp.graph {}
	graph:set_data(data)
	act:start(graph)
	act:update(10)

	lu.assertEquals(graph:get_bb("test_bd_1"), 1)
	lu.assertEquals(graph:get_bb("test_bd_2"), true)
	lu.assertEquals(graph:get_bb("test_bd_3"), "test_bd_3")
end

local ReturnNum = bp.oo.class {
	type 	= "ReturnNum",
	__index = {
		start = function(self, graph)
			self.graph = graph
		end,
		update = function(self, dt)
			return self.return_num
		end,
	},
}
local ReturnNum = bp.oo.class {
	type 	= "ReturnNum",
	__index = {
		start = function(self, graph)
			self.graph = graph
		end,
		update = function(self, dt)
			return self.return_num
		end,
	},
}
function test_ai:test_operator()
	local trueAction = bp.call_func {function(graph)
		graph:get_data().value = 1
	end}
	local falseAction = bp.call_func {function(graph)
		graph:get_data().value = 2
	end}
	--测试：not
	local act = bp.if_else {
		bp.operator {"not", false},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：大于
	local act = bp.if_else {
		bp.operator {">", 22, 11},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：小于
	local act = bp.if_else {
		bp.operator {"<", 11, 22},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：等于
	local act = bp.if_else {
		bp.operator {"==", ReturnNum{return_num=11}, 11},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：小于等于
	local act = bp.if_else {
		bp.operator {"<=", 11, ReturnNum{return_num=22}},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：大于等于
	local act = bp.if_else {
		bp.operator {">=", ReturnNum{return_num=22}, ReturnNum{return_num=11}},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：not nil
	local act = bp.if_else {
		bp.operator {"not", ReturnNum{return_num=nil}},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：传入updater
	local act = bp.if_else {
		bp.operator {"not", Checker{false}},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)

	--测试：传入callable
	local act = bp.if_else {
		bp.operator {"not", CheckerCallable{false}},
		trueAction,
		falseAction
	}
	local graph = bp.graph {}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update()
	lu.assertEquals(data.value, 1)
end

function test_ai:test_slot_action()
	local SlotName = "SlotName1"
	local graph = bp.graph {}
	local fake_act_out = {}
	local fake_act_in = {}
	graph:register_slot(fake_act_out, "out", 1, SlotName)
	graph:register_slot(fake_act_in, "in", 1, SlotName)
	graph:set_slot_value(fake_act_out, 1, "haha")
	local slot_value = graph:get_slot_value(fake_act_in, 1)
	lu.assertEquals(slot_value, "haha")

	local SlotName = "SlotName2"
	local graph = bp.graph {}
	local get_bb_value = bp.get_bb {"test_bd_key"}
	--注册孔是为了在 get_bb 内部的孔是按顺序的数字作 key，只需要调用 self.graph:set_slot_value(1, get到的某值)
	graph:register_slot(get_bb_value, "out", 1, SlotName)

	local op_not_act = bp.operator {"not"}
	graph:register_slot(op_not_act, "in", 1, SlotName)
	local act = bp.sequence {
		bp.set_bb {"test_bd_key", true},
		get_bb_value,
		bp.if_else {
			op_not_act,
			nil,
			bp.call_func {function(graph)
				graph:get_data().value = 1
			end}
		},
	}
	local data = {value=0}
	graph:set_data(data)
	act:start(graph)
	act:update(1)
	lu.assertEquals(data.value, 1)
end