local skynet = require "skynet"
require "Common.Util.util"

local Task = {}

function Task.Task_GetInfoList(userInfo, reqData)
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