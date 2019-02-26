local LoginSelectRoleView = BaseClass()

function LoginSelectRoleView:DefaultVar( )
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
		"item_scroll","close","role_con","role_tip","start:obj","item_scroll/Viewport/item_con",
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
		end
	end
	UIHelper.BindClickEvent(self.start_obj, on_click)

end

function LoginSelectRoleView:UpdateView()
    local role_list = LoginModel:GetInstance():GetRoleList()
    self.data = role_list
    if not role_list or #role_list <= 0 then 
    	return
    end
    self.role_item_com = self.role_item_com or UIMgr:AddUIComponent(self, UI.ItemListCreator)
    local info = {
		data_list = self.data, 
		item_con = self.item_con, 
		prefab_path = GameResPath.GetFullUIPath("login/LoginSelectRoleItem.prefab"), 
		item_height = 121,
		space_y = 15,
		scroll_view = self.item_scroll,
		child_names = {
			"head_bg:img:obj","name_bg:img:obj","role_lv:txt:outline","role_name:txt:outline","role_head:raw:obj",
		},
		on_update_item = function(item, i, v)
			self:UpdateItem(item, i, v)
		end,
	}
	self.role_item_com:UpdateItems(info)
 --    for i=1,3 do
	-- 	local role_info = role_list[i]
	-- 	local item = {
	-- 		UIConfig = {
	-- 			prefab_path = "Assets/AssetBundleRes/ui/login/LoginSelectRoleItem.prefab",
	-- 		},
	-- 		OnLoad = function(item)
	-- 			local names = {
	-- 				"head_bg:img:obj","name_bg:img:obj","role_lv:txt:outline","role_name:txt:outline","role_head:raw:obj",
	-- 			}
	-- 			UI.GetChildren(item, item.transform, names)
	-- 			UIHelper.SetParent(item.transform, self.item_con)

	-- 			print('Cat:LoginSelectRoleView.lua[53] role_info, i', role_info, i)
	-- 			self:UpdateRoleHeadItems(item, i, role_info)
	-- 		end
	-- 	}
	-- 	UIMgr:Show(item)
	-- end
	self.select_role_id = role_list[1].role_id
end

function LoginSelectRoleView:UpdateRoleHeadItems( item, index, v )
	item.data = v
	if item.data then
		item.role_name_txt.text = item.data.name
		local curLv = item.data.base_info and item.data.base_info.level or 0
		item.role_lv_txt.text = curLv.."级"
		local headRes = GameResPath.GetRoleHeadRes(item.data.career, 0)
		UIHelper.SetRawImage(item.role_head_raw, headRes)
		
		item.role_head_obj:SetActive(true)
		item.name_bg_obj:SetActive(true)
	else
		item.role_name_txt.text = ""
		item.role_lv_txt.text = ""
		item.role_head_obj:SetActive(false)
		UIHelper.SetImage(item.head_img, "login/login_circle_head1_1.png")

		item.name_bg_obj:SetActive(false)
	end
	if not item.UpdateSelect and item.data then
		item.UpdateSelect = function (item)
			local is_cur_select = self.select_role_id == item.data.role_id
			UIHelper.SetImage(item.name_bg_img, is_cur_select and "login/login_role_name_bg_sel.png" or "login/login_role_name_bg_nor.png", true)
			UIHelper.SetImage(item.head_bg_img, is_cur_select and "login/login_role_item_bg_sel.png" or "login/login_role_item_bg_nor.png", true)
			self.outline_color_sel = self.outline_color_sel or Color(143/255, 40/255, 75/255, 1)
			self.outline_color_nor = self.outline_color_nor or Color(84/255, 31/255, 49/255, 1)
			item.role_name_outline.effectColor = is_cur_select and self.outline_color_sel or self.outline_color_nor
			item.role_lv_outline.effectColor = is_cur_select and self.outline_color_sel or self.outline_color_nor
		end
	end
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

function LoginSelectRoleView:SetPlayModelInfo( role_vo )
	local show_data = {
		layer_name = self.layer_name,
		action_name_list = {"show"},
		can_rotate = true,
		scale = 200,
		position = Vector3(0, 0, 0),
		need_replay_action = "show", --界面从隐藏到显示，需要重新摆pose
	}
	-- lua_resM:SetRoleModelByVo(self, self.role_con, role_vo, show_data)
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
	
	self.select_role_career = self.select_role_info.career.."@"..self.select_role_info.sex
	lua_resM:setImageSprite(self, self.role_tip_img, "account_asset", "login_role_tip_"..self.select_role_info.sex, true)
	self.select_role_login_day = self.select_role_info.login_day
	self.select_role_Create_time  = self.select_role_info.create_role_time

	self:SetPlayModelInfo(self.select_role_info)
end

return LoginSelectRoleView