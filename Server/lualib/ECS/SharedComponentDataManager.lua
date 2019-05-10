local SharedComponentDataManager = ECS.BaseClass()
ECS.SharedComponentDataManager = SharedComponentDataManager

function SharedComponentDataManager:Constructor(  )
	self.m_SharedComponentData = {}
	self.m_SharedComponentRefCount = {1}
	self.m_SharedComponentType = {-1}
	self.m_SharedComponentVersion = {1}
	self.m_FreeListIndex = -1
end

function SharedComponentDataManager:GetAllUniqueSharedComponents( com_type, sharedComponentValues )
	sharedComponentValues.Add(com_type)
	for var=1,self.m_SharedComponentData.Count do
        local data = self.m_SharedComponentData[i]
        if (data ~= nil and data:GetType() == com_type) then
            sharedComponentValues.Add(self.m_SharedComponentData[i])
        end
    end
end	

return SharedComponentDataManager