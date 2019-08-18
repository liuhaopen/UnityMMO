--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
-- added by wsh @ 2017-12-27
-- 注意：
-- 1、已经被修改，别从tolua轻易替换来做升级

local setmetatable = setmetatable
local xpcall = xpcall
local pcall = pcall
local assert = assert
local rawget = rawget
local error = error
local traceback = debug.traceback
local ilist = ilist

event_err_handle = function(msg)
	error(msg, 2)
end
		
local _pcall = {
	__call = function(self, ...)
		local status, err
		if not self.obj then
			status, err = pcall(self.func, ...)
		else
			status, err = pcall(self.func, self.obj, ...)
		end	
		if not status then
			event_err_handle(err.."\n"..traceback())
		end
		return status
	end,
	
	__eq = function(lhs, rhs)
		return lhs.func == rhs.func and lhs.obj == rhs.obj
	end,
}

local function functor(func, obj)	
	return setmetatable({func = func, obj = obj}, _pcall)			
end

local _event = {}
_event.__index = _event

function _event:CreateListener(func, obj)
	func = functor(func, obj)
	return {value = func, _prev = 0, _next = 0, removed = true}		
end

function _event:AddListener(handle)	
	assert(handle)

	if self.lock then		
		table.insert(self.opList, function() self.list:pushnode(handle) end)		
	else
		self.list:pushnode(handle)
	end	
end

function _event:RemoveListener(handle)	
	assert(handle)	

	if self.lock then		
		table.insert(self.opList, function() self.list:remove(handle) end)				
	else
		self.list:remove(handle)
	end
end

function _event:Count()
	return self.list.length
end	

function _event:Clear()
	self.list:clear()
	self.opList = {}	
	self.lock = false
	self.current = nil
end

_event.__call = function(self, ...)
	local _list = self.list	
	self.lock = true
	local ilist = ilist				

	for i, f in ilist(_list) do
		self.current = i
		if not f(...) then
			_list:remove(i)
			self.lock = false
		end
	end
	
	local opList = self.opList
	self.lock = false

	for i, op in ipairs(opList) do
		op()
		opList[i] = nil
	end
end

function event(name)
	return setmetatable({
		name = name, 
		lock = false, 
		opList = {}, 
		list = list:new(),
	}, _event)
end

UpdateBeat 			= event("Update")
LateUpdateBeat		= event("LateUpdate")
FixedUpdateBeat		= event("FixedUpdate")
--只在协同使用
CoUpdateBeat		= event("CoUpdate")
CoLateUpdateBeat	= event("CoLateUpdate")
CoFixedUpdateBeat 	= event("CoFixedUpdate")

function Update(deltaTime, unscaledDeltaTime)
	Time:SetDeltaTime(deltaTime, unscaledDeltaTime)
	UpdateBeat()
	CoUpdateBeat()
end

function LateUpdate()
	LateUpdateBeat()
	CoLateUpdateBeat()
	Time:SetFrameCount()
end

function FixedUpdate(fixedDeltaTime)
	Time:SetFixedDelta(fixedDeltaTime)
	FixedUpdateBeat()
	CoFixedUpdateBeat()
end