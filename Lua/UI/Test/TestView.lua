local TestView = {
	UIConfig = {
		prefab = "Assets/AssetBundleRes/ui/prefab/test/TestView.prefab",
	},
}

function TestView:OnEnter(  )
	print('Cat:TestView.lua[OnEnter]')
	
	local names = {"icon","ok","ok/ok_txt",}
	GetChildren(self, self.transform, names)

	self.icon_img = self.icon:GetComponent("Image")
	self.ok_txt = self.ok_txt:GetComponent("Text")
	self.ok_btn = self.ok:GetComponent("Button")
	-- print('Cat:TestView.lua[15] self.icon_img, self.ok_txt', self.icon_img, self.ok_txt)

	self:AddEvent()
end

function TestView:AddEvent(  )
	print('Cat:TestView.lua[22]', self.ok_btn)
	local on_click = function (  )
		print('Cat:TestView.lua[on_click ok]')
		UIManager.Close(self)
	end
	UIHelper.BindClickEvent(self.ok_btn, on_click)
end

function TestView:OnLeave(  )
	print('Cat:TestView.lua[OnLeave]')

end

return TestView