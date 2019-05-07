local GraphsOwner = BP.BaseClass()
--负责管理黑板和持有一些graph集合，方便统一调配
function GraphsOwner:Constructor(  )
	self.graphs = {}
	self.blackboard = BP.Blackboard.New()
end

function GraphsOwner.Create(  )
	local owner = GraphsOwner.New()
	return owner
end

function GraphsOwner:GetBlackboard(  )
	return self.blackboard
end

function GraphsOwner:AddGraph( graph )
	table.insert(self.graphs, graph)
	-- graph:SetOwner(self)
end

function GraphsOwner:Start(  )
	for i,v in ipairs(self.graphs) do
		v:StartGraph(self)
	end
end

function GraphsOwner:Update( deltaTime )
	for i,v in ipairs(self.graphs) do
		v:UpdateGraph(deltaTime)
	end
end

function GraphsOwner:Pause(  )
	for i,v in ipairs(self.graphs) do
		v:Pause()
	end
end

function GraphsOwner:Stop(  )
	for i,v in ipairs(self.graphs) do
		v:Stop()
	end
end

return GraphsOwner