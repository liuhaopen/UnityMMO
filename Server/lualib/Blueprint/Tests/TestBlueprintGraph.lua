local BP = require("Blueprint")
do return {} end
TestBlueprintGraph = {}

function TestBlueprintGraph:setUp(  )
end

function TestBlueprintGraph:tearDown(  )
end

function TestBlueprintGraph:TestFlowGraph(  )
	local data = {
		--该蓝图的所有节点
		nodes = {
			{
				id = 1,
				type = "BP.Flow.UpdateEvent",
			},
			{
				id = 2,
				type = "BP.Flow.GetVariable",
				arge = {
					key = "value",
					value = 123
				},
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

function TestBlueprintGraph:TestFSMGraph(  )
	local data = {
		--该蓝图的所有节点
		nodes = {
			{
				id = 1,
				type = "BP.FSM.State",
				name = "idle",
			},
			{
				id = 2,
				type = "BP.FSM.ActionState",
				arge = {
					key = "value",
					value = 123
				},
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

function TestBlueprintGraph:TestBTGraph(  )
	local data = {
		--该蓝图的所有节点
		nodes = {
			{
				id = 1,
				type = "BP.BT.ConditionNode",
				arge = {
					
				},
			},
			{
				id = 2,
				type = "BP.BT.Sequencer",
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