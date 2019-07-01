local TaskConst = {
	--TaskConst.Events.ReqTaskList
	Events = {
		ReqTaskList = "TaskConst.Events.ReqTaskList",
		AckTaskList = "TaskConst.Events.AckTaskList",
	},

	--TaskConst.Type.Unknow
	Type = {
		MainLine = 1,
		SubLine = 2,
		Unknow = 999,
	},
	--TaskConst.SubType.Collect
	SubType = {
		Talk = 1,
		KillMonster = 2,
		Collect = 3,
	},

	--TaskConst.Status.Unmet
	Status = {
		Unmet = 0,--条件未满足
		CanTake = 1,--可接取
		Doing = 2,--进行中
		Finished = 3,--已完成
		Received = 4,--已领取奖励
	},
	--TaskConst.StatusStr[TaskConst.Status.Unmet]
	StatusStr = {
		[0] = "未可接",
		[1] = "可接取",
		[2] = "进行中",
		[3] = "可领奖",
		[4] = "已领奖",
	},

	--TaskConst.Prefix[TaskConst.Type.MainLine]
	Prefix = {
		[1] = [[主]],
		[2] = [[支]],
	},

	--TaskConst.Desc[TaskConst.SubType.Talk]
	Desc = {
		[1] = [[找<#88ff43>%s</color>对话]],
		[2] = [[打败<#88ff43>%s</color>]],
		[3] = [[采集<#88ff43>%s</color>]],
	},

	--TaskConst.DialogBtnFlag.Continue
	DialogBtnFlag = {
		Continue = 1,
		TakeTask = 2,
		DoTask = 3,
		Ok = 4,
	},
}

return TaskConst