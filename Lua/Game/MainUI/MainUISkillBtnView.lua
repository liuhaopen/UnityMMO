local MainUISkillBtnView = BaseClass(UINode)

function MainUISkillBtnView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUISkillBtnView.prefab",
		canvasName = "MainUI",
	}
	self.btnList = {}
end

function MainUISkillBtnView:OnLoad(  )
	local names = {
		"correct:obj","jump:obj","attack:obj",
	}
	UI.GetChildren(self, self.transform, names)
	self.btnContainer = {}
	for i=1,MainUIConst.MaxSkillBtnNum do
		self.btnContainer[i] = self.transform:Find("skill_"..i)
	end
	self:AddEvents()
	self:UpdateView()
end

function MainUISkillBtnView:AddEvents(  )
	local on_click = function ( click_btn )
		if click_btn == self.correct_obj then
        	SceneMgr.Instance:CorrectMainRolePos()
		elseif click_btn == self.attack_obj then
    		-- CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.J, true)
    		SkillManager.GetInstance():CastSkillByIndex(-1)
		elseif click_btn == self.jump_obj then
    		CS.UnityMMO.GameInput.GetInstance():SetKeyUp(CS.UnityEngine.KeyCode.Space, true)
		end
	end
	UI.BindClickEvent(self.correct_obj, on_click)
	UI.BindClickEvent(self.attack_obj, on_click)
	UI.BindClickEvent(self.jump_obj, on_click)

	local SkillCDChanged = function ( skillID, cdEndTime )
		print('Cat:MainUISkillBtnView.lua[54] skillID, cdEndTime', skillID, cdEndTime)
		for i,v in ipairs(self.btnList) do
			if v and v.data and v.data.skillID == skillID then
				v:SetCDEndTime(cdEndTime)
			end
		end
	end
	CSLuaBridge.GetInstance():SetLuaFunc2Num(GlobalEvents.SkillCDChanged, SkillCDChanged)
end

function MainUISkillBtnView:GetSkillInfo(  )
	local info = {}
	info.idList = {}
	for i=1,MainUIConst.MaxSkillBtnNum do
		local skillID = SkillManager.GetInstance():GetSkillIDByIndex(i-1)
		info.idList[i] = skillID
	end
	return info
end

function MainUISkillBtnView:UpdateView(  )
	self.skillInfo = self:GetSkillInfo()
	for i,v in ipairs(self.btnList) do
		v:SetActive(false)
	end
	self.itemNodeNames = self.itemNodeNames or {
		"left_time:txt","cd:img:obj","icon:img:obj",
	}
	for i=1,#self.skillInfo.idList do
		local skillID = self.skillInfo.idList[i]
		if skillID and skillID ~= 0 then
			local item = self.btnList[i]
			if not item then
				item = UINode.Create("Assets/AssetBundleRes/ui/mainui/MainUISkillBtn.prefab", self.btnContainer[i])
				item.OnLoad = function(item)
					UI.GetChildren(item, item.transform, self.itemNodeNames)
					local on_click = function ( )
						if item.data then
	                		SkillManager.GetInstance():CastSkill(item.data.skillID)
	                	else
	                		Message:Show("技能按钮数据有误！")
	                	end
					end
					UI.BindClickEvent(item.icon_obj, on_click)
				end
				item.OnUpdate = function(item)
					if not item.data then return end
					local skillIconPath = string.format("Assets/AssetBundleRes/ui/mainui/skill_icon_%s.png", item.data.skillID)
					UI.SetImage(item, item.icon_img, skillIconPath)
					item:SetCDEndTime(SkillManager.GetInstance():GetSkillCD(item.data.skillID))
				end
				item.SetCDEndTime = function(item, cdEndTime)
					local curTime = Time:GetServerTime()
					item.cdEndTime = cdEndTime
					item.cdRingTime = cdEndTime-curTime
					item.countdown = item.countdown or item:AddUIComponent(UI.Countdown)
					item.cd_obj:SetActive(true)
					item.cd_img.fillAmount = 1
					item.countdown:CountdownByEndTime(cdEndTime, function(leftTime)
						if leftTime > 0 then
							item.left_time_txt.text = math.floor(leftTime/1000)
							local percent = leftTime/item.cdRingTime
							item.cd_img.fillAmount = percent
						else
							item.cd_obj:SetActive(false)
							item.left_time_txt.text = ""
						end
					end, 0.1)
				end
				item:Load()
				self.btnList[i] = item
			end
			item:SetActive(true)
			item:SetData({skillID=skillID, skillIndex=i})
		end
	end
end

return MainUISkillBtnView