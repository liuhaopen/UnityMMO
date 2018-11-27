local ComponentSystemBase = BaseClass(ECS.ScriptBehaviourManager)
ECS.ComponentSystemBase = ComponentSystemBase

function ComponentSystemBase:Constructor(  )
	self.m_InjectedComponentGroups = {}
	self.m_InjectFromEntityData = {}
	self.m_ComponentGroups = {}
	self.m_LastSystemVersion = nil
	self.m_EntityManager = nil
	self.m_World = nil
	self.m_AlwaysUpdateSystem = false
	self.m_PreviouslyEnabled = false
	self.Enabled = true
end

function ComponentSystemBase:ShouldRunSystem(  )
	if not self.m_World.IsCreated then
        return false
    end

    if self.m_AlwaysUpdateSystem then
        return true
    end
    -- var length = m_ComponentGroups?.Length ?? 0;
    -- if (length == 0)
    --     return true;

    -- for (int i = 0;i != length;i++)
    -- {
    --     if (!m_ComponentGroups[i].IsEmptyIgnoreFilter)
    --         return true;
    -- }

    return false;
end

function ComponentSystemBase:OnBeforeCreateManagerInternal( world, capacity )
	self.m_World = world
    self.m_EntityManager = world:GetOrCreateManager(ECS.EntityManager)
    self.m_AlwaysUpdateSystem = self.AlwaysUpdateSystem

    self.m_ComponentGroups = ECS.ComponentGroup.New()
    -- self.m_CachedComponentGroupArrays = new ComponentGroupArrayStaticCache[0]

    ComponentSystemInjection.Inject(self, world, self.m_EntityManager, self.m_InjectedComponentGroups, self.m_InjectFromEntityData)

    self:UpdateInjectedComponentGroups()
end

function ComponentSystemBase:BeforeUpdateVersioning(  )
end

function ComponentSystemBase:AfterUpdateVersioning(  )
	
end

function ComponentSystemBase:GetArchetypeChunkComponentType( com_type_name, isReadOnly )
	
end

function ComponentSystemBase:GetComponentGroupInternal( componentTypes )
	for i,v in ipairs(self.m_ComponentGroups) do
		if v:CompareComponents(componentTypes) then
			return v
		end
	end
	local group = self.m_EntityManager:CreateComponentGroup(componentTypes)
    group:SetFilterChangedRequiredVersion(self.m_LastSystemVersion)
    table.insert(self.m_ComponentGroups, group)
    -- for (int i = 0;i != count;i++)
    --     AddReaderWriter(componentTypes[i])
    return group
end

function ComponentSystemBase:UpdateInjectedComponentGroups(  )
	if not self.m_InjectedComponentGroups then return end
	
	local pinnedSystemPtr = 0
	for i,group in ipairs(self.m_InjectedComponentGroups) do
        group.UpdateInjection(pinnedSystemPtr)
	end
	self.m_InjectFromEntityData:UpdateInjection(pinnedSystemPtr, self.m_EntityManager);
end                


function ComponentSystemBase:xxxxxxxx(  )
	
end

function ComponentSystemBase:xxxxxxxx(  )
	
end

local ComponentSystem = BaseClass(ECS.ComponentSystemBase)
ECS.ComponentSystem = ComponentSystem

function ComponentSystem:OnCreateManager( capacity )
	--提取需要组件的类型信息
end

function ComponentSystem:Update(  )
	
end

function ComponentSystem:Notify(  )
	
end

return ComponentSystem
