local World = BaseClass()

function World:Constructor( name )
	self.name = name
	self.behaviour_mgrs = {}
end

function World:GetBehaviourManagers(  )
	return self.behaviour_mgrs
end

function World:GetOrCreateManager( script_behaviour_mgr )
	
end

function World:CreateManager(  )
	
end

function World:GetExistingManager(  )
	
end

function World:DestroyManager(  )
	
end

return World