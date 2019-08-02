local BagView = BaseClass(UINode)

function BagView:Constructor( parent )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/bag/BagView.prefab",
		parentTrans = parent,
	}
	self.model = BagModel:GetInstance()
end

function BagView:OnLoad(  )
	local names = {
		"item_scroll","sort:obj","role_look:raw","item_scroll/Viewport/item_con","swallow:obj","name:txt","equip_con",
	}
	UI.GetChildren(self, self.transform, names)

	self:InitEquips()
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
	
	self:BindEvent(self.model, BagConst.Event.BagChange, function(pos)
		print('Cat:BagView.lua[37] pos', pos)
		if not pos or pos == BagConst.Pos.Bag then
			self:UpdateGoodsItems()
		end
	end)
end

function BagView:OnUpdate(  )
	self:UpdateRoleLooks()
	self:UpdateGoodsItems()
	self:UpdateEquips()
end

function BagView:UpdateGoodsItems(  )
	local goodsList = self.model:GetFullGoodsList(BagConst.Pos.Bag)
	self.goods_item_com = self.goods_item_com or self:AddUIComponent(UI.ItemListCreator)
	self.infoViewShowData = self.infoViewShowData or {
		comeFrom = "BagView"
	}
	local info = {
		data_list = goodsList, 
		item_con = self.item_con, 
		scroll_view = self.item_scroll,
		create_frequency = 0.1,
		create_num_per_time = 5,
		item_class = require("Game.Bag.BagGoodsItem"),
		item_width = 86,
		item_height = 86,
		space_x = 4,
		space_y = 4,
		on_update_item = function(item, i, v)
			item:SetShowData(self.infoViewShowData)
			item:SetData(v)
		end,
	}
	self.goods_item_com:UpdateItems(info)
end

function BagView:UpdateRoleLooks(  )
	local mainRoleLooksInfo = LRoleMgr:GetMainRoleLooksInfo()
	if not mainRoleLooksInfo then return end
	
	local show_data = {
		showType = UILooksNode.ShowType.Role,
		showRawImg = self.role_look_raw,
		body = mainRoleLooksInfo.body,
		hair = mainRoleLooksInfo.hair,
		career = mainRoleLooksInfo.career,
		canRotate = true,
		position = Vector3(0, 0, 0),
	}
	self.roleUILooksNode = self.roleUILooksNode or UILooksNode.New(self.role_look)
	self.roleUILooksNode:SetData(show_data)
	self.name_txt.text = mainRoleLooksInfo.name
end

function BagView:InitEquips(  )
	
end

function BagView:UpdateEquips(  )
	
end

return BagView