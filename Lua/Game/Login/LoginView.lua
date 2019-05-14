local LoginView = BaseClass()

function LoginView:DefaultVar( )
	return {
	UIConfig = {
			prefab_path = "Assets/AssetBundleRes/ui/login/LoginView.prefab",
			canvas_name = "Normal",
		},
	}
end

function LoginView:OnLoad(  )
	local names = {"login", "account", "single_mode:obj",}
	UI.GetChildren(self, self.transform, names)
	self.login_btn_obj = self.login.gameObject
    self.account_txt = self.account:GetComponent("InputField")
    -- local x, y, z = self.transform.localPosition.xyz
    self.transform.sizeDelta = Vector2.New(0, 0)
	self:AddEvents()
	self:UpdateView()
end

function LoginView:AddEvents(  )
	
	local on_click = function ( click_obj )
		if click_obj == self.login_btn_obj then
	        local account = tonumber(self.account_txt.text)
	        print('Cat:LoginView.lua[26] account, ', account, self.account_txt.text)
	        if not account then
	            account = 123
	        end
	        local login_info = {
	            account = account,
	            password = "password",
	            account_ip = "192.168.5.115",
	            account_port = 8001,
	            game_ip = "192.168.5.115",
	            game_port = 8888,
	        }
	        GlobalEventSystem:Fire(LoginConst.Event.StartLogin, login_info)
	        CookieWrapper:GetInstance():SaveCookie(CookieLevelType.Common, CookieTimeType.TYPE_ALWAYS, CookieKey.LastLoginInfo, login_info)
		elseif click_obj == self.single_mode_obj then
            UIMgr:CloseAllView()
			GameVariable.IsSingleMode = true
        	GlobalEventSystem:Fire(MainUIConst.Event.InitMainUIViews)
        	CS.XLuaManager.Instance:OnLoginOk()
		end
	end
	UIHelper.BindClickEvent(self.login_btn_obj, on_click)
	UIHelper.BindClickEvent(self.single_mode_obj, on_click)
end

function LoginView:UpdateView(  )
	local last_login_info = CookieWrapper:GetInstance():GetCookie(CookieLevelType.Common, CookieKey.LastLoginInfo)
	print("Cat:LoginView [start:42] last_login_info:", last_login_info)
	PrintTable(last_login_info)
	print("Cat:LoginView [end]")
	if last_login_info then
		self.account_txt.text = last_login_info.account
	end

end
        
return LoginView