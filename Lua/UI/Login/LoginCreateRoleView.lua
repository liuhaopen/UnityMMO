local LoginCreateRoleView = BaseClass()

function LoginCreateRoleView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/prefab/login/LoginCreateRoleView.prefab",
		canvas_name = "Normal",
		components = {UIComponent.HideOtherView},
		},
	}
end

function LoginCreateRoleView:OnLoad(  )
	local names = {"career_2","career_1","return_btn","role_name","random","create_role","cur_career",}
	GetChildren(self, self.transform, names)

	self.return_btn = self.return_btn.gameObject
	self.career_1_btn = self.career_1.gameObject
	self.career_2_btn = self.career_2.gameObject
	self.create_role_btn = self.create_role.gameObject

    self.role_name_txt = self.role_name:GetComponent("InputField")
    self.cur_career_txt = self.cur_career:GetComponent("Text")

    self.cur_select_career = 1
	self:AddEvents()
	self:UpdateView()
end

function LoginCreateRoleView:AddEvents(  )
    print('Cat:LoginCreateRoleView.lua[AddEvents]')
	local on_click = function ( click_btn )
		print('Cat:LoginCreateRoleView.lua[29] click_btn', click_btn)
		if click_btn == self.career_1_btn then
			self.cur_select_career = 1
			self:UpdateView()
		elseif click_btn == self.career_2_btn then
			self.cur_select_career = 2
			self:UpdateView()
		elseif click_btn == self.return_btn then
            UIMgr:Close(self)--返回上个界面
        elseif click_btn == self.create_role_btn then
			--请求创建角色
	        local on_ack = function ( ack_data )
	        	print("Cat:LoginCreateRoleView [start:35] ack_data:", ack_data)
	        	PrintTable(ack_data)
	        	print("Cat:LoginCreateRoleView [end]")
	            if ack_data.result == 1 then
	                UIMgr:CloseAllView()
	                --正式进入游戏场景
	                Event.Brocast(LoginConst.Event.SelectRoleEnterGame, ack_data.role_id)
	            else
	           		print('Cat:LoginCreateRoleView.lua[37] ack_data.result', ack_data.result)
	           		--Cat_Todo : 有空要飘字提示了
	           		--创建角色失败
	            end
	        end
	        Network.SendMessage("account_create_role", {name=self.role_name_txt.text, career=self.cur_select_career}, on_ack)
		end
	end
	UIHelper.BindClickEvent(self.return_btn, on_click)
	UIHelper.BindClickEvent(self.career_1_btn, on_click)
	UIHelper.BindClickEvent(self.career_2_btn, on_click)
	UIHelper.BindClickEvent(self.create_role_btn, on_click)

end

function LoginCreateRoleView:UpdateView(  )
	self.cur_career_txt.text = "当前选择职业:"..self.cur_select_career
end

return LoginCreateRoleView