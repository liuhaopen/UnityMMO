local ecs = {
	World = {}
}

local worldMetaTable
function ecs.World.create(  )
	local world = setmetatable({
		entities = {},
		systems = {},
	}, worldMetaTable)
	return world
end

worldMetaTable = {
    __index = {
        addEntity = tiny.addEntity,
        addSystem = tiny.addSystem,
        remove = tiny.remove,
        removeEntity = tiny.removeEntity,
        removeSystem = tiny.removeSystem,
        refresh = tiny.refresh,
        update = tiny.update,
        clearEntities = tiny.clearEntities,
        clearSystems = tiny.clearSystems,
        getEntityCount = tiny.getEntityCount,
        getSystemCount = tiny.getSystemCount,
        setSystemIndex = tiny.setSystemIndex
    },
    __tostring = function()
        return "ECS World"
    end
}

return ecs