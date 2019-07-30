local GMView = BaseClass(UINode)

function GMView:Constructor(  )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/gm/GMView.prefab",
		canvasName = "Normal",
		isShowBackground = true,
	}
	self.model = GMModel:GetInstance()
end

function GMView:OnLoad(  )
	local names = {
		"item_scroll/Viewport/item_con","item_scroll","ok_btn:obj","gm:inp","bg_con",
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

function GMView:AddEvents(  )
	local on_click = function ( click_obj )
		if click_obj == self.ok_btn_obj then	
			local gmStr = self.gm_inp.text
			if Trim(gmStr) ~= "" then
				GMController:GetInstance():ReqExcuteGM(gmStr)
			else
				Message:Show("GM不能为空")
			end
		end
	end
	UI.BindClickEvent(self.ok_btn_obj, on_click)
end

function GMView:OnUpdate(  )
	print('Cat:GMView.lua[31] self.isLoaded', self.isLoaded)
	if not self.isLoaded then return end

	local gmList = self.model:GetGmList()
	if not gmList then return end
	
	self.item_list_com = self.item_list_com or self:AddUIComponent(UI.ItemListCreator)
	local info = {
		data_list = gmList, 
		item_con = self.item_con, 
		scroll_view = self.item_scroll,
		prefab_pool_name = "Button2",
		item_width = 152,
		item_height = 56,
		space_x = 5,
		space_y = 3,
		start_x = 106,
		start_y = -28,
		child_names = {"label:txt"},
		on_update_item = function(item, i, v)
			item.gameObject.name = v.gmName
			item.label_txt.text = v.gmName
			local on_click = function ( click_obj )
				self.gm_inp.text = v.defaultGMStr
			end
			UI.BindClickEvent(item.gameObject, on_click)
		end,
	}
	self.item_list_com:UpdateItems(info)
end

return GMView