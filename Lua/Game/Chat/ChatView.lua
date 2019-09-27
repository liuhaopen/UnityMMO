local ChatView = BaseClass(UINode)

function ChatView:Constructor( parent )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/chat/ChatView.prefab",
		parentTrans = parent,
	}
	self.model = ChatModel:GetInstance()
	self.itemPool = {}
end

function ChatView:OnLoad(  )
	local names = {
		"item_scroll","item_scroll/Viewport/item_con","send_btn:obj","content:input",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:OnUpdate()
end

function ChatView:AddEvents(  )
	local on_click = function ( click_btn )
		if click_btn == self.send_btn_obj then
			local content = self.content_input.text
			local isOk, errorStr = self:VerifyContent(content)
			if not isOk then
				Message:Show(errorStr)
				return
			end
			local chatInfo = {
				content = content,
			}
			self.model:Fire(ChatConst.Event.SendChat, chatInfo)
		end
	end
	UI.BindClickEvent(self.send_btn_obj, on_click)
end

function ChatView:VerifyContent( content )
	return true
end

function ChatView:CreateChatItem(  )
	local item = self.itemPool[#self.itemPool]
	if not item then
		item = require("Game.Chat.ChatItem").New(self.item_con)
		item:Load()
	end
	return item
end

function ChatView:OnUpdate(  )
	
end

return ChatView