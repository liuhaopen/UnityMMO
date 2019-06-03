local ECS = require "ECS"

local DamageSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.DamageSystem", DamageSystem)

function DamageSystem:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)
	self.fightMgr = self.sceneMgr.fightMgr

	self.group = self:GetComponentGroup({"UMO.DamageEvents", "UMO.AOIHandle"})
end

function DamageSystem:OnUpdate(  )
	local deltaTime = Time.deltaTime
	local damages = self.group:GetComponentDataArray("UMO.DamageEvents")
	for i=1,damages.Length do
		local events = damages[i].events
		if events and #events > 0 then

			damages[i].events = nil
		end
	end
end

return DamageSystem