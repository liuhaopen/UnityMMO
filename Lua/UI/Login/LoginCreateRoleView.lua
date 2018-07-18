-- local LoginConst = require("UI/Login/LoginConst")

local LoginCreateRoleView = {}

function LoginCreateRoleView:Open(  )
	local on_load_succeed = function ( view )
		print('Cat:LoginCreateRoleView.lua[on_load_succeed]')
		self.view = view

		local names = {"career_2","career_1","return_btn","role_name",}
		GetChildren(self, view, names)

		self.return_btn = self.return_btn:GetComponent("Button")
		self.career_1_btn = self.career_1:GetComponent("Button")
		self.career_2_btn = self.career_2:GetComponent("Button")
        self.role_name_txt = self.role_name:GetComponent("InputField")
		self:AddEvents()
	end
	PanelMgr:CreatePanel('Assets/AssetBundleRes/ui/prefab/login/LoginCreateRoleView.prefab', on_load_succeed)
	
end

function LoginCreateRoleView:Close(  )
    GameObject.Destroy(self.view.gameObject)
end

function LoginCreateRoleView:AddEvents(  )
    print('Cat:LoginCreateRoleView.lua[AddEvents]')
	local on_click = function ( click_btn )
		print('Cat:LoginCreateRoleView.lua[29] click_btn', click_btn)
		if click_btn == self.career_1_btn or click_btn == self.career_2_btn then
			local career = click_btn == self.career_1_btn and 1 or 2
			--请求创建角色
	        local on_ack = function ( ack_data )
	        	print("Cat:LoginCreateRoleView [start:35] ack_data:", ack_data)
	        	PrintTable(ack_data)
	        	print("Cat:LoginCreateRoleView [end]")
	            if ack_data.result == 1 then
	           		self:Close()

	            else
	           		print('Cat:LoginCreateRoleView.lua[37] ack_data.result', ack_data.result)
	           		--Cat_Todo : 有空要飘字提示了
	            end
	        end
	        Network.SendMessage("account_create_role", {name=self.role_name_txt.text, career=career}, on_ack)
		elseif click_btn == self.return_btn then
            local role_list = LoginModel:GetInstance():GetRoleList()
            if role_list and #role_list > 0 then
            	self:Close()
            	--跳往选择角色界面
            else
            	--返回登录界面
            end
		end
	end
	UIHelper.BindClickEvent(self.return_btn, on_click)
	UIHelper.BindClickEvent(self.career_1_btn, on_click)
	UIHelper.BindClickEvent(self.career_2_btn, on_click)
end

return LoginCreateRoleView