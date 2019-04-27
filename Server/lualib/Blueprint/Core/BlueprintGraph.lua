local BlueprintGraph = {
	name = "unkown",
	category = "BlueprintGraph",
	nodes = {},
	variable = {},
	updatable_nodes = {},
	is_inited = false,
}

function BlueprintGraph:Start()
	if is_inited then return end
	for k,v in pairs(self.nodes) do
		if v.is_updatable then
			table.insert(self.updatable_nodes, v)
		end
	end
	is_inited = true
end

function BlueprintGraph:Update( deltaTime )
	for i,v in ipairs(self.updatable_nodes) do
		v:Update(deltaTime)
	end
end

function BlueprintGraph:Stop()
	
end

return BlueprintGraph