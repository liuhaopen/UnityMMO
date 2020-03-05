local login = require "snax.loginserver"
local crypt = require "skynet.crypt"
local skynet = require "skynet"
require "common.util"

local server = {
	host = "0.0.0.0",
	port = 8001,
	multilogin = false,	-- disallow multilogin
	name = "login_master",
}

local server_list = {}
local user_online = {}
local user_login = {}

function server.auth_handler(token)
	-- the token is base64(user)@base64(server):base64(password)
	local user, server, password = token:match("([^@]+)@([^:]+):(.+)")
	user = crypt.base64decode(user)
	server = crypt.base64decode(server)
	password = crypt.base64decode(password)
	print('Cat:logind.lua[22] user, password', user, password)
	local accountServer = skynet.localname(".AccountDBServer")
	local is_succeed, result = skynet.call(accountServer, "lua", "select_by_key", "Account", "account_id", user)
	if is_succeed then
		local user_info = result and result[1]
		if user_info and user_info.account_id and user_info.password then
			assert(password == user_info.password, "Invalid password")
		elseif server == "DevelopServer" then
			--开发服的话直接创建帐号
			--Cat_Todo : 不要明文保存密码,随便加个固定前后缀再md5都好过明文啦
			skynet.call(accountServer, "lua", "insert", "Account", {account_id=user, password=password})
		end
	else
		--数据库查询失败
	end
	return server, user
end

function server.login_handler(server, uid, secret)
	print(string.format("%s@%s is login, secret is %s", uid, server, crypt.hexencode(secret)))
	local gameserver = assert(server_list[server], "Unknown server")
	-- only one can login, because disallow multilogin
	local last = user_online[uid]
	if last then
		skynet.call(last.address, "lua", "kick", uid, last.subid)
	end
	if user_online[uid] then
		error(string.format("user %s is already online", uid))
	end

	local subid = tostring(skynet.call(gameserver, "lua", "login", uid, secret))
	user_online[uid] = { address = gameserver, subid = subid , server = server}
	return subid
end

local CMD = {}

function CMD.register_gate(server, address)
	server_list[server] = address
end

function CMD.logout(uid, subid)
	local u = user_online[uid]
	if u then
		print(string.format("%s@%s is logout", uid, u.server))
		user_online[uid] = nil
	end
end

function server.command_handler(command, ...)
	local f = assert(CMD[command])
	return f(...)
end

login(server)
