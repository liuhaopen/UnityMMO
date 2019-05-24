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

function TaskModel:GetTaskList( )
	return self.taskList
end

function TaskModel:SetTaskList( value )
	self.taskList = value
end

return TaskModel