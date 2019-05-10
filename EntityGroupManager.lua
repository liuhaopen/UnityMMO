local EntityGroupData = ECS.BaseClass()
ECS.EntityGroupData = EntityGroupData

local MatchingArchetypes = ECS.BaseClass()
ECS.MatchingArchetypes = MatchingArchetypes

local EntityGroupManager = ECS.BaseClass()
ECS.EntityGroupManager = EntityGroupManager

function EntityGroupManager:Constructor( safetyManager )
    self.m_JobSafetyManager = safetyManager
end

function EntityGroupManager:CreateEntityGroup( typeMan, entityDataManager, archetypeQueries, archetypeFiltersCount, requiredComponents, requiredComponentsCount )
	local grp = {}
    grp.PrevGroup = self.m_LastGroupData
    self.m_LastGroupData = grp
    grp.RequiredComponentsCount = requiredComponentsCount
    grp.RequiredComponents = requiredComponents
    -- self:InitializeReaderWriter(grp, requiredComponents, requiredComponentsCount)
    
    grp.ArchetypeQuery = grp.ArchetypeQuery or {}
    table.insert(grp.ArchetypeQuery, archetypeQueries)
    -- grp.ArchetypeQuery = archetypeQueries
    grp.ArchetypeQueryCount = archetypeFiltersCount
    grp.FirstMatchingArchetype = nil
    grp.LastMatchingArchetype = nil
    local type = typeMan.m_LastArchetype
    while type ~= nil do
        self:AddArchetypeIfMatchingWithGroup(type, grp)
        type = type.PrevArchetype
    end
    return ECS.ComponentGroup.New(grp, self.m_JobSafetyManager, typeMan, entityDataManager)
end

function EntityGroupManager:CreateEntityGroupByNames( typeMan, entityDataManager, requiredComponents )
	local requiredComponentPtr, requiredComponentCount = self:CreateRequiredComponents(requiredComponents, #requiredComponents)
    return self:CreateEntityGroup(typeMan, entityDataManager, self:CreateQuery(requiredComponents), 1, requiredComponentPtr, requiredComponentCount)
end

function EntityGroupManager:CreateRequiredComponents( requiredComponents, count )
	local types = {}
    types[1] = ECS.ComponentType.Create(ECS.Entity.Name)
    for i=1,count do
        ECS.SortingUtilities.InsertSorted(types, i + 1, ECS.ComponentType.Create(requiredComponents[i]))
    end
    return types, count + 1
end

function EntityGroupManager:CreateQuery( comp_names )
	local requiredTypes = {}
    for i=1,#comp_names do
        ECS.SortingUtilities.InsertSorted(requiredTypes, i, ECS.ComponentType.Create(comp_names[i]))
    end

	local filter = {}

    local noneCount = 0
    local allCount = 0
    for i=1,#requiredTypes do
        if (requiredTypes[i].AccessModeType == ECS.ComponentType.AccessMode.Subtractive) then
            noneCount = noneCount + 1
        else
            allCount = allCount + 1
        end
    end
    filter.All = {}
    filter.AllCount = allCount

    filter.None = {}
    filter.NoneCount = noneCount

    filter.Any = nil
    filter.AnyCount = 0

    noneCount = 0
    allCount = 0
    for i=1,#requiredTypes do
        if (requiredTypes[i].AccessModeType == ECS.ComponentType.AccessMode.Subtractive) then
            noneCount = noneCount + 1
            filter.None[noneCount] = requiredTypes[i].TypeIndex
        else
            allCount = allCount + 1
            filter.All[allCount] = requiredTypes[i].TypeIndex
        end
    end
    return filter
end

function EntityGroupManager:AddArchetypeIfMatching( type )
	local grp = self.m_LastGroupData
	while grp ~= nil do 
		self:AddArchetypeIfMatchingWithGroup(type, grp)
		grp = grp.PrevGroup
	end
end

function EntityGroupManager:AddArchetypeIfMatchingWithGroup( archetype, group )
	if not self:IsMatchingArchetypeByGroupData(archetype, group) then
        return
    end
    
    local match = {}
    match.Archetype = archetype
    match.IndexInArchetype = {}
    -- local typeIndexInArchetypeArray = match.IndexInArchetype

    if (group.LastMatchingArchetype == nil) then
        group.LastMatchingArchetype = match
    end

    match.Next = group.FirstMatchingArchetype
    group.FirstMatchingArchetype = match

    for component=1,group.RequiredComponentsCount do
        local typeComponentIndex = -1
        if (group.RequiredComponents[component].AccessModeType ~= ECS.ComponentType.AccessMode.Subtractive) then
            typeComponentIndex = ECS.ChunkDataUtility.GetIndexInTypeArray(archetype, group.RequiredComponents[component].TypeIndex)
            assert(-1~=typeComponentIndex, "it must not be -1")
        end
        match.IndexInArchetype[component] = typeComponentIndex
    end
end

function EntityGroupManager:IsMatchingArchetypeByGroupData( archetype, group )
	for i=1,group.ArchetypeQueryCount do
        if self:IsMatchingArchetypeByQuery(archetype, group.ArchetypeQuery[i]) then
            return true
        end
	end
    return false
end

function EntityGroupManager:IsMatchingArchetypeByQuery( archetype, query )
	if not self:TestMatchingArchetypeAll(archetype, query.All, query.AllCount) then
        return false
    end
    if not self:TestMatchingArchetypeNone(archetype, query.None, query.NoneCount) then
        return false
    end
    if not self:TestMatchingArchetypeAny(archetype, query.Any, query.AnyCount) then
        return false
    end
    return true
end

function EntityGroupManager:TestMatchingArchetypeAll( archetype, allTypes, allCount )
	local componentTypes = archetype.Types
    local componentTypesCount = archetype.TypesCount
    local foundCount = 0
    -- local disabledTypeIndex = TypeManager.GetTypeIndex<Disabled>()
    -- local prefabTypeIndex = TypeManager.GetTypeIndex<Prefab>()
    -- local requestedDisabled = false
    -- local requestedPrefab = false
    for i=1,componentTypesCount do
        local componentTypeIndex = componentTypes[i].TypeIndex
        for j=1,allCount do
            local allTypeIndex = allTypes[j]
            -- if (allTypeIndex == disabledTypeIndex) then
            --     requestedDisabled = true
        	-- end
            -- if (allTypeIndex == prefabTypeIndex) then
            --     requestedPrefab = true
        	-- end
            if (componentTypeIndex == allTypeIndex) then
            	foundCount = foundCount + 1
            end
        end
    end
    -- if (archetype.Disabled and (not requestedDisabled)) then
    --     return false
    -- end
    -- if (archetype.Prefab and (not requestedPrefab)) then
    --     return false
    -- end
    return foundCount == allCount
end
 
function EntityGroupManager:TestMatchingArchetypeNone( archetype, noneTypes, noneCount )
	local componentTypes = archetype.Types
    local componentTypesCount = archetype.TypesCount
    for i=1,componentTypesCount do
        local componentTypeIndex = componentTypes[i].TypeIndex
        for j=1,noneCount do
            local noneTypeIndex = noneTypes[j]
            if componentTypeIndex == noneTypeIndex then
            	return false
            end
        end
    end
    return true
end

function EntityGroupManager:TestMatchingArchetypeAny( archetype, anyTypes, anyCount )
	if (anyCount == 0) then
		return true
	end

    local componentTypes = archetype.Types
    local componentTypesCount = archetype.TypesCount
    for i=1,componentTypesCount do
        local componentTypeIndex = componentTypes[i].TypeIndex
        for j=1,anyCount do
            local anyTypeIndex = anyTypes[j]
            if (componentTypeIndex == anyTypeIndex) then
                return true
            end
        end
    end
    return false
end

function EntityGroupManager.CompareComponents( componentTypes, groupData )
	if groupData.RequiredComponents == nil then
        return false
    end
    -- ComponentGroups are constructed including the Entity ID
    if #componentTypes + 1 ~= groupData.RequiredComponentsCount then
        return false
    end
    for i=1,#componentTypes do
        if groupData.RequiredComponents[i + 1] ~= ECS.ComponentType.Create(componentTypes[i]) then
            return false
        end
    end
    return true
end

return EntityGroupManager