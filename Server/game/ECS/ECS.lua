ECS = ECS or {}

require "ECSCore"
require "BaseClass"
require "CoreHelper"
ECS.CoreHelper.Init()
require "ScriptBehaviourManager"
require "World"
require "Entity"
require "EntityManager"
require "EntityDataManager"
require "ComponentGroup"
require "ComponentSystem"
require "ScriptBehaviourUpdateOrder"
require "TypeManager"
require "SharedComponentDataManager"
require "ArchetypeManager"
require "EntityGroupManager"
require "ComponentType"
require "ComponentTypeInArchetype"
require "SortingUtilities"
require "Chunk"
require "UnsafeLinkedListNode"
require "ChunkDataUtility"


function ECS:InitWorld( worldName )
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
	--Cat_Todo : 按照UpdateBefore,After等标签把system们先排好序
	local systems = ECS.World.Active:GetBehaviourManagers()
	for k,v in pairs(systems) do
		v:Update()
	end
end

