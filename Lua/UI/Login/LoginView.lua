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
end

function LoginView:AddEvents(  )
	local on_click = function (  )
        local account = tonumber(self.account_txt.text)
        if not account then
            account = 123
        end
        local login_info = {
            account = account
        }
        GlobalEventSystem:Fire(LoginConst.Event.StartLogin, login_info)
	end
	UIHelper.BindClickEvent(self.login_btn, on_click)
end
        
return LoginView