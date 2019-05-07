local BP = require("Blueprint")

TestFSM = {}

function TestFSM:setUp(  )
end
function TestFSM:tearDown(  )
end

local FSMSampleState = BP.BaseClass(BP.FSM.FSMState)
local FSMSampleState_Two = BP.BaseClass(BP.FSM.FSMState)

function TestFSM:TestFSMFSM(  )
	BP.TypeManager:RegisterType("Blueprint.State.FSMSampleState", FSMSampleState)
	BP.TypeManager:RegisterType("Blueprint.State.FSMSampleState_Two", FSMSampleState_Two)

	local data = {
		nodes = {
			{
				id = 1,
				type = "Blueprint.State.FSMSampleState",
				name = "State_One",
			},
			{
				id = 2,
				type = "Blueprint.State.FSMSampleState_Two",
				name = "State_Two",
			},
		},
	}
	local fsm = BP.FSM.FSMGraph.Create(data)
    lu.assertNotNil(fsm)
    lu.assertEquals(TableSize(fsm.nodes), 2)
    
    local owner = BP.GraphsOwner.Create()
    lu.assertNotNil(owner)
    owner:AddGraph(fsm)

    local bb = owner:GetBlackboard()
    lu.assertNotNil(bb)

    bb:SetVariable("initNum", 0)
    bb:SetVariable("enterNum", 0)
    bb:SetVariable("updateNum", 0)
    bb:SetVariable("pauseNum", 0)
    bb:SetVariable("stopNum", 0)
    bb:SetVariable("initNum_Two", 0)
    bb:SetVariable("enterNum_Two", 0)
    bb:SetVariable("updateNum_Two", 0)
    bb:SetVariable("pauseNum_Two", 0)
    bb:SetVariable("stopNum_Two", 0)
    owner:Start()
    lu.assertEquals(bb:GetVariable("initNum"), 1)
    lu.assertEquals(bb:GetVariable("enterNum"), 1)
    lu.assertEquals(bb:GetVariable("initNum_Two"), 0)
    lu.assertEquals(bb:GetVariable("enterNum_Two"), 0)

    owner:Update()
    lu.assertEquals(bb:GetVariable("updateNum"), 1)
    owner:Update()
    lu.assertEquals(bb:GetVariable("updateNum"), 2)
    lu.assertEquals(bb:GetVariable("updateNum_Two"), 0)

    --暂停状态机
    owner:Pause()
    lu.assertEquals(bb:GetVariable("pauseNum"), 1)
    lu.assertEquals(bb:GetVariable("pauseNum_Two"), 0)

    --因为已经pause掉了，所以不会再进入状态机的update
    owner:Update()
    lu.assertEquals(bb:GetVariable("updateNum"), 2)
    lu.assertEquals(bb:GetVariable("updateNum_Two"), 0)

    --暂停后继续是不会再次进入OnInit和OnEnter函数的
    owner:Start()
    lu.assertEquals(bb:GetVariable("initNum"), 1)
    lu.assertEquals(bb:GetVariable("enterNum"), 1)
    lu.assertEquals(bb:GetVariable("initNum_Two"), 0)
    lu.assertEquals(bb:GetVariable("enterNum_Two"), 0)

    --停止状态机
    owner:Stop()
    --Cat_Todo : --状态停止时，当前状态会进入两次OnExit函数，测试了NodeCanvas库也是这样的，有空再追究下
    lu.assertEquals(bb:GetVariable("stopNum"), 2)
    lu.assertEquals(bb:GetVariable("stopNum_Two"), 0)

    --停止后继续是不会再次进入OnInit函数的，但OnEnter是会再次进入的
    owner:Start()
    lu.assertEquals(bb:GetVariable("initNum"), 1)
    lu.assertEquals(bb:GetVariable("enterNum"), 2)

    --切换状态
    fsm:TriggerState("State_Two")
    lu.assertEquals(bb:GetVariable("initNum_Two"), 1)
    lu.assertEquals(bb:GetVariable("enterNum_Two"), 1)
    lu.assertEquals(bb:GetVariable("initNum"), 1)
    lu.assertEquals(bb:GetVariable("enterNum"), 2)

    owner:Update()
    lu.assertEquals(bb:GetVariable("updateNum_Two"), 1)
    owner:Update()
    lu.assertEquals(bb:GetVariable("updateNum_Two"), 2)
    owner:Update()
    lu.assertEquals(bb:GetVariable("updateNum_Two"), 3)
    lu.assertEquals(bb:GetVariable("updateNum"), 2)

    --暂停状态机
    owner:Pause()
    lu.assertEquals(bb:GetVariable("pauseNum"), 1)
    lu.assertEquals(bb:GetVariable("pauseNum_Two"), 1)
    --停止后继续是不会再次进入OnInit函数的，但OnEnter是会再次进入的
    owner:Start()
    lu.assertEquals(bb:GetVariable("initNum"), 1)
    lu.assertEquals(bb:GetVariable("enterNum"), 2)
    lu.assertEquals(bb:GetVariable("initNum_Two"), 1)
    lu.assertEquals(bb:GetVariable("enterNum_Two"), 1)

    --停止状态机
    owner:Stop()
    lu.assertEquals(bb:GetVariable("stopNum_Two"), 2)
    lu.assertEquals(bb:GetVariable("stopNum"), 3)
    --停止状态机后再开始，默认会跳到第1个状态
    owner:Start()
    lu.assertEquals(bb:GetVariable("initNum_Two"), 1)
    lu.assertEquals(bb:GetVariable("enterNum_Two"), 1)
    lu.assertEquals(bb:GetVariable("initNum"), 1)
    lu.assertEquals(bb:GetVariable("enterNum"), 3)
end

function FSMSampleState:OnInit(  )
	local initNum = self.blackboard:GetVariable("initNum")
	self.blackboard:SetVariable("initNum", initNum+1)
end

function FSMSampleState:OnEnter(  )
	local enterNum = self.blackboard:GetVariable("enterNum")
	self.blackboard:SetVariable("enterNum", enterNum+1)
end

function FSMSampleState:OnUpdate( deltaTime )
	local updateNum = self.blackboard:GetVariable("updateNum")
	self.blackboard:SetVariable("updateNum", updateNum+1)
end

function FSMSampleState:OnPause(  )
	local pauseNum = self.blackboard:GetVariable("pauseNum")
	self.blackboard:SetVariable("pauseNum", pauseNum+1)
end

function FSMSampleState:OnExit(  )
	local stopNum = self.blackboard:GetVariable("stopNum")
	self.blackboard:SetVariable("stopNum", stopNum+1)
end

----------Two

function FSMSampleState_Two:OnInit(  )
	local initNum_Two = self.blackboard:GetVariable("initNum_Two")
	self.blackboard:SetVariable("initNum_Two", initNum_Two+1)
end

function FSMSampleState_Two:OnEnter(  )
	local enterNum_Two = self.blackboard:GetVariable("enterNum_Two")
	self.blackboard:SetVariable("enterNum_Two", enterNum_Two+1)
end

function FSMSampleState_Two:OnUpdate( deltaTime )
	local updateNum_Two = self.blackboard:GetVariable("updateNum_Two")
	self.blackboard:SetVariable("updateNum_Two", updateNum_Two+1)
end

function FSMSampleState_Two:OnPause(  )
	local pauseNum_Two = self.blackboard:GetVariable("pauseNum_Two")
	self.blackboard:SetVariable("pauseNum_Two", pauseNum_Two+1)
end

function FSMSampleState_Two:OnExit(  )
	local stopNum_Two = self.blackboard:GetVariable("stopNum_Two")
	self.blackboard:SetVariable("stopNum_Two", stopNum_Two+1)
end

return TestFSM