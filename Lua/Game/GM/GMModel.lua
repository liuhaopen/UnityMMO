local GMModel = BaseClass(EventDispatcher)
local GMModel = GMModel

function GMModel:__init()
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
end


return GMModel