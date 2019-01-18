# LuaECS
Unity ECS框架Entities的Lua实现，特点是保证组件信息尽量存放在连续内存中，从而提高缓存命中率，所以我用Lua实现时，组件信息都平坦地保存在名为Chunk的userdata里，引用时根据偏移算出指针地址进行读写。  

# 用例
```  
--ComponentType
ECS.TypeManager.RegisterType("ECS.Position", {x="number", y="number", z="integer"})

--Entity
local entity_mgr = ECS.World.Active:GetOrCreateManager(ECS.EntityManager.Name)
local archetype = entity_mgr:CreateArchetype({"ECS.Position"})
local entity = entity_mgr:CreateEntityByArcheType(archetype)
entity_mgr:SetComponentData(entity, "ECS.Position", {x=1.1, y=2.2, z=3})
local comp_data = entity_mgr:GetComponentData(entity, "ECS.Position")

--ComponentSystem
local TestInjectSystem = BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestInjectSystem", TestInjectSystem)
function TestInjectSystem:Constructor(  )
	local data = {
		position = "Array:ECS.Position",
		flag = "Array:FlagType",
		len = "Length",
	}
	self:Inject("m_Data", data)
end
function TestInjectSystem:OnUpdate(  )
	for i=1,self.m_Data.len do
		local pos = self.m_Data.position[i]
		do some thing
	end
end
```  

# 测试
可以在windows或linux上运行测试用例 :  
lua ./Tests/test_all.lua -v  

# Todo
)增加ShardComponent
)增加EntityArray
)命名规则改成lua风格
)支持多线程？
