local BP = require("Blueprint")
local FSMSampleState = BP.BaseClass(BP.FSM.FSMState)

function FSMSampleState:OnInit(  )
	print('Cat:FSMSampleState.lua[OnInit]')
end

function FSMSampleState:OnEnter(  )
	print('Cat:FSMSampleState.lua[OnEnter]')
end

function FSMSampleState:OnUpdate( deltaTime )
	print('Cat:FSMSampleState.lua[24] deltaTime', deltaTime)
end

function FSMSampleState:OnExit(  )
	print('Cat:FSMSampleState.lua[OnExit]')
end

function FSMSampleState:OnPause(  )
	print('Cat:FSMSampleState.lua[OnPause]')
end

return FSMSampleState