local MainUIActivityView = BaseClass()

function MainUIActivityView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/mainui/MainUIActivityView.prefab",
		canvas_name = "Normal",
		components = {
				{UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	}
end

function MainUIActivityView:OnLoad(  )
	local names = {}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function MainUIActivityView:AddEvents(  )
	local on_click = function ( click_btn )
		
	end
	-- UIHelper.BindClickEvent(self.return_btn, on_click)

end

function MainUIActivityView:UpdateView(  )
	
end

return MainUIActivityView