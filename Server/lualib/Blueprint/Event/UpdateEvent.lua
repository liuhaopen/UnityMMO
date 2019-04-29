local UpdateEvent = BP.BaseClass(BP.Node)

UpdateEvent.is_updatable_bp_node = true
function UpdateEvent:Constructor( graph )
	self.graph = graph
	print('Cat:UpdateEvent.lua[5] graph', graph)
end

function UpdateEvent:Update( deltaTime )
	print('Cat:UpdateEvent.lua[8] update')
end

return UpdateEvent