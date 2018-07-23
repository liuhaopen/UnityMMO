local TestView = BaseClass()

function TestView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/prefab/test/TestView.prefab",
		canvas_name = "Normal",
		is_sync_load = false,--是否同步加载prefab
		bg_alpha = 0.5,--背景的透明度,create_bg为true时才有效
		click_bg_to_close = true,--是否点击背景就关闭界面,create_bg为true时才有效
		components = {UIComponent.PlayOpenCloseSound, UIComponent.DelayDestroy, UIComponent.Background},
		},
	}
end

function TestView:OnLoad(  )
	print('Cat:TestView.lua[OnLoad]')
	local names = {"close","open_hide_other_view","open_normal_view","title",}
	GetChildren(self, self.transform, names)

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
		-- elseif self.open_hide_other_view_btn == click_btn then
		-- 	local hide_other_view = require("UI/Test/TestHideOtherView").New()
		-- 	UIMgr:Show(hide_other_view)
		-- elseif self.open_normal_view_btn == click_btn then
		-- 	local view = require("UI/Test/TestNormalView").New()
		-- 	UIMgr:Show(view)
		elseif self.open_hide_other_view_btn == click_btn then
			local hide_other_view = TestView.New()
			print('Cat:TestView.lua[43] hide_other_view, ', hide_other_view, TestView)
			hide_other_view:SetData((self.data or 1) + 1)
			UIMgr:AddUIComponent(hide_other_view, UIComponent.HideOtherView)
			UIMgr:Show(hide_other_view)
		elseif self.open_normal_view_btn == click_btn then
			local view = TestView.New()
			view:SetData((self.data or 1) + 1)
			UIMgr:Show(view)
		end
	end
	-- print('Cat:TestView.lua[50] tostring(self.close_btn)', self.close_btn, self.open_hide_other_view_btn, self.open_normal_view_btn)
	UIHelper.BindClickEvent(self.close_btn, on_click)
	UIHelper.BindClickEvent(self.open_hide_other_view_btn, on_click)
	UIHelper.BindClickEvent(self.open_normal_view_btn, on_click)
end

function TestView:SetData( data )
	self.data = data
end

function TestView:UpdateView( )
	self.title_txt.text = self.data or 1
	do return end
	local item = {
		UIConfig={
		prefab = "Assets/AssetBundleRes/ui/prefab/test/TestItem.prefab",
		is_sync_load=true,--同步加载
		}
	}
	UIMgr:Show(item)
	local names = {"close","open_hide_other_view","open_normal_view",}
	GetChildren(item, item.gameObject, names)
	item.transform:SetParent(self.transform)
end

function TestView:OnVisibleChanged( is_focus )
	
end

function TestView:OnClose(  )
	print('Cat:TestView.lua[OnClose]')

end

function TestView:OnDestroy(  )
	
end

return TestView