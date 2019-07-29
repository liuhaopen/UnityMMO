GMConst = require("Game.GM.GMConst")
GMModel = require("Game.GM.GMModel")

GMController = {}

function GMController:Init(  )
	self.model = GMModel:GetInstance()
	self:AddEvents()
end

function GMController:GetInstance()
	return GMController
end

function GMController:AddEvents(  )
	local onGameStart = function (  )
        -- self.model:Reset()
        self:ReqGMList()
    end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, onGameStart)

end

function GMController:ReqGMList(  )
	local on_ack = function ( ackData )
		self.model:SetGmList(ackData.gmList)
	end
    NetDispatcher:SendMessage("GM_GetList", nil, on_ack)
end

function GMController:ReqExcuteGM( gmStr )
	print('Cat:GMController.lua[ReqExcuteGM] gmStr', gmStr)
    NetDispatcher:SendMessage("GM_Excute", {gmStr=gmStr})
end

function GMController:OpenGMView(  )
	
end

return GMController
