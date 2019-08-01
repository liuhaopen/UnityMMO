local ECS = ECS or {}

local importer = require("ECS.Common.Importer")
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

ECS.BaseClass = importer.require("ECS.Common.BaseClass", ECSEnv)
ECS.TypeManager = importer.require("ECS.Src.TypeManager", ECSEnv)
ECS.ScriptBehaviourManager = importer.require("ECS.Src.ScriptBehaviourManager", ECSEnv)
ECS.World = importer.require("ECS.Src.World", ECSEnv)
ECS.Entity = importer.require("ECS.Src.Entity", ECSEnv)
ECS.EntityManager = importer.require("ECS.Src.EntityManager", ECSEnv)
ECS.EntityDataManager = importer.require("ECS.Src.EntityDataManager", ECSEnv)
ECS.ComponentGroup = importer.require("ECS.Src.ComponentGroup", ECSEnv)
ECS.ComponentSystem = importer.require("ECS.Src.ComponentSystem", ECSEnv)
ECS.SharedComponentDataManager = importer.require("ECS.Src.SharedComponentDataManager", ECSEnv)
ECS.ArchetypeManager = importer.require("ECS.Src.ArchetypeManager", ECSEnv)
ECS.EntityGroupManager = importer.require("ECS.Src.EntityGroupManager", ECSEnv)
ECS.ComponentType = importer.require("ECS.Src.ComponentType", ECSEnv)
ECS.ComponentTypeInArchetype = importer.require("ECS.Src.ComponentTypeInArchetype", ECSEnv)
ECS.SortingUtilities = importer.require("ECS.Common.SortingUtilities", ECSEnv)
ECS.Chunk = importer.require("ECS.Src.Chunk", ECSEnv)
ECS.UnsafeLinkedListNode = importer.require("ECS.Common.UnsafeLinkedListNode", ECSEnv)
ECS.ChunkDataUtility = importer.require("ECS.Src.ChunkDataUtility", ECSEnv)
ECS.ComponentSystemInjection = importer.require("ECS.Src.ComponentSystemInjection", ECSEnv)
ECS.InjectComponentGroupData = importer.require("ECS.Src.InjectComponentGroupData", ECSEnv)
ECS.ComponentChunkIterator = importer.require("ECS.Src.ComponentChunkIterator", ECSEnv)
ECS.ComponentDataArray = importer.require("ECS.Src.ComponentDataArray", ECSEnv)
ECS.EntityArray = importer.require("ECS.Src.EntityArray", ECSEnv)

local function InitWorld( worldName )
	local world = ECS.World.New(worldName)
	ECS.World.Active = world

	world.EntityManager = world:GetOrCreateManager(ECS.EntityManager.Name)

	return world
end

ECS.InitWorld = InitWorld

--为了不影响全局，这里要还原一下package.searchers
importer.disable()

return ECS