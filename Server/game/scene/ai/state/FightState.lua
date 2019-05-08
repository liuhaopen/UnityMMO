local BP = require("Blueprint")
local time = require "game.scene.time"

local FightState = BP.BaseClass(BP.FSM.FSMState)

local SubState = {
	Catch = 1,--追捕
	Fighting = 2,--攻击
}
function FightState:OnInit(  )
	print('Cat:FightState.lua[OnInit]')
end

function FightState:OnEnter(  )
	print('Cat:FightState.lua[OnEnter]')
	self.aoi = self.blackboard:GetVariable("aoi")
	self.aoi_handle = self.blackboard:GetVariable("aoi_handle")
	self.aoi_area = self.blackboard:GetVariable("aoi_area")
	self.entity = self.blackboard:GetVariable("entity")
	self.entityMgr = self.blackboard:GetVariable("entityMgr")
	self.monsterMgr = self.blackboard:GetVariable("monsterMgr")
end

function FightState:OnUpdate( deltaTime )
	
end

function FightState:SetSubState( sub_state )
	self.sub_state = sub_state
	self.sub_elapsed_time = Time.time
	if sub_state == SubState.Idle then
	end
end

function FightState:OnExit(  )
end

function FightState:OnPause(  )
end

return FightState