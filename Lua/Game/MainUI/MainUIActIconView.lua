
local MainUIActIconView = BaseClass(UINode)

function MainUIActIconView:Constructor( )
	self.prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUIActIconView.prefab"
	self.canvasName = "MainUI"
end

function MainUIActIconView:OnLoad(  )
	local names = {
		"dungeon:obj",
	}
	UI.GetChildren(self, self.transform, names)
	self:AddEvents()
	self:OnUpdate()
end

function MainUIActIconView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.dungeon_obj == click_obj then
			SceneMgr.Instance.ReqEnterScene(2001, 0)
		end
	end
	AddClickEvent(self.dungeon_obj, on_click)
	
end

function MainUIActIconView:OnUpdate(  )
end

return MainUIActIconView