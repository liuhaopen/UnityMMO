ECS = ECS or {}

ECSCore = require "ECSCore"
require "BaseClass"
require "CoreHelper"
require "TypeManager"
require "ScriptBehaviourManager"
require "ECS.World"
require "Entity"
require "EntityManager"
require "EntityDataManager"
require "ComponentGroup"
require "ComponentSystem"
require "ScriptBehaviourUpdateOrder"
require "SharedComponentDataManager"
require "ArchetypeManager"
require "EntityGroupManager"
require "ComponentType"
require "ComponentTypeInArchetype"
require "SortingUtilities"
require "Chunk"
require "UnsafeLinkedListNode"
require "ChunkDataUtility"
require "ComponentSystemInjection"
require "InjectFromEntityData"
require "InjectComponentGroupData"
require "ComponentChunkIterator"
require "ComponentDataArray"
require "EntityArray"

local function InitWorld( worldName )
	local world = ECS.World.New(worldName)
	ECS.World.Active = world

	world:GetOrCreateManager(ECS.EntityManager.Name)

	--register all systems
	local systems = ECS.TypeManager.GetScriptMgrMap()
	for k,v in pairs(systems) do
		world:GetOrCreateManager(k)
	end
	return world
end

ECS.InitWorld = InitWorld

return ECS