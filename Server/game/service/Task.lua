local skynet = require "skynet"
require "Common.Util.util"

local Task = {}

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
				taskID = 1, subTaskList = {
					typeID = 1, state = 1, curNum = 0
				},
			},
		},
	}
	return ackData
end

function Task.Task_GetInfoListInNPC( userInfo, reqData )
	print("Cat:Task [start:20] reqData:", reqData)
	PrintTable(reqData)
	print("Cat:Task [end]")
	--Cat_Todo : check if it is close to npc 
	return {npcUID=reqData.npcUID}
end


return Task