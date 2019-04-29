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
				source = 1,
				source_name = "out",
				target = 2,
				target_name = "in",
			},
		},
	}
	local graph = BP.Graph.Create(data)
    lu.assertNotNil(graph)

    graph:Start()

    graph:Update()
end

return TestBlueprintGraph