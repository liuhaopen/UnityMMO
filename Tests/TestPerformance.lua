local ECS = require "ECS"
TestPerformance = ECS.BaseClass(require("TestBaseClass"))

local testTimes = 200000
print('Cat:TestPerformance.lua[5] pure testTimes', testTimes)

local normal_lua_cost = function (  )
	local bt = os.clock()
	local test_tbl = {}
	local table_insert = table.insert
	for i=1,testTimes do
		test_tbl[i] = {}
	end
	local writeCost = os.clock() - bt
	print('\nnormal lua write cost : ', writeCost)

	local bt = os.clock()
	for i=1,testTimes do
		test_tbl[i] = {x=1.123456, y=true, z="123"}
		local a = test_tbl[i]
	end
	local readCost = os.clock() - bt
	print('normal lua read cost : ', readCost)
end

function TestPerformance:TestMany(  )
	normal_lua_cost()

	local bt = os.clock()
	ECS.TypeManager.RegisterType("DataForTestPerformance1", {x=0, y=false, z=""})
    ECS.TypeManager.RegisterType("DataForTestPerformance2", {x=false, b=false})
    ECS.TypeManager.RegisterType("DataForTestPerformance3", {value=0})

    local archetype = self.m_Manager:CreateArchetype({"DataForTestPerformance1", "DataForTestPerformance2", "DataForTestPerformance3"})
    local entities = {}
    for i=1,testTimes do
    	entities[i] = self.m_Manager:CreateEntityByArcheType(archetype)
    end
	local onWriteCost = os.clock() - bt
	print('ecs write cost : ', onWriteCost)

	local bt = os.clock()
	for i=1,testTimes do
    	self.m_Manager:SetComponentData(entities[i], "DataForTestPerformance1", {x=1.123456, y=true, z="123"})
    	local a = self.m_Manager:GetComponentData(entities[i], "DataForTestPerformance1")
	end
	local readCost = os.clock() - bt
	print('ecs read cost : ', readCost)
end	
