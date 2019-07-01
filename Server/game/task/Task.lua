local skynet = require "skynet"
require "Common.Util.TableUtil"
local TaskConst = require "game.task.TaskConst"

local Task = {
	cfg = require "Config.ConfigTask",
	taskInfos = {},
	ackTaskProgressChanged = {},
	cacheChangedTaskInfos = {}
}

local notifyNewChangedTaskInfo = function ( userInfo )
	local taskInfo = Task.cacheChangedTaskInfos[userInfo.cur_role_id] and Task.cacheChangedTaskInfos[userInfo.cur_role_id][1]
	print('Cat:Task.lua[15] taskInfo, ', taskInfo, Task.ackTaskProgressChanged[userInfo.cur_role_id])
	if taskInfo and Task.ackTaskProgressChanged[userInfo.cur_role_id] then
		-- Task.ackTaskProgressChanged[userInfo.cur_role_id](true, {taskInfo=taskInfo})
		-- Task.ackTaskProgressChanged[userInfo.cur_role_id] = nil
		-- table.remove(Task.cacheChangedTaskInfos[userInfo.cur_role_id], 1)
		local co = Task.ackTaskProgressChanged[userInfo.cur_role_id]
		Task.ackTaskProgressChanged[userInfo.cur_role_id] = nil
		skynet.wakeup(co)
	end
end

local addNewChangedTaskInfo = function ( userInfo, taskInfo )
	Task.cacheChangedTaskInfos[userInfo.cur_role_id] = Task.cacheChangedTaskInfos[userInfo.cur_role_id] or {}
	table.insert(Task.cacheChangedTaskInfos[userInfo.cur_role_id], taskInfo)
	notifyNewChangedTaskInfo(userInfo, taskInfo)
end
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
		subType = subTaskCfg.subType, 
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
	local taskInfos = Task.taskInfos[userInfo.cur_role_id]
	if not taskInfos then
		taskInfos = InitTaskInfos(userInfo)
		Task.taskInfos[userInfo.cur_role_id] = taskInfos
	end
	return taskInfos
end

local completeSubTask = function ( taskInfo )
	if not taskInfo then return end
	local cfg = Task.cfg[taskInfo.taskID]
	if not cfg then return end
	
	taskInfo.subTaskIndex = taskInfo.subTaskIndex + 1
	local isFinishTask = taskInfo.subTaskIndex > #cfg.subTasks
	if isFinishTask then
		taskInfo.subTaskIndex = #cfg.subTasks
		taskInfo.status = TaskConst.Status.Finished
		taskInfo.curProgress = taskInfo.maxProgress
	else
		local nextSubTask = cfg.subTasks[taskInfo.subTaskIndex]
		taskInfo.subType = nextSubTask.subType
		taskInfo.curProgress = 0
		taskInfo.maxProgress = nextSubTask.maxProgress or 1
	end	
	return isFinishTask
end

function SprotoHandlers.Task_TakeTask( userInfo, reqData )
	local taskInfos = Task.taskInfos[userInfo.cur_role_id]
	local taskInfo = table.get_value_in_array(taskInfos and taskInfos.taskList, "taskID", reqData.taskID)
	local result = TaskConst.ErrorCode.Unknow
	if taskInfo then
		if taskInfo.status == TaskConst.Status.CanTake then
			completeSubTask(taskInfo)
			taskInfo.status = TaskConst.Status.Doing
			result = TaskConst.ErrorCode.NoError
			skynet.timeout(1,function()
				addNewChangedTaskInfo(userInfo, taskInfo)
			end)
		elseif taskInfo.status == TaskConst.Status.CanTake then
			result = TaskConst.ErrorCode.NoError
		end
	end
	return {result=result}
end

function SprotoHandlers.Task_DoTask( userInfo, reqData )
	local taskInfos = Task.taskInfos[userInfo.cur_role_id]
	local taskInfo = table.get_value_in_array(taskInfos and taskInfos.taskList, "taskID", reqData.taskID)
	if taskInfo then
		if TaskConst.ClientDoTask[taskInfo.subType] then
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
					taskInfo.subType = nextSubTask.subType
					taskInfo.curProgress = 0
					taskInfo.maxProgress = nextSubTask.maxProgress or 1
				end
				skynet.timeout(1,function()
					addNewChangedTaskInfo(userInfo, taskInfo)
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

local getTaskListInNPC = function ( roleID, npcID )
	--Cat_Todo : check if it is close to npc 
	local taskInfos = Task.taskInfos[roleID]
	local taskIDs = {}
	if taskInfos and taskInfos.taskList then
		for i,v in ipairs(taskInfos.taskList) do
			local cfg = Task.cfg[v.taskID]
			local subCfg = cfg and cfg.subTasks[v.subTaskIndex]
			if v.subType == TaskConst.SubType.Talk and subCfg and subCfg.contentID == npcID then
				table.insert(taskIDs, v.taskID)
			end
		end
	end
	return taskIDs
	--Cat_Todo : dynamic task
end

function SprotoHandlers.Task_GetInfoListInNPC( userInfo, reqData )
	local taskIDs = getTaskListInNPC(userInfo.cur_role_id, reqData.npcID)
	return {npcID=reqData.npcID, taskIDList=taskIDs}
end

function SprotoHandlers.Task_ProgressChanged( userInfo, reqData )
	if not Task.ackTaskProgressChanged[userInfo.cur_role_id] then
		local taskInfo = Task.cacheChangedTaskInfos[userInfo.cur_role_id] and Task.cacheChangedTaskInfos[userInfo.cur_role_id][1]
		if not taskInfo then
			Task.ackTaskProgressChanged[userInfo.cur_role_id] = coroutine.running()
			skynet.wait(Task.ackTaskProgressChanged[userInfo.cur_role_id])
		end
		taskInfo = Task.cacheChangedTaskInfos[userInfo.cur_role_id] and Task.cacheChangedTaskInfos[userInfo.cur_role_id][1]
		if taskInfo then
			table.remove(Task.cacheChangedTaskInfos[userInfo.cur_role_id], 1)
			return {taskInfo=taskInfo}
		else
		end
	else
		--shouldn't be here,the client requested it again before replying
	end
	return {}
end

local PublicFuncs = {}
function PublicFuncs.NPCHasTask( roleID, npcID )
	--Cat_Todo : cache npc status
	local taskIDs = getTaskListInNPC(roleID, npcID)
	return #taskIDs > 0
end

function PublicFuncs.KillMonster( roleID, monsterID, killNum )
	print('Cat:Task.lua[203] roleID, monsterID, killNum', roleID, monsterID, killNum)
	
end

SprotoHandlers.PublicClassName = "Task"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers