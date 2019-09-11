local BOD = BaseClass()

BOD.Type = {
	Veto = 1,--一票否决权，即只要有个 false 就结果为 false
	Half = 2,--过半数通过
}

function BOD:Constructor( default, type )
	self.type = type or BOD.Type.Veto
	self.bod = {}
	if default == nil then
		default = true
	end
	self.default = default
	self.value = default
end

function BOD:Update(  )
	local newValue = self.default
	if self.type == BOD.Type.Veto then
		for k,v in pairs(self.bod) do
			if not v then
				newValue = false
				break
			end
		end
	elseif self.type == BOD.Type.Half then
		--useless for now
	end
	self.value = newValue
end

function BOD:Change( bodName, isSet, value )
	-- print('Cat:BOD.lua[34] bodName, isSet, value', bodName, isSet, value)
	if isSet then
		self.bod[bodName] = value
	else
		self.bod[bodName] = nil
	end
	self:Update()
end

return BOD