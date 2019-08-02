local LoginCreateRoleView = BaseClass()

function LoginCreateRoleView:DefaultVar( )
    require("Game/Login/LoginSceneBgView"):SetActive(true)
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/login/LoginCreateRoleView.prefab",
		canvas_name = "Normal",
		components = {
				{UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	select_sex = 1,
	is_first_select = true,
	}
end

function LoginCreateRoleView:OnLoad(  )
	local names = {
		"head_scroll/Viewport/head_con","close:obj","create_role:obj","item_con","head_scroll","role_tip:img","random:obj","role_name:input","effect","select_role_con:raw",
	}
	UI.GetChildren(self, self.transform, names)
	self.transform.sizeDelta = Vector2.zero

	self:AddEvents()
	self:InitView()
	self:LoadRolesModel()
end

function LoginCreateRoleView:LoadRolesModel(  )
	local sex = math.random(1, 2)
	self:SetCurSelectSex(sex)
end

function LoginCreateRoleView:InitView(  )
	self.sex_item_com = self.sex_item_com or UIMgr:AddUIComponent(self, UI.ItemListCreator)
	local info = {
		data_list = {1,2}, 
		item_con = self.item_con, 
		prefab_path = ResPath.GetFullUIPath("login/LoginCreateRoleItem.prefab"), 
		item_height = 128,
		space_y = 5,
		scroll_view = self.item_con,
		child_names = {"sex:txt","role_head:raw","bg:img:obj",},
		on_update_item = function(item, i, v)
			print('Cat:LoginCreateRoleView.lua[47] item, i, v', item, i, v)
			item.sex_txt.text = v==1 and "男" or "女"
			item.sex_value = v
			if not item.UpdateSelect then
				item.UpdateSelect = function(item)
					local cur_is_select = self.select_sex==v
					print('Cat:LoginCreateRoleView.lua[53] cur_is_select, item, item.bg_img', cur_is_select, item, item.bg_img)
					UI.SetImage(item, item.bg_img, cur_is_select and "login/login_role_item_bg_sel.png" or "login/login_role_item_bg_nor.png", true)
				end
			end
			item:UpdateSelect()
			if not item.UpdateHead then
				item.UpdateHead = function(item)
					local headRes = ResPath.GetRoleHeadRes(v, 0)
					UI.SetRawImage(item, item.role_head_raw, headRes)
				end
			end
			item:UpdateHead()
			if not item.had_bind_click then
				item.had_bind_click = true
				local on_click = function ( )
					print('Cat:LoginCreateRoleView.lua[66] item.sex_value', item.sex_value)
					self:SetCurSelectSex(item.sex_value)
				end
				UIHelper.BindClickEvent(item.bg_obj, on_click)
			end
		end,
	}
	self.sex_item_com:UpdateItems(info)
end

function LoginCreateRoleView:SetCurSelectSex( sex )
	print('Cat:LoginCreateRoleView.lua[75] sex', sex)
	if self.select_sex == sex and not self.is_first_select then
		return
	end
	self.is_first_select = false
	self.select_sex = sex
	self.sex_item_com:IterateItems(function(item, i)
		if item.UpdateSelect then
			item:UpdateSelect()
		end
	end)
	-- self:UpdateRoleHead()
	UI.SetImage(self, self.role_tip_img, "login/login_role_tip_"..sex..".png", true)
	self:OnClickRandomName()
	self:UpdateRoleMesh(sex)
	-- self:PlayRoleSound(sex)
end

function LoginCreateRoleView:UpdateRoleMesh( sex )
	local show_data = {
		showType = UILooksNode.ShowType.Role,
		showRawImg = self.select_role_con_raw,
		body = 1,
		hair = 1,
		career = sex==1 and 1 or 2,
		canRotate = true,
	}
	self.roleUILooksNode = self.roleUILooksNode or UILooksNode.New(self.select_role_con)
	self.roleUILooksNode:SetData(show_data)
end

function LoginCreateRoleView:OnClickRandomName(  )
	local nick_name = nil
	math.randomseed(os.clock() * 10000)
	self.surname = self.surname or {"开朗哒","开朗的","可爱哒","可爱的","刻苦的",}
	self.forename_man = self.forename_man or {"刚史","恭平","光希","光一","圭介",}
	self.forename_woman = self.forename_woman or {"菜菜","成美","赤穗","初音","雏子",}
	nick_name = self.surname[math.ceil(#self.surname * math.random())]
	if self.select_sex == 1 then --临时随机名称，也可以用配置列表
		nick_name = nick_name .. self.forename_man[math.ceil(#self.forename_man*math.random())]
	else
		nick_name = nick_name .. self.forename_woman[math.ceil(#self.forename_woman*math.random())]
	end
	print('Cat:LoginCreateRoleView.lua[106] nick_name', nick_name)
	self.role_name_input.text = nick_name
end

function LoginCreateRoleView:AddEvents(  )
	local on_click = function ( click_btn )
		print('Cat:LoginCreateRoleView.lua[29] click_btn', click_btn)
		if click_btn == self.close_obj then
            UIMgr:Close(self)--返回上个界面
        elseif click_btn == self.random_obj then
			self:OnClickRandomName()
        elseif click_btn == self.create_role_obj then
			--请求创建角色
	        local on_ack = function ( ack_data )
	        	print("Cat:LoginCreateRoleView [start:35] ack_data:", ack_data)
	        	PrintTable(ack_data)
	        	print("Cat:LoginCreateRoleView [end]")
	            if ack_data.result == 0 then
	                UIMgr:CloseAllView()
	                require("Game/Login/LoginSceneBgView"):SetActive(false)
	                --正式进入游戏场景
	                GlobalEventSystem:Fire(LoginConst.Event.SelectRoleEnterGame, ack_data.role_id)
	            else
	           		print('Cat:LoginCreateRoleView.lua[37] ack_data.result', ack_data.result)
	           		--创建角色失败
            		Message:Show("创建角色失败,错码码："..ack_data.result)
	            end
	        end
	        NetDispatcher:SendMessage("account_create_role", {name=self.role_name_input.text, career=self.select_sex}, on_ack)
		end
	end
	UIHelper.BindClickEvent(self.close_obj, on_click)
	UIHelper.BindClickEvent(self.create_role_obj, on_click)
	UIHelper.BindClickEvent(self.random_obj, on_click)

end

function LoginCreateRoleView:OnClose(  )
    -- require("Game/Login/LoginSceneBgView"):SetActive(false)
end

return LoginCreateRoleView