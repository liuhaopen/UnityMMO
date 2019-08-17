local Ac = require "Action"
--[[
用法： Attr {duration=持续毫秒，attr_id=属性索引，value=偏移数值，is_percent=是否是绝对值，否则就是万份比}
e.g.  Attr {duration=1200, attr_id=3, value=-1000} 扣万份之1000的防御（防御索引是3，见SceneConst.Attr）,持续1200毫秒
	  Attr {duration=2000, attr_id=1, value=500, is_percent=true} 加500点攻击，持续2000毫秒
--]]
local ChangeAttr = function ( self, additionValue )
	local attr_id = self.attr_id
	local fightAttr = self.entityMgr:GetComponentData(self.buffData.victim_entity, "UMO.FightAttr")
	local curValue = fightAttr[attr_id]
	if self.is_percent then
		--绝对值
		fightAttr[attr_id] = math.max(0, curValue + additionValue)
	else
		--默认是万分比
		local baseAttr = self.entityMgr:GetComponentData(self.buffData.victim_entity, "UMO.BaseAttr")
		local baseAttrValue = baseAttr[attr_id] or 0
		local additionAbsValue = baseAttrValue*additionValue/10000
		fightAttr[attr_id] = math.max(0, curValue+additionAbsValue)
	end
	return fightAttr[attr_id]-curValue
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
			self.isDone = self.elapsed >= self.duration
			if self.state == "start" and not self.isDone then
				local additionValue = self.value
				self.lastAdditionValue = ChangeAttr(self, additionValue)
				self.state = "have set"
			end
			if self.isDone then
				self.is_percent = true
				ChangeAttr(self, -self.lastAdditionValue)
			end
		end,
	},
}
return Attr