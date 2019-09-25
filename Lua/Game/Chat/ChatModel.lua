ChatModel = BaseClass(EventDispatcher)

function ChatModel:Constructor()
	ChatModel.Instance = self
	self:Reset()
end

function ChatModel:Reset()
end

function ChatModel.GetInstance()
	if ChatModel.Instance == nil then
		ChatModel.Instance = ChatModel.New()
	end
	return ChatModel.Instance
end

return ChatModel