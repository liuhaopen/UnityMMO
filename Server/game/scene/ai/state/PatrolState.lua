local BP = require("Blueprint")
local Time = require "game.scene.time"

local PatrolState = BP.BaseClass(BP.FSM.FSMState)
--巡逻有两状态，1是发呆，2是跑去哨点，就是跑一下停一下，期间一旦发现敌人就切换战斗状态
local SubState = {
	Idle = 1,
	Walk = 2,
}
function PatrolState:OnInit(  )
	self.idle_time = 0
	self.walk_time = 0
	-- print('Cat:patrol_state.lua[OnInit]')
	self.aoi = self.blackboard:GetVariable("aoi")
	self.aoi_handle = self.blackboard:GetVariable("aoi_handle")
	self.aoi_area = self.blackboard:GetVariable("aoi_area")
	self.entity = self.blackboard:GetVariable("entity")
	self.entityMgr = self.blackboard:GetVariable("entityMgr")
	self.monsterMgr = self.blackboard:GetVariable("monsterMgr")
	self.patrolInfo = self.entityMgr:GetComponentData(self.entity, "umo.patrol_info")
end

function PatrolState:OnEnter(  )
	-- print('Cat:PatrolState.lua[OnEnter]')
	self:EnterSubState(SubState.Idle)
end

function PatrolState:OnUpdate( deltaTime )
	if self.sub_state == SubState.Idle then
		if Time.time - self.sub_elapsed_time > self.idle_time then
			--站着一段时间什么都不干，然后就开始巡逻
			self:EnterSubState(SubState.Walk)
		end
	elseif self.sub_state == SubState.Walk then
		local curPos = self.entityMgr:GetComponentData(self.entity, "umo.position")
		local targetPos = self.entityMgr:GetComponentData(self.entity, "umo.target_pos")
		local distanceFromTargetSqrt = Vector3.DistanceNoSqrt(targetPos, curPos)
		if distanceFromTargetSqrt < 100 and Time.time - self.sub_elapsed_time > self.walk_time then
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
		local radius = self.patrolInfo.radius/2
		local pos_x = self.patrolInfo.x + math.random(-radius, radius)
		local pos_y = self.patrolInfo.y + math.random(-radius, radius)
		local pos_z = self.patrolInfo.z + math.random(-radius, radius)
		local randomPos = {x=pos_x, y=pos_y, z=pos_z}
		-- self.entityMgr:SetComponentData(self.entity, "umo.target_pos", randomPos)
		self.monsterMgr:change_target_pos(self.entity, randomPos)
	end
end

function PatrolState:OnExit(  )
end

function PatrolState:OnPause(  )
end

return PatrolState