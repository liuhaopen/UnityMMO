local TestGoodsItem = BaseClass(UINode)

function TestGoodsItem:Constructor( parentTrans )
	self.prefabPath = "Assets/AssetBundleRes/ui/test/TestGoodsItem.prefab"
	self.canvasName = "Dynamic"
	self:Load()
end


function TestGoodsItem:OnLoad(  )
	local names = {
		"item_scroll","item_scroll/Viewport/item_con",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:OnUpdate()
end

function TestGoodsItem:AddEvents(  )
end

function TestGoodsItem:OnUpdate(  )
	local item = GoodsItem.New(self.item_con)
	print('Cat:TestGoodsItem.lua[25] item, self.item_con', item, self.item_con)
	item:SetIcon(100000, 456)
end	
return TestGoodsItem