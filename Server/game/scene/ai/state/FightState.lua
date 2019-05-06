local BP = require("Blueprint")
local time = require "game.scene.time"

local FightState = BP.BaseClass(BP.FSM.FSMState)

local SubState = {
	Idle = 1,
	Walk = 2,
}
function FightState:OnInit(  )
	self.idle_time = 0
	print('Cat:patrol_state.lua[OnInit]')
end

function FightState:OnEnter(  )
	print('Cat:FightState.lua[OnEnter]')
	self.aoi = self.blackboard:GetVariable("aoi")
	self.aoi_handle = self.blackboard:GetVariable("aoi_handle")
	self.aoi_area = self.blackboard:GetVariable("aoi_area")
	self:SetSubState(SubState.Idle)
end

function FightState:OnUpdate( deltaTime )
	if self.sub_state == SubState.Idle then
		-- idle-站着一段时间什么都不干
		if Time.time - self.sub_elapsed_time > self.idle_time then
			self:SetSubState(SubState.Walk)
		end
	elseif self.sub_state == SubState.Walk then
		-- 前往下个盯哨点
	end
	self:CheckAround()
end

--主动打人的怪需要经常判断附近有没人
function FightState:CheckAround(  )
	if not self.last_check_around or Time.time - self.last_check_around > 1.5 then
		self.last_check_around = Time.time
		local around = self.aoi:get_around_offset(self.aoi_handle, self.aoi_area, self.aoi_area)
		--发现敌人了，进入战斗状态
		if next(around) ~= nil then
			-- self.fsm:TriggerState("Fighting")
		end
	end
end

function FightState:SetSubState( sub_state )
	self.sub_state = sub_state
	self.sub_elapsed_time = Time.time
	if sub_state == SubState.Idle then
		--随机一个发呆时间
		self.idle_time = math.random(2,5)
	end
end

function FightState:OnExit(  )
end

function FightState:OnPause(  )
end

return FightState