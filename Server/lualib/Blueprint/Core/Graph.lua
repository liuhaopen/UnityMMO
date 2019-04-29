local Graph = BP.BaseClass()

function Graph:DefaultVar(  )
	return {
		name = "unkown",
		category = "Graph",
		nodes = {},
		variable = {},
		updatable_nodes = {},
		is_inited = false,
	}
end

function Graph.Create( luaData )
	local graph = Graph.New()
	local isOk = graph:LoadFromLuaData(luaData)
	if isOk then
		return graph
	end
	return nil
end

function Graph:Start()
	if self.is_inited then return end
	for k,v in pairs(self.nodes) do
		if v.is_updatable_bp_node then
			table.insert(self.updatable_nodes, v)
		end
		if v.OnStart then
			v.OnStart()
		end
	end
	self.is_inited = true
end

function Graph:Update( deltaTime )
	for i,v in ipairs(self.updatable_nodes) do
		v:Update(deltaTime)
	end
end

function Graph:LoadFromLuaData( luaData )
	self.nodes = self:CreateNodes(luaData.nodes)
	self.wires = luaData.wires
	self:Validate()
	return true
end

function Graph:CreateNodes( nodesData )
	local ret = {}
	for i,v in ipairs(nodesData) do
		local classTbl = v.type and BP.TypeManager:GetType(v.type)
		if classTbl then 
			table.insert(ret, classTbl.New(self))
		else
			print("try to get an unexist type name : "..v.type)
		end
	end
	return ret
end

function Graph:CreateWires( wiresData )
	
end

function Graph:Validate(  )
	for k,v in pairs(self.nodes) do
		v.OnValidate(self)
	end

	self:OnGraphValidate()
end

function Graph:OnGraphValidate(  )
	--wait for override
end

function Graph:Stop()
	
end

return Graph