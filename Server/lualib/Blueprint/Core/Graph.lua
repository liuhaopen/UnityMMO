local Graph = BP.BaseClass(BP.Node)

function Graph:Constructor(  )
	self.name = "unkown"
	self.category = "Graph"
	self.nodes = {}--key为节点id，value为节点引用
	self.variable = {}
	self.updatable_nodes = {}
	self.is_inited = false
end

function Graph.Create( luaData )
	local graph = Graph.New()
	local isOk = graph:LoadFromLuaData(luaData)
	if isOk then
		return graph
	end
	return nil
end

function Graph:SetOwner( owner )
	self.owner = owner
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
	self:InitNodes(luaData.nodes)
	self:InitWires(luaData.wires)
	self:Validate()
	return true
end

function Graph:InitNodes( nodesData )
	self.nodes = {}
	for i,v in ipairs(nodesData) do
		local classTbl = v.type and BP.TypeManager:GetType(v.type)
		if classTbl then 
			-- table.insert(self.nodes, classTbl.New(self))
			self.nodes[v.id] = classTbl.New()
		else
			error("try to get an unexist type name : "..v.type, 2)
		end
	end
end

function Graph:InitWires( wiresData )
	if not wiresData then return end
	
	for i,v in ipairs(wiresData) do
		local sourceNode = self.nodes[v.sourceID]
		assert(sourceNode, "cannot find node by id:"..v.sourceID)
		local targetNode = self.nodes[v.targetID]
		assert(targetNode, "cannot find node by id:"..v.targetID)
		targetNode:SetInSlot(v.targetSlotName, inNode)
		sourceNode:SetOutSlot(v.sourceSlotName, targetNode)
	end
end

function Graph:Validate(  )
	for k,v in pairs(self.nodes) do
		v:OnValidate()
	end

	self:OnGraphValidate()
end

function Graph:OnGraphValidate(  )
	--wait for override
end

function Graph:Stop()
	for k,v in pairs(self.nodes) do
		if v.OnStop then
			v.OnStop()
		end
	end
end

return Graph