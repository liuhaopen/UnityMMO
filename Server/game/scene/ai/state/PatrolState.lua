local BP = require("Blueprint")
local time = require "game.scene.time"

local PatrolState = BP.BaseClass(BP.FSM.FSMState)

local SubState = {
	Idle = 1,
	Walk = 2,
}
function PatrolState:OnInit(  )
	self.idle_time = 0
	print('Cat:patrol_state.lua[OnInit]')
end

function PatrolState:OnEnter(  )
	print('Cat:PatrolState.lua[OnEnter]')
	self.aoi = self.blackboard:GetVariable("aoi")
	self.aoi_handle = self.blackboard:GetVariable("aoi_handle")
	self.aoi_area = self.blackboard:GetVariable("aoi_area")
	self:EnterSubState(SubState.Idle)
end

function PatrolState:OnUpdate( deltaTime )
	if self.sub_state == SubState.Idle then
		if Time.time - self.sub_elapsed_time > self.idle_time then
			--站着一段时间什么都不干，然后就开始巡逻
			self:EnterSubState(SubState.Walk)
		end
	elseif self.sub_state == SubState.Walk then
		local isReachTargetPos = true
		if isReachTargetPos and Time.time - self.sub_elapsed_time > self.walk_time then
			--到达巡逻点时就进入待机状态
			self:EnterSubState(SubState.Idle)
		end
	end
	self:CheckAround()
end

--主动打人的怪需要经常判断附近有没人
function PatrolState:CheckAround(  )
	if not self.last_check_around or Time.time - self.last_check_around > 1.5 then
		self.last_check_around = Time.time
		local around = self.aoi:get_around_offset(self.aoi_handle, self.aoi_area, self.aoi_area)
		--发现敌人了，进入战斗状态
		if next(around) ~= nil then
			-- self.fsm:TriggerState("Fight")
		end
	end
end

function PatrolState:EnterSubState( sub_state )
	self.sub_state = sub_state
	self.sub_elapsed_time = Time.time
	if sub_state == SubState.Idle then
		--随机一个发呆时间
		self.idle_time = math.random(2,5)
	elseif sub_state == SubState.Walk then
		--随机跑去某点
		self.walk_time = 1
	end
end

function PatrolState:OnExit(  )
end

function PatrolState:OnPause(  )
end

return PatrolState