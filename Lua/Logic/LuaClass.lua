--Author : Administrator
--Date   : 2014/11/25

--声明，这里声明了类名还有属性，并且给出了属性的初始值。
LuaClass = {x = 0, y = 0}

--这句是重定义元表的索引，就是说有了这句，这个才是一个类。
LuaClass.__index = LuaClass

--构造体，构造体的名字是随便起的，习惯性改为New()
function LuaClass:New(x, y) 
    local self = {};    --初始化self，如果没有这句，那么类所建立的对象改变，其他对象都会改变
    setmetatable(self, LuaClass);  --将self的元表设定为Class
    self.x = x;
    self.y = y;
    return self;    --返回自身
end

--测试打印方法--
function LuaClass:test() 
    logWarn("x:>" .. self.x .. " y:>" .. self.y);
end

--endregion
