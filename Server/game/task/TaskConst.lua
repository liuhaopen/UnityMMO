local TaskConst = {
	--TaskConst.SubType.Talk
	SubType = {
		Talk = 1,
		KillMonster = 2,
		Collect = 3,
	},
	--TaskConst.ClientDoTask[TaskConst.SubType.Talk]
	ClientDoTask = {
		[1] = true,
	},
	--TaskConst.Status.CanTake
	Status = {
		Unmet = 0,--条件未满足
		CanTake = 1,--可接取
		Doing = 2,--进行中
		Finished = 3,--已完成
		Received = 4,--已领取奖励
	},
	--TaskConst.ErrorCode.Unknow
	ErrorCode = {
		NoError = 0,
		CannotDoByClient = 1,
		HadNotTaskInfoInRole = 2,
		HadNotTaskCfg = 3,
		Unknow = 4,
	},
	
}
return TaskConst