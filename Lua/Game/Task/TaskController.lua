TaskConst = require("Game/Task/TaskConst")
TaskModel = require("Game/Task/TaskModel")

TaskController = {}

function TaskController:Init(  )
	self.model = TaskModel:GetInstance()

    self:AddEvents()
end

function TaskController:GetInstance()
	return TaskController
end

function TaskController:AddEvents(  )
    local onGameStart = function (  )
        self.model:Reset()
        self:ReqTaskList()
        self:ListenTaskProgressChange()
    end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, onGameStart)

	self.model:Bind(TaskConst.Events.ReqTaskList, function()
		self:ReqTaskList()
	end)
end

function TaskController:ListenTaskProgressChange(  )
    local on_progress_changed = function ( ack_data )
        print("Cat:TaskController [start:38] on_progress_changed ack_data:", ack_data)
        PrintTable(ack_data)
        print("Cat:TaskController [end]")
        self.model:UpdateTaskInfo(ack_data.taskInfo)
        print('Cat:TaskController.lua[35] ack_data.status, ', ack_data.status, TaskConst.Status.Finished)
        if ack_data.taskInfo and ack_data.taskInfo.status == TaskConst.Status.Finished then
            self:ReqTaskList()
        else
            self.model:Fire(TaskConst.Events.AckTaskList)
            self:HandleAutoDoTask()
        end
    end
    NetDispatcher:Listen("Task_ProgressChanged", nil, on_progress_changed)
end

function TaskController:ReqTaskList(  )
	local on_ack = function ( ackData )
        print("Cat:TaskController [start:47] ackData: ", ackData)
        PrintTable(ackData)
        print("Cat:TaskController [end]")
		self.model:SetTaskInfos(ackData)
		self.model:Fire(TaskConst.Events.AckTaskList)
        self:HandleAutoDoTask()
	end
    NetDispatcher:SendMessage("Task_GetInfoList", nil, on_ack)
    print('Cat:TaskController.lua[55] ReqTaskList')
end

function TaskController:HandleAutoDoTask(  )
    if not self.curDoingTaskInfo then return end
    
    local lastDoingType = self.model:GetTaskType(self.curDoingTaskInfo.taskID)
    local taskInfo = self.model:GetTaskInfoByType(lastDoingType)
    if taskInfo and (taskInfo.subTaskIndex ~= self.curDoingTaskInfo.subTaskIndex) and (taskInfo.status == TaskConst.Status.CanTake or taskInfo.status == TaskConst.Status.Doing or taskInfo.status == TaskConst.Status.Finished) then
        self:DoTask(taskInfo)
    end
end

function TaskController:DoTask( taskInfo )
    print("Cat:TaskController [start:DoTask] taskInfo:", taskInfo)
    PrintTable(taskInfo)
    print("Cat:TaskController [end]")
	if not taskInfo or not taskInfo.subType then return end
    RoleMgr.GetInstance():StopMainRoleRunning()
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
    local npcPos = ConfigMgr:GetNPCPosInScene(taskInfo.sceneID, npcID)
    local findInfo = {
        destination = npcPos,
        stoppingDistance = 1.2,
        onStop = onApproachingNpc,
        sceneID = taskInfo.sceneID,
    }
    --Cat_Todo : handle destination are in different scene
    moveQuery:StartFindWay(findInfo)
end

function TaskController:DoKillMonster( taskInfo )
    print('Cat:TaskController.lua[112] taskInfo.sceneID, taskInfo.monsterID', taskInfo.sceneID, taskInfo.monsterID)
    local monsterPos = ConfigMgr:GetMonsterPosInScene(taskInfo.sceneID, taskInfo.monsterID)
    if not monsterPos then return end

    local onApproachingMonster = function (  )
        local goe = RoleMgr.GetInstance():GetMainRole()
        if goe then
            local autoFight = goe:GetComponent(typeof(CS.UnityMMO.AutoFight))
            print('Cat:TaskController.lua[119] autoFight', autoFight)
            autoFight.enabled = true
        end
    end
    print("Cat:TaskController [start:123] monsterPos:", monsterPos)
    PrintTable(monsterPos)
    print("Cat:TaskController [end]")
    local findInfo = {
        destination = monsterPos,
        stoppingDistance = 1,
        onStop = onApproachingMonster,
        sceneID = taskInfo.sceneID,
    }
    --Cat_Todo : handle destination are in different scene
    local goe = RoleMgr.GetInstance():GetMainRole()
    local moveQuery = goe:GetComponent(typeof(CS.UnityMMO.MoveQuery))
    moveQuery:StartFindWay(findInfo)
end

function TaskController:DoCollect( taskInfo )
    
end

return TaskController
