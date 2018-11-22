ECS = ECS or {}
ECS.DefaultWorldInitialization = ECS.DefaultWorldInitialization or BaseClass()

function ECS.DefaultWorldInitialization:Initialize( worldName )
	local world = ECS.World.New(worldName)
	ECS.World.Active = world

	--register all systems
	for k,v in pairs(ECS.Systems or {}) do
		world:GetOrCreateManager(k)
	end

	ECS.ScriptBehaviourUpdateOrder.UpdatePlayerLoop(world)
end

return ECS.DefaultWorldInitialization