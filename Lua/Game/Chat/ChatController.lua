ChatConst = require("Game.Chat.ChatConst")
ChatModel = require("Game.Chat.ChatModel")

ChatController = {}

function ChatController:Init(  )
	self.model = ChatModel:GetInstance()
	self:AddEvents()
end

function ChatController:GetInstance()
	return ChatController
end

function ChatController:AddEvents(  )
	local onGameStart = function (  )
        self:ReqChatList()
        self:ListenNewChat()
    end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, onGameStart)

end

function ChatController:ReqChatList(  )
	local on_ack = function ( ackData )
		print("Cat:ChatController [start:25] ackData: ", ackData)
        PrintTable(ackData)
        print("Cat:ChatController [end]")
	end
    NetDispatcher:SendMessage("Chat_GetHistory", {channel=ChatConst.Channel.World}, on_ack)
    NetDispatcher:SendMessage("Chat_GetHistory", {channel=ChatConst.Channel.Notify}, on_ack)
    NetDispatcher:SendMessage("Chat_GetHistory", {channel=ChatConst.Channel.Private}, on_ack)
end

function ChatController:ListenNewChat(  )
    local onNewChat = function ( ackData )
        print("Cat:ChatController [start:38] onNewChat ackData:", ackData)
        PrintTable(ackData)
        print("Cat:ChatController [end]")
    end
    NetDispatcher:Listen("Chat_GetNew", nil, onNewChat)
end

return ChatController
