local TestView = BaseClass()

function TestView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/prefab/test/TestView.prefab",
		canvas_name = "Normal",
		bg_alpha = 0.5,--背景的透明度,create_bg为true时才有效
		click_bg_to_close = true,--是否点击背景就关闭界面,create_bg为true时才有效
		components = {UIComponent.PlayOpenCloseSound, UIComponent.DelayDestroy, UIComponent.Background},
		},
	}
end

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
			UIMgr:AddUIComponent(hide_other_view, UIComponent.HideOtherView)
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
	self.title_txt.text = self.data or 1
	for i=1,5 do
		local item = 
		{
			UIConfig =
			{
				prefab_path = "Assets/AssetBundleRes/ui/prefab/test/TestItem.prefab",
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

function TestView:OnVisibleChanged( is_focus )
	
end

function TestView:OnClose(  )
	print('Cat:TestView.lua[OnClose]')

end

function TestView:OnDestroy(  )
	
end

return TestView