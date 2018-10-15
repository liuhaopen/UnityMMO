# LuaCocosAction
Lua版本实现的cocos2dx action系统  

使用方法：  
）每帧调用cc.ActionManager:getInstance():Update(deltaTime)并传入帧间隔时间  
）因为大部分action都会调用节点的某些接口，比如MoveTo就是每帧调用节点改变坐标的接口，而不同引擎其接口都不一样的，所以为了action们能够通用就增加了中间层ActionNodeWrapper.lua，为你的引擎实现本文件的所有接口就可以了。   
）像cocos一样使用action就可以了，例子：  
local action = cc.MoveBy.New(2, 100, 50)  
cc.ActionManager:getInstance():addAction(action, node)