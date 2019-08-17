local Ac = require "Action"
--[[
用法： HP {target=目标, value=偏移数值, is_percent=是否万分比}
e.g.  HP {1200, 3, -1000} 扣万份之1000的防御（防御索引是3，见SceneConst.HP）,持续1200毫秒
	  HP {2000, 1, 500, true} 加500点攻击，持续2000毫秒
--]]

local HP = Ac.OO.Class {
	type 	= "HP",
	__index = {
		Start = function(self, buffData)
			self.buffData = buffData
			self.entityMgr = buffData.sceneMgr.entityMgr
		end,
		IsDone = function(self)
			return true
		end,
		Update = function(self, deltaTime)
			
		end,
	},
	TargetType = {
		Caster = 1,--施法者
		Victim = 2,--受害者
	},
}
return HP