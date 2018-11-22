ECS = ECS or {}
local World = BaseClass()
ECS.World = World

local AddTypeLookup
function World:Constructor( name )
	self.name = name
	self.behaviour_mgrs = {}
	self.behaviour_mgrs_lookup = {}
end

function World:GetBehaviourManagers(  )
	return self.behaviour_mgrs
end

function World:GetOrCreateManager( script_behaviour_mgr_type )
	local mgr = self:GetExistingManager(script_behaviour_mgr_type)
	if mgr then
		return mgr
	else
		return self:CreateManager(script_behaviour_mgr_type)
	end
end

function World:CreateManager( script_behaviour_mgr_type )
	local mgr = script_behaviour_mgr_type.New()
	table.insert(self.behaviour_mgrs, mgr)
	self.behaviour_mgrs_lookup[script_behaviour_mgr_type] = mgr
	return mgr
end

function World:GetExistingManager( script_behaviour_mgr_type )
	return self.behaviour_mgrs_lookup[script_behaviour_mgr_type]
end

function World:DestroyManager(  )
end

return World