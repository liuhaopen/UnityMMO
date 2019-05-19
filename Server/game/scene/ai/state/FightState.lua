local BP = require("Blueprint")
local FightHelper = require("game.scene.FightHelper")
local Time = Time
local FightState = BP.BaseClass(BP.FSM.FSMState)

local SubState = {
	Chase = 1,--追捕
	Fighting = 2,--攻击
	GoBack = 3,--回到巡逻点
}
function FightState:OnInit(  )
	print('Cat:FightState.lua[OnInit]')
	self.aoi_handle = self.blackboard:GetVariable("aoi_handle")
	self.aoi_area = self.blackboard:GetVariable("aoi_area")
	self.entity = self.blackboard:GetVariable("entity")
	self.sceneMgr = self.blackboard:GetVariable("sceneMgr")
	self.aoi = self.sceneMgr.aoi
	self.entityMgr = self.sceneMgr.entityMgr
	self.monsterMgr = self.sceneMgr.monsterMgr
	self.cfg = self.blackboard:GetVariable("cfg")
end

function FightState:OnEnter(  )
	print('Cat:FightState.lua[OnEnter]')
	self.targetEnemyEntity = self.blackboard:GetVariable("targetEnemyEntity")
	print('Cat:FightState.lua[23] self.targetEnemyEntity', self.targetEnemyEntity)
	self:SetSubState(SubState.Chase)
end

function FightState:OnUpdate( )
	-- print('Cat:FightState.lua[27] self.targetEnemyEntity', self.targetEnemyEntity)
	if self.sub_state == SubState.Chase then
		if self.targetEnemyEntity and self.entityMgr:Exists(self.targetEnemyEntity) then
			if not self.last_retarget_time or Time.time-self.last_retarget_time > 1.5 then
				self.last_retarget_time = Time.time
				--判断是否进入攻击范围，是则发起攻击，否则追上去
				local myPos = self.entityMgr:GetComponentData(self.entity, "UMO.Position")
				local enemyPos = self.entityMgr:GetComponentData(self.targetEnemyEntity, "UMO.Position")
				local distanceFromTargetSqrt = Vector3.Distance(myPos, enemyPos)
				local isMaxOk = distanceFromTargetSqrt <= self.cfg.ai.attack_area.max_distance
				local isMinOk = distanceFromTargetSqrt >= self.cfg.ai.attack_area.min_distance
				local isOverHuntDis = distanceFromTargetSqrt > self.cfg.ai.hunt_radius
				-- print('Cat:FightState.lua[34] distanceFromTargetSqrt', distanceFromTargetSqrt, isMaxOk)
				if isOverHuntDis then
					self.targetEnemyEntity = nil
					self.blackboard:SetVariable("targetEnemyEntity", nil)
				elseif isMaxOk and isMinOk then
					--离敌人距离刚好，发动攻击
					self:Attack()
				else
					--离敌人太远或太近了，走位移动到可攻击的目标点
					local newPos = FightHelper.GetAssailablePos(myPos, enemyPos, self.cfg.ai.attack_area.min_distance, self.cfg.ai.attack_area.max_distance)			
					self.monsterMgr:ChangeTargetPos(self.entity, newPos)
				end
			end
		else
			--没有攻击目标了，回去耕田吧
			-- self:SetSubState(SubState.GoBack)
			self.fsm:TriggerState("Patrol")
		end
	elseif self.sub_state == SubState.Fighting then
		--Cat_Todo : 攻击时间间隔需要加上其触发的技能等信息判断
		if not self.last_attack_time or Time.time - self.last_attack_time > 2 then
			self.last_attack_time = Time.time
			-- self.sceneMgr.fightMgr:Add()
		end

	end
end

function FightState:RunToAssailablePos(  )
	
end

function FightState:Attack(  )
	--先随机挑个技能
	print('Cat:FightState.lua[76] monster attack!!!!!!')
	self.sceneMgr.fightMgr:cast_skill()
	
end

function FightState:SetSubState( sub_state )
	self.sub_state = sub_state
	self.sub_elapsed_time = Time.time
	if sub_state == SubState.Chase then
	end
end

function FightState:OnExit(  )
end

function FightState:OnPause(  )
end

return FightState