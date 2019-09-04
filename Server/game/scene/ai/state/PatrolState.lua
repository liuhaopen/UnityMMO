local BP = require("Blueprint")
local SceneHelper = require "game.scene.SceneHelper"
local SceneConst = require "game.scene.SceneConst"
local FightHelper = require("game.scene.FightHelper")

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
	self.aoi_handle = self.blackboard:GetVariable("aoi_handle")
	self.entity = self.blackboard:GetVariable("entity")
	self.sceneMgr = self.blackboard:GetVariable("sceneMgr")
	self.aoi = self.sceneMgr.aoi
	self.entityMgr = self.sceneMgr.entityMgr
	self.monsterMgr = self.sceneMgr.monsterMgr
	self.cfg = self.blackboard:GetVariable("cfg")
	self.patrolInfo = self.entityMgr:GetComponentData(self.entity, "UMO.PatrolInfo")
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
		if Time.time - self.sub_elapsed_time > self.walk_time then
			local curPos = self.entityMgr:GetComponentData(self.entity, "UMO.Position")
			local targetPos = self.entityMgr:GetComponentData(self.entity, "UMO.TargetPos")
			local distanceFromTargetSqrt = Vector3.DistanceNoSqrt(targetPos, curPos)
			if distanceFromTargetSqrt < 100 then
				--到达巡逻点时就进入待机状态
				self:EnterSubState(SubState.Idle)
			end
		end
	end
	self:CheckAround()
end

--主动打人的怪需要经常判断附近有没人
function PatrolState:CheckAround(  )
	if not self.cfg.ai.patrol.auto_attack_radius or self.cfg.ai.patrol.auto_attack_radius == 0 then return end
	if not self.last_check_around or Time.time - self.last_check_around > 1.5 then
		self.last_check_around = Time.time
		local nearestEnemy = self:GetNearestEnemy()
		--发现敌人了，进入战斗状态
		if nearestEnemy ~= nil then
			self.blackboard:SetVariable("targetEnemyEntity", nearestEnemy)
			self.fsm:TriggerState("FightState")
		end
	end
end

function PatrolState:GetNearestEnemy(  )
	local myPos = self.entityMgr:GetComponentData(self.entity, "UMO.Position")
	local around = self.aoi:get_around(self.aoi_handle, self.cfg.ai.patrol.auto_attack_radius, self.cfg.ai.patrol.auto_attack_radius)
	local minDistance = nil
	local nearestEnemy = nil
	for aoi_handle,_ in pairs(around) do
		local uid = self.aoi:get_user_data(aoi_handle, "uid")
		local sceneObjType = SceneHelper:GetSceneObjTypeByUID(uid)
		if sceneObjType == SceneConst.ObjectType.Role then
			-- local enemyEntity = self.aoi:get_user_data(aoi_handle, "entity")
			local enemyEntity = self.sceneMgr:GetEntity(uid)
			local enemyPos = self.entityMgr:GetComponentData(enemyEntity, "UMO.Position")
			local distanceFromTargetSqrt = Vector3.DistanceNoSqrt(myPos, enemyPos)
			if not minDistance or minDistance > distanceFromTargetSqrt then
				minDistance = distanceFromTargetSqrt
				nearestEnemy = enemyEntity
			end
		end
	end
	return nearestEnemy
end

function PatrolState:EnterSubState( sub_state )
	self.sub_state = sub_state
	self.sub_elapsed_time = Time.time
	if sub_state == SubState.Idle then
		--随机一个发呆时间
		self.idle_time = math.random(self.cfg.ai.patrol.idle_min/1000, self.cfg.ai.patrol.idle_max/1000)
	elseif sub_state == SubState.Walk then
		--随机跑去某点
		if self.cfg.ai.patrol.type == 1 then
			local radius = self.patrolInfo.radius/2
			local pos_x = self.patrolInfo.x + math.random(-radius, radius)
			local pos_y = self.patrolInfo.y + math.random(-radius, radius)
			local pos_z = self.patrolInfo.z + math.random(-radius, radius)
			local randomPos = {x=pos_x, y=pos_y, z=pos_z}
			-- self.entityMgr:SetComponentData(self.entity, "UMO.TargetPos", randomPos)
			FightHelper:ChangeTargetPos(self.entity, randomPos)
		else
			print('Cat:PatrolState.lua unkown patrol type:', self.cfg.ai.patrol.type)
		end
		self.walk_time = 1--1秒后再判断是否到达目标点
	end
end

function PatrolState:OnExit(  )
end

function PatrolState:OnPause(  )
end

return PatrolState