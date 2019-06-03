local BP = require("Blueprint")

local BlueprintRegister = {}

function BlueprintRegister:register_all(  )
	BP.TypeManager:RegisterType("Blueprint.State.PatrolState", require("game.scene.ai.state.PatrolState"))
	BP.TypeManager:RegisterType("Blueprint.State.FightState", require("game.scene.ai.state.FightState"))
end

return BlueprintRegister