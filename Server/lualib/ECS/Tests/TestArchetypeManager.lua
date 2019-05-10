local ECS = require "ECS"
TestArchetypeManager = ECS.BaseClass(require("TestBaseClass"))

function TestArchetypeManager:TestCreateArchetypeOne(  )
    local test_comp_name = "EcsTestData"
	local EcsTestData = {value="number"}
	ECS.TypeManager.RegisterType(test_comp_name, EcsTestData)

	local archetype = self.m_Manager:CreateArchetype({test_comp_name})
    lu.assertNotNil(archetype)
    archetype = archetype.Archetype

    lu.assertNotNil(archetype)
    lu.assertEquals(archetype.NumSharedComponents, 0)
    lu.assertEquals(archetype.TypesCount, 2)
    lu.assertEquals(archetype.EntityCount, 0)
    lu.assertEquals(#archetype.Types, 2)

    --test get existing archetype
    local _ArchetypeMgr = self.m_Manager:GetArchetypeManager()
    local CachedComponentTypeInArchetypeArray = {}
    CachedComponentTypeInArchetypeArray[1] = ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(ECS.Entity.Name))
    ECS.SortingUtilities.InsertSorted(CachedComponentTypeInArchetypeArray, 2, ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(test_comp_name)))
    local existing_arche = _ArchetypeMgr:GetExistingArchetype(CachedComponentTypeInArchetypeArray, 2)
    lu.assertEquals(existing_arche, archetype)

end

function TestArchetypeManager:TestCreateArchetypeTwoComp(  )
    local type_info_one = ECS.TypeManager.RegisterType("EcsTestDataOne", {n="number", b="boolean"})
    local type_info_two = ECS.TypeManager.RegisterType("EcsTestDataTwo", {x="integer", y="number"})
    local entity_type_info = ECS.TypeManager.GetTypeInfoByName(ECS.Entity.Name)

    local archetype = self.m_Manager:CreateArchetype({"EcsTestDataOne", "EcsTestDataTwo"})
    lu.assertNotNil(archetype)
    --创建的Archetype与传入的组件顺序无关
    local archetype2 = self.m_Manager:CreateArchetype({"EcsTestDataTwo", "EcsTestDataOne"})
    lu.assertEquals(archetype, archetype2)

    archetype = archetype.Archetype
    lu.assertNotNil(archetype)
    lu.assertEquals(archetype.NumSharedComponents, 0)
    lu.assertEquals(archetype.TypesCount, 3)
    lu.assertEquals(archetype.EntityCount, 0)
    lu.assertEquals(#archetype.Types, 3)
    lu.assertEquals(archetype.Types[1].TypeIndex, entity_type_info.TypeIndex)
    lu.assertEquals(archetype.SizeOfs[1], entity_type_info.SizeInChunk)
    lu.assertEquals(archetype.Offsets[1], 0)

    lu.assertEquals(archetype.Types[2].TypeIndex, type_info_one.TypeIndex)
    lu.assertEquals(archetype.SizeOfs[2], ECS.CoreHelper.GetNumberSize()+ECS.CoreHelper.GetBooleanSize())

    lu.assertEquals(archetype.Types[3].TypeIndex, type_info_two.TypeIndex)
    lu.assertEquals(archetype.SizeOfs[3], ECS.CoreHelper.GetIntegerSize()+ECS.CoreHelper.GetNumberSize())
end

-- function TestArchetypeManager:TestGetTypesStr(  )
    -- ArchetypeManager.GetTypesStr
-- end
