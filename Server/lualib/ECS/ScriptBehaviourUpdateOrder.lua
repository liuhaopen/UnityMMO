local ScriptBehaviourUpdateOrder = {}
ECS.ScriptBehaviourUpdateOrder = ScriptBehaviourUpdateOrder

local DependantBehavior = {
    Manager = nil,
    UpdateAfter = {},
    UpdateBefore = {},
    LongestSystemsUpdatingAfterChain = 0,
    LongestSystemsUpdatingBeforeChain = 0,
    MaxInsertPos = 0,
    MinInsertPos = 0,
    spawnsJobs = 0,
    UnvalidatedSystemsUpdatingBefore = 0,
    WaitsForJobs = false,
}

function ScriptBehaviourUpdateOrder.SortSystemList( system_map )
	local list = {}
	for k,v in pairs(system_map) do
		v.__name_for_sort__ = k
		table.insert(list, v)
	end
	-- list = ScriptBehaviourUpdateOrder.CreateSystemDependencyList(list)
	return list
end

function ScriptBehaviourUpdateOrder.CreateSystemDependencyList( activeManagers )
    local dependencyGraph = ScriptBehaviourUpdateOrder.BuildSystemGraph(activeManagers)

end

function ScriptBehaviourUpdateOrder.BuildSystemGraph( activeManagers )
	
end