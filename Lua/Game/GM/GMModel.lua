local GMModel = BaseClass(EventDispatcher)
local GMModel = GMModel

local clientGMList = {
	{gmName="角色id", defaultGMStr="roleID"},
	{gmName="场景信息", defaultGMStr="sceneInfo"},
}

function GMModel:Constructor()
	GMModel.Instance = self
	self:Reset()
end

function GMModel:Reset()
end

function GMModel.GetInstance()
	if GMModel.Instance == nil then
		GMModel.Instance = GMModel.New()
	end
	return GMModel.Instance
end

function GMModel:GetGmList( )
	return self.gmList
end

function GMModel:SetGmList( value )
	self.gmList = value
	for i,v in ipairs(clientGMList) do
		table.insert(self.gmList, v)
	end
end


return GMModel