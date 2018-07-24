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
	local names = {"item_con","start_game",}
	GetChildren(self, self.transform, names)

	-- self.career_2_btn = self.career_2:GetComponent("Button")
	self:AddEvents()
	self:UpdateView()
end

function LoginSelectRoleView:AddEvents(  )
    print('Cat:LoginSelectRoleView.lua[AddEvents]')
	local on_click = function ( click_btn )
		print('Cat:LoginSelectRoleView.lua[29] click_btn', click_btn)
		if click_btn == self.return_btn then
            
		end
	end
	UIHelper.BindClickEvent(self.return_btn, on_click)
	UIHelper.BindClickEvent(self.career_1_btn, on_click)
	UIHelper.BindClickEvent(self.career_2_btn, on_click)
end

return LoginSelectRoleView