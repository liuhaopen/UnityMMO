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
	self.BagInfo = nil
end

return BagModel
