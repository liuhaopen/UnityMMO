return [[

#status:0未接取 1进行中 2已完成 3已完成未领取 4已领取奖励
#maxProgress要求数量 
#curProgress已完成数量
.taskSubInfo {
	subTypeID 0 : integer
	status 1 : integer
	maxProgress 2 : integer
	curProgress 3 : integer
}

#subTaskIndex:当前处于哪个子任务阶段
.taskInfo {
	taskID 0 : integer
	subTaskIndex 1 : integer
	subTaskList 2 : taskSubInfo
}

.taskNPCTaskInfo {
	taskID 0 : integer
}

#获取任务列表
Task_GetInfoList 200 {
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
		content 2 : string
	}
}

]]