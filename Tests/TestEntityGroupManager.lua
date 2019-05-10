local ECS = require "ECS"
TestEntityGroupManager = ECS.BaseClass(require("TestBaseClass"))

function TestEntityGroupManager:TestCreateEntityGroup()
	ECS.TypeManager.RegisterType("ECSTestCompType1", {x="number", y="boolean", z="integer"})
	ECS.TypeManager.RegisterType("ECSTestCompType2", {x="integer", y="boolean"})
	ECS.TypeManager.RegisterType("ECSTestCompType3", {x="boolean"})
	ECS.TypeManager.RegisterType("ECSTestCompType4", {x="integer", y="number"})

	local archetype1 = self.m_Manager:CreateArchetype({"ECSTestCompType1", "ECSTestCompType2", "ECSTestCompType4"})
    local entity1_1 = self.m_Manager:CreateEntityByArcheType(archetype1)
    local entity1_2 = self.m_Manager:CreateEntityByArcheType(archetype1)
    local entity1_3 = self.m_Manager:CreateEntityByArcheType(archetype1)

    local archetype2 = self.m_Manager:CreateArchetype({"ECSTestCompType2"})
    local entity2_1 = self.m_Manager:CreateEntityByArcheType(archetype2)
    local entity2_2 = self.m_Manager:CreateEntityByArcheType(archetype2)

    local archetype3 = self.m_Manager:CreateArchetype({"ECSTestCompType2", "ECSTestCompType3"})
    local entity3_1 = self.m_Manager:CreateEntityByArcheType(archetype3)
    local entity3_2 = self.m_Manager:CreateEntityByArcheType(archetype3)
    local entity3_3 = self.m_Manager:CreateEntityByArcheType(archetype3)
    local entity3_4 = self.m_Manager:CreateEntityByArcheType(archetype3)

    local archetype4 = self.m_Manager:CreateArchetype({"ECSTestCompType4", "ECSTestCompType2"})
    local entity4_1 = self.m_Manager:CreateEntityByArcheType(archetype3)

    local group = self.m_Manager:CreateComponentGroup({"ECSTestCompType2", "ECSTestCompType4"})
    lu.assertNotNil(group)
    lu.assertNotNil(group.m_GroupData)
    lu.assertEquals(group.m_GroupData.RequiredComponentsCount, 3)
    lu.assertEquals(#group.m_GroupData.RequiredComponents, 3)
    local entity_type_index = ECS.TypeManager.GetTypeIndexByName(ECS.Entity.Name)
    lu.assertEquals(group.m_GroupData.RequiredComponents[1].TypeIndex, entity_type_index)
    local comp_type_index = ECS.TypeManager.GetTypeIndexByName("ECSTestCompType2")
    lu.assertEquals(group.m_GroupData.RequiredComponents[2].TypeIndex, comp_type_index)
    local comp_type_index2 = ECS.TypeManager.GetTypeIndexByName("ECSTestCompType4")
    lu.assertEquals(group.m_GroupData.RequiredComponents[3].TypeIndex, comp_type_index2)

    lu.assertNotNil(group.m_GroupData.FirstMatchingArchetype)
    lu.assertNotNil(group.m_GroupData.FirstMatchingArchetype.Next)
    lu.assertNil(group.m_GroupData.FirstMatchingArchetype.Next.Next)
    
    lu.assertEquals(group.m_GroupData.FirstMatchingArchetype.Archetype, archetype1.Archetype)
    lu.assertEquals(group.m_GroupData.FirstMatchingArchetype.Next.Archetype, archetype4.Archetype)
end