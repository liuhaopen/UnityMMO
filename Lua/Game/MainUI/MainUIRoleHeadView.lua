local MainUIRoleHeadView = BaseClass()

function MainUIRoleHeadView:DefaultVar( )
	return { 
		UIConfig = {
			prefab_path = "Assets/AssetBundleRes/ui/mainui/MainUIRoleHeadView.prefab",
			canvas_name = "MainUI",
			components = {
			},
		},
	}
end

function MainUIRoleHeadView:OnLoad(  )
	local names = {
		"money_1","money_2","flag","head_icon:raw","lv","lv_bg","blood_bar",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function MainUIRoleHeadView:AddEvents(  )
	local on_click = function ( click_btn )
		-- if click_btn == self.correct_obj then
  --       	SceneMgr.Instance:CorrectMainRolePos()
  --   	elseif click_btn == self.skill_1_obj then
  --   		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.I, true)
		-- end
	end
	UIHelper.BindClickEvent(self.correct_obj, on_click)
	UIHelper.BindClickEvent(self.skill_1_obj, on_click)

end

function MainUIRoleHeadView:UpdateView(  )
	local career = MainRole:GetInstance():GetCareer()
	print('Cat:MainUIRoleHeadView.lua[39] career', career)
	local headRes = ResPath.GetRoleHeadRes(career, 0)
	UIHelper.SetRawImage(self.head_icon_raw, headRes)
end

return MainUIRoleHeadView