local TaskConst = require("Game/Task/TaskConst")
require("Game/Task/TaskModel")

TaskController = {}

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

function TaskController:GetInstance()
	return TaskController
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

function TaskController:DoTask( taskInfo )
	if not taskInfo then return end
	if taskInfo.type == TaskConst.SubType.Talk then
		self:DoConversation(taskInfo.npcID)
	elseif taskInfo.type == TaskConst.SubType.KillMonster then
	end
end

function TaskController:DoConversation( npcID )
	local onApproachingNpc = function (  )
        local onGetTaskListInNPC = function ( ackData )
            print("Cat:SceneController [start:56] ackData:", ackData)
            PrintTable(ackData)
            print("Cat:SceneController [end]")
            local hasTask = ackData.taskList and #ackData.taskList > 0
            if hasTask then
            else
                --show default conversation
            end
        end
        NetDispatcher:SendMessage("Task_GetInfoListInNPC", {npcUID=npcUID}, onGetTaskListInNPC)
    end
    local goe = RoleMgr.GetInstance():GetMainRole()
    local moveQuery = goe:GetComponent(typeof(CS.UnityMMO.MoveQuery))
    local findInfo = {
        destination = hit.point,
        stoppingDistance = 1,
        onStop = onApproachingNpc,
    }
    --Cat_Todo : handle destination are in different scene
    moveQuery:StartFindWay(findInfo)
end

return TaskController
