local MainUITaskView = BaseClass()

function MainUITaskView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/mainui/MainUITaskView.prefab",
		canvas_name = "Normal",
		components = {
				{UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	}
end

function MainUITaskView:OnLoad(  )
	local names = {}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function MainUITaskView:AddEvents(  )
	local on_click = function ( click_btn )
		
	end
	-- UIHelper.BindClickEvent(self.return_btn, on_click)

end

function MainUITaskView:UpdateView(  )
	
end

return MainUITaskView