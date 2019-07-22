local MainUIMenuView = BaseClass(UINode)

function MainUIMenuView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUIMenuView.prefab",
		canvasName = "MainUI",
	}
end

function MainUIMenuView:OnLoad(  )
	local names = {
		"bag:obj",
	}
	UI.GetChildren(self, self.transform, names)
	self:AddEvents()
	self:OnUpdate()
end

function MainUIMenuView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.bag_obj == click_obj then
			local view = require("Game.Bag.BagMainView").New()
    		view:Load()
		-- elseif self.main_city_obj == click_obj then
			-- SceneMgr.Instance:ReqEnterScene(1001, 0)
		end
	end
	-- UI.BindClickEvent(self.main_city_obj, on_click)
	UI.BindClickEvent(self.bag_obj, on_click)
	
end

function MainUIMenuView:OnUpdate(  )
end

return MainUIMenuView