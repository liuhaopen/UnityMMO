local ECS = require "ECS"

local DamageSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("UMO.DamageSystem", DamageSystem)

function DamageSystem:Constructor( )
	self.fight_mgr = require "game.scene.fight_mgr"
end

function DamageSystem:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

end

function DamageSystem:OnUpdate(  )
	local deltaTime = Time.deltaTime
	
end

return DamageSystem