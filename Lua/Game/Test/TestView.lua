local TestView = BaseClass()

function TestView:Constructor( )
	self.UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/test/TestView.prefab",
		canvas_name = "Normal",
		components = {
			-- {UI.PlayOpenCloseSound},
			{UI.DelayDestroy, {delay_time=5}},
			{UI.Background, {is_click_to_close=true, alpha=0.5}}
		}
	}
	--the same as set in self.UIConfig.Components
	-- UIMgr:AddUIComponent(self, UI.PlayOpenCloseSound)
	-- local delay_comp = UIMgr:AddUIComponent(self, UI.DelayDestroy)
	-- delay_comp:SetDelayTime(5)
	-- local bg_comp = UIMgr:AddUIComponent(self, UI.Background)
	-- bg_com:SetIsClickToClose(true)
	-- bg_com:SetBgAlpha(0.5)--背景的透明度
end
-- function TestView:DefaultVar( )
-- 	return { 
-- 	UIConfig = {
-- 		prefab_path = "Assets/AssetBundleRes/ui/test/TestView.prefab",
-- 		canvas_name = "Normal",
-- 		bg_alpha = 0.5,--背景的透明度,create_bg为true时才有效
-- 		click_bg_to_close = true,--是否点击背景就关闭界面,create_bg为true时才有效
-- 		components = {UIComponent.PlayOpenCloseSound, UIComponent.DelayDestroy, UIComponent.Background},
-- 		},
-- 	}
-- end

function TestView:OnLoad(  )
	print('Cat:TestView.lua[OnLoad]')
	local names = {"close","open_hide_other_view","open_normal_view","title","ScrollView/Viewport/item_con",}
	UI.GetChildren(self, self.transform, names)

	self.close_btn = self.close.gameObject
	self.open_hide_other_view_btn = self.open_hide_other_view.gameObject
	self.open_normal_view_btn = self.open_normal_view.gameObject
	self.title_txt = self.title:GetComponent("Text")

	print('Cat:TestView.lua[50] tostring(self.close_btn)', self.close, self.close_btn)
	self:AddEvent()
	self:UpdateView()
end

function TestView:AddEvent(  )
	local on_click = function ( click_btn, x, y )
		print('Cat:TestView.lua[on_click ok]', tostring(click_btn), x, y)
		if self.close_btn == click_btn then
			UIMgr:Close(self)
		elseif self.open_hide_other_view_btn == click_btn then
			local hide_other_view = TestView.New()
			hide_other_view:SetData((self.data or 1) + 1)
			UIMgr:AddUIComponent(hide_other_view, UI.HideOtherView)
			UIMgr:Show(hide_other_view)
		elseif self.open_normal_view_btn == click_btn then
			local view = TestView.New()
			view:SetData((self.data or 1) + 1)
			UIMgr:Show(view)
		end
	end
	UIHelper.BindClickEvent(self.close_btn, on_click)
	UIHelper.BindClickEvent(self.open_hide_other_view_btn, on_click)
	UIHelper.BindClickEvent(self.open_normal_view_btn, on_click)
end

function TestView:SetData( data )
	self.data = data
end

function TestView:UpdateView( )
	-- self:TestCreateItem()
	self:TestItemListCreator()
end

function TestView:TestCreateItem(  )
	self.title_txt.text = self.data or 1
	for i=1,5 do
		local item = 
		{
			UIConfig =
			{
				prefab_path = "Assets/AssetBundleRes/ui/test/TestItem.prefab",
			},
			OnLoad = function(item)
				local names = {"title","icon",}
				UI.GetChildren(item, item.transform, names)
				print('Cat:TestView.lua[68] self.item_con', self.item_con)
				UIHelper.SetParent(item.transform, self.item_con)
				item.title_txt = item.title:GetComponent("Text")
				item.icon_img = item.title:GetComponent("Image")
				item.title_txt.text = "I am item No."..i
			end
		}
		UIMgr:Show(item)
	end
end

function TestView:TestItemListCreator( )
	local test_flag = 1
	if not self.rank_item_com then
		self.rank_item_com = self:AddUIComponent(UI.ItemListCreator)
	end
	self.rank_item_com:Reset()
	local info = self.info or {
		data_list = {1,2,3,4,4,1,2,3,4,4,1}, 
		item_con = self.item_con, 
		item_height = 60, 
		-- item_width = 250, 
		space_y = 5, 
		start_y = -50,
		space_x=10,
		scroll_view = self.scroll_view,
		-- reuse_item_num = 0,
		create_frequency = 0.0,
		-- is_scroll_back_on_update = true,
	}
	-- table.insert(info.data_list, 1)
	table.remove(info.data_list, 1)
	self.info = info
	if test_flag == 1 then
		info.item_class = TestItem
		info.on_update_item = function(item, i, v)
			item:SetData({i=i, v=v})
		end
	elseif test_flag == 2 then
		info.prefab_ab_name = "test"
		info.prefab_res_name = "TestItem"
		info.child_names = {"city_name:img","room_name:img","num:txt",}
		info.on_update_item = function(item, i, v)
			item.num_txt.text = i
		end
	elseif test_flag == 3 then
		info.obj_pool_type = UIObjPool.UIType.AwardItem
		info.item_height = 80
		info.space_y = 10
		info.on_update_item = function(item, i, v)
			item:SetData(110003, i)
		end
	end
	self.rank_item_com:UpdateItems(info)
end

function TestView:OnClose(  )
	print('Cat:TestView.lua[OnClose]')

end

function TestView:OnDestroy(  )
	
end

return TestView