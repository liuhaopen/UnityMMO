local ecs = require "ecs.ecs"

local mt = ecs.class("animation_system", ecs.system)

function mt:on_update()
	self.filter = self.filter or ecs.all("")
	self:foreach(self.filter, function(ed)
		
	end)
end

return mt