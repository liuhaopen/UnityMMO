local LoginConst = require("UI/Login/LoginConst")

local LoginView = {}

function LoginView:Open(  )
	local on_load_succeed = function ( view )
		print('Cat:LoginView.lua[on_load_succeed]')
		self.view = view

		local names = {"login", "account",}
		GetChildren(self, view, names)
		self.login_btn = self.login:GetComponent("Button")
        self.account_txt = self.account:GetComponent("InputField")
        print('Cat:LoginView.lua[16] self.account_txt', self.account_txt)
		self:AddEvents()
	end
	PanelMgr:CreatePanel('Assets/AssetBundleRes/ui/prefab/login/LoginView.prefab', on_load_succeed)
	
end

function LoginView:AddEvents(  )
    print('Cat:LoginView.lua[AddEvents]')
	local on_click = function (  )
		print('Cat:LoginView.lua[on_click]', self.account_txt.text)
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