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

	self.item_scroll_scr = self.item_scroll:GetComponent("ScrollRect")

	self:AddEvents()
	self:SetShowChannel(self.curChannel or ChatConst.Channel.World)
end

function ChatView:AddEvents(  )
	local on_click = function ( click_btn )
		if click_btn == self.send_btn_obj then
			self:SendChat()
		end
	end
	UI.BindClickEvent(self.send_btn_obj, on_click)

	local on_scroll_changed = function (  )
		self:OnUpdate()
	end
	self.item_scroll_scr.onValueChanged:AddListener(on_scroll_changed)
end

function ChatView:SendChat(  )
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

function ChatView:GetItemPosY( index, height )
end

function ChatView:SetShowChannel( channel )
	self.curChannel = channel
	if self.isLoaded then
		self:OnUpdate()
	end
end

function ChatView:OnUpdate(  )
	--因为每个聊天子节点的高度是不确定的，所以最简单的方式是把所有聊天记录都创建了，但刚打开界面先显示的是最后几条，所以最好就是从下往上地创建，这样就要求滚动容器真实大小一开始就要给个最大值（见 maxScrollHeight 字段，然后往上滚时判断没内容了就不让它滚动。另外还需要做个复用不可见子节点的功能，所以代码会复杂一点。
	local chatList = self.model:GetChatList(self.curChannel)
	if not chatList then return end
	
	local itemMaxHeight = 350
	local maxItemCount = #chatList
	local maxScrollHeight = itemMaxHeight*maxItemCount
	for i = maxItemCount, 1, -1 do
		local item = self:CreateChatItem()
		item:SetData(chatList[i])
		local height = item:GetHeight()
		local posY = self:GetItemPosY(i, height)
		item:SetPosition(0, posY)
	end
end

return ChatView