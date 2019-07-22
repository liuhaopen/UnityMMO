local BagView = BaseClass(UINode)

function BagView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/bag/BagView.prefab",
		canvasName = "Normal",
	}
	self.model = BagModel:GetInstance()
end

function BagView:OnLoad(  )
	local names = {
		"item_scroll","sort:obj","role_look:raw","item_scroll/Viewport/item_con","swallow:obj",
	}
	UI.GetChildren(self, self.transform, names)
	self:AddEvents()
	self:OnUpdate()
end

function BagView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.sort_obj == click_obj then
			local on_ack = function ( ackData )
				self:OnUpdate()
			end
		    NetDispatcher:SendMessage("Bag_Sort", {pos=BagConst.Pos.Bag}, on_ack)
		elseif self.swallow_obj == click_obj then
			Message:Show("功能未开启")
		end
	end
	UI.BindClickEvent(self.swallow_obj, on_click)
	UI.BindClickEvent(self.sort_obj, on_click)
	
	self:BindEvent(self.model, BagConst.Event.BagChange, function()
		self:UpdateGoodsItems()
	end)
end

function BagView:OnUpdate(  )
	self:UpdateRoleLooks()
	self:UpdateGoodsItems()
	self:UpdateEquips()
end

function BagView:UpdateGoodsItems(  )
	local goodsList = self.model:GetFullGoodsList(BagConst.Pos.Bag)
	print("Cat:BagView [start:46] goodsList: ", goodsList)
	PrintTable(goodsList)
	print("Cat:BagView [end]")
	self.goods_item_com = self.goods_item_com or self:AddUIComponent(UI.ItemListCreator)
	local info = {
		data_list = goodsList, 
		item_con = self.item_con, 
		scroll_view = self.item_scroll,
		lua_pool_name = "GoodsItem",
		item_width = 86,
		item_height = 86,
		space_x = 5,
		space_y = 5,
		on_update_item = function(item, i, v)
			local isEmpty = not v
			if isEmpty then
				item:SetBg("Assets/AssetBundleRes/ui/bag/bag_item_bg.png")
			else
				item:SetIcon(v.typeID, v.num)
			end
		end,
	}
	self.goods_item_com:UpdateItems(info)
end

function BagView:UpdateRoleLooks(  )
	
end

function BagView:UpdateEquips(  )
	
end

return BagView