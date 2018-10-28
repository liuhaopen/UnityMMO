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
	local names = {"login", "account"}
	UI.GetChildren(self, self.transform, names)
	self.login_btn = self.login.gameObject
    self.account_txt = self.account:GetComponent("InputField")

    self.transform.sizeDelta = Vector2.New(0, 0)
	self:AddEvents()
	self:UpdateView()
end

function LoginView:AddEvents(  )
	local on_click = function (  )
        local account = tonumber(self.account_txt.text)
        print('Cat:LoginView.lua[26] account, ', account, self.account_txt.text)
        if not account then
            account = 123
        end
        local login_info = {
            account = account,
            password = "password",
        }
        GlobalEventSystem:Fire(LoginConst.Event.StartLogin, login_info)
        CookieWrapper:GetInstance():SaveCookie(CookieLevelType.Common, CookieTimeType.TYPE_ALWAYS, CookieKey.LastLoginInfo, login_info)
	end
	UIHelper.BindClickEvent(self.login_btn, on_click)
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