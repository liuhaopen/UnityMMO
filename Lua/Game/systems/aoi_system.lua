--[[
因为后端会发比较大范围的场景对象过来，前端这边也做下更小的 aoi 以降低消耗。
另外后端只是发了角色 id 和坐标，然后前端这边根据特定规则过滤，排序等（比如优先显示队友，好友或是我的敌人等等）；
然后根据这个排序会把角色分为三级并告诉后端，后端根据这三级将区别发送信息给前端，三级为（不推送，只推送移动，推送全信息）
--]]
local ecs = require "ecs.ecs"

local mt = ecs.class("aoi_system", ecs.system)

function mt:on_update()
	self.filter = self.filter or ecs.all("aoi")
	self:foreach(self.filter, function(ed)
		
	end)
end

return mt