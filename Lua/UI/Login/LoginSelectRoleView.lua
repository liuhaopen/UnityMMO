local LoginSelectRoleView = BaseClass()

function LoginSelectRoleView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/prefab/login/LoginSelectRoleView.prefab",
		canvas_name = "Normal",
		components = {UIComponent.HideOtherView},
		},
	}
end

function LoginSelectRoleView:OnLoad(  )
	local names = {"item_con","start_game:obj",}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:UpdateView()
end

function LoginSelectRoleView:AddEvents(  )
    print('Cat:LoginSelectRoleView.lua[AddEvents]')
	local on_click = function ( click_btn )
		print('Cat:LoginSelectRoleView.lua[29] click_btn', click_btn)
        if click_btn == self.start_game_obj then
        	print('Cat:LoginSelectRoleView.lua[26]')
            GlobalEventSystem:Fire(LoginConst.Event.SelectRoleEnterGame, self.cur_select_role_id)
            print('Cat:LoginSelectRoleView.lua[28]')
		end
	end
	UIHelper.BindClickEvent(self.start_game_obj, on_click)

end

function LoginSelectRoleView:UpdateView()
    local role_list = LoginModel:GetInstance():GetRoleList()
    if not role_list or #role_list <= 0 then 
    	return
    end
	for i,v in ipairs(role_list) do
		local item = 
		{
			UIConfig =
			{
				prefab_path = "Assets/AssetBundleRes/ui/prefab/login/LoginSelectRoleItem.prefab",
			},
			OnLoad = function(item)
				local names = {"head:img","name:txt","click_bg:obj",}
				UI.GetChildren(item, item.transform, names)
				UIHelper.SetParent(item.transform, self.item_con)
				item.name_txt.text = v.name
				local on_click = function (  )
					print('Cat:LoginSelectRoleView.lua[51] v.role_id', v.role_id)
					self.cur_select_role_id = v.role_id
					--Cat_Todo : 加个人物模型才行啊,现在太难看了
				end
				UIHelper.BindClickEvent(item.click_bg_obj, on_click)
			end
		}
		UIMgr:Show(item)
	end
	self.cur_select_role_id = role_list[1].role_id
end

return LoginSelectRoleView