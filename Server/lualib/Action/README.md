# Action
在实现技能和 buff 系统时，最简单粗暴的方法是为每个技能写一个函数甚至类，这样每个技能间必定会存在大量的重复代码。  
稍好点的方案是为技能分类，比如 a 类是一次性攻击，b 类是持续每间隔 n 秒攻击1次，c 类是把 n 只怪吸过来然后攻击...最后会发现还是分了很多类且很多重复逻辑。  
用上继承吧，按技能的复杂度你肯定是逃不过“鸭嘴兽”效应的，比如你创建了一个动物世界的继承体系：哺乳动物->猪，猫，狗；鸟类->啄木鸟；爬行动物；无脊椎动物等等。但当鸭嘴兽出现时你就蛋疼了，你是该继承哪个类？还是为它创建一个新的分类？   
鲁迅就曾经说过：组合优于继承。许多框架都用此指导思想做了不少高复用的设计，比如 unity 的基于组件开发，cocos 里的 Action 系统。    
具体到 GamePlay 上的有守望先锋里的 Statescript 系统（暴雪自研的脚本系统），虚幻的 blueprint 或 unity 上的 FlowCanvas，NodeCanvas。实质都是把逻辑划分为多个节点，包括流程逻辑如 if,else,for 等都有对应的节点，然后通过节点间的排列组合实现各种逻辑，这样可以极大地提高复用率，而且还可以做个界面进行可视化编程。  
在 MMO 项目弄怪物 AI 时我就想弄一套差不多的，就是 UnityMMO/Server/lualib/Blueprint。但目前还不想在后端里做太多复杂的 AI，而且自由也是有代价的，所以就先抑制住了做这把牛刀的冲动，先用状态机凑合着，所以目前只有一种怪物 AI,等以后需求养肥了再考虑换成行为树。  
但技能系统就不能将就了，前期弄不好，后期就改不动或不想改了。  
所以还是得花点时间好好捋一捋。技能的逻辑大多都不需要像 AI 逻辑那么长，所以应该可以砍掉许多行为树和节点流的功能，我想弄得越简单越好。  
为了和我的 Blueprint 区分，这里的逻辑节点就叫 Action 吧，就是一个 lua table，只要有三个函数：Start, Update, IsDone 就能成为一个 Action。框架内提供了几个 Action 用作粘合剂:  

## Sequence
可以包含 n 个 Action，下面的代码就是先一直调用 Action1 的 Update 函数直到其 IsDone 返回 true，然后就是轮到 Action2 和 Action3，最后 Action3 的 IsDone 也返回 true 后就结束循环了。：  
```  
local Ac = require "Action"  
local action = Ac.Sequence {Action1, Action2, Action3}
while not action:IsDone() do
	action:Update(Time.deltaTime)
end
```  

## Repeat
重复跑某个 Action，下面的代码就是创建一个 Action，将重复 Done Action1 5次：  
```  
local action = Ac.Repeat {5, Action1}  
```  

## Delay
延迟 Action：  
```  
local action = Ac.Sequence {Action1, Ac.Delay{500}, Action2}  
```  

## If
判定 Action，第一参数是个条件 Action（要求有 \__call 元函数的 table，可参与 Random 这个 Action），如果该 Action 返回 true 就运行第二参数传入的 Action，否则跑第三参数的 Action。下面的代码就是30%概率跑 Action1，70%概率跑 Action2：  
```  
local action = Ac.If {Ac.Random{30}, Action1, Action2}  
```  
当然你可以自己定制条件 Action，比如可以做个 CheckAttr 的条件 Action，用于判断角色的某个属性是否大于或小于某个值：
```  
local action = Ac.If {CheckAttr{"hp","<=",10}, Action1, Action2}
```  

# Usage
```  
local Ac = require "Action"  
--该技能仅有两个步骤：PickTarget 选取攻击目标(把结果保存在 targets 里)，Hurt 把目标列表 (targets) 里的角色造成100点伤害  
local skill1 = Sequence { PickTarget{}, Hurt{100} }  
local skillData = { targets = {} }  
skill1:Start(skillData)--从 Start 函数传入要处理的 table，然后所有的 Action 针对它进行某些处理，相当于行为树里的黑板     
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
