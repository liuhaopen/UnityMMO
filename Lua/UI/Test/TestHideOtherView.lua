local TestHideOtherView = BaseClass()

function TestHideOtherView:DefaultVar()
	print('Cat:TestHideOtherView.lua[DefaultVar]')
	return {UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/prefab/test/TestHideOtherView.prefab",
		canvas_name = "Normal",
		-- show_type = UIMgr.ShowType.HideOther,--显示本界面时隐藏底下的界面
		create_bg = true,--是否在界面底下创建半透明黑色背景
		bg_alpha = 0.5,--背景的透明度,create_bg为true时才有效
		click_bg_to_close = true,--是否点击背景就关闭界面,create_bg为true时才有效
		destroy_delay_time = 5,--关闭界面隔这么多秒后再销毁,0的话就立即销毁
		components = {UIComponent.HideOtherView},
	}}
end

function TestHideOtherView:Constructor()
	print('Cat:TestHideOtherView.lua[Constructor]')
end

function TestHideOtherView:OnLoad(  )
	print('Cat:TestHideOtherView.lua[OnLoad]')
	print('Cat:TestHideOtherView.lua[15] self.close', self.close)
	local names = {"close",}
	GetChildren(self, self.gameObject, names)

	self.close_btn = self.close:GetComponent("Button")

	self:AddEvent()
end

function TestHideOtherView:AddEvent(  )
	print('Cat:TestHideOtherView.lua[22]', self.close_btn)
	local on_click = function (  )
		print('Cat:TestHideOtherView.lua[on_click ok]')
		UIMgr:Close(self)
	end
	UIHelper.BindClickEvent(self.close_btn, on_click)
end

function TestHideOtherView:OnVisibleChanged( new_visible )
	
end

function TestHideOtherView:OnClose(  )
	print('Cat:TestHideOtherView.lua[OnClose]')

end

function TestHideOtherView:OnDestroy(  )
	
end

return TestHideOtherView