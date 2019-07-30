local GoodsInfoView = BaseClass(UINode)

function GoodsInfoView:Constructor( )
	self.viewCfg = {
		prefabPath = ResPath.GetFullUIPath("common/GoodsInfoView.prefab"),
		canvasName = "Normal",
		components = {
			{UI.Background, {is_click_to_close=true, alpha=0.5}},
		},
	}
	self.model = BagModel:GetInstance()
end

function GoodsInfoView.Create()
	return LuaPool:Get("GoodsInfoView")
end

function GoodsInfoView:OnLoad(  )
	local names = {
		"layout/info_con/desc_scroll/Viewport/desc_con/overdue:txt","layout/info_con/desc_scroll/Viewport/desc_con/desc:txt","layout/info_con/head_con/name:txt","layout/info_con/head_con/icon_con","layout/info_con/head_con/num:txt","layout/info_con/head_con/level:txt","layout","layout/info_con/desc_scroll/Viewport/desc_con/use_desc:txt","layout/get_way_con:obj","layout/info_con","layout/info_con/head_con","layout/info_con/desc_scroll/Viewport/desc_con/title_use:obj",
		
	}
	UI.GetChildren(self, self.transform, names)

	local btnsName = {
		"layout/btns_con/drop_btn:obj","layout/btns_con/use_btn:obj","layout/btns_con/sell_btn:obj","layout/btns_con/buy_btn:obj","layout/btns_con/store_btn:obj",
	}
	self.btns = {}
	UI.GetChildren(self.btns, self.transform, btnsName)

	self.overdue_txt.text = ""
	self.iconNode = GoodsItem.Create()
	self.iconNode:Load()
	self.iconNode:SetParent(self.icon_con)
	self.iconNode:SetSizeType("Medium")

	self:AddEvents()
end

function GoodsInfoView:AddEvents( )
	local on_click = function ( click_obj )
		print('Cat:GoodsInfoView.lua[37] click_obj', click_obj, self.btns.drop_btn_obj)
		if self.btns.drop_btn_obj == click_obj then
			if not self.goodsInfo or not self.goodsInfo.uid then
				Message:Show("道具信息有误")
				return
			end
			local on_ack = function ( ackData )
		        print("Cat:GoodsInfoView [start:29] ackData: ", ackData)
		        PrintTable(ackData)
		        print("Cat:GoodsInfoView [end]")
		        if ackData.result == ErrorCode.Succeed then
		        	Message:Show("销毁成功")
		        	self:Unload()
		        end
		    end
		    NetDispatcher:SendMessage("Bag_DropGoods", {uid=self.goodsInfo.uid}, on_ack)
		elseif self.btns.store_btn_obj == click_obj then
		elseif self.btns.buy_btn_obj == click_obj then
		elseif self.btns.sell_btn_obj == click_obj then
		elseif self.btns.use_btn_obj == click_obj then
			
		end
	end
	UI.BindClickEvent(self.btns.use_btn_obj, on_click)
	UI.BindClickEvent(self.btns.sell_btn_obj, on_click)
	UI.BindClickEvent(self.btns.buy_btn_obj, on_click)
	UI.BindClickEvent(self.btns.store_btn_obj, on_click)
	UI.BindClickEvent(self.btns.drop_btn_obj, on_click)
	
end

--[[showData可配置字段：
comeFrom:一个字符串，指定来自哪里点开的
isShowGetWay:是否显示获取途径
btnList:显示的按钮列表
--]]
function GoodsInfoView:SetData( goodsInfo, showData )
	self.goodsInfo = goodsInfo
	self.showData = showData
	if self.isLoaded then
		self:OnUpdate()
	else
		self.isNeedUpdateOnLoad = true
	end
end

function GoodsInfoView:UpdateBtns()
	for k,v in pairs(self.btns) do
		v.gameObject:SetActive(false)
	end
	local showBtnList = {}
	if self.showData and self.showData.comeFrom then
		local comeFrom = self.showData.comeFrom
		if comeFrom == "BagView" then
			-- self.showData.btnList = self.showData.btnList or {}
			table.insert(showBtnList, "drop_btn")
		elseif comeFrom == "WarehouseView" then
			table.insert(showBtnList, "store_btn")
		end
	end
	if showBtnList then
		for i,v in ipairs(showBtnList) do
			self.btns[v.."_obj"]:SetActive(true)
		end
	end
end

function GoodsInfoView:OnUpdate(  )
	if not self.isLoaded then return end
	
	self:UpdateInfo()
	self:UpdateBtns()
	self:UpdateGetWay()
end

function GoodsInfoView:UpdateGetWay(  )
	local isShow = self.showData and self.showData.isShowGetWay
	self.get_way_con_obj:SetActive(isShow)
	if isShow then
		--Cat_Todo : add get way view
	end
end

function GoodsInfoView:UpdateInfo(  )
	if not self.goodsInfo.cfg then
		self.goodsInfo.cfg = ConfigMgr:GetGoodsCfg(self.goodsInfo.typeID)
	end

	self.name_txt.text = self.model:GetGoodsName(self.goodsInfo.typeID, true)
	self.num_txt.text = string.format("<color=#5C3536>数量: %s</color>", self.goodsInfo.num)
	self.level_txt.text = string.format("<color=#5C3536>等级: %s</color>", self.goodsInfo.cfg.level)
	self.iconNode:SetIcon(self.goodsInfo.typeID, self.goodsInfo.num)

	local intro_str = Trim(self.goodsInfo.cfg and self.goodsInfo.cfg.intro or "")
	self.desc_txt.text = intro_str
	
	local use_intro_str = Trim(self.goodsInfo.cfg and self.goodsInfo.cfg.use_intro or "")
	if use_intro_str ~= "" then
		self.use_desc_txt.text = use_intro_str
		self.title_use_obj:SetActive(true)
		UI.SetSizeDeltaY(self.use_desc, self.use_desc_txt.preferredHeight+20)
	else
		self.use_desc_txt.text = ""
		self.title_use_obj:SetActive(false)
		UI.SetSizeDeltaY(self.use_desc, 0)
	end
end

function GoodsInfoView:Recycle(  )
end

function GoodsInfoView:Unload(  )
	print('Cat:GoodsInfoView.lua[Unload]')
	self:Hide()
	LuaPool:Recycle("GoodsInfoView", self)
end

return GoodsInfoView