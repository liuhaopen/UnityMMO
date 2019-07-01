local BP = require("Blueprint")

TestGraphsOwner = {}

function TestGraphsOwner:setUp(  )
end

function TestGraphsOwner:tearDown(  )
end

function TestGraphsOwner:TestGraphsOwnerFSM(  )
	do return end
	local sampleClass = require("Blueprint.Tests.FSMSampleState")
	BP.TypeManager:RegisterType("Blueprint.State.TestGraphsOwnerFSMState", sampleClass)

	local data = {
		nodes = {
			{
				id = 1,
				type = "Blueprint.State.FSMSampleState",
				name = "PatrolState",
			},
			{
				id = 2,
				type = "Blueprint.State.FSMSampleState",
				name = "FightState",
			},
		},
	}
	local graph = BP.FSM.FSMGraph.Create(data)
    lu.assertNotNil(graph)
    lu.assertEquals(TableSize(graph.nodes), 2)
    
    local owner = BP.GraphsOwner.Create()
    lu.assertNotNil(owner)
    owner:AddGraph(graph)
    owner:Start()
    owner:Update(1)
end

return TestGraphsOwner