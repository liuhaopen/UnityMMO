TestEntityManager = BaseClass(require("TestBaseClass"))

function TestEntityManager:TestEntityGSetComponent(  )
    local test_compponent_name = "EcsTestData"
	local EcsTestData = {x="number", y="number", z="number"}
	ECS.TypeManager.RegisterType(test_compponent_name, EcsTestData)

	local archetype = self.m_Manager:CreateArchetype({test_compponent_name})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    lu.assertNotNil(entity)
    local comp_data = self.m_Manager:GetComponentData(entity, test_compponent_name)
    lu.assertEquals(comp_data.x, 0)
    lu.assertEquals(comp_data.y, 0)
    lu.assertEquals(comp_data.z, 0)
    self.m_Manager:SetComponentData(entity, test_compponent_name, {x=1, y=2, z=3})
    comp_data = self.m_Manager:GetComponentData(entity, test_compponent_name)
    lu.assertEquals(comp_data.x, 1)
    lu.assertEquals(comp_data.y, 2)
    lu.assertEquals(comp_data.z, 3)

    local entity2 = self.m_Manager:CreateEntityByArcheType(archetype)
    lu.assertNotNil(entity2)
    local comp_data2 = self.m_Manager:GetComponentData(entity2, test_compponent_name)
    lu.assertEquals(comp_data2.x, 0)
    lu.assertEquals(comp_data2.y, 0)
    lu.assertEquals(comp_data2.z, 0)
    self.m_Manager:SetComponentData(entity2, test_compponent_name, {x=4.412376453451, y=5.000000000001, z=6})
    comp_data2 = self.m_Manager:GetComponentData(entity2, test_compponent_name)
    lu.assertEquals(comp_data2.x, 4.412376453451)
    lu.assertEquals(comp_data2.y, 5.000000000001)
    lu.assertEquals(comp_data2.z, 6)
end

function TestEntityManager:TestEntityAddComponent(  )
    local test_compponent_name = "EcsTestData"
    local test_compponent_name_two = "EcsTestDataTwo"
    local EcsTestData = {x="number", y="number", z="number"}
    ECS.TypeManager.RegisterType(test_compponent_name, EcsTestData)
    ECS.TypeManager.RegisterType(test_compponent_name_two, {value="number"})

    local archetype = self.m_Manager:CreateArchetype({test_compponent_name})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    lu.assertNotNil(entity)
    self.m_Manager:AddComponent(entity, test_compponent_name_two)
    local comp_data = self.m_Manager:GetComponentData(entity, test_compponent_name_two)
    lu.assertEquals(comp_data.value, 0)
    self.m_Manager:SetComponentData(entity, test_compponent_name_two, {value=321})
    comp_data = self.m_Manager:GetComponentData(entity, test_compponent_name_two)
    lu.assertEquals(comp_data.value, 321)
end

function TestEntityManager:TestIncreaseEntityCapacity(  )
	local EcsTestData = {value="number"}
	ECS.TypeManager.RegisterType("EcsTestData", EcsTestData)

	local archetype = self.m_Manager:CreateArchetype({"EcsTestData"})
    local count = 1024
    local array = self.m_Manager:CreateEntitiesByArcheType(archetype, count)
    for i=1,count do
        lu.assertEquals(array[i].Index, i)
    end
end