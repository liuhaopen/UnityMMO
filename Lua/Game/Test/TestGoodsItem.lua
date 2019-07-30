local TestGoodsItem = BaseClass(UINode)

function TestGoodsItem:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/test/TestGoodsItem.prefab",
		canvasName = "Dynamic",
	}
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
	-- local item = GoodsItem.New(self.item_con)
	-- item:Load()
	-- print('Cat:TestGoodsItem.lua[25] item, self.item_con', item, self.item_con)
	-- item:SetIcon(100000, 456)

	-- local goodsList = self.model:GetFullGoodsList(BagConst.Pos.Bag)
	local goodsList = {}
	for i=1,100 do
		table.insert(goodsList, {})
	end
	self.goods_item_com = self.goods_item_com or self:AddUIComponent(UI.ItemListCreator)
	local info = {
		data_list = goodsList, 
		item_con = self.item_con, 
		scroll_view = self.item_scroll,
		lua_pool_name = "GoodsItem",
		item_width = 86,
		item_height = 86,
		space_x = 0,
		space_y = 0,
		on_update_item = function(item, i, v)
			-- item:SetIcon(v.typeID, v.num)
			item:SetIcon(100000, 456)
		end,
	}
	self.goods_item_com:UpdateItems(info)
end	
return TestGoodsItem