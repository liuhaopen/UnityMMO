local EntityGroupData = BaseClass()
ECS.EntityGroupData = EntityGroupData

local MatchingArchetypes = BaseClass()
ECS.MatchingArchetypes = MatchingArchetypes

local EntityGroupManager = BaseClass()
ECS.EntityGroupManager = EntityGroupManager

function EntityGroupManager:Constructor( safetyManager )
    self.m_JobSafetyManager = safetyManager
end

function EntityGroupManager:CreateEntityGroup( typeMgr, entityDataManager, requiredTypes, requiredCount )
	local requiredComponentPtr
    local requiredComponentCount
    self:CreateRequiredComponents(requiredComponents, requiredComponentPtr, requiredComponentCount)
    return self:CreateEntityGroup(typeMan, entityDataManager, self:CreateQuery(requiredComponents), 1, requiredComponentPtr, requiredComponentCount)
end

function EntityGroupManager:CreateEntityGroup( typeMan, entityDataManager, requiredTypes )
	local requiredComponentPtr
    local requiredComponentCount
    self:CreateRequiredComponents(requiredComponents, requiredComponentPtr, requiredComponentCount)
    return self:CreateEntityGroup(typeMan, entityDataManager, self:CreateQuery(requiredComponents), 1, requiredComponentPtr, requiredComponentCount)
end

function EntityGroupManager:AddArchetypeIfMatching( type )
	local grp = self.m_LastGroupData
	while (grp ~= nil) do
		self:AddArchetypeIfMatchingWithGroupData(type, grp)
		grp = grp.PrevGroup
	end
end

function EntityGroupManager:AddArchetypeIfMatchingWithGroupData( type, group )
	
end