local Node = BP.BaseClass()

BP.NodeState = {
	Success = 1,
	Faild = 2,
	Running = 3,
	None = 4,
}
function Node:DefaultVar(  )
	return {
		id = 0,
		name = "node",
		typeName = "Node",
		graph = nil,
		inConnections = {},
		outConnections = {},
		state = BP.NodeState.None,
	}
end

function Node:OnValidate( graph )
	--override me
end

function Node:Run(  )
	self.state = self:OnRun()
	return self.state
end

function Node:OnRun(  )
	--override me
	return self.state
end

return Node