local ECS = ECS or {}

local importer = require("ECS.Importer")
importer.enable()

--让本框架里的文件都有ECS这个全局变量
local ECSEnv = {
	ECS = ECS
}
setmetatable(ECSEnv, {
	__index = _ENV,	
	__newindex = function (t,k,v)
		--本框架内不允许新增和修改全局变量，实在想要的也可以使用_ENV.xx = yy这种形式，但我像是这种没节操的人吗？！
		error("attempt to set a global value", 2)
	end,
})

-- ECS.Core = require("ECSCore")--这是个c库
ECS.BaseClass = importer.require("ECS.BaseClass", ECSEnv)
-- ECS.CoreHelper = importer.require("ECS.CoreHelper", ECSEnv)
ECS.TypeManager = importer.require("ECS.TypeManager", ECSEnv)
ECS.ScriptBehaviourManager = importer.require("ECS.ScriptBehaviourManager", ECSEnv)
ECS.World = importer.require("ECS.World", ECSEnv)
ECS.Entity = importer.require("ECS.Entity", ECSEnv)
ECS.EntityManager = importer.require("ECS.EntityManager", ECSEnv)
ECS.EntityDataManager = importer.require("ECS.EntityDataManager", ECSEnv)
ECS.ComponentGroup = importer.require("ECS.ComponentGroup", ECSEnv)
ECS.ComponentSystem = importer.require("ECS.ComponentSystem", ECSEnv)
ECS.SharedComponentDataManager = importer.require("ECS.SharedComponentDataManager", ECSEnv)
ECS.ArchetypeManager = importer.require("ECS.ArchetypeManager", ECSEnv)
ECS.EntityGroupManager = importer.require("ECS.EntityGroupManager", ECSEnv)
ECS.ComponentType = importer.require("ECS.ComponentType", ECSEnv)
ECS.ComponentTypeInArchetype = importer.require("ECS.ComponentTypeInArchetype", ECSEnv)
ECS.SortingUtilities = importer.require("ECS.SortingUtilities", ECSEnv)
ECS.Chunk = importer.require("ECS.Chunk", ECSEnv)
ECS.UnsafeLinkedListNode = importer.require("ECS.UnsafeLinkedListNode", ECSEnv)
ECS.ChunkDataUtility = importer.require("ECS.ChunkDataUtility", ECSEnv)
ECS.ComponentSystemInjection = importer.require("ECS.ComponentSystemInjection", ECSEnv)
ECS.InjectFromEntityData = importer.require("ECS.InjectFromEntityData", ECSEnv)
ECS.InjectComponentGroupData = importer.require("ECS.InjectComponentGroupData", ECSEnv)
ECS.ComponentChunkIterator = importer.require("ECS.ComponentChunkIterator", ECSEnv)
ECS.ComponentDataArray = importer.require("ECS.ComponentDataArray", ECSEnv)
ECS.EntityArray = importer.require("ECS.EntityArray", ECSEnv)

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

--为了不影响全局，这里要还原一下package.searchers
importer.disable()

return ECS