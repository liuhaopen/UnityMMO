return [[

#0未完成 1已完成 #要求数量 #已完成数量
.task_sub_info {
	type_id 0 : integer
	state 1 : integer
	condition 2 : integer
	cur_num 3 : integer
}

.task_info {
	task_id 0 : integer
	sub_task_list 1 : *task_sub_info
}

.task_npc_task_info {
	task_id 0 : integer	
}

#获取任务列表
task_get_info_list 200 {
	request {
	}
	response {
		task_list 0 : *task_info
	}
}

#获取该NPC身上的任务列表
task_in_npc 201 {
	request {
		npc_uid 0 : integer
	}
	response {
		npc_uid 0 : integer
		task_list 1 : *task_npc_task_info
	}
}

]]