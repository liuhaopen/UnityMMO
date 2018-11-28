local InjectComponentGroupData = BaseClass()
ECS.InjectComponentGroupData = InjectComponentGroupData

function InjectComponentGroupData:Constructor(  )
	self.m_ComponentDataInjections = {}
end

function InjectComponentGroupData:UpdateInjection(  )
	for i,v in ipairs(self.m_ComponentDataInjections) do
		print(i,v)
	end
end