local ChatView = BaseClass(UINode)

function ChatView:Constructor( parent )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/chat/ChatView.prefab",
		parentTrans = parent,
	}
	self.model = ChatModel:GetInstance()
end

function ChatView:OnLoad(  )
	local names = {

	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:OnUpdate()
end

function ChatView:AddEvents(  )
	
end

function ChatView:OnUpdate(  )
	
end

return ChatView