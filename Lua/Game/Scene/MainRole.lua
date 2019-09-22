local MainRole = BaseClass()

function MainRole:Constructor(  )
	
end

function MainRole:GetInstance(  )
	if not MainRole.Instance then
		MainRole.Instance = MainRole.New()
	end
	return MainRole.Instance
end

function MainRole:SetBaseInfo( baseInfo )
	self.baseInfo = baseInfo
end

function MainRole:GetBaseInfo()
	return self.baseInfo
end

function MainRole:GetCareer()
	print("Cat:MainRole [start:23] self.baseInfo:", self.baseInfo)
	PrintTable(self.baseInfo)
	print("Cat:MainRole [end]")
	return self.baseInfo and self.baseInfo.career or 1
end

function MainRole:UpdateLv( lv )
	if self.baseInfo and self.baseInfo.base_info then
		self.baseInfo.base_info.level = lv
	end
end

function MainRole:GetLv(  )
	if self.baseInfo and self.baseInfo.base_info then
		return self.baseInfo.base_info.level
	end
	return 1
end

return MainRole