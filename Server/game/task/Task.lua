local skynet = require "skynet"
require "Common.Util.TableUtil"
local TaskConst = require "game.Task.TaskConst"

local Task = {
	cfg = require "Config.ConfigTask",
	taskInfos = {},
	ackTaskProgressChanged = {},
	cacheChangedTaskInfos = {}
}

local isTaskCanTake = function ( userInfo, taskID )
	return true
end

local createTaskInfoByID = function ( userInfo, taskID )
	local cfg = Task.cfg[taskID]
	local subTaskCfg = cfg.subTasks[1]
	local isCanTake = isTaskCanTake(userInfo, taskID)
	local taskInfo = {
		taskID = taskID, 
		status = isCanTake and TaskConst.Status.CanTake or TaskConst.Status.Unmet, 
		subTaskIndex = 1, 
		subTypeID = subTaskCfg.subType, 
		curProgress = 0, 
		maxProgress = subTaskCfg.maxProgress
	}
	return taskInfo
end

local InitTaskInfos = function ( userInfo )
	local taskInfos = {}
	Task.gameDBServer = Task.gameDBServer or skynet.localname(".GameDBServer")
	-- local isSucceed, taskInfo = skynet.call(gameDBServer, "lua", "select_by_key", "TaskInfo", "roleID", userInfo.cur_role_id)
	local hasTaskList, taskList = skynet.call(Task.gameDBServer, "lua", "select_by_key", "TaskList", "roleID", userInfo.cur_role_id)
	if hasTaskList and taskList and #taskList > 0 then
		taskInfos.taskList = taskList
	else
		--init the first task
		local firstMainTaskID = 1
		-- skynet.call(gameDBServer, "lua", "insert", "TaskInfo", {role_id=userInfo.cur_role_id, main_task_id=firstMainTaskID})
		local taskInfo = createTaskInfoByID(userInfo, firstMainTaskID)
		taskInfos.taskList = {taskInfo}
		taskInfo.roleID = userInfo.cur_role_id
		skynet.call(Task.gameDBServer, "lua", "insert", "TaskList", taskInfo)
	end
	return taskInfos
end

local SprotoHandlers = {}
function SprotoHandlers.Task_GetInfoList(userInfo, reqData)
	print("Cat:Task [start:7] userInfo:", userInfo)
	PrintTable(userInfo)
	print("Cat:Task [end]")
	print("Cat:Task [start:10] reqData:", reqData)
	PrintTable(reqData)
	print("Cat:Task [end]")
	local taskInfos = Task.taskInfos[userInfo.cur_role_id]
	if not taskInfos then
		taskInfos = InitTaskInfos(userInfo)
		Task.taskInfos[userInfo.cur_role_id] = taskInfos
	end
	print("Cat:Task [start:16] taskInfos:", taskInfos)
	PrintTable(taskInfos)
	print("Cat:Task [end]")
	return taskInfos
end

function SprotoHandlers.Task_TakeTask( userInfo, reqData )
end

local notifyNewChangedTaskInfo = function ( userInfo )
	local taskInfo = Task.cacheChangedTaskInfos[userInfo.cur_role_id] and Task.cacheChangedTaskInfos[userInfo.cur_role_id][1]
	if taskInfo and Task.ackTaskProgressChanged[userInfo.cur_role_id] then
		Task.ackTaskProgressChanged[userInfo.cur_role_id](true, taskInfo)
		Task.ackTaskProgressChanged[userInfo.cur_role_id] = nil
		table.remove(Task.cacheChangedTaskInfos[userInfo.cur_role_id], 1)
	end
end

function SprotoHandlers.Task_DoTask( userInfo, reqData )
	local taskInfos = Task.taskInfos[userInfo.cur_role_id]
	local taskInfo = table.get_value_in_array(taskInfos and taskInfos.taskList, "taskID", reqData.taskID)
	if taskInfo then
		if TaskConst.ClientDoTask[taskInfo.subTypeID] then
			local cfg = Task.cfg[reqData.taskID]
			if cfg and cfg.subTasks then
				taskInfo.subTaskIndex = taskInfo.subTaskIndex + 1
				local isFinishTask = taskInfo.subTaskIndex > #cfg.subTasks
				if isFinishTask then
					taskInfo.subTaskIndex = #cfg.subTasks
					taskInfo.status = TaskConst.Status.Finished
					taskInfo.curProgress = taskInfo.maxProgress
					table.remove_value_in_array(taskInfos.taskList, "taskID", reqData.taskID)
					local nextTaskInfo = createTaskInfoByID(userInfo, cfg.nextTaskID)
					table.insert(taskInfos.taskList, nextTaskInfo)
				else
					local nextSubTask = cfg.subTasks[taskInfo.subTaskIndex]
					taskInfo.subTypeID = nextSubTask.subType
					taskInfo.curProgress = 0
					taskInfo.maxProgress = nextSubTask.maxProgress or 1
				end
				skynet.timeout(1,function()
					Task.cacheChangedTaskInfos[userInfo.cur_role_id] = Task.cacheChangedTaskInfos[userInfo.cur_role_id] or {}
					table.insert(Task.cacheChangedTaskInfos[userInfo.cur_role_id], taskInfo)
					notifyNewChangedTaskInfo(userInfo, taskInfo)
				end)
				return {result=TaskConst.ErrorCode.NoError}
			else
				return {result=TaskConst.ErrorCode.HadNotTaskCfg}
			end
		else
			return {result=TaskConst.ErrorCode.CannotDoByClient}
		end
	else
		return {result=TaskConst.ErrorCode.HadNotTaskInfoInRole}
	end
end

function SprotoHandlers.Task_GetInfoListInNPC( userInfo, reqData )
	--Cat_Todo : check if it is close to npc 
	local taskInfos = Task.taskInfos[userInfo.cur_role_id]
	if taskInfos and taskInfos.taskList then
		
	end
	return {npcUID=reqData.npcUID}
end

function SprotoHandlers.Task_ProgressChanged( userInfo, reqData )
	if not Task.ackTaskProgressChanged[userInfo.cur_role_id] then
		Task.cacheChangedTaskInfos[userInfo.cur_role_id] = Task.cacheChangedTaskInfos[userInfo.cur_role_id] or {}
		local taskInfo = Task.cacheChangedTaskInfos[userInfo.cur_role_id] and Task.cacheChangedTaskInfos[userInfo.cur_role_id][1]
		if taskInfo then
			table.remove(Task.cacheChangedTaskInfos[userInfo.cur_role_id], 1)
			return taskInfo
		else
			Task.ackTaskProgressChanged[userInfo.cur_role_id] = skynet.response()
			return NORET
		end
	end
	return {}
end

local PublicFuncs = {}

function PublicFuncs.NPCHasTask( roleID, npcID )
	return false
end

SprotoHandlers.PublicClassName = "Task"
SprotoHandlers.PublicFuncs = PublicFuncs

return SprotoHandlers, "Task", PublicFuncs