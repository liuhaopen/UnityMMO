require "common.util"
local skynet = require "skynet"
require "skynet.manager"

--使用snowflake算法生成唯一id
local start_use_time = 1563482410*100 --注：单位为10毫秒，该值正式使用后就不能改了，因为每次id的时间部分存储的是和此值的差

local this = {
	increase_info = {},
	server_id = 0,
	platform_id = 0,
}
local increase_bit = 11--每10毫秒里自增1024个
local server_bit = 5--16个平台id
local platform_bit = 9--512个服务器id
local time_bit = 38--2199023255552
local left_shift_for_server = increase_bit
local left_shift_for_platform = increase_bit+server_bit
local left_shift_for_time = increase_bit+server_bit+platform_bit
local sequenceMask = -1 ~ (-1 << increase_bit)

local CMD = {}

local get_cur_time = function (  )
	local curTime = math.floor(skynet.time()*100)
	return curTime
end

local test_gen_uid = function (  )
	print('Cat:id_service.lua[start test uid generator]')
	local test_uids = {}
	for i=1,100000 do
		local uid = CMD.gen_uid("goods")
		assert(test_uids[uid]==nil, "repetitive uid!")
		test_uids[uid] = true
	end
	print('Cat:id_service.lua[end test uid generator]')
end

function CMD.open( conf )
	this.server_id = conf.server_id
	this.platform_id = conf.platform_id
	skynet.register(conf.name)
	-- test_gen_uid()
end

local tilNextTime = function ( lastTimestamp )
	local timestamp = get_cur_time()
    while timestamp <= lastTimestamp do
        timestamp = get_cur_time()
    end
    return timestamp
end

local init_increase_info = function ( type_name )
	local curTime = get_cur_time()
	local deltaTime = curTime-start_use_time
	this.increase_info[type_name] = {deltaTime=deltaTime, value=0}
	return this.increase_info[type_name]
end

function CMD.gen_uid( type_name )
	type_name = type_name or "empty string"
	local increase_info = this.increase_info[type_name]
	if not increase_info then
		increase_info = init_increase_info(type_name)
	end
	local curTime = get_cur_time()
	local deltaTime = curTime - start_use_time
	if deltaTime == increase_info.deltaTime then
		increase_info.value = (increase_info.value + 1) & sequenceMask
		if increase_info.value == 0 then
			--自增序列用满了，阻塞到下一毫秒
			curTime = tilNextTime(curTime)
			deltaTime = curTime - start_use_time
			increase_info.deltaTime = deltaTime
		end
	elseif deltaTime < increase_info.deltaTime then
		skynet.error("gen uid error : time back?")
		return
	else
		increase_info.value = 0
		increase_info.deltaTime = deltaTime
	end
	local increase_num = increase_info.value
	
	local uid = (deltaTime<<left_shift_for_time)|(this.platform_id<<left_shift_for_platform)|(this.server_id<<left_shift_for_server)|increase_num
	-- print('Cat:id_service.lua[30] uid', uid, deltaTime, this.platform_id, this.server_id, increase_num)
	return uid
end

skynet.start (function ()
	skynet.dispatch ("lua", function (_, _, command, ...)
		local f = assert(CMD[command])
		skynet.retpack(f(...))
	end)
end)
