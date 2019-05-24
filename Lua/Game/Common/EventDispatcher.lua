local EventDispatcher = BaseClass()
local EventSystem = require "Common.EventSystem"
function EventDispatcher:__init()
	self.eventSys = EventSystem.New()
end

function EventDispatcher:Bind(type_str, listener_func, owner)
	if owner then
		listener_func = Functor(listener_func, owner)
	end
	return EventSystem.Bind(self.eventSys, type_str, listener_func)
end
function EventDispatcher:UnBind(obj)
	EventSystem.UnBind(self.eventSys, obj)
end
function EventDispatcher:UnBindAll()
	EventSystem.UnBindAll(self.eventSys)
end

function EventDispatcher:Fire(type_str, ...)
	EventSystem.Fire(self.eventSys, type_str, ...)
end

function EventDispatcher:DelayFire(type_str, ...)
	EventSystem.DelayFire(self.eventSys, type_str, ...)
end

return EventDispatcher