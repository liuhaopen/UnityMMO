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
		self.model:SetTaskInfo(ack_data)
		self.model:Fire(TaskConst.Events.AckTaskList)
	end
    NetDispatcher:SendMessage("Task_GetInfoList", nil, on_ack)

    local on_progress_changed = function ( ack_data )
        self.model:UpdateTaskInfo(ack_data)
        self.model:Fire(TaskConst.Events.AckTaskList)
    end
    NetDispatcher:Listen("Task_ProgressChanged", nil, on_progress_changed)
end

function TaskController:DoTask( taskInfo )
    print("Cat:TaskController [start:39] taskInfo:", taskInfo)
    PrintTable(taskInfo)
    print("Cat:TaskController [end]")
	if not taskInfo or not taskInfo.subTypeID then return end

    if not self.handleTaskFuncs then
        self.handleTaskFuncs = {
            [TaskConst.SubType.Talk] = TaskController.DoTalk,
            [TaskConst.SubType.KillMonster] = TaskController.DoKillMonster,
        }
    end
    local func = self.handleTaskFuncs[taskInfo.subTypeID]
    if func then
        func(self, taskInfo)
    else
        error("had not find handle func for subtype : "..taskInfo.subTypeID)
    end
end

function TaskController:DoTalk( taskInfo )
    local npcID = taskInfo.npcID
    print('Cat:TaskController.lua[47] npcID', npcID)
	local onApproachingNpc = function (  )
        local onGetTaskListInNPC = function ( ackData )
            print("Cat:SceneController [start:56] ackData:", ackData)
            PrintTable(ackData)
            print("Cat:SceneController [end]")
            -- local hasTask = ackData.taskList and #ackData.taskList > 0
            local taskNum = ackData.taskList and #ackData.taskList or 0
            if taskNum == 1 then
                
            elseif taskNum > 1 then
            else
                --show default conversation
                local view = require("Game/Task/TaskDialogView").New()
                local data = {
                    {
                        npcID = npcID,
                        content = "哈哈，你猜我是谁？",
                        btnName = "继续",
                    },
                    {

                    },
                }
                view:SetData(data)
            end
        end
        NetDispatcher:SendMessage("Task_GetInfoListInNPC", {npcUID=npcUID}, onGetTaskListInNPC)
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

return TaskController
