--场景里的节点状态变更事件及战斗事件管理器
local EventSystem = BaseClass()
local table_insert = table.insert
local table_remove = table.remove
EventSystem.all_event_count = 0

function EventSystem:Constructor()
	self.all_event_dic = {}
	self.bind_id_to_event_id_dic = {}
	self.calling_event_dic = {}
end

local getEvent = function ( self, event_id )
	return self.all_event_dic[event_id]
end

function EventSystem:Bind(event_id, event_func, func_owner)
	if event_id == nil then
		print("Cat:EventSystem [Try to bind to a nil event_id] : ",debug.traceback())
		return
	end
	
	if event_func == nil then
		--故意报错输出调用堆栈
		print("Cat:EventSystem [Try to bind to a nil event_func] : ",debug.traceback())
		return
	end
	if func_owner then
		local origin_func = event_func
		event_func = function ( ... )
			origin_func(func_owner, ...)
		end
	end
	local event_list = self.all_event_dic[event_id]
	if event_list == nil then
		event_list = {}
		self.all_event_dic[event_id] = event_list
	end
	EventSystem.all_event_count = EventSystem.all_event_count + 1
	self.bind_id_to_event_id_dic[EventSystem.all_event_count] = event_id
	event_list[EventSystem.all_event_count] = event_func
	return EventSystem.all_event_count
end

function EventSystem:UnBind(bind_id)
	if bind_id == nil then
		return
	end
	local event_id = self.bind_id_to_event_id_dic[bind_id]
	if event_id then
		local calling_event = self.calling_event_dic[event_id]
		if calling_event ~= nil then
			if calling_event == false then
				calling_event = {}
				self.calling_event_dic[event_id] = calling_event
			end
			calling_event[bind_id] = true
			local event_list = getEvent(self, event_id)
			if event_list then
				event_list[bind_id] = false
			end
			return
		end
		self.bind_id_to_event_id_dic[bind_id] = nil
		local event_list = getEvent(self, event_id)
		if event_list then
			event_list[bind_id] = nil
		end
	end
end

function EventSystem:UnBindAll(is_delete)
	if is_delete then
		self.all_event_dic = nil
		self.bind_id_to_event_id_dic = nil
		self.calling_event_dic = nil	
	else
		self.all_event_dic = {}
		self.bind_id_to_event_id_dic = {}
		self.calling_event_dic = {}
	end
end

--立即触发
function EventSystem:Fire(event_id, ...)
	if event_id == nil then
		print("Cat:EventSystem [Try to call EventSystem:Fire() with a nil event_id] : ",debug.traceback())
		return
	end

	local event_list = getEvent(self, event_id)
	if event_list then
		self.calling_event_dic[event_id] = false
		for bind_id, event_call_back in pairs(event_list) do
			if event_call_back then
				event_call_back(...)
			end
		end
		local calling_event = self.calling_event_dic[event_id]
		self.calling_event_dic[event_id] = nil
		if calling_event then
			for bind_id, _ in pairs(calling_event) do
				EventSystem.UnBind(self, bind_id)
			end
		end
	end
end

return EventSystem