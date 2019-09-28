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
		"content:txt","name:txt",
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
	self.height = self.content_txt.renderedHeight
	self.name_txt.text = self.data.roleName
end

function ChatItem:GetHeight(  )
	return self.height or 0
end

function ChatItem:SetDirection( isLeft )
	self.isLeft = isLeft
end

return ChatItem