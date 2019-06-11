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
	-- print('Cat:FightState.lua[OnInit]')
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

function FightState:OnEnter(  )
	self.targetEnemyEntity = self.blackboard:GetVariable("targetEnemyEntity")
	self:SetSubState(SubState.Chase)
end

function FightState:OnUpdate( )
	-- print('Cat:FightState.lua[27] self.targetEnemyEntity', self.targetEnemyEntity)
	if self.sub_state == SubState.Chase then
		if self.targetEnemyEntity and self.entityMgr:Exists(self.targetEnemyEntity) then
			if not self.last_retarget_time or Time.time-self.last_retarget_time > 0.5 then
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
					self.fsm:TriggerState("Patrol")
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
	-- elseif self.sub_state == SubState.Fighting then
		--Cat_Todo : 攻击时间间隔需要加上其触发的技能等信息判断
		-- if not self.last_attack_time or Time.time - self.last_attack_time > 2 then
		-- 	self.last_attack_time = Time.time
		-- 	-- self.sceneMgr.fightMgr:Add()
		-- end
	end
end

function FightState:Attack(  )
	if self.last_attack_time and Time.time-self.last_attack_time < self.last_attack_duration then
		return
	end
	self.last_attack_time = Time.time
	self.last_attack_duration = math.random(1, 3)
	--先随机挑个技能
	if not self.cfg or not self.cfg.ai.skill_list then return end
	local skill_id = nil
	local randomNum = math.random(1,100)
	local elapsedNum = 0
	for i,v in ipairs(self.cfg.ai.skill_list) do
		if randomNum <= v.random+elapsedNum then
			skill_id = v.skill_id
			break
		end
		elapsedNum = elapsedNum + v.random
	end
	local pos = self.entityMgr:GetComponentData(self.entity, "UMO.Position")
	local enemyPos = self.entityMgr:GetComponentData(self.targetEnemyEntity, "UMO.Position")
	local defender_uid = self.entityMgr:GetComponentData(self.targetEnemyEntity, "UMO.UID")

	-- local dir = Vector3.Sub(enemyPos, pos)
	-- local angle = dir:Angle(Vector3.right)
	-- print('Cat:FightState.lua[76] monster attack!!!!!!')
	self.sceneMgr.fightMgr:CastSkill(self.uid, {
		skill_id = skill_id,
		cur_pos_x = math.floor(pos.x),
		cur_pos_y = math.floor(pos.y),
		cur_pos_z = math.floor(pos.z),
		target_pos_x = math.floor(enemyPos.x),
		target_pos_y = math.floor(enemyPos.y),
		target_pos_z = math.floor(enemyPos.z),
		-- direction = math.floor(angle*100),
		uid_defenders_map = {[defender_uid]=true},	
	})
	
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