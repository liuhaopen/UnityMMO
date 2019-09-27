local ChatItem = BaseClass(UINode)

function ChatItem:Constructor( parent )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/chat/ChatItem.prefab",
		parentTrans = parent,
	}
	self.model = ChatModel:GetInstance()
end

function ChatItem:OnLoad(  )
	local names = {
		"content:txt",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:OnUpdate()
end

function ChatItem:AddEvents(  )
	
end

function ChatItem:OnUpdate(  )
	if not self.data then return end

	self.content_txt.text = self.data.content
end

return ChatItem