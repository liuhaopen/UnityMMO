local MainUISkillBtnView = BaseClass()

function MainUISkillBtnView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/prefab/mainui/MainUISkillBtnView.prefab",
		canvas_name = "Normal",
		components = {
				{UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	}
end

function MainUISkillBtnView:OnLoad(  )
	local names = {}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function MainUISkillBtnView:AddEvents(  )
	local on_click = function ( click_btn )
		
	end
	-- UIHelper.BindClickEvent(self.return_btn, on_click)

end

function MainUISkillBtnView:UpdateView(  )
	
end

return MainUISkillBtnView