local ECS = require "ECS"
TestEntityManager = ECS.BaseClass(require("TestBaseClass"))

function TestEntityManager:TestIncreaseEntityCapacity(  )
    local EcsTestData = {value=0}
    ECS.TypeManager.RegisterType("DataForTestIncreaseEntityCapacity", EcsTestData)

    local archetype = self.m_Manager:CreateArchetype({"DataForTestIncreaseEntityCapacity"})
    local count = 1024
    local array = self.m_Manager:CreateEntitiesByArcheType(archetype, count)
    for i=1,count do
        lu.assertEquals(array[i].Index, i)
        lu.assertTrue(self.m_Manager:Exists(array[i]))
    end
end

function TestEntityManager:TestEntityGSetComponent(  )
    local test_compponent_name = "DataForTestEntityGSetComponent"
    local test_compponent_name2 = "DataForTestEntityGSetComponent2"
    local test_compponent_name3 = "DataForTestEntityGSetComponent3"
	local EcsTestData = {x=0, y=false, z=0}
    ECS.TypeManager.RegisterType(test_compponent_name, EcsTestData)
    ECS.TypeManager.RegisterType(test_compponent_name2, {value=0})
	ECS.TypeManager.RegisterType(test_compponent_name3, 123)

	local archetype = self.m_Manager:CreateArchetype({test_compponent_name, test_compponent_name3, test_compponent_name2})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    lu.assertNotNil(entity)
    local comp_data = self.m_Manager:GetComponentData(entity, test_compponent_name)
    lu.assertEquals(comp_data.x, 0)
    lu.assertEquals(comp_data.y, false)
    lu.assertEquals(comp_data.z, 0)

    local comp_data2 = self.m_Manager:GetComponentData(entity, test_compponent_name2)
    lu.assertEquals(comp_data2.value, 0)
    local comp_data3 = self.m_Manager:GetComponentData(entity, test_compponent_name3)
    lu.assertEquals(comp_data3, 123)

    self.m_Manager:SetComponentData(entity, test_compponent_name, {x=1.123456, y=true, z=3})
    self.m_Manager:SetComponentData(entity, test_compponent_name2, {value=123456789})
    self.m_Manager:SetComponentData(entity, test_compponent_name3, "456")--组件想换类型存储也行，不过不建议这么用
    comp_data = self.m_Manager:GetComponentData(entity, test_compponent_name)
    lu.assertEquals(comp_data.x, 1.123456)
    lu.assertEquals(comp_data.y, true)
    lu.assertEquals(comp_data.z, 3)
    comp_data2 = self.m_Manager:GetComponentData(entity, test_compponent_name2)
    lu.assertEquals(comp_data2.value, 123456789)
    comp_data3 = self.m_Manager:GetComponentData(entity, test_compponent_name3)
    lu.assertEquals(comp_data3, "456")

    local entity2 = self.m_Manager:CreateEntityByArcheType(archetype)
    lu.assertNotNil(entity2)
    local comp_data2 = self.m_Manager:GetComponentData(entity2, test_compponent_name)
    lu.assertEquals(comp_data2.x, 0)
    lu.assertEquals(comp_data2.y, false)
    lu.assertEquals(comp_data2.z, 0)
    self.m_Manager:SetComponentData(entity2, test_compponent_name, {x=4.412376453451, y=false, z=0})
    comp_data2 = self.m_Manager:GetComponentData(entity2, test_compponent_name)
    lu.assertEquals(comp_data2.x, 4.412376453451)
    lu.assertEquals(comp_data2.y, false)
    lu.assertEquals(comp_data2.z, 0)
end

function TestEntityManager:TestEntityAddComponent(  )
    local test_compponent_name = "DataForTestEntityAddComponent1"
    local test_compponent_name_two = "TestEntityAddComponent2"
    local EcsTestData = {x=0, y=0, z=0}
    ECS.TypeManager.RegisterType(test_compponent_name, EcsTestData)
    ECS.TypeManager.RegisterType(test_compponent_name_two, {value=0})

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

function TestEntityManager:TestEntityRemoveComponent(  )
    local test_compponent_name = "DataForTestEntityAddComponent"
    local EcsTestData = {x=0, y=0, z=0}
    ECS.TypeManager.RegisterType(test_compponent_name, EcsTestData)

    local archetype = self.m_Manager:CreateArchetype({test_compponent_name})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    lu.assertNotNil(entity)
    local has = self.m_Manager:HasComponent(entity, test_compponent_name)
    lu.assertTrue(has)

    self.m_Manager:RemoveComponent(entity, test_compponent_name)
    local has = self.m_Manager:HasComponent(entity, test_compponent_name)
    lu.assertFalse(has)
end


local TestDestroyEntitySystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestDestroyEntitySystem", TestDestroyEntitySystem)

function TestDestroyEntitySystem:OnCreate(  )
    ECS.ComponentSystem.OnCreate(self)
    self.group = self:GetComponentGroup({"DataForTestRemoveEntity"})
end
function TestDestroyEntitySystem:OnUpdate(  )
end
function TestEntityManager:TestRemoveEntity(  )
    local EcsTestData = {value=0}
    ECS.TypeManager.RegisterType("DataForTestRemoveEntity", EcsTestData)

    local archetype = self.m_Manager:CreateArchetype({"DataForTestRemoveEntity"})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    lu.assertNotNil(entity)
    lu.assertTrue(self.m_Manager:Exists(entity))

    local sys = ECS.World.Active:GetOrCreateManager("TestDestroyEntitySystem")
    sys:Update()
    lu.assertNotNil(sys.group)
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 1)

    self.m_Manager:DestroyEntity(entity)
    lu.assertFalse(self.m_Manager:Exists(entity))
    sys:Update()
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity")
    lu.assertEquals(entities.Length, 0)
    
    local count = 1024
    local array = self.m_Manager:CreateEntitiesByArcheType(archetype, count)
    for i=1,count do
        lu.assertEquals(array[i].Index, i)
        lu.assertTrue(self.m_Manager:Exists(array[i]))
        self.m_Manager:DestroyEntity(array[i])
        lu.assertFalse(self.m_Manager:Exists(array[i]))
    end
end