local ecs_system_mgr = {}

function ecs_system_mgr:init( world, sceneMgr )
	self.ecs_system_mgr_list = {}

	local systems = {
		"UMO.DamageSystem",
		"UMO.AISystem",
		"UMO.MovementUpdateSystem",
	}
	for i,v in ipairs(systems) do
		local system = world:GetOrCreateManager(v)
		system.sceneMgr = sceneMgr
		table.insert(self.ecs_system_mgr_list, system)
	end
end

function ecs_system_mgr:update( delta_time )
	for i,v in ipairs(self.ecs_system_mgr_list) do
		v:Update()
	end
end

return ecs_system_mgr