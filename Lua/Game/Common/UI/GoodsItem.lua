local GoodsItem = BaseClass(UINode)

function GoodsItem:Constructor(  )
	self.viewCfg = {
		prefabPoolName = "GoodsItem",
	}
	self:Load()
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
	PrefabPool:Recycle(self.viewCfg.prefabPoolObj)
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
		print('Cat:GoodsItem.lua[38] click_obj', click_obj, self.click_obj)
		if self.click_obj == click_obj then
			print('Cat:GoodsItem.lua[40] self.clickFunc', self.clickFunc)
			if self.clickFunc then
				self.clickFunc(self.clickArge)
			end
		end
	end
	UI.BindClickEvent(self.click_obj, on_click)
	
end

function GoodsItem:SetIcon( goodsTypeID, num )
	local cfg = ConfigMgr:GetGoodsCfg(goodsTypeID)
	UI.SetImage(self.bg_img, "Assets/AssetBundleRes/ui/common/com_icon_bg_"..cfg.color..".png", false)
	UI.SetImage(self.icon_img, "Assets/AssetBundleRes/ui/goods/"..cfg.icon..".png", false)
	self:SetNum(num)
end

function GoodsItem:SetBg( res )
	UIHelper.SetImage(self.bg_img, res)
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


function GoodsItem:SetSize( x, y )
	
end


return GoodsItem