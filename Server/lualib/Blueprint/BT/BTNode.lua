local BTNodee = BP.BaseClass(BP.Node)

function BTNodee:Constructor( graph )
	self.is_updatable_bp_node = true
	self.graph = graph
	print('Cat:BTNodee.lua[5] graph', graph)
end

function BTNodee:Update( deltaTime )
	print('Cat:BTNodee.lua[8] update')
end

return BTNodee