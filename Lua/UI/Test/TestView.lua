local TestView = {
	UIConfig = {
		prefab = "Assets/AssetBundleRes/ui/prefab/test/TestView.prefab",
		create_bg = true,--是否在界面底下创建半透明黑色背景
		bg_alpha = 0.5,--背景的透明度,create_bg为true时才有效
		click_bg_to_close = true,--是否点击背景就关闭界面,create_bg为true时才有效
		destroy_delay_time = 5,--关闭界面隔这么多秒后再销毁,0的话就立即销毁
		show_type = UIMgr.ShowType.HideOther,--显示本界面时隐藏底下的界面
		components = {UIComponent.PlayOpenCloseSound, UIComponent.DelayDestroy},
	},
}

function TestView:OnLoadOk(  )
	print('Cat:TestView.lua[OnLoadOk]')
	
	local names = {"close","open_hide_other_view","open_normal_view",}
	GetChildren(self, self.gameObject, names)

	self.close_btn = self.close:GetComponent("Button")
	self.open_hide_other_view_btn = self.open_hide_other_view:GetComponent("Button")
	self.open_normal_view_btn = self.open_normal_view:GetComponent("Button")
	-- print('Cat:TestView.lua[15] self.icon_img, self.ok_txt', self.icon_img, self.ok_txt)

	self:AddEvent()
end

function TestView:AddEvent(  )
	local on_click = function ( click_btn )
		print('Cat:TestView.lua[on_click ok]', tostring(click_btn))
		if self.close_btn == click_btn then
			UIMgr:Close(self)
		elseif self.open_hide_other_view_btn == click_btn then
			UIMgr:Show(require("UI/Test/TestHideOtherView").New())
		elseif self.open_normal_view_btn == click_btn then
		end
	end
	UIHelper.BindClickEvent(self.close_btn, on_click)
	UIHelper.BindClickEvent(self.open_hide_other_view_btn, on_click)
	UIHelper.BindClickEvent(self.open_normal_view_btn, on_click)
end

function TestView:OnFocusChanged( is_focus )
	
end

function TestView:OnClose(  )
	print('Cat:TestView.lua[OnClose]')

end

function TestView:OnDestroy(  )
	
end

return TestView