local ecs = require "ecs.ecs"

local mt = ecs.class("aoi_system", ecs.system)

function mt:on_update()
	self.filter = self.filter or ecs.all("aoi")
	self:foreach(self.filter, function(ed)
		
	end)
end

return mt