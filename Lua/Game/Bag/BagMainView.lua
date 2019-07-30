local BagMainView = BaseClass(UINode)

local TabInfo = {
	{id=BagConst.TabID.Bag, 	  name="背包"},
	{id=BagConst.TabID.Warehouse, name="仓库"},
}
function BagMainView:Constructor( )
	self.viewCfg = {
		name = "BagMainView",
		prefabPoolName = "EmptyContainer",
		canvasName = "Normal",
		isShowBackground = true,
	}	
end

function BagMainView:OnLoad(  )
	local names = {
		"container"
	}
	UI.GetChildren(self, self.transform, names)

	self.window = UI.Window.Create()
	local winData = {
		style = "WindowStyle.Big", 
		tabInfo = TabInfo,
		bg = "bag_main_bg",
		onSwitchTab = function( tabID )
			self:OnSwitchTab(tabID)
		end,
		onClose = function()
			self:Unload()
		end,
	}
	self.window:Load(winData)
	self.window:SetParent(self.container)
	self:AutoDestroy(self.window)
	self:AddEvents()
	self:OnUpdate()

	self:OnSwitchTab(BagConst.TabID.Bag)
end

function BagMainView:AddEvents(  )
end

function BagMainView:OnSwitchTab( tabID )
	if tabID == BagConst.TabID.Bag then
		print('Cat:BagMainView.lua[49] self.bagView', self.bagView)
		if not self.bagView then
			self.bagView = require("Game.Bag.BagView").New(self.container)
			self.bagView:Load()
			self:AutoDestroy(self.bagView)
		end
		self:JustShowMe(self.bagView)
		self.window:SetTitle("Assets/AssetBundleRes/ui/bag/bag_txt_title_1.png")
	elseif tabID == BagConst.TabID.Warehouse then
		if not self.warehouseView then
			self.warehouseView = require("Game.Bag.WarehouseView").New(self.container)
			self.warehouseView:Load()
			self:AutoDestroy(self.warehouseView)
		end
		self:JustShowMe(self.warehouseView)
		self.window:SetTitle("Assets/AssetBundleRes/ui/bag/bag_txt_title_2.png")
	end
	self.curTabID = tabID
end

return BagMainView