local ecs = require "ecs.ecs"

local mt = ecs.class("res_system", ecs.system)

--资源加载都统一创建一个带 res_order 订单的 entity，加载完成就销毁
function mt:on_update()
	self.filter = self.filter or ecs.all("res_order")
	self:foreach(self.filter, function(ed)
		local order = ed.res_order

	end)
end

return mt