local MainUISkillBtnView = BaseClass()

function MainUISkillBtnView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/mainui/MainUISkillBtnView.prefab",
		canvas_name = "MainUI",
		components = {
				-- {UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	}
end

function MainUISkillBtnView:OnLoad(  )
	local names = {
		"correct:obj","skill_3:obj","skill_4:obj","jump:obj","skill_1:obj","skill_2:obj","attack:obj",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()

	self:UpdateView()
end

function MainUISkillBtnView:AddEvents(  )
	local on_click = function ( click_btn )
		if click_btn == self.correct_obj then
        	SceneMgr.Instance:CorrectMainRolePos()
    	elseif click_btn == self.skill_1_obj then
    		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.I, true)
		elseif click_btn == self.skill_2_obj then
    		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.O, true)
		elseif click_btn == self.skill_3_obj then
    		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.K, true)
		elseif click_btn == self.skill_4_obj then
    		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.L, true)
		elseif click_btn == self.attack_obj then
    		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.J, true)
		elseif click_btn == self.jump_obj then
    		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.Space, true)
		end
	end
	UIHelper.BindClickEvent(self.correct_obj, on_click)
	UIHelper.BindClickEvent(self.skill_1_obj, on_click)
	UIHelper.BindClickEvent(self.skill_2_obj, on_click)
	UIHelper.BindClickEvent(self.skill_3_obj, on_click)
	UIHelper.BindClickEvent(self.skill_4_obj, on_click)
	UIHelper.BindClickEvent(self.attack_obj, on_click)
	UIHelper.BindClickEvent(self.jump_obj, on_click)

end

function MainUISkillBtnView:UpdateView(  )
	
end

return MainUISkillBtnView