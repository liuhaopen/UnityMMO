--[[
id:任务id
type:任务类型 1主线 2支线 3日常 4公会
name:任务名
sub_tasks:每个任务都由几个子任务组成
sub_type:具体每条子任务的类型 1对话 2打怪 3采集 4完成副本...
who:谁，0为自己 
--]]
local config = {
	[1] = {
		id = 1, type = 1, name = [[主线1]], sub_tasks = {
			{
				sub_type = 1, content = {
					{who=2, chat="我说的第一句"},
					{who=1, chat="npc说的第一句"}, 
				},
			},
		},
	},
}

return config