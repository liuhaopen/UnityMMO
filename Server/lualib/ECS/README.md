# LuaECS
Unity ECS 框架 Entities 的 Lua 实现  
对实现细节有兴趣的可以看我对 UnityECS 的框架源码分析：https://blog.csdn.net/yudianxia/column/info/31641   
Lua 版本的话基本上和 Unity 实现是一样的，除了数据的存放方式，在早期版本试过直接开辟一段内存然后把组件的数据逐字段地写入，读取时计算下该字段的指针偏移就可以了，但 lua 和 c 的交互消耗比连续存放数据带来的优化更大，所以就放弃了该方案，现在改成直接存放 table了。  

# 用例
```  
local ECS = require "ECS"
--ComponentType
ECS.TypeManager.RegisterType("ECS.CustomCom", {x=0, y="", z=false})

--Entity
local entityMgr = ECS.World.Active:GetOrCreateManager(ECS.EntityManager.Name)
local archetype = entityMgr:CreateArchetype({"ECS.CustomCom"})
local entity = entityMgr:CreateEntityByArcheType(archetype)
entityMgr:SetComponentData(entity, "ECS.CustomCom", {x=1.1, y="hello", z=true, tbl={a=1,b=false}})
local comp_data = entityMgr:GetComponentData(entity, "ECS.CustomCom")

--ComponentSystem
local TestInjectSystem = ECS.BaseClass(ECS.ComponentSystem)
ECS.TypeManager.RegisterScriptMgr("TestInjectSystem", TestInjectSystem)
function TestInjectSystem:OnCreate(  )
	ECS.ComponentSystem.OnCreate(self)
	self.group = self:GetComponentGroup({"ECS.CustomCom"})
end
function TestInjectSystem:OnUpdate(  )
	local comps = self.group:ToComponentDataArray("ECS.CustomCom")
	for i=1,comps.Length do
		local comp = comps[i]
		if comp.tbl.b then
			do some thing...
		end
	end
end
```  

# 测试
可以在 windows 或 linux 上运行测试用例，Lua5.2以上都是可以的 :  
lua ./Tests/test_all.lua -v  

# Todo
)增加 ShardComponent  
)支持多线程？  
)System 根据 UpdateBefore，After 等排序  
