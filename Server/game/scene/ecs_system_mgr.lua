local ecs_system_mgr = {}

function ecs_system_mgr:init( world )
	self.ecs_system_mgr_list = {}

	local systems = {
		-- "umo.patrol_system",
		"umo.damage_system",
		-- "umo.monster_nest_system"
	}
	for i,v in ipairs(systems) do
		table.insert(self.ecs_system_mgr_list, world:GetOrCreateManager(v))
	end
end

function ecs_system_mgr:update( delta_time )
	for i,v in ipairs(self.ecs_system_mgr_list) do
		v:Update()
	end
end

return ecs_system_mgr