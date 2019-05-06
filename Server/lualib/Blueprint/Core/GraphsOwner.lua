local GraphsOwner = BP.BaseClass()

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

function GraphsOwner:AddGraph(  )
	
end

function GraphsOwner:Start(  )
	
end

function GraphsOwner:Update( deltaTime )
	
end

function GraphsOwner:Pause(  )
	
end

function GraphsOwner:Stop(  )
	
end

return GraphsOwner