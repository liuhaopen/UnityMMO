local ScriptBehaviourManager = BaseClass()
ECS.ScriptBehaviourManager = ScriptBehaviourManager

function ScriptBehaviourManager:CreateInstance( world )
	if self.OnBeforeCreateManagerInternal then
		self:OnBeforeCreateManagerInternal(world)
	end

	if self.OnCreateManager then
		self:OnCreateManager()
	end
end

function ScriptBehaviourManager:Update(  )
	if self.InternalUpdate then
		self:InternalUpdate()
	end
end

function ScriptBehaviourManager:DestroyInstance(  )
	if self.OnBeforeDestroyManagerInternal then
		self:OnBeforeDestroyManagerInternal()
	end
	if self.OnDestroyManager then
		self:OnDestroyManager()
	end
	if self.OnAfterDestroyManagerInternal then
		self:OnAfterDestroyManagerInternal()
	end
end

