local GoodsItem = BaseClass(UINode)

function GoodsItem:Constructor(  )
	self.viewCfg = {
		prefabPoolName = "GoodsItem",
	}
	self:Load()
end

function GoodsItem.Create( )
	return LuaPool:Get("Pool-GoodsItem")
end

function GoodsItem:Reset(  )
	PrefabPool:Recycle(self.viewCfg.prefabPoolObj)
end

function GoodsItem:OnLoad(  )
	local names = {
		"icon:img","num:txt","bg:img",
	}
	UI.GetChildren(self, self.transform, names)
end

function GoodsItem:SetIcon( goodsTypeID, num )
	local cfg = ConfigMgr:GetGoodsCfg(goodsTypeID)
	UI.SetImage(self.bg_img, "Assets/AssetBundleRes/ui/common/com_icon_bg_"..cfg.color..".png", false)
	UI.SetImage(self.icon_img, "Assets/AssetBundleRes/ui/goods/"..cfg.icon..".png", false)
	self.num_txt.text = num
end

function GoodsItem:SetBg( res )
	UIHelper.SetImage(self.bg_img, res)
end


return GoodsItem