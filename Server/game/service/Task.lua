local skynet = require "skynet"
require "Common.Util.util"

local Task = {}

function Task.Task_GetInfoList(user_info, req_data)
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


return Task