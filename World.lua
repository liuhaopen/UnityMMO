local World = BaseClass()
ECS.World = World
ECS.World.Active = nil
ECS.World.allWorlds = {}
function World:Constructor( name )
	self.name = name
	self.behaviour_mgrs = {}
	self.behaviour_mgrs_lookup = {}
	setmetatable(self.behaviour_mgrs_lookup, {__mode = "kv"})   

	self.IsCreated = true
	table.insert(ECS.World.allWorlds, self)
end

function World:GetBehaviourManagers(  )
	return self.behaviour_mgrs
end

function World:GetOrCreateManager( script_behaviour_mgr_type )
	local mgr = self:GetExistingManager(script_behaviour_mgr_type)
	if not mgr then
		mgr = self:CreateManager(script_behaviour_mgr_type)
	end
	return mgr
end

function World:CreateManager( script_behaviour_mgr_type )
	assert(script_behaviour_mgr_type, "nil mgr type : "..(script_behaviour_mgr_type or "nilstr"))
	local mgr_class = require(script_behaviour_mgr_type)
	assert(mgr_class, script_behaviour_mgr_type.." file had not return a class")
	local mgr = mgr_class.New()
	mgr:CreateInstance()
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