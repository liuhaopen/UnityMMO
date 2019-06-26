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
		id = 1, taskType = 1, name = [[帮小花赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 2,
		subTasks = {
			{
				subType = 1, contentID = 3001, content = {
					{who=3001, chat=[[我家被怪物占领了，你可以帮我赶走它们吗？]]},
					{who=0, chat=[[好啊，但是你能给我什么好处？]]}, 
					{who=3001, chat=[[我愿意献上我所有的节操...]]},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2000, maxProgress=10
			},
			{
				subType = 1, contentID = 3001, content = {
					{who=0, chat=[[我把怪物都打掉了，快给我节操吧]]},
					{who=3001, chat=[[去那边地上捡吧...]]},
				},
			},
		},
	},
	[2] = {
		id = 2, taskType = 1, name = [[帮小白采花]], condition = {
			{conditionType = 1, conditionValue = 1}, {conditionType = 2, conditionValue = 1},
		}, nextTaskID = 3,
		subTasks = {
			{
				subType = 1, contentID = 3002, content = {
					{who=3002, chat=[[可以采朵花给我吗？]]},
					{who=0, chat=[[没问题，但是你可以嫁给我吗？]]}, 
					{who=3002, chat=[[你先采来看看嘛]]},
				},
			},
			{
				subType = 3, sceneID=1001, contentID=4000, maxProgress=1
			},
			{
				subType = 1, contentID = 3002, content = {
					{who=0, chat=[[我们结婚吧]]},
					{who=3002, chat=[[来吧，刚好我怀了你的孩子...]]},
					{who=0, chat=[[...我们好像才第一天认识]]},
				},
			},
		},
	},
}

return config