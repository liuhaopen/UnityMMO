local ECS = require "ECS"
local TestBaseClass = ECS.BaseClass()

function TestBaseClass:Constructor(  )
	
end

function TestBaseClass:setUp(  )
	-- print('Cat:TestBaseClass.lua[setUp]')
	self.m_PreviousWorld = ECS.World.Active
    ECS.World.Active = ECS.World.New("Test World")
    self.m_World = ECS.World.Active

    self.m_Manager = self.m_World:GetOrCreateManager("ECS.EntityManager")
    -- m_ManagerDebug = new EntityManager.EntityManagerDebug(self.m_Manager)
end

function TestBaseClass:tearDown(  )
	-- print('Cat:TestBaseClass.lua[tearDown]')
	if (m_Manager ~= nil) then
        -- self.m_World:Delete()
        self.m_World = nil
        ECS.World.Active = self.m_PreviousWorld
        self.m_PreviousWorld = nil
        self.m_Manager = nil
	end
end

return TestBaseClass