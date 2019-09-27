
local skynet = require "skynet"
local ChatConst = require "game.chat.ChatConst"
local table_insert = table.insert
local succeed = {ret=ErrorCode.Succeed}
local unknow = {ret=ErrorCode.Unknow}
local NORET = {}
local CMD = {}
local this = {}

local getRoleInfo = function ( roleID )
    local info = this.cacheRoleInfos[roleID]
    print("Cat:chat [start:10] ", info, roleID)
    if not info then
        local is_ok, role_info = skynet.call(this.db, "lua", "select_one_by_key", "RoleBaseInfo", "role_id", roleID)
        print("Cat:chat [start:12] ", is_ok, role_info)
        if is_ok then
            info = role_info
            this.cacheRoleInfos[roleID] = info
        end
    end
    return info
end

function CMD.get_handlers()
    local handlers = {}
    for k,v in pairs(CMD) do
        table.insert(handlers, k)
    end
    return handlers
end

function CMD.Chat_GetHistory( user_info, req_data )
    print("Cat:chat [start:11] ", user_info, req_data)
end

function CMD.Chat_Send( user_info, req_data )
    local roleInfo = getRoleInfo(user_info.cur_role_id)
    if not roleInfo then
        return unknow
    end
    local chatInfo = {
        roleID = user_info.cur_role_id,
        name = roleInfo.name,
        career = roleInfo.career,
        vip = roleInfo.vip or 0,
        headID = roleInfo.headID or 0,
        headUrl = roleInfo.headUrl or "",
        bubbleID = roleInfo.bubbleID or 0,
        channel = req_data.channel,
        content = req_data.content,
        extra = req_data.extra,
        time = Time.timeMS,
    }
    local hisy = this.history[req_data.channel] 
    table_insert(hisy, chatInfo)
    table_insert(this.newChatList, chatInfo)
    return succeed
end

function CMD.Chat_GetNew( user_info, req_data )
    local ack = this.ackGetNews[user_info.cur_role_id]
    if not ack then
        this.ackGetNews[user_info.cur_role_id] = skynet.response()
    end
    return NORET
end

local boardcast = function (  )
    
end

local init = function (  )
    this.history = {}
    for i = ChatConst.Channel.World, ChatConst.Channel.Count do
        this.history[i] = {}
    end
    this.newChatList = {}
    this.cacheRoleInfos = {}
    setmetatable(this.cacheRoleInfos, {__mode = "v"})
    this.db = skynet.localname(".GameDBServer")
    this.ackGetNews = {}
end

skynet.start (function ()
    init()
    skynet.fork(function()
		while true do
			boardcast()
			skynet.sleep(50)
		end
	end)
	skynet.dispatch ("lua", function (_, _, command, ...)
        local f = assert(CMD[command])
        if r ~= NORET then
			skynet.retpack(f(...))
		end
	end)
end)
