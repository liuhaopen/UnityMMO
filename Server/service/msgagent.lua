local skynet = require "skynet"
local sproto = require "Common.Util.sproto"
local sprotoloader = require "Common.Util.sprotoloader"
local print_r = require "Common.Util.print_r"
require "common.util"
ErrorCode = require "game.config.ErrorCode"
local Dispatcher = require "game.util.Dispatcher"

skynet.register_protocol {
	name = "client",
	id = skynet.PTYPE_CLIENT,
	unpack = skynet.tostring,
}

local gate
local userid, subid
local user_info
local c2s_sproto
local CMD = {}

local registerAllModule = function( user_info )
	Dispatcher:Init()
	local handlers = {
		"game.account.Account",
		"game.task.Task",
		"game.bag.BagMgr",
		"game.gm.GM",
	}
	for i,v in ipairs(handlers) do
		local handler = require(v)
		if handler then
			Dispatcher:RegisterSprotoHandler(handler)
			if handler.PublicClassName and handler.PublicFuncs then
				Dispatcher:RegisterPublicFuncs(handler.PublicClassName, handler.PublicFuncs)
				if handler.PublicFuncs.Init then
					handler.PublicFuncs.Init(user_info, Dispatcher)
				end
			end
		end
	end
	local services = {
		"chat",
	}
	for i = 1, #services do
		local service = skynet.queryservice(services[i])
		local handler = skynet.call(service, "lua", "get_handlers")
		Dispatcher:RegisterSprotoService(service, handler)
	end
end

function CMD.execute( source, className, funcName, ... )
	local func = Dispatcher:GetPublicFunc(className, funcName)
	if func then
		return func(...)
	end
	return nil
end

function CMD.login(source, uid, sid, secret, platform, server_id)
	-- you may use secret to make a encrypted data stream
	skynet.error(string.format("%s is login", uid))
	gate = source
	userid = uid
	subid = sid
	user_info = {user_id=uid, platform=platform, server_id=server_id, agent=skynet.self()}
	print('Cat:msgagent.lua[50] user_info.agent', user_info.agent)
	-- you may load user data from database
	c2s_sproto = sprotoloader.load(1)
	registerAllModule(user_info)
end

local function logout()
	Dispatcher:CallAllPublicFunc("Logout")
	if gate then
		skynet.call(gate, "lua", "logout", userid, subid)
	end
	skynet.exit()
end

function CMD.logout(source)
	-- NOTICE: The logout MAY be reentry
	skynet.error(string.format("%s is logout", userid))
	logout()
end

function CMD.afk(source)
	-- the connection is broken, but the user may back
	-- skynet.error(string.format("AFK"))
	local world = skynet.uniqueservice ("world")
	skynet.call(world, "lua", "role_leave_game", user_info.cur_role_id)
end

local is_game_play_proto = function ( tag )
	return tag >= 100 and tag <= 199
end

skynet.start(function()
	skynet.dispatch("lua", function(session, source, command, ...)
		local f = assert(CMD[command])
		skynet.ret(skynet.pack(f(source, ...)))
	end)

	skynet.dispatch("client", function(_,_, msg)
		local tag, msg = string.unpack(">I4c"..#msg-4, msg)
			--收到前端的请求,先解析再分发
			local proto_info = c2s_sproto:query_proto(tag)
			if proto_info and proto_info.name then
				local content = c2s_sproto:request_decode(tag, msg)
				-- print_r(content)
				local response
				if is_game_play_proto(tag) then
					local world = skynet.uniqueservice("world")
					-- response = skynet.call(world, "lua", "dispatch", proto_info.name, user_info, content)
					--先取到角色所在场景的服务id
					local scene_service = skynet.call(world, "lua", "get_role_scene_service", user_info.cur_role_id)
					assert(scene_service, "cannot find scene service! req proto name:"..proto_info.name.." tag:"..tag)
					-- print('Cat:msgagent.lua[70] user_info, content', user_info, content)
					response = skynet.call(scene_service, "lua", proto_info.name, user_info, content)
				else
					local handler = Dispatcher:GetSprotoHandler(proto_info.name)
					if handler then
						--Cat_Todo : for temporary
						if tag >= 300 and tag <= 499 then
							response = handler(content)
						else
							response = handler(user_info, content)
						end
					else
						local service = Dispatcher:GetSprotoService(proto_info.name)
						if service then
							response = skynet.call(service, "lua", proto_info.name, user_info, content)
						else
							skynet.error("msgagent handle proto failed! cannot find handler:", proto_info.name)
						end
					end
				end
				local ok, response_str = pcall(c2s_sproto.response_encode, c2s_sproto, tag, response)
				-- print_r(decode_response_str)
				if ok then
					skynet.ret(response_str)
				else
					skynet.error("msgagent handle proto failed!", proto_info.name)
					skynet.ignoreret()
				end
			else
				skynet.error("recieve wrong proto string : ", msg)
				skynet.ignoreret()
			end
	end)
end)
