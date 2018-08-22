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
    for i=1,3 do
		local role_info = role_list[i]
		local item = 
		{
			UIConfig =
			{
				prefab_path = "Assets/AssetBundleRes/ui/prefab/login/LoginSelectRoleItem.prefab",
			},
			OnLoad = function(item)
				local names = {"head:img:obj","name:txt","click_bg:obj","plus:obj",}
				UI.GetChildren(item, item.transform, names)
				UIHelper.SetParent(item.transform, self.item_con)

				print('Cat:LoginSelectRoleView.lua[53] role_info, i', role_info, i)
				if role_info then
					item.name_txt.text = role_info.name
					UI.SetVisible(item.head_obj, true)
					UI.SetVisible(item.plus_obj, false)
					if role_info.career == 1 then
    					UIHelper.SetImage(item.head_img, "Assets/AssetBundleRes/ui/texutre/login/login_circle_head1_1.png")
    				else
    					UIHelper.SetImage(item.head_img, "Assets/AssetBundleRes/ui/texutre/login/login_circle_head1_2.png")
    				end
				else
					item.name_txt.text = ""
					UI.SetVisible(item.head_obj, false)
					UI.SetVisible(item.plus_obj, true)
				end
				local on_click = function (  )
					if role_info then
						print('Cat:LoginSelectRoleView.lua[51] role_info.role_id', role_info.role_id)
						self.cur_select_role_id = role_info.role_id
						--Cat_Todo : 加个人物模型才行啊,现在太难看了
					else
						--显示创建角色界面
						local view = require("UI/Login/LoginCreateRoleView").New()
                		UIMgr:Show(view)
					end
				end
				UIHelper.BindClickEvent(item.click_bg_obj, on_click)
			end
		}
		UIMgr:Show(item)
	end
	self.cur_select_role_id = role_list[1].role_id
end

return LoginSelectRoleView