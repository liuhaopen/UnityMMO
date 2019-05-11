# LuaECS
Unity ECS框架Entities的Lua实现  
对实现细节有兴趣的可以看我对UnityECS的框架源码分析：https://blog.csdn.net/yudianxia/column/info/31641   
Lua版本的话基本上和Unity实现是一样的，除了数据的存放方式，在早期版本试过直接开辟一段内存然后把组件的数据  

# 用例
```  
local ECS = require "ECS"
--ComponentType
ECS.TypeManager.RegisterType("ECS.CustomCom", {x=0, y="", z=false})

--Entity
local entity_mgr = ECS.World.Active:GetOrCreateManager(ECS.EntityManager.Name)
local archetype = entity_mgr:CreateArchetype({"ECS.CustomCom"})
local entity = entity_mgr:CreateEntityByArcheType(archetype)
entity_mgr:SetComponentData(entity, "ECS.CustomCom", {x=1.1, y="hello", z=true, tbl={a=1,b=false}})
local comp_data = entity_mgr:GetComponentData(entity, "ECS.CustomCom")

--ComponentSystem
local TestInjectSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestInjectSystem", TestInjectSystem)
function TestInjectSystem:OnCreateManager(  )
	ECS.ComponentSystem.OnCreateManager(self)
	self.group = self:GetComponentGroup({"ECS.CustomCom"})
end
function TestInjectSystem:OnUpdate(  )
	local comps = self.group:GetComponentDataArray("ECS.CustomCom")
	for i=1,comps.Length do
		local comp = comps[i]
		if comp.tbl.b then
			do some thing...
		end
	end
end
```  

# 测试
可以在windows或linux上运行测试用例，Lua5.2以上都是可以的 :  
lua ./Tests/test_all.lua -v  

# Todo
)整理目录结构  
)增加ShardComponent  
)命名规则改成lua风格  
)安全相关（为指针作边界判定）  
)支持多线程？  
