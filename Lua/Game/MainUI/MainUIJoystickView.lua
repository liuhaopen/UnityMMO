local MainUIJoystickView = BaseClass()

function MainUIJoystickView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/mainui/MainUIJoystickView.prefab",
		canvas_name = "MainUI",
		-- components = {
				-- {UI.HideOtherView},
				-- {UI.DelayDestroy, {delay_time=5}},
			-- },
		},
	}
end

function MainUIJoystickView:OnLoad(  )
	local names = {
		"chat_btn:obj",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function MainUIJoystickView:AddEvents(  )
	local on_click = function ( click_btn )
		ChatModel:GetInstance():Fire(ChatConst.Event.SetChatViewVisible, true)
	end
	UI.BindClickEvent(self.chat_btn_obj, on_click)

end

function MainUIJoystickView:UpdateView(  )
	
end

return MainUIJoystickView