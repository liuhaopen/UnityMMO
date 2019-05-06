local BP = require("Blueprint")

local blueprint_register = {}

function blueprint_register:register_all(  )
	BP.TypeManager:RegisterType("Blueprint.State.PatrolState", require("game.scene.ai.state.PatrolState"))
	BP.TypeManager:RegisterType("Blueprint.State.FightState", require("game.scene.ai.state.FightState"))
end

return blueprint_register