local BagGoodsItem = BaseClass(UINode)

function BagGoodsItem:Constructor( )
	self.goodsItem = GoodsItem.Create()
	self.goodsItem:Load()
	self:Attach(self.goodsItem)
end

function BagGoodsItem:OnUpdate(  )
	if not self.isLoaded then
		return
	end
	local isEmpty = not self.data
	self.goodsItem:Reset()
	self.goodsItem:SetIconVisible(not isEmpty)
	if isEmpty then
		self.goodsItem:SetBg("Assets/AssetBundleRes/ui/bag/bag_item_bg.png")
	else
		self.goodsItem:SetIcon(self.data.typeID, self.data.num)
	end
	local on_click = function ( )
		print('Cat:BagView.lua[68] self.data', self.data)
		if self.data then
			BagController:GetInstance():ShowGoodsTips(self.data, self.showData)
		else
			--Cat_Todo : 判断是否要显示扩展背包格子界面
		end
	end
	self.goodsItem:SetClickFunc(on_click)
end

function BagGoodsItem:SetShowData( showData )
	self.showData = showData
end

function BagGoodsItem:OnDestroy(  )
	if self.goodsItem then
		LuaPool:Recycle("GoodsItem", self.goodsItem)
		self.goodsItem = nil
	end
end

return BagGoodsItem