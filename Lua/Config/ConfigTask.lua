--Cat_Todo : 因为通常需要用的数据量只占很少部分，所以最好就拆分文件存放(按任务大类拆，大类里还可再按id分段拆)
--[[
id:任务id
taskType:任务类型 1主线 2支线 3日常 4公会
name:任务名
condition:触发条件
condition.type:条件类型 1等级 2已完成任务 3从npc领取 4进入某区域 5获得某道具 6打死某怪
condition.value:触发条件，根据触发类型而不同
subTasks:每个任务都由几个子任务组成
subType:具体每条子任务的类型 1对话 2打怪 3采集 4完成副本 5收集掉落物 6玩家自制道具 7护送 8探索某区域 9破坏某场景物品 10捕鱼 
who:谁，0为自己，其它为NPC类型id
--]]
local config = {
	[1] = {
		id = 1, taskType = 1, name = [[主线1]], condition = {
			{conditionType = 1, conditionValue = 1},
		},
		subTasks = {
			{
				subType = 1, content = {
					{who=3001, chat=[[我家被怪物占领了，你可以帮我赶走它们吗？]]},
					{who=0, chat=[[好啊，但是你能给我什么好处？]]}, 
					{who=3001, chat=[[我愿意献上我所有的节操...]]},
				},
			},
			{
				subType = 2, content = {
					{sceneID=1001, monsterTypeID=2000, num=10},
				},
			},
			{
				subType = 1, content = {
					{who=3001, chat=4},
				},
			},
		},
	},
	[2] = {
		id = 2, taskType = 1, name = [[主线2]], condition = {
			{conditionType = 1, conditionValue = 2}, {conditionType = 2, conditionValue = 1},
		},
		subTasks = {
			{
				subType = 1, content = {
					{who=2, chat=1},
					{who=1, chat=2}, 
				},
			},
		},
	},
}

return config