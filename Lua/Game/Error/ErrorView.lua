local ErrorView = {}

function ErrorView:Open(  )
	local on_load_succeed = function ( view )
		print('Cat:ErrorView.lua[on_load_succeed]')
		self.view = view

		local names = {"login", "account",}
		UI.GetChildren(self, view, names)
		self.login_btn = self.login:GetComponent("Button")
        self.account_txt = self.account:GetComponent("InputField")
        print('Cat:ErrorView.lua[16] self.account_txt', self.account_txt)
		self:AddEvents()
	end
	PanelMgr:CreatePanel('Assets/AssetBundleRes/ui/gm/ErrorView.prefab', on_load_succeed)
	
end

function ErrorView:AddEvents(  )
    
end

return ErrorView