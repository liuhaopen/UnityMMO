local FSMState = BP.BaseClass(BP.Node)

function FSMState:Constructor( )
	self.elapsedTime = 0
	self.hasInit = false
end

function FSMState:OnExecute( owner )
	if not self.hasInit then
		self.hasInit = true
		self.fsm = self.graph
		self.owner = owner
		self.blackboard = owner:GetBlackboard()
		self:OnInit()
	end

	if self.status == BP.Status.Resting or status == BP.Status.Running then
		self.status = BP.Status.Running
		self:OnEnter()
	end
    return self.status
end

function FSMState:OnGraphPaused(  )
	if self.status == BP.Status.Running then 
		self:OnPause()
	end
end

function FSMState:Update(  )
	-- self.elapsedTime += Time.deltaTime;
    if self.status == BP.Status.Running then
        self:OnUpdate()
    end
end

function FSMState:OnReset(  )
	self.status = BP.Status.Resting
	self.elapsedTime = 0
	self:OnExit()
end

function FSMState:OnInit(  )
	--override me
end

function FSMState:OnEnter(  )
	--override me
end

function FSMState:OnUpdate( deltaTime )
	--override me
end

function FSMState:OnExit(  )
	--override me
end

function FSMState:OnPause(  )
	--override me
end

function FSMState:Finish( inSuccess )
	if inSuccess == nil then
		inSuccess = true
	end
	self.status = inSuccess and BP.Status.Success or BP.Status.Failure
end

return FSMState