local ComponentSystemBase = ECS.BaseClass(ECS.ScriptBehaviourManager)
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
	self.inject_info_list = {}
end

function ComponentSystemBase:ShouldRunSystem(  )
	if not self.m_World.IsCreated then
        return false
    end

    if self.m_AlwaysUpdateSystem then
        return true
    end
    local length = self.m_ComponentGroups and #self.m_ComponentGroups or 0
    if length == 0 then
        return true
    end

    for i=1,length do
        if not self.m_ComponentGroups[i].IsEmptyIgnoreFilter then
            return true
        end
    end

    return false
end

function ComponentSystemBase:Inject( inject_field_name, inject_info )
	table.insert(self.inject_info_list, {inject_field_name, inject_info})
end

function ComponentSystemBase:GetInjectInfoList(  )
	return self.inject_info_list
end

function ComponentSystemBase:OnBeforeCreateManagerInternal( world, capacity )
	self.m_World = world
    self.m_EntityManager = world:GetOrCreateManager(ECS.EntityManager.Name)
    self.EntityManager = self.m_EntityManager
    self.m_AlwaysUpdateSystem = self.AlwaysUpdateSystem

    self.m_ComponentGroups = ECS.ComponentGroup.New()

    ECS.ComponentSystemInjection.Inject(self, world, self.m_EntityManager, self.m_InjectedComponentGroups, self.m_InjectFromEntityData)

    self:UpdateInjectedComponentGroups()
end

function ComponentSystemBase:BeforeUpdateVersioning(  )
end

function ComponentSystemBase:AfterUpdateVersioning(  )
end

function ComponentSystemBase:OnStartRunning(  )
end

function ComponentSystemBase:GetArchetypeChunkComponentType( com_type_name, isReadOnly )
	self:AddReaderWriter(isReadOnly and ComponentType.ReadOnly(com_type_name) or ComponentType.Create(com_type_name))
    return EntityManager.GetArchetypeChunkComponentType(isReadOnly)
end

function ComponentSystemBase:GetComponentGroup( componentTypes )
	for i,v in ipairs(self.m_ComponentGroups) do
		if v:CompareComponents(componentTypes) then
			return v
		end
	end
	local group = self.m_EntityManager:CreateComponentGroup(componentTypes)
    -- group:SetFilterChangedRequiredVersion(self.m_LastSystemVersion)
    table.insert(self.m_ComponentGroups, group)
    -- for (int i = 0;i != count;i++)
    --     AddReaderWriter(componentTypes[i])
    return group
end

function ComponentSystemBase:UpdateInjectedComponentGroups(  )
	if not self.m_InjectedComponentGroups then return end
	
	local pinnedSystemPtr = 0
	for i,group in ipairs(self.m_InjectedComponentGroups) do
        group:UpdateInjection(pinnedSystemPtr)
	end
	-- self.m_InjectFromEntityData:UpdateInjection(pinnedSystemPtr, self.m_EntityManager);
end      

function ComponentSystemBase:CompleteDependencyInternal(  )
end

local ComponentSystem = ECS.BaseClass(ECS.ComponentSystemBase)
function ComponentSystem:OnCreate( )
	self.PostUpdateCommands = nil
end

function ComponentSystem:BeforeOnUpdate(  )
	self:BeforeUpdateVersioning()
	self:CompleteDependencyInternal()
	self:UpdateInjectedComponentGroups()
	-- self.PostUpdateCommands = ECS.EntityCommandBuffer.New()
end

function ComponentSystem:InternalUpdate(  )
	if self.Enabled and self:ShouldRunSystem() then
        if not self.m_PreviouslyEnabled then
            self.m_PreviouslyEnabled = true
            self:OnStartRunning()
        end
        self:BeforeOnUpdate()
        self:OnUpdate()
        self:AfterOnUpdate()
    elseif self.m_PreviouslyEnabled then
        self.m_PreviouslyEnabled = false
        self:OnStopRunning()
    end
end

function ComponentSystem:AfterOnUpdate(  )
	self:AfterUpdateVersioning()
end

return ComponentSystem
