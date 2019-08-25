local LoginView = BaseClass(UINode)

function LoginView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/login/LoginView.prefab",
		canvasName = "Normal",
	}
end

function LoginView:OnLoad(  )
	local names = {
		"login", "account", "server_ip", "port", "game_server_ip:input","game_port:input","setting:obj",
	}
	UI.GetChildren(self, self.transform, names)
	self.login_btn_obj = self.login.gameObject
    self.account_txt = self.account:GetComponent("InputField")
    self.server_ip_txt = self.server_ip:GetComponent("InputField")
    self.port_txt = self.port:GetComponent("InputField")
    self.transform.sizeDelta = Vector2.New(0, 0)
	self:AddEvents()
	self:UpdateView()
	CS.UnityMMO.LoadingView.Instance:SetActive(false)
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
	            account_ip = self.server_ip_txt.text,
	            account_port = self.port_txt.text,
	            game_ip = self.game_server_ip_input.text,
	            game_port = self.game_port_input.text,
	        }
	        print("Cat:LoginView [start:40] login_info:", login_info)
	        PrintTable(login_info)
	        print("Cat:LoginView [end]")
	        GlobalEventSystem:Fire(LoginConst.Event.StartLogin, login_info)
	        CookieWrapper:GetInstance():SaveCookie(CookieLevelType.Common, CookieTimeType.TYPE_ALWAYS, CookieKey.LastLoginInfo, login_info)
		elseif click_obj == self.setting_obj then
			local view = require("Game.GM.GMSettingView").New()
			view:Load()
			-- elseif click_obj == self.single_mode_obj then
  --           UIMgr:CloseAllView()
		-- 	GameVariable.IsSingleMode = true
  --       	GlobalEventSystem:Fire(GlobalEvents.GameStart)
  --       	CS.XLuaManager.Instance:OnLoginOk()
		end
	end
	UIHelper.BindClickEvent(self.login_btn_obj, on_click)
	UIHelper.BindClickEvent(self.setting_obj, on_click)
	-- UIHelper.BindClickEvent(self.single_mode_obj, on_click)
end

function LoginView:UpdateView(  )
	local last_login_info = CookieWrapper:GetInstance():GetCookie(CookieLevelType.Common, CookieKey.LastLoginInfo)
	print("Cat:LoginView [start:42] last_login_info:", last_login_info)
	PrintTable(last_login_info)
	print("Cat:LoginView [end]")
	if last_login_info then
		self.account_txt.text = last_login_info.account or ""
		self.server_ip_txt.text = last_login_info.account_ip or "192.168.5.132"
		self.port_txt.text = last_login_info.account_port or "8001"
		self.game_server_ip_input.text = last_login_info.game_ip or "192.168.5.132"
        self.game_port_input.text = last_login_info.game_port or "8888"
	end
end
        
return LoginView