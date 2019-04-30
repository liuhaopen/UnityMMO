local Node = BP.BaseClass()

BP.NodeState = {
	Success = 1,
	Faild = 2,
	Running = 3,
	None = 4,
}
function Node:Constructor(  )
	self.id = 0
	self.name = "node"
	self.typeName = "Node"
	self.graph = nil
	self.inSlots = {}
	self.outSlots = {}
	self.state = BP.NodeState.None
end

function Node:OnValidate( graph )
	--override me
end

function Node:SetInSlot( slotName, node )
	self.inSlots[slotName] = node
end

function Node:SetOutSlot( slotName, node )
	self.outSlots[slotName] = node
end

function Node:Run(  )
	self.state = self:OnRun()
	print('Cat:Node.lua[35] self.state', self.state)
	return self.state
end

function Node:OnRun(  )
	--override me
	return self.state
end

return Node