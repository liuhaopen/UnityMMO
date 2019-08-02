local GoodsItem = BaseClass(UINode)

function GoodsItem:Constructor(  )
	self.viewCfg = {
		prefabPoolName = "GoodsItem",
	}
end

function GoodsItem.Create()
	return LuaPool:Get("GoodsItem")
end

function GoodsItem:Reset(  )
	self.clickFunc = nil
	self.num_txt.text = ""
	if self.iconVisible ~= nil then
		self.icon_obj:SetActive(true)
		self.iconVisible = nil
	end
end

function GoodsItem:Recycle(  )
	self:Reset()
	self:Hide()
end

function GoodsItem:OnLoad(  )
	local names = {
		"icon:img:obj","num:txt","bg:img","click:obj",
	}
	UI.GetChildren(self, self.transform, names)
	self:AddEvents()
end

function GoodsItem:AddEvents(  )
	local on_click = function ( click_obj )
		if self.click_obj == click_obj then
			if self.clickFunc then
				self.clickFunc(self.clickArge)
			end
		end
	end
	UI.BindClickEvent(self.click_obj, on_click)
	
end

function GoodsItem:SetIcon( goodsTypeID, num )
	local cfg = ConfigMgr:GetGoodsCfg(goodsTypeID)
	UI.SetImage(self, self.bg_img, "Assets/AssetBundleRes/ui/common/com_icon_bg_"..cfg.color..".png", false)
	UI.SetImage(self, self.icon_img, "Assets/AssetBundleRes/ui/goods/"..cfg.icon..".png", false)
	self:SetNum(num)
end

function GoodsItem:SetBg( res )
	UI.SetImage(self, self.bg_img, res)
end

function GoodsItem:SetIconVisible( isShow )
	self.iconVisible = isShow
	self.icon_obj:SetActive(isShow)
end

function GoodsItem:SetNum( num )
	self.num_txt.text = num
end

function GoodsItem:SetClickFunc( clickFunc, arge )
	self.clickFunc = clickFunc
	self.clickArge = arge
end

local scaleMap = {
	["Big"] = 1,
	["Medium"] = 0.7,
	["Small"] = 0.5,
}
function GoodsItem:SetSizeType( sizeType )
	local scale = sizeType and scaleMap[sizeType]
	if not scale then return end
	self.transform.localScale = Vector2(scale, scale)
end

return GoodsItem