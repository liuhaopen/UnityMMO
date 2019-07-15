local BagMainView = BaseClass(UINode)

local TabInfo = {
	{id=BagConst.TabID.Bag, 	  name="背包"},
	{id=BagConst.TabID.Warehouse, name="仓库"},
}
function BagMainView:Constructor( )
	self.prefabPath = ResPath.GetFullUIPath("bag/BagMainView.prefab")
	self.canvasName = "Normal"
end

function BagMainView:OnLoad(  )
	local names = {
		
	}
	UI.GetChildren(self, self.transform, names)

	self.window = Window.Create(Window.Style.Big)

	self:AddEvents()
	self:OnUpdate()
end

function BagMainView:AddEvents(  )
end

function BagMainView:OnSwitchTab( tabID )
	if tabID == BagConst.TabID.Bag then
		self.bagView = self.bagView or require("Game.Bag.BagView").New(self.sub_con)
		self:JustShowMe(self.bagView)
	elseif tabID == BagConst.TabID.Warehouse then
		self.warehouseView = self.warehouseView or require("Game.Bag.WarehouseView").New(self.sub_con)
		self:JustShowMe(self.warehouseView)
	end
	self.curTabID = tabID
end

return BagMainView