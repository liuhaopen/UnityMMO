--Cat_Todo : 因为通常需要用的数据量只占很少部分,所以最好就拆分文件存放(按任务大类拆,大类里还可再按id分段拆)
--[[
id:任务id
taskType:任务类型 1主线 2支线 3日常 4公会
name:任务名
condition:触发条件
condition.type:条件类型 1等级 2已完成任务 3从npc领取 4进入某区域 5获得某道具 6打死某怪
condition.value:触发条件,根据触发类型而不同
subTasks:每个任务都由几个子任务组成
subType:具体每条子任务的类型 1对话 2打怪 3采集 4完成副本 5收集掉落物 6玩家自制道具 7护送 8探索某区域 9破坏某场景物品 10捕鱼 
who:谁,0为自己,其它为NPC类型id
flag:详见 TaskConst.DialogBtnFlag
--]]
local config = {
	[1] = {
		id = 1, taskType = 1, name = [[1.帮风车男孩赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 2,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=3001, chat=[[我家被怪物占领了,你可以帮我赶走它们吗?]], flag=1},
					{who=0, chat=[[好啊,但是你能给我什么好处?]], flag=1}, 
					{who=3001, chat=[[我愿意献上我所有的节操...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2000, maxProgress=10
			},
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=0, chat=[[我把怪物都打掉了,快给我节操吧]], flag=1},
					{who=3001, chat=[[去那边地上捡吧...]], flag=3},
				},
			},
		},
	},
	[2] = {
		id = 2, taskType = 1, name = [[2.帮猫女赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 3,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=3002, chat=[[我被那些怪物欺负了,可以帮我教训它们吗?]], flag=1},
					{who=0, chat=[[看你这么骚，好吧]], flag=1}, 
					{who=3002, chat=[[就喜欢你这么直接的...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2002, maxProgress=20
			},
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=0, chat=[[我帮你收拾它们了,给我一个吻吧]], flag=1},
					{who=3002, chat=[[你确定吗?我是人妖]], flag=3},
				},
			},
		},
	},
	[3] = {
		id = 3, taskType = 1, name = [[3.帮帮流浪剑客]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 4,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=3000, chat=[[虽然我是剑客,但我答应过我师傅不再动刀的?]], flag=1},
					{who=0, chat=[[说吧，被谁打了?]], flag=1}, 
					{who=3000, chat=[[呜,是断头鬼]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, sceneID=1001, contentID=2004, maxProgress=30
			},
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=0, chat=[[以后它们不敢打你了]], flag=1},
					{who=3000, chat=[[请受我一跪]], flag=3},
				},
			},
		},
	},
	[4] = {
		id = 4, taskType = 1, name = [[4.帮下美女]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 5,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=3002, chat=[[我被那些怪物欺负了,可以帮我教训它们吗?]], flag=1},
					{who=0, chat=[[看你这么骚，好吧]], flag=1}, 
					{who=3002, chat=[[就喜欢你这么直接的...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2000, maxProgress=5
			},
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=0, chat=[[我帮你收拾它们了,给我一个吻吧]], flag=1},
					{who=3002, chat=[[你确定吗?我是人妖]], flag=3},
				},
			},
		},
	},
	[5] = {
		id = 5, taskType = 1, name = [[5.帮风车男孩赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 6,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=3001, chat=[[我家被怪物占领了,你可以帮我赶走它们吗?]], flag=1},
					{who=0, chat=[[好啊,但是你能给我什么好处?]], flag=1}, 
					{who=3001, chat=[[我愿意献上我所有的节操...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2002, maxProgress=10
			},
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=0, chat=[[我把怪物都打掉了,快给我节操吧]], flag=1},
					{who=3001, chat=[[去那边地上捡吧...]], flag=3},
				},
			},
		},
	},
	[6] = {
		id = 6, taskType = 1, name = [[6.帮帮流浪剑客]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 7,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=3000, chat=[[虽然我是剑客,但我答应过我师傅不再动刀的?]], flag=1},
					{who=0, chat=[[说吧，被谁打了?]], flag=1}, 
					{who=3000, chat=[[呜,是断头鬼]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, sceneID=1001, contentID=2004, maxProgress=5
			},
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=0, chat=[[以后它们不敢打你了]], flag=1},
					{who=3000, chat=[[请受我一跪]], flag=3},
				},
			},
		},
	},
	[7] = {
		id = 7, taskType = 1, name = [[7.帮风车男孩赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 8,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=3001, chat=[[我家被怪物占领了,你可以帮我赶走它们吗?]], flag=1},
					{who=0, chat=[[好啊,但是你能给我什么好处?]], flag=1}, 
					{who=3001, chat=[[我愿意献上我所有的节操...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2000, maxProgress=10
			},
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=0, chat=[[我把怪物都打掉了,快给我节操吧]], flag=1},
					{who=3001, chat=[[去那边地上捡吧...]], flag=3},
				},
			},
		},
	},
	[8] = {
		id = 8, taskType = 1, name = [[8.帮下美女]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 9,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=3002, chat=[[我被那些怪物欺负了,可以帮我教训它们吗?]], flag=1},
					{who=0, chat=[[看你这么骚，好吧]], flag=1}, 
					{who=3002, chat=[[就喜欢你这么直接的...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2002, maxProgress=25
			},
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=0, chat=[[我帮你收拾它们了,给我一个吻吧]], flag=1},
					{who=3002, chat=[[你确定吗?我是人妖]], flag=3},
				},
			},
		},
	},
	[9] = {
		id = 9, taskType = 1, name = [[9.帮风车男孩赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 10,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=3001, chat=[[我家被怪物占领了,你可以帮我赶走它们吗?]], flag=1},
					{who=0, chat=[[好啊,但是你能给我什么好处?]], flag=1}, 
					{who=3001, chat=[[我愿意献上我所有的节操...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2000, maxProgress=10
			},
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=0, chat=[[我把怪物都打掉了,快给我节操吧]], flag=1},
					{who=3001, chat=[[去那边地上捡吧...]], flag=3},
				},
			},
		},
	},
	[10] = {
		id = 10, taskType = 1, name = [[10.帮猫女赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 11,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=3002, chat=[[我被那些怪物欺负了,可以帮我教训它们吗?]], flag=1},
					{who=0, chat=[[看你这么骚，好吧]], flag=1}, 
					{who=3002, chat=[[就喜欢你这么直接的...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2004, maxProgress=20
			},
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=0, chat=[[我帮你收拾它们了,给我一个吻吧]], flag=1},
					{who=3002, chat=[[你确定吗?我是人妖]], flag=3},
				},
			},
		},
	},
	[11] = {
		id = 11, taskType = 1, name = [[11.帮帮流浪剑客]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 12,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=3000, chat=[[虽然我是剑客,但我答应过我师傅不再动刀的?]], flag=1},
					{who=0, chat=[[说吧，被谁打了?]], flag=1}, 
					{who=3000, chat=[[呜,是断头鬼]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, sceneID=1001, contentID=2004, maxProgress=30
			},
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=0, chat=[[以后它们不敢打你了]], flag=1},
					{who=3000, chat=[[请受我一跪]], flag=3},
				},
			},
		},
	},
	[12] = {
		id = 12, taskType = 1, name = [[12.帮风车男孩赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 13,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=3001, chat=[[我家被怪物占领了,你可以帮我赶走它们吗?]], flag=1},
					{who=0, chat=[[好啊,但是你能给我什么好处?]], flag=1}, 
					{who=3001, chat=[[我愿意献上我所有的节操...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2000, maxProgress=10
			},
			{
				subType = 1, sceneID=1001, contentID = 3001, content = {
					{who=0, chat=[[我把怪物都打掉了,快给我节操吧]], flag=1},
					{who=3001, chat=[[去那边地上捡吧...]], flag=3},
				},
			},
		},
	},
	[13] = {
		id = 13, taskType = 1, name = [[13.帮猫女赶怪]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 14,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=3002, chat=[[我被那些怪物欺负了,可以帮我教训它们吗?]], flag=1},
					{who=0, chat=[[看你这么骚，好吧]], flag=1}, 
					{who=3002, chat=[[就喜欢你这么直接的...]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, contentID=2002, maxProgress=20
			},
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=0, chat=[[我帮你收拾它们了,给我一个吻吧]], flag=1},
					{who=3002, chat=[[你确定吗?我是人妖]], flag=3},
				},
			},
		},
	},
	[14] = {
		id = 14, taskType = 1, name = [[14.帮帮流浪剑客]], condition = {
			{conditionType = 1, conditionValue = 1},
		}, nextTaskID = 15,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=3000, chat=[[虽然我是剑客,但我答应过我师傅不再动刀的?]], flag=1},
					{who=0, chat=[[说吧，被谁打了?]], flag=1}, 
					{who=3000, chat=[[呜,是断头鬼]], flag=2},
				},
			},
			{
				subType = 2, sceneID=1001, sceneID=1001, contentID=2004, maxProgress=30
			},
			{
				subType = 1, sceneID=1001, contentID = 3000, content = {
					{who=0, chat=[[以后它们不敢打你了]], flag=1},
					{who=3000, chat=[[请受我一跪]], flag=3},
				},
			},
		},
	},
	[999] = {
		id = 999, taskType = 1, name = [[帮小白采花]], condition = {
			{conditionType = 1, conditionValue = 1}, {conditionType = 2, conditionValue = 1},
		}, nextTaskID = 3,
		subTasks = {
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=3002, chat=[[可以采朵花给我吗?]]},
					{who=0, chat=[[没问题,但是你可以嫁给我吗?]]}, 
					{who=3002, chat=[[你先采来看看嘛]]},
				},
			},
			{
				subType = 3, sceneID=1001, sceneID=1001, contentID=4000, maxProgress=1
			},
			{
				subType = 1, sceneID=1001, contentID = 3002, content = {
					{who=0, chat=[[我们结婚吧]]},
					{who=3002, chat=[[来吧,刚好我怀了你的孩子...]]},
					{who=0, chat=[[...我们好像才第一天认识]]},
				},
			},
		},
	},
}

return config