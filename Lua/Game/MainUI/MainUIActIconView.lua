
local MainUIActIconView = BaseClass(UINode)

function MainUIActIconView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUIActIconView.prefab",
		canvasName = "MainUI",
	}
end

function MainUIActIconView:OnLoad(  )
	local names = {
		"dungeon:obj","main_city:obj","gm:obj",
	}
	UI.GetChildren(self, self.transform, names)
	self:AddEvents()
	self:OnUpdate()
end

function MainUIActIconView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.dungeon_obj == click_obj then
			SceneMgr.Instance:ReqEnterScene(2001, 0)
		elseif self.main_city_obj == click_obj then
			SceneMgr.Instance:ReqEnterScene(1001, 0)
		elseif self.gm_obj == click_obj then
			local gmView = require("Game.GM.GMView").New()
			gmView:Load()
		end
	end
	UI.BindClickEvent(self.gm_obj, on_click)
	UI.BindClickEvent(self.main_city_obj, on_click)
	UI.BindClickEvent(self.dungeon_obj, on_click)
	
end

function MainUIActIconView:OnUpdate(  )
end

return MainUIActIconView