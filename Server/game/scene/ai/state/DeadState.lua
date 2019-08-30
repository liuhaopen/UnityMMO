local BP = require("Blueprint")
local SceneConst = require "game.scene.SceneConst"
local SceneHelper = require "game.scene.SceneHelper"
local Time = Time
local DeadState = BP.BaseClass(BP.FSM.FSMState)

local SubState = {
	Catch = 1,--追捕
	Fighting = 2,--攻击
}
function DeadState:OnInit(  )
	-- print('Cat:DeadState.lua[OnInit]')
	self.entity = self.blackboard:GetVariable("entity")
	self.sceneMgr = self.blackboard:GetVariable("sceneMgr")
	self.entityMgr = self.sceneMgr.entityMgr
	self.monsterMgr = self.sceneMgr.monsterMgr
	self.cfg = self.blackboard:GetVariable("cfg")
	self.patrolInfo = self.entityMgr:GetComponentData(self.entity, "UMO.PatrolInfo")

	self.uid = self.entityMgr:GetComponentData(self.entity, "UMO.UID")
end

function DeadState:OnEnter(  )
	-- print('Cat:DeadState.lua[OnEnter]')
	self.dead_time = Time.time
	self.wait_for_relive = self.cfg.ai.reborn_time/1000
end

function DeadState:Relive(  )
	local hpData = self.entityMgr:GetComponentData(self.entity, "UMO.HP")
	hpData.cur = hpData.max
	local change_target_pos_event_info = {key=SceneConst.InfoKey.HPChange, value=math.floor(hpData.cur)..",relive", time=Time.timeMS}
	self.sceneMgr.eventMgr:AddSceneEvent(self.uid, change_target_pos_event_info)
	--set a new position
	local radius = self.patrolInfo.radius/2
	local randomPos = {
		x=self.patrolInfo.x + math.random(-radius, radius), 
		y=self.patrolInfo.y + math.random(-radius, radius), 
		z=self.patrolInfo.z + math.random(-radius, radius)
	}
	SceneHelper.ChangePos(self.entity, randomPos, self.entityMgr, self.sceneMgr.eventMgr)

	self.fsm:TriggerState("PatrolState")
end

function DeadState:OnUpdate( )
	if Time.time - self.dead_time > self.wait_for_relive then
		self:Relive()
	end
end

function DeadState:OnExit(  )
end

function DeadState:OnPause(  )
end

return DeadState