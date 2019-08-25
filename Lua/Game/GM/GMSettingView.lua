local GMSettingView = BaseClass(UINode)
local ConfigGame = CS.UnityMMO.ConfigGame
function GMSettingView:Constructor(  )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/gm/GMSettingView.prefab",
		canvasName = "Normal",
		isShowBackground = true,
	}
	self.model = GMModel:GetInstance()
end

function GMSettingView:OnLoad(  )
	local names = {
		"item_scroll/Viewport/item_con","item_scroll","ok_btn:obj","bg_con","value:inp","key:txt",
	}
	UI.GetChildren(self, self.transform, names)

	self.window = UI.Window.Create()
	local winData = {
		style = "WindowStyle.NoTab", 
		bg = "default",
		onClose = function()
			self:Unload()
		end,
	}
	self.window:Load(winData)
	self.window:SetParent(self.bg_con)
	self:AutoDestroy(self.window)
	self:AddEvents()
	self:OnUpdate()
end

function GMSettingView:AddEvents(  )
	local on_click = function ( click_obj )
        if click_obj == self.ok_btn_obj then	
            if not self.curSelectKey or self.curSelectKey=="" then
                Message:Show("未选中需要修改的值")
                return
            end
			local newValue = self.value_inp.text
			if Trim(newValue) ~= "" then
                ConfigGame.GetInstance().Data[self.curSelectKey] = newValue
                ConfigGame.GetInstance():Save();
                Message:Show("修改成功!")
			else
				Message:Show("值不能为空")
			end
		end
	end
	UI.BindClickEvent(self.ok_btn_obj, on_click)
end

function GMSettingView:GetSettingList()
    if not self.settingList then
        self.settingList = {
            {name="文件服务器地址", key="FileServerURL"},
        }
    end
    return self.settingList
end

function GMSettingView:OnUpdate(  )
	print('Cat:GMSettingView.lua[31] self.isLoaded', self.isLoaded)
	if not self.isLoaded then return end

	local settingList = self:GetSettingList()
	
	self.item_list_com = self.item_list_com or self:AddUIComponent(UI.ItemListCreator)
	local info = {
		data_list = settingList, 
		item_con = self.item_con, 
		scroll_view = self.item_scroll,
		prefab_pool_name = "Button2",
		item_width = 200,
		item_height = 56,
		space_x = 5,
		space_y = 3,
		start_x = 106,
		start_y = -28,
		child_names = {"label:txt"},
        on_update_item = function(item, i, v)
            UI.SetSizeDeltaX(item.transform, 200)
			item.gameObject.name = v.name
			item.label_txt.text = v.name
            local on_click = function ( click_obj )
                self.curSelectKey = v.key
				self.key_txt.text = v.key
				self.value_inp.text = ConfigGame.GetInstance().Data[v.key]
			end
			UI.BindClickEvent(item.gameObject, on_click)
		end,
	}
	self.item_list_com:UpdateItems(info)
end

return GMSettingView