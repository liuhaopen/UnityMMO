local ECSSystemMgr = BaseClass()

function ECSSystemMgr:Init( world, sceneMgr )
	self.ecsSystemList = {}
	self.delayDestroyList = {}
	self.entityMgr = sceneMgr.entityMgr
	local arge = {
		sceneMgr=sceneMgr
	}
	local systems = {
		-- "UMO.DamageSystem",
		"UMO.AISystem",
		"UMO.MovementUpdateSystem",
		--skill sys
		-- "UMO.SkillSys",
	}

	for i,v in ipairs(systems) do
		local system = world:CreateManager(v, arge)
		table.insert(self.ecsSystemList, system)
	end
end

function ECSSystemMgr:Update( delta_time )
	for i,v in ipairs(self.ecsSystemList) do
		v:Update()
	end
	for i,v in ipairs(self.delayDestroyList) do
		-- print('Cat:ECSSystemMgr.lua[29]destroy v', v)
		self.entityMgr:DestroyEntity(v)
	end
	self.delayDestroyList = {}
end

function ECSSystemMgr:AddDestroyEntity( entity )
	-- print('Cat:ECSSystemMgr.lua[36]AddDestroyEntity entity', entity)
	table.insert(self.delayDestroyList, entity)
end

return ECSSystemMgr