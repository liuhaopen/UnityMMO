BagConst = require("Game/Bag/BagConst")
BagModel = require("Game/Bag/BagModel")

BagController = {}

function BagController:Init(  )
	self.model = BagModel:GetInstance()
	self:AddEvents()
end

function BagController:GetInstance()
	return BagController
end

function BagController:AddEvents(  )
	local onGameStart = function (  )
        self.model:Reset()
        self:ReqBagList()
    end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, onGameStart)

end

return BagController