local BP = require("Blueprint")

TestBlueprintGraph = {}

function TestBlueprintGraph:setUp(  )
end

function TestBlueprintGraph:tearDown(  )
end

function TestBlueprintGraph:TestGraph(  )
	local data = {
		--该蓝图的所有节点
		nodes = {
			{
				id = 1,
				type = "BP.UpdateEvent",
			},
			{
				id = 2,
				type = "BP.Node",
				arge = {name="a", value=3},
			},
		},
		--该蓝图的所有线段，表示了哪两个节点相连
		wires = {
			{
				sourceID = 1,
				sourceSlotName = "out",
				targetID = 2,
				targetSlotName = "in",
			},
		},
	}
	local graph = BP.Graph.Create(data)
    lu.assertNotNil(graph)
    lu.assertEquals(TableSize(graph.nodes), 2)
    graph:Start()

    graph:Update()
end

return TestBlueprintGraph