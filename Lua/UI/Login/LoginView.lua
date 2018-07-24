local LoginView = {
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/prefab/login/LoginView.prefab",
		canvas_name = "Normal",
	},
}

function LoginView:OnLoad(  )
	local names = {"login", "account",}
	GetChildren(self, self.transform, names)
	self.login_btn = self.login.gameObject
    self.account_txt = self.account:GetComponent("InputField")
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
        Event.Brocast(LoginConst.Event.StartLogin, login_info)
	end
	UIHelper.BindClickEvent(self.login_btn, on_click)
end

return LoginView