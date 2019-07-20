local BagMainView = BaseClass(UINode)

local TabInfo = {
	{id=BagConst.TabID.Bag, 	  name="背包"},
	{id=BagConst.TabID.Warehouse, name="仓库"},
}
function BagMainView:Constructor( )
	-- self.prefabPath = ResPath.GetFullUIPath("bag/BagMainView.prefab")
	-- self.canvasName = "Normal"
	self.isShowBackground = true

	self.window = UI.Window.Create()
	local winData = {
		style = "WindowStyle.Big", 
		tabInfo = TabInfo,
		bg = "bag_main_bg",
		onSwitchTab = function( tabID )
			self:OnSwitchTab(tabID)
		end,
		onClose = function()
			-- self:Destroy()
			self.window:Destroy()
		end,
	}
	self.window:Load(winData)
	-- self.window:SetParent(self.transform)
	self.window:SetCanvas("Normal")
	self:AutoDestroy(self.window)
end

function BagMainView:OnLoad(  )
	local names = {
		
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:OnUpdate()

	self:OnSwitchTab(BagConst.TabID.Bag)
end

function BagMainView:AddEvents(  )
end

function BagMainView:OnSwitchTab( tabID )
	if tabID == BagConst.TabID.Bag then
		if not self.bagView then
			self.bagView = require("Game.Bag.BagView").New(self.sub_con)
			self:AutoDestroy(self.bagView)
		end
		self:JustShowMe(self.bagView)
	elseif tabID == BagConst.TabID.Warehouse then
		if not self.warehouseView then
			self.warehouseView = require("Game.Bag.WarehouseView").New(self.sub_con)
			self:AutoDestroy(self.warehouseView)
		end
		self:JustShowMe(self.warehouseView)
	end
	self.curTabID = tabID
end

return BagMainView