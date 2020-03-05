local skynet = require "skynet"
require "common.TableUtil"
local TaskConst = require "game.task.TaskConst"

local user_info
local Task = {
	cfg = require "Config.ConfigTask",
	taskInfos = {},
	ackTaskProgressChanged = {},
	cacheChangedTaskInfos = {},
	monsterTargetIDs = {},
}

local notifyNewChangedTaskInfo = function ( roleID )
	local taskInfo = Task.cacheChangedTaskInfos[roleID] and Task.cacheChangedTaskInfos[roleID][1]
	if taskInfo and Task.ackTaskProgressChanged[roleID] then
		local co = Task.ackTaskProgressChanged[roleID]
		Task.ackTaskProgressChanged[roleID] = nil
		skynet.wakeup(co)
	end
end

local addNewChangedTaskInfo = function ( roleID, taskInfo )
	Task.cacheChangedTaskInfos[roleID] = Task.cacheChangedTaskInfos[roleID] or {}
	table.insert(Task.cacheChangedTaskInfos[roleID], taskInfo)
	notifyNewChangedTaskInfo(roleID, taskInfo)
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
		maxProgress = subTaskCfg.maxProgress,
		contentID = subTaskCfg.contentID,
	}
	return taskInfo, cfg, subTaskCfg
end

local addTargetMonsterIfKillTask = function ( roleID, taskInfo )
	if taskInfo.subType == TaskConst.SubType.KillMonster then
		Task.monsterTargetIDs[roleID] = Task.monsterTargetIDs[roleID] or {}
		Task.monsterTargetIDs[roleID][taskInfo.contentID] = Task.monsterTargetIDs[roleID][taskInfo.contentID] or 0
		Task.monsterTargetIDs[roleID][taskInfo.contentID] = Task.monsterTargetIDs[roleID][taskInfo.contentID] + 1
	end 
end

local updateTargetMonsterIfKillTask = function ( roleID, taskInfo )
	if taskInfo.subType == TaskConst.SubType.KillMonster and Task.monsterTargetIDs[roleID] and Task.monsterTargetIDs[roleID][taskInfo.contentID] then
		Task.monsterTargetIDs[roleID][taskInfo.contentID] = Task.monsterTargetIDs[roleID][taskInfo.contentID] - 1
		if Task.monsterTargetIDs[roleID][taskInfo.contentID] <= 0 then
			Task.monsterTargetIDs[roleID][taskInfo.contentID] = nil
		end
	end 
end

local InitTaskInfos = function ( userInfo )
	local taskInfos = {}
	Task.gameDBServer = Task.gameDBServer or skynet.localname(".GameDBServer")
	-- local isSucceed, taskInfo = skynet.call(gameDBServer, "lua", "select_by_key", "TaskInfo", "roleID", userInfo.cur_role_id)
	local hasTaskList, taskList = skynet.call(Task.gameDBServer, "lua", "select_by_key", "TaskList", "roleID", userInfo.cur_role_id)
	local isDataOk = hasTaskList and taskList and #taskList > 0
	if not isDataOk then
		--init the first task
		local firstMainTaskID = 1
		local taskInfo, cfg, subTaskCfg = createTaskInfoByID(userInfo, firstMainTaskID)
		taskInfos.taskList = {taskInfo}
		taskInfo.roleID = userInfo.cur_role_id
		skynet.call(Task.gameDBServer, "lua", "insert", "TaskList", taskInfo)
		--need to use the 'id' value in the database, so query again
		hasTaskList, taskList = skynet.call(Task.gameDBServer, "lua", "select_by_key", "TaskList", "roleID", userInfo.cur_role_id)
		isDataOk = hasTaskList and taskList and #taskList > 0
	end
	if isDataOk then
		taskInfos.taskList = taskList
	end
	if taskInfos and taskInfos.taskList then
		for i,v in ipairs(taskInfos.taskList) do
			addTargetMonsterIfKillTask(userInfo.cur_role_id, v)
		end
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

local completeSubTask = function ( roleID, taskInfo )
	if not taskInfo then return end
	local cfg = Task.cfg[taskInfo.taskID]
	if not cfg then return end
	
	updateTargetMonsterIfKillTask(roleID, taskInfo)
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
		taskInfo.contentID = nextSubTask.contentID or 0
		addTargetMonsterIfKillTask(roleID, taskInfo)
		if taskInfo.id then
			skynet.send(Task.gameDBServer, "lua", "update", "TaskList", "id", taskInfo.id, taskInfo)
		end
	end	
	return isFinishTask
end

function SprotoHandlers.Task_TakeTask( userInfo, reqData )
	local taskInfos = Task.taskInfos[userInfo.cur_role_id]
	local taskInfo = table.get_value_in_array(taskInfos and taskInfos.taskList, "taskID", reqData.taskID)
	local result = TaskConst.ErrorCode.Unknow
	if taskInfo then
		if taskInfo.status == TaskConst.Status.CanTake then
			completeSubTask(userInfo.cur_role_id, taskInfo)
			taskInfo.status = TaskConst.Status.Doing
			result = TaskConst.ErrorCode.NoError
			skynet.timeout(1,function()
				addNewChangedTaskInfo(userInfo.cur_role_id, taskInfo)
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
				local isFinishTask = completeSubTask(userInfo.cur_role_id, taskInfo)
				if isFinishTask then
					table.remove_value_in_array(taskInfos.taskList, "taskID", reqData.taskID)
					local nextTaskInfo = createTaskInfoByID(userInfo, cfg.nextTaskID)
					nextTaskInfo.id = taskInfo.id
					addTargetMonsterIfKillTask(userInfo.cur_role_id, nextTaskInfo)
					table.insert(taskInfos.taskList, nextTaskInfo)
					skynet.send(Task.gameDBServer, "lua", "update", "TaskList", "id", taskInfo.id, nextTaskInfo)
				end
				skynet.timeout(1,function()
					addNewChangedTaskInfo(userInfo.cur_role_id, taskInfo)
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

function PublicFuncs.Init( userInfo )
	user_info = userInfo
end

function PublicFuncs.Logout( )
	print('Cat:Task.lua[Logout]')
	-- local taskInfos = Task.taskInfos[user_info.cur_role_id]
end

function PublicFuncs.NPCHasTask( roleID, npcID )
	--Cat_Todo : cache npc status
	local taskIDs = getTaskListInNPC(roleID, npcID)
	return #taskIDs > 0
end

function PublicFuncs.KillMonster( roleID, monsterID, killNum )
	-- print('Cat:Task.lua[203] roleID, monsterID, killNum', roleID, monsterID, killNum, Task.monsterTargetIDs[roleID], Task.monsterTargetIDs[roleID] and Task.monsterTargetIDs[roleID][monsterID] or "nil", Task.taskInfos[roleID])
	if Task.monsterTargetIDs[roleID] and Task.monsterTargetIDs[roleID][monsterID] then
		local taskInfos = Task.taskInfos[roleID]
		if taskInfos and taskInfos.taskList then
			for i,v in ipairs(taskInfos.taskList) do
				-- print('Cat:Task.lua[216] v.curProgress', v.curProgress, v.maxProgress, v.contentID, v.subType)
				if v.subType == TaskConst.SubType.KillMonster and v.contentID == monsterID then
					v.curProgress = v.curProgress + killNum
					if v.curProgress >= v.maxProgress then
						completeSubTask(roleID, v)
						addNewChangedTaskInfo(roleID, v)
					else
						addNewChangedTaskInfo(roleID, v)
						--avoid frequent update database
						if v.curProgress%5==0 then
							skynet.send(Task.gameDBServer, "lua", "update", "TaskList", "id", v.id, v)
						end
					end
				end
			end
		end
	end
end

SprotoHandlers.PublicClassName = "Task"
SprotoHandlers.PublicFuncs = PublicFuncs
return SprotoHandlers