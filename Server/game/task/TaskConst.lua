local TaskConst = {
	--SubType.Talk
	SubType = {
		Talk = 1,
		KillMonster = 2,
		Collect = 3,
	},
	--Status.CanTake
	Status = {
		Unmet = 0,--条件未满足
		CanTake = 1,--可接取
		Doing = 2,--进行中
		Finished = 3,--已完成
		Received = 4,--已领取奖励
	},
	-- ErrorCode.NoError
	ErrorCode = {
		NoError = 0,
		CannotDoByClient = 1,
		HadNotTaskInfoInRole = 2,
		HadNotTaskCfg = 3,
	},
	ClientDoTask = {
		[SubType.Talk] = true,
	},
}
return TaskConst