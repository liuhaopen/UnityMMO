local skynet = require "skynet"
local sproto = require "Common.Util.sproto"
local sprotoloader = require "Common.Util.sprotoloader"
local print_r = require "Common.Util.print_r"
require "Common.Util.util"
local netdispatcher = require "game.netdispatcher"

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

function CMD.login(source, uid, sid, secret, platform, server_id)
	-- you may use secret to make a encrypted data stream
	skynet.error(string.format("%s is login", uid))
	gate = source
	userid = uid
	subid = sid
	user_info = {user_id=uid, platform=platform, server_id=server_id}
	-- you may load user data from database

	c2s_sproto = sprotoloader.load(1)
end

local function logout()
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
	skynet.call(world, "lua", "role_leave_game", user_info)
end

skynet.start(function()
	-- If you want to fork a work thread , you MUST do it in CMD.login
	skynet.dispatch("lua", function(session, source, command, ...)
		local f = assert(CMD[command])
		skynet.ret(skynet.pack(f(source, ...)))
	end)

	skynet.dispatch("client", function(_,_, msg)
		-- print('Cat:msgagent.lua[50] msg|'..msg.."| c2s_sproto:", c2s_sproto)
		local tag, msg = string.unpack(">I4c"..#msg-4, msg)
			-- print('Cat:msgagent.lua[62] tag:', tag, " msg:", msg)
			--收到前端的请求,先解析再分发
			local proto_info = c2s_sproto:query_proto(tag)
			if proto_info and proto_info.name then
				local content = c2s_sproto:request_decode(tag, msg)
				-- print_r(content)
				local response
				if tag >= 100 and tag <= 199 then
					local world = skynet.uniqueservice("world")
					--先取到角色所在场景的服务id
					local scene_service = skynet.call(world, "lua", "get_role_scene_service", user_info.cur_role_id)
					assert(scene_service, "cannot find scene service! req proto name:"..proto_info.name.." tag:"..tag)
					-- print('Cat:msgagent.lua[70] user_info, content', user_info, content)
					response = skynet.call(scene_service, "lua", proto_info.name, user_info, content)
				else
					local f = netdispatcher:dispatch(tag)
					if f and f[proto_info.name] then
						response = f[proto_info.name](user_info, content)
					else
						skynet.error("msgagent handle proto failed! cannot find handler:", proto_info.name)
					end
				end
				local ok, response_str = pcall(c2s_sproto.response_encode, c2s_sproto, tag, response)
				-- local decode_response_str = c2s_sproto:response_decode(tag, response_str)
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
