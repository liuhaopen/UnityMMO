local TestHideOtherView = BaseClass()

function TestHideOtherView:DefaultVar()
	return {UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/test/TestHideOtherView.prefab",
		canvas_name = "Normal",
		components = {
			{UI.HideOtherView},
			{UI.DelayDestroy, {delay_time=5}},
			{UI.Background, {is_click_to_close=true, alpha=0.5}},
		},
	}}
end

function TestHideOtherView:Constructor()
	print('Cat:TestHideOtherView.lua[Constructor]')
end

function TestHideOtherView:OnLoad(  )
	print('Cat:TestHideOtherView.lua[OnLoad]')
	print('Cat:TestHideOtherView.lua[15] self.close', self.close)
	local names = {"close",}
	UI.GetChildren(self, self.gameObject, names)

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