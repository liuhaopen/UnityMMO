local Graph = BP.BaseClass()

function Graph:Constructor(  )
	self.name = "unkown"
	self.category = "Graph"
	self.nodes = {}--key为节点id，value为节点引用
	self.variable = {}
	self.updatable_nodes = {}
	self.is_inited = false
	self.requiresPrimeNode = true
	self.isRunning = false
	self.isPaused = false
end

function Graph.Create( luaData )
	local graph = Graph.New()
	local isOk = graph:LoadFromLuaData(luaData)
	if isOk then
		return graph
	end
	return nil
end

-- function Graph:SetOwner( owner )
-- 	self.owner = owner
-- 	self.blackboard = owner:GetBlackboard()
-- end

function Graph:StartGraph( owner )
	if self.isRunning then
        print("Graph is already Active.")
        return
    end
    local primeNode = self:GetPrimeNode()
    if primeNode == nil and self.requiresPrimeNode then
        print("You've tried to start graph without a 'Start' node.")
        return
    end
	self.owner = owner
	self.blackboard = owner:GetBlackboard()
	self.isRunning = true
	if not self.isPaused then
        -- self.timeStarted = Time.time
        self:OnGraphStarted()
    else
        self:OnGraphUnpaused()
    end

    for k,v in pairs(self.nodes) do
    	if not self.isPaused then
			v:OnGraphStarted()
		else
			v:OnGraphUnpaused()
		end
	end
    self.isPaused = false
end

function Graph:OnGraphStarted(  )
	--override me
end

function Graph:OnGraphUnpaused(  )
	--override me
end

function Graph:GetPrimeNode(  )
	if not self.primeNode then
		local maxNum = 99999999
		local minID = maxNum
		local hadFind = false
		for k,v in pairs(self.nodes or {}) do
			if k < minID then
				minID = k
				hadFind = true
			end
		end
		if minID < maxNum then
			self.primeNode = self.nodes[minID]
		else
			error("has no prime node!", 2)
		end
	end
	return self.primeNode
end

function Graph:UpdateGraph( deltaTime )
	if self.isRunning then
		self:OnGraphUpdate()
	end
	-- for i,v in ipairs(self.updatable_nodes) do
	-- 	v:OnGraphUpdate(deltaTime)
	-- end
end

function Graph:Pause(  )
	if not self.isRunning then
        return
    end

    self.isRunning = false
    self.isPaused = true

    for k,v in pairs(self.nodes) do
    	v:OnGraphPaused()
    end

    self:OnGraphPaused()
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
		local newNode = BP.Node.Create(self, v.type)
		if newNode then 
			newNode.name = v.name
			newNode.id = v.id
			self.nodes[v.id] = newNode
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

function Graph:OnGraphUpdate(  )
	--wait for override
end

function Graph:OnGraphPaused(  )
	--wait for override
end

function Graph:OnGraphStoped(  )
	--wait for override
end

function Graph:Stop(success)
	-- if success == nil then
	-- 	success = true
	-- end
	if not self.isRunning and not self.isPaused then
        return
    end
    self.isRunning = false
    self.isPaused = false

    for k,v in pairs(self.nodes) do
    	v:Reset(false)
    	v:OnGraphStoped()
    end

    self:OnGraphStoped()
end

return Graph