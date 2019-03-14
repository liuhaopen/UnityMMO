local MainUISkillBtnView = BaseClass()

function MainUISkillBtnView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/mainui/MainUISkillBtnView.prefab",
		canvas_name = "Normal",
		components = {
				-- {UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	}
end

function MainUISkillBtnView:OnLoad(  )
	local names = {
		"correct:obj","skill_3","skill_4","jump","skill_1","skill_2","attack",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function MainUISkillBtnView:AddEvents(  )
	local on_click = function ( click_btn )
		if click_btn == self.correct_obj then
        	SceneMgr.Instance:CorrectMainRolePos()
		end
	end
	UIHelper.BindClickEvent(self.correct_obj, on_click)

end

function MainUISkillBtnView:UpdateView(  )
	
end

return MainUISkillBtnView