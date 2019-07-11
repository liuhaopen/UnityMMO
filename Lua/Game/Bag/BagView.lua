local BagView = BaseClass(UINode)

function BagView:Constructor( )
	self.prefabPath = "Assets/AssetBundleRes/ui/bag/BagView.prefab"
	self.canvasName = "Normal"
end

function BagView:OnLoad(  )
	local names = {
		
	}
	UI.GetChildren(self, self.transform, names)
	self:AddEvents()
	self:OnUpdate()
end

function BagView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.bag_obj == click_obj then
			local view = require("Game/Bag/BagView").New()
    		view:Load()
		-- elseif self.main_city_obj == click_obj then
			-- SceneMgr.Instance:ReqEnterScene(1001, 0)
		end
	end
	-- UI.BindClickEvent(self.main_city_obj, on_click)
	UI.BindClickEvent(self.bag_obj, on_click)
	
end

function BagView:OnUpdate(  )
end

return BagView