local ecs = {}

ecs.class = require "ecs.common.class"
ecs.world = require "ecs.src.world"
ecs.entity_mgr = require "ecs.src.entity_mgr"
ecs.system = require "ecs.src.system"

--inject : ecs.all ecs.any ecs.no
local filters = require "ecs.src.filter"
for k,filter in pairs(filters) do
	ecs[k] = filter
end

return ecs