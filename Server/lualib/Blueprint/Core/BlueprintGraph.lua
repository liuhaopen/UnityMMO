local BlueprintGraph = {
	name = "unkown",
	category = "BlueprintGraph",
	nodes = {},
	variable = {},
	updatable_nodes = {},
	is_inited = false,
}
print('Cat:BlueprintGraph.lua123123[9] BP', tostring(_Env), BP)
tostring = function()
end
function BlueprintGraph:Start()
	if is_inited then return end
	for k,v in pairs(self.nodes) do
		if v.is_updatable_bp_node then
			table.insert(self.updatable_nodes, v)
		end
		if v.OnStart then
			v.OnStart()
		end
	end
	is_inited = true
end

function BlueprintGraph:Update( deltaTime )
	for i,v in ipairs(self.updatable_nodes) do
		v:Update(deltaTime)
	end
end

function BlueprintGraph:Validate(  )
	
end

function BlueprintGraph:Stop()
	
end

return BlueprintGraph