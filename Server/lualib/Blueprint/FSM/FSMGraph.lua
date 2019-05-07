local FSMGraph = BP.BaseClass(BP.Graph)

function FSMGraph:Constructor( )
	self.currentState = nil
	self.previousState = nil
	self.hasInitialized = false
end

function FSMGraph.Create( luaData )
	local graph = FSMGraph.New()
	local isOk = graph:LoadFromLuaData(luaData)
	if isOk then
		return graph
	end
	return nil
end

function FSMGraph:OnGraphStarted(  )
	if not self.hasInitialized then
		self.hasInitialized = true
	end
	self:EnterState(self.previousState == nil and self.primeNode or self.previousState)
end

function FSMGraph:OnGraphUpdate( deltaTime )
	if self.currentState == nil then
        self:Stop(false)
        return
	end

	if self.currentState ~= nil then 
        self.currentState:Update(deltaTime)
	end
end

function FSMGraph:GetStateWithName( stateName )
	if not self.nodes then return end
	
	for k,v in pairs(self.nodes) do
		if v.name and v.name == stateName then
			return v
		end
	end
	return nil
end

function FSMGraph:TriggerState( stateName )
	local state = self:GetStateWithName(stateName)
    if state ~= nil then
        self:EnterState(state)
        return state
    end
    error("No State with name '"..stateName.."' found on FSM '"..name.."'", 2)
    return nil
end

function FSMGraph:EnterState( newState )
	if not self.isRunning then
        print("Tried to EnterState on an FSM that was not running")
        return false
    end

    if newState == nil then
        print("Tried to Enter nil State")
        return false
    end

    if self.currentState ~= nil then
        self.currentState:Finish()
        self.currentState:Reset()
    end

    self.previousState = self.currentState
    self.currentState = newState

    self.currentState:Execute(self.owner)
    return true
end

function FSMGraph:OnGraphStoped(  )
	if self.currentState ~= nil then
		self.currentState:Finish()
		self.currentState:Reset()
	end
	self.previousState = nil
	self.currentState = nil
end

function FSMGraph:OnGraphPaused(  )
	self.previousState = self.currentState
    self.currentState = nil
end

function FSMGraph:OnGraphUnpaused(  )
	self:EnterState(self.previousState == nil and self.primeNode or self.previousState)
end

function FSMGraph:GetCurrentState(  )
	return self.currentState
end

function FSMGraph:GetPreviousState(  )
	return self.previousState
end

return FSMGraph