TaskConst = require("Game/Task/TaskConst")
TaskModel = require("Game/Task/TaskModel")

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
		self.model:SetTaskInfos(ack_data)
		self.model:Fire(TaskConst.Events.AckTaskList)
        self:HandleAutoDoTask()
	end
    NetDispatcher:SendMessage("Task_GetInfoList", nil, on_ack)

    local on_progress_changed = function ( ack_data )
        print("Cat:TaskController [start:38] on_progress_changed ack_data:", ack_data)
        PrintTable(ack_data)
        print("Cat:TaskController [end]")
        self.model:UpdateTaskInfo(ack_data.taskInfo)
        if ack_data.status == TaskConst.Status.Finished then
            self.model:Fire(TaskConst.Events.ReqTaskList)
        else
            self.model:Fire(TaskConst.Events.AckTaskList)
            self:HandleAutoDoTask()
        end
    end
    NetDispatcher:Listen("Task_ProgressChanged", nil, on_progress_changed)
end

function TaskController:HandleAutoDoTask(  )
    if not self.curDoingTaskInfo then return end
    
    local lastDoingType = self.model:GetTaskType(self.curDoingTaskInfo.taskID)
    local taskInfo = self.model:GetTaskInfoByType(lastDoingType)
    if taskInfo and (taskInfo.status == TaskConst.Status.CanTake or taskInfo.status == TaskConst.Status.Doing or taskInfo.status == TaskConst.Status.Finished) then
        self:DoTask(taskInfo)
    end
end

function TaskController:DoTask( taskInfo )
    print("Cat:TaskController [start:39] taskInfo:", taskInfo)
    PrintTable(taskInfo)
    print("Cat:TaskController [end]")
	if not taskInfo or not taskInfo.subType then return end

    if not self.handleTaskFuncs then
        self.handleTaskFuncs = {
            [TaskConst.SubType.Talk] = TaskController.DoTalk,
            [TaskConst.SubType.KillMonster] = TaskController.DoKillMonster,
            [TaskConst.SubType.Collect] = TaskController.DoCollect,
        }
    end
    local func = self.handleTaskFuncs[taskInfo.subType]
    if func then
        self.curDoingTaskInfo = taskInfo
        func(self, taskInfo)
    else
        error("had not find handle func for subtype : "..taskInfo.subType)
    end
end

function TaskController:DoTalk( taskInfo )
    local npcID = taskInfo.npcID
	local onApproachingNpc = function (  )
        local onGetTaskListInNPC = function ( ackData )
            local view = require("Game/Task/TaskDialogView").New()
            view:SetData(ackData)
        end
        NetDispatcher:SendMessage("Task_GetInfoListInNPC", {npcID=npcID}, onGetTaskListInNPC)
    end
    local goe = RoleMgr.GetInstance():GetMainRole()
    local moveQuery = goe:GetComponent(typeof(CS.UnityMMO.MoveQuery))
    --Cat_Todo : read npc pos from config file
    local npcPosList = {
        [3000] = {x = 787.90, y = 166.19, z = 1073.90},
        [3001] = {x = 749.90, y = 163.00, z = 1182.40},
        [3002] = {x = 845.00, y = 169.00, z = 1221.00},
    }
    local findInfo = {
        destination = npcPosList[npcID],
        stoppingDistance = 1.2,
        onStop = onApproachingNpc,
    }
    --Cat_Todo : handle destination are in different scene
    moveQuery:StartFindWay(findInfo)
end

function TaskController:DoKillMonster( taskInfo )
    
end

function TaskController:DoCollect( taskInfo )
    
end

return TaskController
