local FSMNode = BP.BaseClass(BP.Node)

function FSMNode:Constructor( graph )
	self.is_updatable_bp_node = true
	self.graph = graph
	print('Cat:FSMNode.lua[5] graph', graph)
end

function FSMNode:Update( deltaTime )
	print('Cat:FSMNode.lua[8] update')
end

return FSMNode