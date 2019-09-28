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

    self.model:Bind(ChatConst.Event.SetChatViewVisible, function( isShow, channel )
        if isShow then
            if self.chatView then
                self.chatView:SetActive(true)
            else
                self.chatView = require("Game.Chat.ChatView").New()
                self.chatView:Load()
            end
            if channel then
                self.chatView:SetShowChannel(channel)
            end
        else
            if self.chatView then
                self.chatView:Unload()
                self.chatView = nil
            end
        end
    end)
    
end

function ChatController:ReqChatList(  )
	local on_ack = function ( ackData )
		print("Cat:ChatController [start:25] ackData: ", ackData)
        PrintTable(ackData)
        print("Cat:ChatController [end]")
        self.model:SetChatList(ackData.channel, ackData.list)
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
        self.model:UpdateChatList(ackData)
    end
    NetDispatcher:Listen("Chat_GetNew", nil, onNewChat)
end

return ChatController
