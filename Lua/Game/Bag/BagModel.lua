BagModel = BaseClass(EventDispatcher)

function BagModel:Constructor(  )
	self:Reset()
end

function BagModel:GetInstance(  )
	if not BagModel.Instance then
		BagModel.Instance = BagModel.New()
	end
	return BagModel.Instance
end

function BagModel:Reset(  )
	self.bagInfo = {}
	self.fullGoodsList = {}--包含空格子的全道具列表，主要用于背包界面显示之用
end

function BagModel:GetFullGoodsList( pos )
	return self.fullGoodsList[pos]
end

function BagModel:GetBagInfo( pos )
	return self.bagInfo[pos]
end

function BagModel:SetBagInfo( bagInfo )
	if not bagInfo or not bagInfo.pos then return end
	
	self.bagInfo[bagInfo.pos] = bagInfo
	for i,v in ipairs(bagInfo.goodsList) do
		v.cfg = ConfigMgr:GetGoodsCfg(v.typeID)
	end
	local fullGoodsList = {}
	self.fullGoodsList[bagInfo.pos] = fullGoodsList
	local goodsIndex = 1
	for i=1,BagConst.MaxCell do
		local goods = bagInfo.goodsList[goodsIndex]
		if goods and goods.cell == i then
			goodsIndex = goodsIndex + 1
		else
			goods = false
		end
		fullGoodsList[i] = goods
	end
	self:Fire(BagConst.Event.BagChange, bagInfo.pos)
end

function BagModel:UpdateBagInfos( bagInfos )
	if not bagInfos or not bagInfos.goodsList then return end

	for i,v in ipairs(bagInfos.goodsList) do
		
	end
end

return BagModel
