local UpdateEvent = BP.BaseClass(BP.Node)

function UpdateEvent:Constructor(  )
	self.is_updatable_bp_node = true
	self.typeName = "UpdateEvent"
end

function UpdateEvent:OnValidate(  )
	self.outNode = self.outSlots["out"]
	assert(self.outNode, "empty out node!")
end

function UpdateEvent:Update( deltaTime )
	self.outNode:Run()
end

return UpdateEvent