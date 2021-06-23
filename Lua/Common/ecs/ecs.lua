local ecs = {}

ecs.class = require "common.class"
ecs.world = require "src.world"
ecs.entity_mgr = require "src.entity_mgr"
ecs.system = require "src.system"

--inject : ecs.all ecs.any ecs.no
local filters = require "src.filter"
for k,filter in pairs(filters) do
	ecs[k] = filter
end

return ecs