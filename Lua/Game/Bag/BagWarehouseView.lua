local BagWarehouseView = BaseClass(UINode)

function BagWarehouseView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/bag/BagWarehouseView.prefab",
		canvasName = "Normal",
	}
end

function BagWarehouseView:OnLoad(  )
	local names = {
		
	}
	UI.GetChildren(self, self.transform, names)
	self:AddEvents()
	self:OnUpdate()
end

function BagWarehouseView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.bag_obj == click_obj then
			local view = require("Game/Bag/BagWarehouseView").New()
    		view:Load()
		-- elseif self.main_city_obj == click_obj then
			-- SceneMgr.Instance:ReqEnterScene(1001, 0)
		end
	end
	-- UI.BindClickEvent(self.main_city_obj, on_click)
	UI.BindClickEvent(self.bag_obj, on_click)
	
end

function BagWarehouseView:OnUpdate(  )
end

return BagWarehouseView