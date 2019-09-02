--清除掉目标身上所有的 buff, target参数指定消除哪个目标身上的 buff，clear_type 指定消除哪些类型的 buff，见：SceneConst.ClearBuffType 枚举，有三种：所有，坏的和好的 buff.
local Ac = require "Action"
local FightHelper = require("game.scene.FightHelper")
local SceneConst = require "game.scene.SceneConst"
local BuffActions = require("game.scene.fight.BuffActions")

--对于造成临时效果,比如晕眩或一定时间内降低属性的 buff,只需要快速播放至结束就好了
local HandleDurationBuff = function ( self, buffAction )
	while true do 
		buffAction:Update(100000000000)
		if buffAction:IsDone() then
			self.actionMgr:RemoveAction(buffAction)
			break
		end
	end
end

--对于造成不可恢复效果的，直接结束播放就行了
local HandleUnrecoverableBuff = function ( self, buffAction )
	self.actionMgr:RemoveAction(buffAction)
end

--有些 buff 虽然造成不可恢复的效果，但不能直接删掉该 action，比如冰冻，它会每隔一定时间就扣血，所以不能快速把它播放完。但它还会减你速度，直到播放结束时才把速度还原，所以不能直接把它给删了，不然就跳过了还原速度的处理。所以这种 buff 需要特殊处理
local HandleSpecialBuff = function ( self, buffAction )
	local clearer = BuffActions:GetActionClearer(buffAction.buffData.buff_id)
	if clearer then 
		clearer()
	end
end

local BuffTypeMap = {
	[400000] = HandleDurationBuff,
	[400001] = HandleUnrecoverableBuff,
	[400002] = HandleDurationBuff,
	[400003] = HandleSpecialBuff,
}

local BadBuffDic = {
	[400000] = true,
	[400001] = true,
	[400002] = true,
	[400003] = true,
}

local ClearBuff = Ac.OO.Class {
	type 	= "ClearBuff",
	__index = {
		Start = function(self, buffData)
			self.buffData = buffData
			self.sceneMgr = buffData.sceneMgr
			self.actionMgr = self.sceneMgr.actionMgr
		end,
		IsDone = function(self)
			return true
		end,
		Update = function(self, deltaTime)
			local target = self[1]
			local clearType = self[2]
			--针对不同的 buff 类型做处理
			local targetEntity
			if target == SceneConst.SkillTargetType.Enemy then
				targetEntity = self.buffData.victim_entity
			elseif target == SceneConst.SkillTargetType.Me then
				targetEntity = self.buffData.caster_entity
			end
			local buffList = self.entityMgr:GetComponentData(targetEntity, "UMO.Buff")
			for i,v in ipairs(buffList) do
				local needClear = false
				if clearType == SceneConst.ClearBuffType.All then
					needClear = true
				elseif clearType == SceneConst.ClearBuffType.Bad then
					needClear = BadBuffDic[v.buff_id]
				else
					needClear = true
				end
				if needClear then
					local handler = BuffTypeMap[v.buff_id]
					if handler then
						handler(v.action)
					end
				end
			end
		end,
	},
}
return ClearBuff