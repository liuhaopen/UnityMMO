local EventDispatcher = BaseClass()
local EventSystem = require "Common.EventSystem"
function EventDispatcher:Constructor()
	self.eventSys = EventSystem.New()
end

function EventDispatcher:Bind(type_str, listener_func, owner)
	if owner then
		listener_func = Functor(listener_func, owner)
	end
	return self.eventSys:Bind(type_str, listener_func)
end
function EventDispatcher:UnBind(obj)
	self.eventSys:UnBind(obj)
end
function EventDispatcher:UnBindAll()
	self.eventSys:UnBindAll(self.eventSys)
end

function EventDispatcher:Fire(type_str, ...)
	self.eventSys:Fire(type_str, ...)
end

function EventDispatcher:DelayFire(type_str, ...)
	self.eventSys:DelayFire(type_str, ...)
end

return EventDispatcher