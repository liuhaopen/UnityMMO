--本文件仅作参考，不可运行
local BP = require('Blueprint')

local blueprint = BP.Blueprint.CreateByJsonFile("../monster_ai.bp")
local blueprint = BP.Blueprint.Create({
	--该蓝图的所有节点
	nodes = {
		{
			id = 1,
			type = "UpdateEvent",
		},
		{
			id = 2,
			type = "SetVariable",
			arge = {name="a", value=3},
		},
	},
	--该蓝图的所有线段，表示了哪两个节点相连
	wires = {
		{
			source = 1,
			source_name = "out",
			target = 2,
			target_name = "in",
		},
	},

})

blueprint:Start()
--每帧调用
for i=1,10 do
	blueprint:Update(deltaTime)
end

blueprint:Stop()
