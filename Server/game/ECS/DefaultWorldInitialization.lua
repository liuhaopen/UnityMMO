local DefaultWorldInitialization = BaseClass()

function DefaultWorldInitialization:Initialize( worldName )
	local world = World.New(worldName)
	World.Active = world

	--register all systems
	for k,v in pairs(ECSSystems or {}) do
		world:GetOrCreateManager(k)
	end
end

return DefaultWorldInitialization