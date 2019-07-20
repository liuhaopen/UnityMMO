local GoodsItem = BaseClass(UINode)

function GoodsItem:Constructor( parentTrans )
	self.widget = PrefabPool:Get("GoodsItem")
	self.gameObject = self.widget.gameObject
	self.transform = self.widget.transform
	self.transform:SetParent(parentTrans)
	self.transform.localScale = Vector3.one
	self.transform.localRotation = Quaternion.identity
	self.transform.anchoredPosition = Vector2.zero
	local localPos = self.transform.localPosition
	localPos.z = 0
	self.transform.localPosition = localPos

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



return GoodsItem