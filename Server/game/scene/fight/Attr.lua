local Ac = require "Action"
--[[
用法： Attr {持续毫秒，属性索引，偏移数值，是否是绝对值，否则就是万份比}
e.g.  Attr {1200, 3, -1000} 扣万份之1000的防御（防御索引是3，见SceneConst.Attr）,持续1200毫秒
	  Attr {2000, 1, 500, true} 加500点攻击，持续2000毫秒
--]]
local ChangeAttr = function ( self, additionValue )
	local attrIndex = self[2]
	local fightAttr = self.entityMgr:GetComponentData(self.buffData.target_entity, "UMO.FightAttr")
	local curValue = fightAttr[attrIndex]
	if self[4] then
		--绝对值
		fightAttr[attrIndex] = math.max(0, curValue + additionValue)
	else
		--默认是万分比
		local baseAttr = self.entityMgr:GetComponentData(self.buffData.target_entity, "UMO.BaseAttr")
		local baseAttrValue = baseAttr[attrIndex] or 0
		local additionAbsValue = baseAttrValue*additionValue/10000
		fightAttr[attrIndex] = math.max(0, curValue+additionAbsValue)
	end
	return fightAttr[attrIndex]-curValue
end
local Attr = Ac.OO.Class {
	type 	= "Attr",
	__index = {
		Start = function(self, buffData)
			self.buffData = buffData
			self.entityMgr = buffData.sceneMgr.entityMgr
			self.elapsed = 0
			self.isDone = false
			self.state = "start"
		end,
		IsDone = function(self)
			return self.isDone, true
		end,
		Update = function(self, deltaTime)
			self.elapsed = self.elapsed + deltaTime
			self.isDone = self.elapsed >= self[1]
			if self.state == "start" and not self.isDone then
				local additionValue = self[3]
				self.lastAdditionValue = ChangeAttr(self, additionValue)
				self.state = "have set"
			end
			if self.isDone then
				self[4] = true
				ChangeAttr(self, -self.lastAdditionValue)
			end
		end,
	},
}
return Attr