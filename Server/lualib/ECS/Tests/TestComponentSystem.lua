local ECS = require "ECS"
local TestSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestSystem", TestSystem)

function TestSystem:Constructor(  )
	self.Created = false
end

function TestSystem:OnCreate( )
	self.Created = true
end

function TestSystem:OnDestroyManager(  )
	self.Created = false
end

TestComponentSystem = ECS.BaseClass(require("TestBaseClass"))
	
function TestComponentSystem:TestCreate(  )
	local system = ECS.World.Active:CreateManager("TestSystem")
    lu.assertEquals(system, ECS.World.Active:GetExistingManager("TestSystem"))
    lu.assertTrue(system.Created)
end

function TestComponentSystem:TestCreateAndDestroy(  )
	local system = ECS.World.Active:CreateManager("TestSystem")
    ECS.World.Active:DestroyManager("TestSystem")
    lu.assertNotNil(ECS.World.Active:GetExistingManager("TestSystem"))
    lu.assertFalse(system.Created)
end

function TestComponentSystem:TestGetOrCreateManagerReturnsSameSystem(  )
	local system = ECS.World.Active:GetOrCreateManager("TestSystem")
    lu.assertEquals(system, ECS.World.Active:GetOrCreateManager("TestSystem"))
end

function TestComponentSystem:TestCreateTwoSystemsOfSameType()
	local systemA = ECS.World.Active:CreateManager("TestSystem")
    local systemB = ECS.World.Active:CreateManager("TestSystem")
    -- CreateManager makes a new manager
    lu.assertTrue(systemA~=systemB)
    -- Return first system
    lu.assertEquals(systemA, ECS.World.Active:GetOrCreateManager("TestSystem"))
end

local EmptySystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("EmptySystem", EmptySystem)
function TestComponentSystem:TestGetComponentGroup()
    local empty_sys = ECS.World.Active:GetOrCreateManager("EmptySystem")

    ECS.TypeManager.RegisterType("DataForTestGetComponentGroup1", {x=0})
    ECS.TypeManager.RegisterType("DataForTestGetComponentGroup2", {x=0, y=false})
    ECS.TypeManager.RegisterType("DataForTestGetComponentGroup3", {z=false})
	local ro_rw = {"DataForTestGetComponentGroup1", "DataForTestGetComponentGroup2"}
	local rw_rw = {"DataForTestGetComponentGroup1", "DataForTestGetComponentGroup3"}
	local rw = {"DataForTestGetComponentGroup1"}
    
    local ro_rw0_system = empty_sys:GetComponentGroup(ro_rw)
    local rw_rw_system = empty_sys:GetComponentGroup(rw_rw)
    local rw_system = empty_sys:GetComponentGroup(rw)

    lu.assertEquals(ro_rw0_system, empty_sys:GetComponentGroup(ro_rw))
    lu.assertEquals(rw_rw_system, empty_sys:GetComponentGroup(rw_rw))
    lu.assertEquals(rw_system, empty_sys:GetComponentGroup(rw))
    
    lu.assertEquals(3, #empty_sys.m_ComponentGroups)
end

local TestInjectSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestInjectSystem", TestInjectSystem)

function TestInjectSystem:Constructor(  )
	local data = {
		position = "Array:DataForTestInject1",
		flag = "Array:DataForTestInject3",
		len = "Length",
	}
	self:Inject("m_Data", data)
end
function TestInjectSystem:OnUpdate(  )
end
function TestComponentSystem:TestInject(  )
    ECS.TypeManager.RegisterType("DataForTestInject1", {x=0, y=false, z=0})
    ECS.TypeManager.RegisterType("DataForTestInject2", {x=false, b=false})
    ECS.TypeManager.RegisterType("DataForTestInject3", {value=0})
	
    local sys = ECS.World.Active:GetOrCreateManager("TestInjectSystem")
    sys:Update()
    lu.assertNotNil(sys.m_Data)
    lu.assertEquals(sys.m_Data.len, 0)
    lu.assertNil(sys.m_Data.position[1])
    lu.assertNil(sys.m_Data.flag[1])

    local archetype = self.m_Manager:CreateArchetype({"DataForTestInject1", "DataForTestInject2", "DataForTestInject3"})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    sys:Update()
    lu.assertEquals(sys.m_Data.len, 1)
    local pos = sys.m_Data.position[1]
    lu.assertNotNil(pos)
    lu.assertEquals(pos.x, 0)
    lu.assertEquals(pos.y, false)
    lu.assertEquals(pos.z, 0)
    lu.assertNil(sys.m_Data.position[2])
    
    local flag = sys.m_Data.flag[1]
    lu.assertNotNil(flag)
    lu.assertEquals(flag.value, 0)

    -- self.m_Manager:SetComponentData(entity, "DataForTestInject1", {x=1.23, y=true, z=789})
    -- sys.m_Data.flag[1] = {value=456}
    -- 以上两个调用方式是同价的
    self.m_Manager:SetComponentData(entity, "DataForTestInject3", {value=456})
    sys.m_Data.position[1] = {x=1.23, y=true, z=789}

    sys:Update()
    lu.assertEquals(sys.m_Data.len, 1)
    local pos = sys.m_Data.position[1]
    lu.assertNotNil(pos)
    lu.assertEquals(pos.x, 1.23)
    lu.assertEquals(pos.y, true)
    lu.assertEquals(pos.z, 789)

    local flag = sys.m_Data.flag[1]
    lu.assertNotNil(flag)
    lu.assertEquals(flag.value, 456)
end

local TestComponentDataArraySystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestComponentDataArraySystem", TestComponentDataArraySystem)

function TestComponentDataArraySystem:OnCreate(  )
    ECS.ComponentSystem.OnCreate(self)
    self.group = self:GetComponentGroup({"DataForTestComponentDataArray3", "DataForTestComponentDataArray2"})
end
function TestComponentDataArraySystem:OnUpdate(  )
end
function TestComponentSystem:TestComponentDataArray(  )
    ECS.TypeManager.RegisterType("DataForTestComponentDataArray1", {x=0, y=false, z=0})
    ECS.TypeManager.RegisterType("DataForTestComponentDataArray2", {x=false, b=false})
    ECS.TypeManager.RegisterType("DataForTestComponentDataArray3", {value=0})
    
    local sys = ECS.World.Active:GetOrCreateManager("TestComponentDataArraySystem")
    sys:Update()
    lu.assertNotNil(sys.group)
    local entities = sys.group:ToComponentDataArray("DataForTestComponentDataArray3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 0)

    local archetype = self.m_Manager:CreateArchetype({"DataForTestComponentDataArray1", "DataForTestComponentDataArray2", "DataForTestComponentDataArray3"})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    self.m_Manager:SetComponentData(entity, "DataForTestComponentDataArray3", {value=123546})
    self.m_Manager:SetComponentData(entity, "DataForTestComponentDataArray2", {x=false, b=true})
    sys:Update()
    local entities = sys.group:ToComponentDataArray("DataForTestComponentDataArray3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 1)
    local compData = self.m_Manager:GetComponentData(entity, "DataForTestComponentDataArray3")
    lu.assertEquals(entities[1], compData)
    lu.assertEquals(entities[1].value, compData.value)

    local entities = sys.group:ToComponentDataArray("DataForTestComponentDataArray2")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 1)
    local compData = self.m_Manager:GetComponentData(entity, "DataForTestComponentDataArray2")
    lu.assertEquals(entities[1], compData)
    lu.assertEquals(entities[1].x, compData.x)
    lu.assertEquals(entities[1].b, compData.b)

    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    self.m_Manager:SetComponentData(entity, "DataForTestComponentDataArray2", {x=true, b=false})
    self.m_Manager:SetComponentData(entity, "DataForTestComponentDataArray3", {value=53212})
    sys:Update()
    local entities = sys.group:ToComponentDataArray("DataForTestComponentDataArray3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 2)
    local compData = self.m_Manager:GetComponentData(entity, "DataForTestComponentDataArray3")
    lu.assertEquals(entities[2], compData)
    lu.assertEquals(entities[2].value, compData.value)

    local entities = sys.group:ToComponentDataArray("DataForTestComponentDataArray2")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 2)
    local compData = self.m_Manager:GetComponentData(entity, "DataForTestComponentDataArray2")
    lu.assertEquals(entities[2], compData)
    lu.assertEquals(entities[2].x, compData.x)
    lu.assertEquals(entities[2].b, compData.b)
end

local TestEntityArraySystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestEntityArraySystem", TestEntityArraySystem)

function TestEntityArraySystem:OnCreate(  )
    ECS.ComponentSystem.OnCreate(self)
    self.group = self:GetComponentGroup({"DataForTestEntityArray3", "DataForTestEntityArray2"})
end
function TestEntityArraySystem:OnUpdate(  )
end
function TestComponentSystem:TestEntityArray(  )
    ECS.TypeManager.RegisterType("DataForTestEntityArray1", {x=0, y=false, z=0})
    ECS.TypeManager.RegisterType("DataForTestEntityArray2", {x=false, b=false})
    ECS.TypeManager.RegisterType("DataForTestEntityArray3", {value=0})
    
    local sys = ECS.World.Active:GetOrCreateManager("TestEntityArraySystem")
    sys:Update()
    lu.assertNotNil(sys.group)
    local entities = sys.group:ToEntityArray()
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 0)

    local archetype = self.m_Manager:CreateArchetype({"DataForTestEntityArray1", "DataForTestEntityArray2", "DataForTestEntityArray3"})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    sys:Update()
    local entities = sys.group:ToEntityArray()
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 1)
    lu.assertEquals(entities[1], entity)
end

local TestRemoveEntitySystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestRemoveEntitySystem", TestRemoveEntitySystem)

function TestRemoveEntitySystem:OnCreate(  )
    ECS.ComponentSystem.OnCreate(self)
    self.group = self:GetComponentGroup({"DataForTestRemoveEntity3", "DataForTestRemoveEntity2"})
end
function TestRemoveEntitySystem:OnUpdate(  )
end
function TestComponentSystem:TestRemoveEntity(  )
    ECS.TypeManager.RegisterType("DataForTestRemoveEntity1", {x=0, y=false, z=0})
    ECS.TypeManager.RegisterType("DataForTestRemoveEntity2", {x=false, b=false})
    ECS.TypeManager.RegisterType("DataForTestRemoveEntity3", {value=0})
    
    local sys = ECS.World.Active:GetOrCreateManager("TestRemoveEntitySystem")
    sys:Update()
    lu.assertNotNil(sys.group)
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(#entities, 0)

    local archetype = self.m_Manager:CreateArchetype({"DataForTestRemoveEntity1", "DataForTestRemoveEntity2", "DataForTestRemoveEntity3"})
    local archetype2 = self.m_Manager:CreateArchetype({"DataForTestRemoveEntity2", "DataForTestRemoveEntity3"})
    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    self.m_Manager:SetComponentData(entity, "DataForTestRemoveEntity3", {value=123546})
    self.m_Manager:SetComponentData(entity, "DataForTestRemoveEntity2", {x=false, b=true})
    sys:Update()
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 1)
    local compData = self.m_Manager:GetComponentData(entity, "DataForTestRemoveEntity3")
    lu.assertEquals(entities[1], compData)
    lu.assertEquals(entities[1].value, compData.value)
    --delete entity
    self.m_Manager:DestroyEntity(entity)
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 0)
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity2")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 0)

    local entity = self.m_Manager:CreateEntityByArcheType(archetype)
    self.m_Manager:SetComponentData(entity, "DataForTestRemoveEntity2", {x=true, b=false})
    self.m_Manager:SetComponentData(entity, "DataForTestRemoveEntity3", {value=53212})
    sys:Update()
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 1)
    local compData = self.m_Manager:GetComponentData(entity, "DataForTestRemoveEntity3")
    lu.assertEquals(entities[1], compData)
    lu.assertEquals(entities[1].value, compData.value)

    local entity2 = self.m_Manager:CreateEntityByArcheType(archetype2)
    self.m_Manager:SetComponentData(entity2, "DataForTestRemoveEntity2", {x=false, b=true})
    self.m_Manager:SetComponentData(entity2, "DataForTestRemoveEntity3", {value=123})
    sys:Update()
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 2)
    local compData = self.m_Manager:GetComponentData(entity2, "DataForTestRemoveEntity3")
    local correctMap = {}
    for i=1,entities.Length do
        correctMap[entities[i].value] = true
    end
    lu.assertTrue(correctMap[123])
    lu.assertTrue(correctMap[53212])
    
    -- delete entity
    self.m_Manager:DestroyEntity(entity)
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, 1)
    lu.assertEquals(entities[1].value, 123)

    --create and delete many
    local count = 1234
    local array = self.m_Manager:CreateEntitiesByArcheType(archetype, count)
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, count+1)
    local cutNum = 500
    for i=1,cutNum do
        self.m_Manager:DestroyEntity(array[i])
    end
    local entities = sys.group:ToComponentDataArray("DataForTestRemoveEntity3")
    lu.assertNotNil(entities)
    lu.assertEquals(entities.Length, count+1-cutNum)
end