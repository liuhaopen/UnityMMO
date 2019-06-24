TaskModel = BaseClass(EventDispatcher)

function TaskModel:Constructor(  )
	self:Reset()
end

function TaskModel:GetInstance(  )
	if not TaskModel.Instance then
		TaskModel.Instance = TaskModel.New()
	end
	return TaskModel.Instance
end

function TaskModel:Reset(  )
	
end

function TaskModel:GetTaskInfo( )
	return self.taskInfo
end

function TaskModel:SetTaskInfo( value )
	self.taskInfo = value
end

return TaskModel