local ECSSystemMgr = BaseClass()

function ECSSystemMgr:Init( world, sceneMgr )
	self.ecs_system_mgr_list = {}

	local arge = {
		sceneMgr=sceneMgr
	}
	local systems = {
		"UMO.DamageSystem",
		"UMO.AISystem",
		"UMO.MovementUpdateSystem",

		--skill sys
		-- "SkillMaxTargetNumBuffSys",
		-- "SkillSys",
		-- "SkillTargetSys",
	}

	for i,v in ipairs(systems) do
		local system = world:CreateManager(v, arge)
		table.insert(self.ecs_system_mgr_list, system)
	end
end

function ECSSystemMgr:update( delta_time )
	for i,v in ipairs(self.ecs_system_mgr_list) do
		v:Update()
	end
end

return ECSSystemMgr