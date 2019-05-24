return [[

#0未完成 1已完成 #要求数量 #已完成数量
.taskSubInfo {
	typeID 0 : integer
	state 1 : integer
	condition 2 : integer
	curNum 3 : integer
}

.taskInfo {
	taskID 0 : integer
	subTaskList 1 : *taskSubInfo
}

.taskNPCTaskInfo {
	taskID 0 : integer	
}

#获取任务列表
Task_GetInfoList 200 {
	request {
	}
	response {
		taskList 0 : *taskInfo
	}
}

#获取该NPC身上的任务列表
Task_GetInfoListInNPC 201 {
	request {
		npcUID 0 : integer
	}
	response {
		npcUID 0 : integer
		taskList 1 : *taskNPCTaskInfo
	}
}

]]