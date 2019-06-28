return [[

#status:见TaskConst.Status：0条件不足不可接取 1可接取 2进行中 3已完成 4已领取奖励

#maxProgress要求数量 
#curProgress已完成数量
#contentID:根据不同子任务类型可为npcID或怪物id或物品id
.taskInfo {
	taskID 0 : integer
	status 1 : integer
	subTaskIndex 2 : integer
	subType 3 : integer
	curProgress 4 : integer
	maxProgress 5 : integer
	addition 6 : string
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
		npcID 0 : integer
	}
	response {
		npcID 0 : integer
		taskIDList 1 : *integer
		content 2 : string
	}
}

Task_TakeTask 202 {
	request {
		taskID 0 : integer
	}
	response {
		result 0 : integer
	}
}

#有些任务阶段需要前端自己完成的，比如npc对话
Task_DoTask 203 {
	request {
		taskID 0 : integer
	}
	response {
		result 0 : integer
	}
}

Task_ProgressChanged 204 {
	response {
		taskInfo 0 : taskInfo
	}
}


]]