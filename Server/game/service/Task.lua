local skynet = require "skynet"
require "Common.Util.util"

local Task = {
	cfg = require "Config.ConfigTask",
	taskInfo = {},
	ackTaskProgressChanged = {},
}

--SubType.Talk
local SubType = {
	Talk = 1,
	KillMonster = 2,
	Collect = 3,
}
-- ErrorCode.NoError
local ErrorCode = {
	NoError = 0,
	CannotDoByClient = 1,
	HadNotTaskInfoInRole = 2,
	HadNotTaskCfg = 3,
}
local InitTaskInfos = function (  )
	--init the first task

end

function Task.Task_GetInfoList(userInfo, reqData)
	print("Cat:Task [start:7] userInfo:", userInfo)
	PrintTable(userInfo)
	print("Cat:Task [end]")
	print("Cat:Task [start:10] reqData:", reqData)
	PrintTable(reqData)
	print("Cat:Task [end]")
	local gameDBServer = skynet.localname(".GameDBServer")
	local isSucceed, taskList = skynet.call(gameDBServer, "lua", "select_by_key", "Task", "role_id", userInfo.cur_role_id)
	print('Cat:Task.lua[15] isSucceed', isSucceed)
	print("Cat:Task [start:16] taskList:", taskList)
	PrintTable(taskList)
	print("Cat:Task [end]")
	if not isSucceed or #taskList <= 0 then
		InitTaskInfos(userInfo.cur_role_id)
	end
	local ackData = {
		taskList = {
			{
				taskID = 1, status = 1, subTaskIndex = 1, subTypeID = 1, curProgress = 0, maxProgress = 1,
			},
		},
	}
	return ackData
end

function Task.Task_TakeTask( userInfo, reqData )
	
end

local ClientDoTask = {
	[SubType.Talk] = true,
}
local isClientDoTask = function ( subTypeID )
	return ClientDoTask[subTypeID]
end

function Task.Task_DoTask( userInfo, reqData )
	local taskInfo = Task.taskInfo[userInfo.cur_role_id]
	taskInfo = taskInfo and taskInfo[reqData.taskID]
	if taskInfo then
		if ClientDoTask[taskInfo.subTypeID] then
			local cfg = Task.cfg[reqData.taskID]
			if cfg and cfg.subTasks then
				taskInfo.subTaskIndex = taskInfo.subTaskIndex + 1
				local isFinishTask = taskInfo.subTaskIndex > #cfg.subTasks
				if isFinishTask then
					taskInfo.subTaskIndex = 1
				else
					local nextSubTask = cfg.subTasks[taskInfo.subTaskIndex]
					taskInfo.subTypeID = nextSubTask.subType
					taskInfo.curProgress = 0
					taskInfo.maxProgress = nextSubTask.maxProgress or 1
				end
				return {result=ErrorCode.NoError}
			else
				return {result=ErrorCode.HadNotTaskCfg}
			end
		else
			return {result=ErrorCode.CannotDoByClient}
		end
	else
		return {result=ErrorCode.HadNotTaskInfoInRole}
	end
end

function Task.Task_GetInfoListInNPC( userInfo, reqData )
	--Cat_Todo : check if it is close to npc 
	return {npcUID=reqData.npcUID}
end


function Task.Task_ProgressChanged( userInfo, reqData )
	if not ackTaskProgressChanged then
		ackTaskProgressChanged = skynet.response()
		return NORET
	end
	return {}
end


return Task