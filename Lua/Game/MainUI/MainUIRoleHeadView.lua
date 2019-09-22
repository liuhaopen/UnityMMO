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
		"money_1","money_2","flag","head_icon:raw","lv:txt","lv_bg","blood_bar:img",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function MainUIRoleHeadView:AddEvents(  )
	local HPChange = function ( curHp, maxHp )
		self.blood_bar_img.fillAmount = Mathf.Clamp01(curHp/maxHp)
	end
	CSLuaBridge.GetInstance():SetLuaFunc2Num(GlobalEvents.MainRoleHPChanged, HPChange)

    GlobalEventSystem:Bind(GlobalEvents.ExpChanged, MainUIRoleHeadView.UpdateLv, self)
end

function MainUIRoleHeadView:UpdateLv(  )
	self.lv_txt.text = MainRole:GetLv()
end

function MainUIRoleHeadView:UpdateHP(  )
	local goe = RoleMgr.GetInstance():GetMainRole()
	local entity = goe.Entity;
	if not ECS:HasComponent(entity, CS.UnityMMO.Component.HealthStateData) then return end
	local hpData = ECS:GetComponentData(entity, CS.UnityMMO.Component.HealthStateData)
	self.blood_bar_img.fillAmount = Mathf.Clamp01(hpData.CurHp/hpData.MaxHp)
end

function MainUIRoleHeadView:UpdateHead(  )
	local career = MainRole:GetInstance():GetCareer()
	local headRes = ResPath.GetRoleHeadRes(career, 0)
	UI.SetRawImage(self, self.head_icon_raw, headRes)
end

function MainUIRoleHeadView:UpdateView(  )
	self:UpdateHead()
	self:UpdateHP()
	self:UpdateLv()
end

return MainUIRoleHeadView