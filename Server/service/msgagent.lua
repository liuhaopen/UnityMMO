local skynet = require "skynet"
local sproto = require "sproto"
local sprotoloader = require "sprotoloader"
local print_r = require "print_r"

skynet.register_protocol {
	name = "client",
	id = skynet.PTYPE_CLIENT,
	unpack = skynet.tostring,
}

local gate
local userid, subid
local c2s_sproto
-- local host
-- local send_request
local CMD = {}

function CMD.login(source, uid, sid, secret)
	-- you may use secret to make a encrypted data stream
	skynet.error(string.format("%s is login", uid))
	gate = source
	userid = uid
	subid = sid
	-- you may load user data from database

	c2s_sproto = sprotoloader.load(1)
	-- host = sprotoloader.load(1):host "package"
	-- send_request = host:attach(sprotoloader.load(2))
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
	skynet.error(string.format("AFK"))
end

function CMD.get(req_data)
	print('Cat:msgagent.lua[CMD.get]')
	print_r(req_data)
	return { result = "ackget" }
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
		
			--收到前端的请求,先解析再分发
			--Cat_Todo : 分发给各模块
			local proto_info = c2s_sproto:query_proto(tag)
			if proto_info and proto_info.name then
				local content = c2s_sproto:request_decode(tag, msg)
				-- print_r(content)
				local f = CMD[proto_info.name]
				if f then
					local response = f(content)
					local ok, response_str = pcall(c2s_sproto.response_encode, c2s_sproto, tag, response)
					if ok then
						skynet.ret(response_str)
					else
						skynet.error("msgagent handle proto failed!", msg)
						skynet.ignoreret()
					end
				end
			else
				skynet.error("recieve wrong proto string!", msg)
				skynet.ignoreret()
			end
	end)
end)
