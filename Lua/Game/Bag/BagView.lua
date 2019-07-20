local BagView = BaseClass(UINode)

function BagView:Constructor( )
	self.prefabPath = "Assets/AssetBundleRes/ui/bag/BagView.prefab"
	self.canvasName = "Normal"
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
	
end

function BagView:OnUpdate(  )
	self:UpdateRoleLooks()
	self:UpdateGoodsItems()
	self:UpdateEquips()
end

function BagView:UpdateGoodsItems(  )
	
end

function BagView:UpdateRoleLooks(  )
	
end

function BagView:UpdateEquips(  )
	
end

return BagView