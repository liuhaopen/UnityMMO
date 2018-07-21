-- local LoginConst = require("UI/Login/LoginConst")

local LoginView = {
	UIConfig = {
		prefab = "Assets/AssetBundleRes/ui/prefab/login/LoginView.prefab",
		-- create_bg = true,--是否在界面底下创建半透明黑色背景
		-- bg_alpha = 0.5,--背景的透明度,create_bg为true时才有效
		-- click_bg_to_close = true,--是否点击背景就关闭界面,create_bg为true时才有效
		-- destroy_delay_time = 5,--关闭界面隔这么多秒后再销毁,0的话就立即销毁
		-- show_type = UIMgr.ShowType.HideOther,--显示本界面时隐藏底下的界面
		-- components = {UIComponent.PlayOpenCloseSound, UIComponent.DelayDestroy},
	},
}

function LoginView:OnLoadOk(  )
	-- local on_load_succeed = function ( view )
	-- 	print('Cat:LoginView.lua[on_load_succeed]')
	-- 	self.view = view

	-- 	local names = {"login", "account",}
	-- 	GetChildren(self, view, names)
	-- 	self.login_btn = self.login:GetComponent("Button")
 --        self.account_txt = self.account:GetComponent("InputField")
	-- 	self:AddEvents()
	-- end
	-- PanelMgr:CreatePanel('Assets/AssetBundleRes/ui/prefab/login/LoginView.prefab', on_load_succeed)
	local names = {"login", "account",}
	GetChildren(self, self.gameObject, names)
	self.login_btn = self.login:GetComponent("Button")
    self.account_txt = self.account:GetComponent("InputField")
	self:AddEvents()
end

-- function LoginView:Close(  )
-- 	print('Cat:LoginView.lua[22] self.view.gameObject', self.view)
-- 	self.view:SetActive(false)
--     GameObject.Destroy(self.view)
-- end

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