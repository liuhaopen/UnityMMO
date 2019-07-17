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
end

function BagModel:GetBagInfo( pos )
	return self.bagInfo[pos]
end

function BagModel:SetBagInfo( bagInfo )
	if not bagInfo or not bagInfo.pos then return end
	
	self.bagInfo[bagInfo.pos] = bagInfo
	self:Fire(BagConst.Event.BagChange, bagInfo.pos)
end

function BagModel:UpdateBagInfos( bagInfos )
	if not bagInfos or not bagInfos.goodsList then return end

	for i,v in ipairs(bagInfos.goodsList) do
		
	end
end

return BagModel
