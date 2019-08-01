--[[
-- added by wsh @ 2017-11-28
-- 消息系统
-- 使用范例：
-- local Messenger = require "Framework.Common.Messenger";
-- local TestEventCenter = Messenger.New() --创建消息中心
-- TestEventCenter:AddListener(Type, callback) --添加监听
-- TestEventCenter:AddListener(Type, callback, ...) --添加监听
-- TestEventCenter:Bind(Type, callback) --添加监听
-- TestEventCenter:Fire(Type, ...) --发送消息
-- TestEventCenter:RemoveListener(Type, callback, ...) --移除监听
-- TestEventCenter:Cleanup() --清理消息中心
-- 注意：
-- 1、模块实例销毁时，要自动移除消息监听，不移除的话不能自动清理监听
-- 2、使用弱引用，即使监听不手动移除，消息系统也不会持有对象引用，所以对象的销毁是不受消息系统影响的
-- 3、换句话说：广播发出，回调一定会被调用，但回调参数中的实例对象，可能已经被销毁，所以回调函数一定要注意判空
--]]

local Messenger = BaseClass()

function Messenger:Constructor()
	self.events = {}
end

function Messenger:__delete()
	self.events = nil	
	self.error_handle = nil
end

function Messenger:AddListener(e_type, e_listener, ...)
	local event = self.events[e_type]
	if event == nil then
		event = setmetatable({}, {__mode = "k"})
	end
	
	for k, v in pairs(event) do
		if k == e_listener then
			error("Aready cotains listener : "..tostring(e_listener))
			return
		end
	end
	
	event[e_listener] = setmetatable(SafePack(...), {__mode = "kv"}) 
	self.events[e_type] = event;
end

function Messenger:Bind(e_type, call_back, obj)	
	local handler = nil
	if obj then
		handler = BindCallback(obj, call_back)
	else
		handler = Bind(nil, call_back)
	end
    self:AddListener(e_type, handler)
    return handler
end

function Messenger:Fire(e_type, ...)
	local event = self.events[e_type]
	if event == nil then
		return
	end
	for k, v in pairs(event) do
		assert(k ~= nil)
		local args = ConcatSafePack(v, SafePack(...))
		k(SafeUnpack(args))
	end
end

function Messenger:RemoveListener(e_type, e_listener)
	local event = self.events[e_type]
	if event == nil then
		return
	end

	event[e_listener] = nil
end
Messenger.UnBind = Messenger.RemoveListener

function Messenger:RemoveListenerByType(e_type)
	self.events[e_type] = nil
end

function Messenger:Cleanup()
	self.events = {};
end

return Messenger;