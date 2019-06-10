local BP = require("Blueprint")

local DeadState = BP.BaseClass(BP.FSM.FSMState)

local SubState = {
	Catch = 1,--追捕
	Fighting = 2,--攻击
}
function DeadState:OnInit(  )
	print('Cat:DeadState.lua[OnInit]')
	self.aoi_handle = self.blackboard:GetVariable("aoi_handle")
	self.aoi_area = self.blackboard:GetVariable("aoi_area")
	self.entity = self.blackboard:GetVariable("entity")
	self.sceneMgr = self.blackboard:GetVariable("sceneMgr")
	self.aoi = self.sceneMgr.aoi
	self.entityMgr = self.sceneMgr.entityMgr
	self.monsterMgr = self.sceneMgr.monsterMgr
	self.cfg = self.blackboard:GetVariable("cfg")

	self.uid = self.entityMgr:GetComponentData(self.entity, "UMO.UID")
end

function DeadState:OnEnter(  )
	print('Cat:DeadState.lua[OnEnter]')
	
end

function DeadState:OnUpdate( deltaTime )

end

function DeadState:SetSubState( sub_state )
	self.sub_state = sub_state
	self.sub_elapsed_time = Time.time
	if sub_state == SubState.Idle then
	end
end

function DeadState:OnExit(  )
end

function DeadState:OnPause(  )
end

return DeadState