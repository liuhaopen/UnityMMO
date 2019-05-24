require("Game/Task/TaskConst")
require("Game/Task/TaskModel")

local TaskController = {}

function TaskController:Init(  )
	self.model = TaskModel:GetInstance()
	-- TaskConst.Events.ReqTaskList

	local onGameStart = function (  )
		self.model:Reset()
		self:ReqTaskList()
        
	end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, onGameStart)
    self:AddEvents()
end

function TaskController:AddEvents(  )
	self.model:Bind(TaskConst.Events.ReqTaskList, function()
		self:ReqTaskList()
	end)
	
end

function TaskController:ReqTaskList(  )
	local on_ack = function ( ack_data )
		self.model:SetTaskList(ack_data)
		self.model:Fire(TaskConst.Events.AckTaskList)
	end
    NetDispatcher:SendMessage("Task_GetInfoList", nil, on_ack)
end

return TaskController
