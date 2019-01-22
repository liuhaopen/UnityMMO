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

local system_list = {}
local function InitWorld( worldName )
	local world = ECS.World.New(worldName)
	ECS.World.Active = world

	world:GetOrCreateManager(ECS.EntityManager.Name)

	--register all systems
	local systems = ECS.TypeManager.GetScriptMgrMap()
	for k,v in pairs(systems) do
		local sys = world:GetOrCreateManager(k)
		table.insert(system_list, sys)
	end
	-- system_list = ECS.ScriptBehaviourUpdateOrder.SortSystemList(systems)
end

local function Update(  )
	if not system_list then return end
	
	for k,v in ipairs(system_list) do
		v:Update()
	end
end

ECS.InitWorld = InitWorld
ECS.Update = Update

return ECS