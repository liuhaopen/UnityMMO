ECS = ECS or {}

require "Common.BaseClass"
require "game.ECS.ScriptBehaviourManager"
require "game.ECS.World"
require "game.ECS.EntityManager"
require "game.ECS.ComponentSystem"
require "game.ECS.ScriptBehaviourUpdateOrder"

function ECS:Init( worldName )
	self.system_list = {}

	local world = ECS.World.New(worldName)
	ECS.World.Active = world

	--register all systems
	for k,v in pairs(self.system_list) do
		world:GetOrCreateManager(v)
	end

	ECS.ScriptBehaviourUpdateOrder.UpdatePlayerLoop(world)
end

function ECS:RegisterSystem( system_type )
	table.insert(self.system_list, system_type)
end

function ECS:Update(  )
	
end

