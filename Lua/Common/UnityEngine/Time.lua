--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
-- added by wsh @ 2017-12-28
-- 注意：
-- 1、已经被修改，别从tolua轻易替换来做升级

local rawget = rawget
local unity_time = CS.UnityEngine.Time

local _Time = 
{	
	deltaTime			= 0,
	fixedDeltaTime 	 	= 0,
	maximumDeltaTime	= 0.3333333,
	fixedTime			= 0,
	frameCount			= 1,	
	-- realtimeSinceStartup= 0,
	time 				= 0,
	timeScale			= 1,
	timeSinceLevelLoad	= 0,
	unscaledDeltaTime	= 0,	
	unscaledTime		= 0,	
	serverTimeMSOnStart	= 0,
	req_time			= 0,
	is_start_synch_time = false,

}

local _set = {}

function _set.fixedDeltaTime(v)
	_Time.fixedDeltaTime = v
	unity_time.fixedDeltaTime = v
end

function _set.maximumDeltaTime(v)
	_Time.maximumDeltaTime = v
	unity_time.maximumDeltaTime = v
end

function _set.timeScale(v)
	_Time.timeScale = v
	unity_time.timeScale = v
end

function _set.captureFramerate(v)
	_Time.captureFramerate = v
	unity_time.captureFramerate = v
end

function _set.timeSinceLevelLoad(v)
	_Time.timeSinceLevelLoad = v
end

_Time.__index = function(t, k)
	local var = rawget(_Time, k)
	
	if var then
		return var
	end

	return unity_time.__index(unity_time, k)	
end

_Time.__newindex = function(t, k, v)
	local func = rawget(_set, k)

	if func then
		return func(v)
	end

	error(string.format("Property or indexer `CS.UnityEngine.Time.%s' cannot be assigned to (it is read only)", k))	
end

local Time = {}
local counter = 1

function Time:SetDeltaTime(deltaTime, unscaledDeltaTime)	
	local _Time = _Time
	_Time.deltaTime = deltaTime	
	_Time.unscaledDeltaTime = unscaledDeltaTime
	counter = counter - 1

	-- if counter == 0 and unity_time then	
		_Time.time = unity_time.time
		_Time.timeSinceLevelLoad = unity_time.timeSinceLevelLoad
		_Time.unscaledTime = unity_time.unscaledTime
		-- _Time.realtimeSinceStartup = unity_time.realtimeSinceStartup
		_Time.frameCount = unity_time.frameCount
		counter = 1000000
	-- else
	-- 	_Time.time = _Time.time + deltaTime
	-- 	_Time.realtimeSinceStartup = _Time.realtimeSinceStartup + unscaledDeltaTime
	-- 	_Time.timeSinceLevelLoad = _Time.timeSinceLevelLoad + deltaTime	
	-- 	_Time.unscaledTime = _Time.unscaledTime + unscaledDeltaTime 
	-- end		
end

--获取服务器时间，单位秒
function Time:GetServerTimeSec( )
	return math.floor((_Time.serverTimeMSOnStart+1000*unity_time.realtimeSinceStartup)/1000+0.5)
end

--获取服务器时间，单位毫秒
function Time:GetServerTime( )
	return math.floor(_Time.serverTimeMSOnStart+unity_time.realtimeSinceStartup*1000+0.5)
end

function Time:SetServerTime( value )
	_Time.serverTimeMSOnStart = value-unity_time.realtimeSinceStartup*1000
end

function Time:SetFixedDelta(fixedDeltaTime)	
	_Time.deltaTime = fixedDeltaTime
	_Time.fixedDeltaTime = fixedDeltaTime

	_Time.fixedTime = _Time.fixedTime + fixedDeltaTime
end

function Time:SetFrameCount()
	_Time.frameCount = _Time.frameCount + 1
end

function Time:SetTimeScale(scale)
	local last = _Time.timeScale
	_Time.timeScale = scale
	unity_time.timeScale = scale
	return last
end

function Time:GetTimestamp()
	return gettime()
end

function Time:StartSynchServerTime(  )
    if not _Time.is_start_synch_time then
        _Time.is_start_synch_time = true
        local synch_time
        synch_time = function()
            _Time.req_time = unity_time.realtimeSinceStartup*1000
            local on_server_time_ack = function ( server_time_info )
            	--从请求至收到回复的时间间隔
            	local time_offset = unity_time.realtimeSinceStartup*1000 - _Time.req_time
                local server_time = server_time_info.server_time+time_offset/2
                Time:SetServerTime(server_time)
                CS.UnityMMO.TimeEx.UpdateServerTime(server_time)
                print('Cat:Time.lua[152] CS.UnityMMO.TimeEx.ServerTime', CS.UnityMMO.TimeEx.ServerTime, server_time)
                local timer = Timer.New(function()
                	--每隔几秒就同步一次
	                synch_time()
				end, 600)
				timer:Start()
            end
            NetDispatcher:SendMessage("account_get_server_time", nil, on_server_time_ack)
        end
        synch_time()
    end
 	--  do --test 
	--     local show_time_func
	--     show_time_func = function()
	--     	local timer = Timer.New(function()
	--     		print('Cat:Time.lua[162] Time.GetServ', Time:GetServerTime(), Time:GetServerTimeSec())
	--             show_time_func()
	-- 		end, 0.5)
	-- 		timer:Start()
	-- 	end
	-- 	show_time_func()
	-- end
end


Time.unity_time = unity_time
CS.UnityEngine.Time = Time
setmetatable(Time, _Time)

if unity_time ~= nil then
	_Time.maximumDeltaTime = unity_time.maximumDeltaTime	
	_Time.timeScale = unity_time.timeScale	
end


return Time