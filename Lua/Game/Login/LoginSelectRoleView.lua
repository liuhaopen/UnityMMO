local LoginSelectRoleView = BaseClass()

function LoginSelectRoleView:DefaultVar( )
    require("Game/Login/LoginSceneBgView"):SetActive(true)
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/login/LoginSelectRoleView.prefab",
		canvas_name = "Normal",
		components = {
				{UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	}
end

function LoginSelectRoleView:OnLoad(  )
	local names = {
		"item_scroll","role_mesh:raw","role_tip:img","start:obj","item_scroll/Viewport/item_con",
	}
	UI.GetChildren(self, self.transform, names)
	self.transform.sizeDelta = Vector2.zero

	self:AddEvents()
	self:UpdateView()
end

function LoginSelectRoleView:AddEvents(  )
	local on_click = function ( click_btn )
        if click_btn == self.start_obj then
            GlobalEventSystem:Fire(LoginConst.Event.SelectRoleEnterGame, self.select_role_id)
			CookieWrapper:GetInstance():SaveCookie(CookieLevelType.Common, CookieTimeType.TYPE_ALWAYS, CookieKey.LastSelectRoleID, self.select_role_id)
		end
	end
	UI.BindClickEvent(self.start_obj, on_click)

end

function LoginSelectRoleView:UpdateView()
    local role_list = LoginModel:GetInstance():GetRoleList()
    self.data = role_list
    if not role_list or #role_list <= 0 then 
    	return
    end
    self.role_item_com = self.role_item_com or UIMgr:AddUIComponent(self, UI.ItemListCreator)
    --最少要显示3个
	local min_role_num = 3
	if #self.data < min_role_num then
		for i=1,min_role_num - #self.data do
			table.insert(self.data, false)
		end
	end
    local info = {
		data_list = self.data, 
		item_con = self.item_con, 
		prefab_path = ResPath.GetFullUIPath("login/LoginSelectRoleItem.prefab"), 
		item_height = 121,
		space_y = 15,
		scroll_view = self.item_scroll,
		child_names = {
			"head_bg:img:obj","name_bg:img:obj","role_lv:txt","role_name:txt","role_head:raw:obj",
		},
		on_update_item = function(item, i, v)
			self:UpdateRoleHeadItems(item, i, v)
		end,
	}
	self.role_item_com:UpdateItems(info)

	local best_role_id = nil
	local last_role_id = CookieWrapper.Instance:GetCookie(CookieLevelType.Common, CookieKey.LastSelectRoleID)
	print('Cat:LoginSelectRoleView.lua[75] last_role_id', last_role_id)
	if last_role_id then
		for i,v in ipairs(self.data) do
			if v and v.role_id == last_role_id then
				best_role_id = v.role_id
				break
			end
		end
	end
	if #self.data > 0 and not best_role_id then
		best_role_id = self.data[1].role_id
	end
	print('Cat:LoginSelectRoleView.lua[87] best_role_id', best_role_id)
	if best_role_id then
		self:SetCurSelectRoleID(best_role_id)
	end
end

function LoginSelectRoleView:UpdateRoleHeadItems( item, index, v )
	item.data = v
	if item.data then
		item.role_name_txt.text = item.data.name
		local curLv = item.data.base_info and item.data.base_info.level or 0
		item.role_lv_txt.text = curLv.."级"
		local headRes = ResPath.GetRoleHeadRes(item.data.career, 0)
		UI.SetRawImage(item, item.role_head_raw, headRes)
		
		item.role_head_obj:SetActive(true)
		item.name_bg_obj:SetActive(true)
	else
		item.role_name_txt.text = ""
		item.role_lv_txt.text = ""
		item.role_head_obj:SetActive(false)
		UI.SetRawImage(item, item.role_head_raw, ResPath.GetRoleHeadRes(2, 0))

		item.name_bg_obj:SetActive(false)
	end
	if not item.UpdateSelect and item.data then
		item.UpdateSelect = function (item)
			local is_cur_select = self.select_role_id == item.data.role_id
			print('Cat:LoginSelectRoleView.lua[110] is_cur_select', is_cur_select)
			UI.SetImage(item, item.name_bg_img, is_cur_select and "login/login_role_name_bg_sel.png" or "login/login_role_name_bg_nor.png", true)
			UI.SetImage(item, item.head_bg_img, is_cur_select and "login/login_role_item_bg_sel.png" or "login/login_role_item_bg_nor.png", true)
			-- self.outline_color_sel = self.outline_color_sel or Color(143/255, 40/255, 75/255, 1)
			-- self.outline_color_nor = self.outline_color_nor or Color(84/255, 31/255, 49/255, 1)
			-- item.role_name_outline.effectColor = is_cur_select and self.outline_color_sel or self.outline_color_nor
			-- item.role_lv_outline.effectColor = is_cur_select and self.outline_color_sel or self.outline_color_nor
		end
	end
	print('Cat:LoginSelectRoleView.lua[118] item.data', item.data)
	if item.data then
		item:UpdateSelect()
	end

	if not item.had_bind_click then
		item.had_bind_click = true
		local on_item_click = function ( )
			if item.data then
				self:SetCurSelectRoleID(item.data.role_id)
			else
				--显示创建角色界面
				local view = require("Game/Login/LoginCreateRoleView").New()
	    		UIMgr:Show(view)
			end
		end
		UIHelper.BindClickEvent(item.gameObject, on_item_click)
	end
end

function LoginSelectRoleView:GetRoleInfoByID( role_id )
	for k,v in pairs(self.data) do
		if v.role_id == role_id then
			return v
		end
	end
	return nil
end

function LoginSelectRoleView:UpdateRoleMesh( role_vo )
	local show_data = {
		showType = UILooksNode.ShowType.Role,
		showRawImg = self.role_mesh_raw,
		body = role_vo.body,
		hair = role_vo.hair,
		career = role_vo.career,
		canRotate = true,
		position = Vector3(0, 0, 0),
	}
	self.roleUILooksNode = self.roleUILooksNode or UILooksNode.New(self.role_mesh)
	self.roleUILooksNode:SetData(show_data)
end

function LoginSelectRoleView:SetCurSelectRoleID( role_id )
	if self.select_role_id == role_id or not self.data then
		return
	end
	self.select_role_id = role_id
	self.select_role_info = self:GetRoleInfoByID(role_id)
	if not self.select_role_info then return end

	self.role_item_com:IterateItems(function(item, i)
		if item.UpdateSelect then
			item:UpdateSelect()
		end
	end)
	
	self.select_role_career = self.select_role_info.career
	UI.SetImage(self, self.role_tip_img, "login/login_role_tip_"..self.select_role_career..".png", true)
	-- self.select_role_login_day = self.select_role_info.login_day
	-- self.select_role_Create_time  = self.select_role_info.create_role_time
	self:UpdateRoleMesh(self.select_role_info)
end

function LoginSelectRoleView:OnClose(  )
    require("Game/Login/LoginSceneBgView"):SetActive(false)
end

return LoginSelectRoleView