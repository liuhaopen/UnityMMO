--[[
key：怪物类型id
belong:归属：1大家都可捡，2最大伤害者，3最后一击者
goods:类型为数组，元素结构为：[1]=道具类型id,[2]=数量,[3]=概率,
--]]
local config_drop = {
	[2000] = {
		belong = 1, goods = {{100000,1,1200}}, 
	},
}

return config_drop