TestArchetypeManager = BaseClass(require("TestBaseClass"))

function TestArchetypeManager:TestCreateArchetypeOne(  )
    local test_comp_name = "EcsTestData"
	local EcsTestData = {value="number"}
	ECS.TypeManager.RegisterType(test_comp_name, EcsTestData)

	local archetype = self.m_Manager:CreateArchetype({test_comp_name})
    lu.assertNotNil(archetype)
    archetype = archetype.Archetype

    local _ArchetypeMgr = self.m_Manager:GetArchetypeManager()
    local CachedComponentTypeInArchetypeArray = {}
    CachedComponentTypeInArchetypeArray[1] = ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(ECS.Entity.Name))
    ECS.SortingUtilities.InsertSorted(CachedComponentTypeInArchetypeArray, 2, ECS.ComponentTypeInArchetype.Create(ECS.ComponentType.Create(test_comp_name)))
    local existing_arche = _ArchetypeMgr:GetExistingArchetype(CachedComponentTypeInArchetypeArray, 2)
    lu.assertEquals(existing_arche, archetype)

    lu.assertNotNil(archetype)
    lu.assertEquals(archetype.NumSharedComponents, 0)
    lu.assertEquals(archetype.TypesCount, 2)
    lu.assertEquals(archetype.EntityCount, 0)
    lu.assertEquals(#archetype.Types, 2)

end

function TestArchetypeManager:TestCreateArchetypeTwoComp(  )
    local type_info_one = ECS.TypeManager.RegisterType("EcsTestDataOne", {value="number"})
    local type_info_two = ECS.TypeManager.RegisterType("EcsTestDataTwo", {x="number", y="number"})

    local archetype = self.m_Manager:CreateArchetype({"EcsTestDataOne", "EcsTestDataTwo"})
    lu.assertNotNil(archetype)
    archetype = archetype.Archetype
    lu.assertNotNil(archetype)
    lu.assertEquals(archetype.NumSharedComponents, 0)
    lu.assertEquals(archetype.TypesCount, 3)
    lu.assertEquals(archetype.EntityCount, 0)
    lu.assertEquals(#archetype.Types, 3)
    lu.assertEquals(archetype.Types[2].TypeIndex, type_info_one.TypeIndex)
    lu.assertEquals(archetype.Types[3].TypeIndex, type_info_two.TypeIndex)
end

-- function TestArchetypeManager:TestGetTypesStr(  )
    -- ArchetypeManager.GetTypesStr
-- end
