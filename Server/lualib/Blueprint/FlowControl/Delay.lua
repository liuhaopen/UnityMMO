local Delay = BP.BaseClass(BP.Node)

function Delay:Constructor( graph )
	self.is_updatable_bp_node = true
	self.graph = graph
	print('Cat:Delay.lua[5] graph', graph)
end

function Delay:Update( deltaTime )
	print('Cat:Delay.lua[8] update')
end

return Delay