local skynet = require "skynet"
require "common.util"

skynet.start(function()
	skynet.error("Server start")
	skynet.uniqueservice("protoloader")
	if not skynet.getenv "daemon" then
		local console = skynet.newservice("console")
	end
	skynet.newservice("debug_console",8000)
	
	local loginserver = skynet.newservice("logind")
	local platform_id = 1
	local server_id = 1
	local gate = skynet.newservice("gated", loginserver, platform_id, server_id)
	skynet.call(gate, "lua", "open" , {
		port = 8888,
		maxclient = 512,
		servername = "DevelopServer",
	})

	--Cat_Todo : 登录服务器需要配置到其它物理机器上
	local dbserver = skynet.newservice("dbserver")
	skynet.call(dbserver, "lua", "open", {
		host = "127.0.0.1",
		port = 3306,
		database = "UnityMMOAccount",
		user = "root",
		password = "123456",
		name = ".AccountDBServer",
	})

	local gamedbserver = skynet.newservice("dbserver")
	skynet.call(gamedbserver, "lua", "open", {
		host = "127.0.0.1",
		port = 3306,
		database = "UnityMMOGame",
		user = "root",
		password = "123456",
		name = ".GameDBServer",
	})

	local id_service = skynet.newservice("id_service")
	skynet.call(id_service, "lua", "open", {
		platform_id = platform_id,
		server_id = server_id,
		name = ".id_service",
	})

	skynet.exit()
end)
