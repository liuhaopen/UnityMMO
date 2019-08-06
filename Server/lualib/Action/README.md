# Action
行为树的简化版本，但用法上更像 cocos2dx 的 Action。所有对象都是一个 Action，只具有三个函数：Start, Update, IsDone，可以通过一些特殊的 Action 排列组合，比如 Sequence 就可以包含不限定个 Action，目前我用来实现技能系统。  

# Usage
```  
local Ac = require "Action"  
--该技能仅有两个步骤：PickTarget 选取攻击目标(把结果保存在 targets 里)，Hurt 把目标列表 (targets) 里的角色造成100点伤害  
local skill1 = Sequence { PickTarget{}, Hurt{100} }  
local skillData = { targets = {} }  
skill1:Start(skillData)--从 Start 函数传入要处理的 table，然后所有的 Action 针对它进行某些处理  
skill1:Update()  

--复杂一点的技能如：一共触发5次，每次有20%的概率暴发（伤害500点）并触发 Buff1,另外的80%可能造成100点伤害但没有 Buff,然后延迟1000毫秒后再继续下次循环。  
local skill2 = Repeat { 5, If { Random{2000}, 
		Sequence{ PickTarget{}, Hurt{500}, Buff{1}, Delay{1000} } ,
		Sequence{ PickTarget{}, Hurt{100}, Delay{1000} } ,
	}
}
skill2:Start(skillData)  
while true do  
	--每次 Update 都要传入每帧间隔的时间，用于计算延迟 Action  
	skill2:Update(Time.deltaTime)  
	if skill2:IsDone() then  
		break  
	end  
end  
--上面的 PickTarget、Hurt 和 Buff 三个 Action都是根据项目需要自定义的，各自实现了 Action 三函数,用 Start 函数接收要处理的 table，然后在 Update 函数里添加业务逻辑，因为都是一次性完成的，所以 IsDone 可以直接返回 true。
}  
```  

# Test
可以在windows或linux上运行测试用例，Lua5.2以上都是可以的 :   
lua ./Tests/Test.lua -v  
