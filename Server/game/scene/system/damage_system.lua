ECS.TypeManager.RegisterType("umo.damage_event", {id="integer"})

local damage_system = BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("umo.damage_system", damage_system)

function damage_system:Constructor( )
	self.fight_mgr = require "game.scene.fight_mgr"
end

function damage_system:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)

end

function damage_system:OnUpdate(  )
	local deltaTime = Time.deltaTime
	
end

return damage_system