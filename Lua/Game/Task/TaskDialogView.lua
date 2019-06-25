local TaskDialogView = BaseClass(UINode)

function TaskDialogView:Constructor(  )
	self.prefabPath = "Assets/AssetBundleRes/ui/task/TaskDialogView.prefab"
	self:Load()
end

function TaskDialogView:OnLoad(  )
	local names = {
		"skip_con/skip_btn:obj","bottom/npc:raw","bottom/btn:obj","bottom/npc_name:txt","bottom/chat:txt","click_bg:obj","btn/btn_label:txt"
	}
	UI.GetChildren(self, self.transform, names)
	self.transform.sizeDelta = Vector2.zero

	self:AddEvents()
end

function TaskDialogView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.btn_obj == click_obj then
			self:Destroy()
		elseif self.skip_btn_obj == click_obj then
			self:Destroy()
		elseif self.click_bg_obj == click_obj then
			self:Destroy()
		end
	end
	UI.BindClickEvent(self.btn_obj, on_click)
	UI.BindClickEvent(self.skip_btn_obj, on_click)
	UI.BindClickEvent(self.click_bg_obj, on_click)
	
end

function TaskDialogView:OnUpdate(  )
	print('Cat:TaskDialogView.lua[34]')
	self.cfg = self.cfg or require("Config.ConfigNPC")
	print('Cat:TaskDialogView.lua[36] self.cfg', self.cfg)

	self:UpdateContent()
	self:UpdateLooks()
end

function TaskDialogView:UpdateContent(  )
	self.npc_name_txt.text = self.cfg[self.data.npcID].name
	self.chat_txt.text = self.data.content
end

function TaskDialogView:UpdateLooks( )
	local show_data = {
		showType = UILooksNode.ShowType.NPC,
		showRawImg = self.npc_raw,
		npcID = self.data.npcID,
		canRotate = true,
	}
	self.looksNode = self.looksNode or UILooksNode.New(self.npc)
	self.looksNode:SetData(show_data)
end

return TaskDialogView