local lu = require('luaunit')

local EcsTestData = {value="number"}
ECS.TypeManager.RegisterType("EcsTestData", EcsTestData)

TestEntityManager = BaseClass(require("TestBaseClass"))

function TestEntityManager:TestIncreaseEntityCapacity(  )
	local archetype = self.m_Manager:CreateArchetype({"EcsTestData"})
    local count = 1024
    local array = {}
    self.m_Manager:CreateEntityByArcheType(archetype, array)
    for i=1,count do
        lu.assertEquals(i, array[i].Index)
    end
end