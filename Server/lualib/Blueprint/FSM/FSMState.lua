local FSMState = BP.BaseClass(BP.Node)

function FSMState:Constructor( fsmGraph )
	self.fsm = fsmGraph
	self.elapsedTime = 0
	self.hasInit = false
end

function FSMState:GetFSM(  )
	return self.fsm
end

return FSMState