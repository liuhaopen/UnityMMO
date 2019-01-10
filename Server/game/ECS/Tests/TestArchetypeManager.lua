TestArchetypeManager = BaseClass(require("TestBaseClass"))

function TestArchetypeManager:TestIncreaseEntityCapacity(  )
	local EcsTestData = {value="number"}
	ECS.TypeManager.RegisterType("EcsTestData", EcsTestData)

	local archetype = self.m_Manager:CreateArchetype({"EcsTestData"})
    lu.assertNotNil(archetype)
    archetype = archetype.Archetype
    lu.assertNotNil(archetype)
    lu.assertEquals(archetype.NumSharedComponents, 0)
    lu.assertEquals(archetype.TypesCount, 2)
    lu.assertEquals(archetype.EntityCount, 0)
    lu.assertEquals(#archetype.Types, 2)

end