local skynet = require "skynet"
require "util.util"

skynet.start(function()
	skynet.error("Server start")
	skynet.uniqueservice("protoloader")
	if not skynet.getenv "daemon" then
		local console = skynet.newservice("console")
	end
	skynet.newservice("debug_console",8000)
	
	local loginserver = skynet.newservice("logind")
	local gate = skynet.newservice("gated", loginserver)
	skynet.call(gate, "lua", "open" , {
		port = 8888,
		maxclient = 64,
		servername = "sample",
	})

	-- local dbserver = skynet.newservice("dbserver")
	-- print('Cat:main.lua[17] dbserver', dbserver)
	-- skynet.call(dbserver, "lua", "open", {
	-- 	host = "127.0.0.1",
	-- 	port = 3306,
	-- 	database = "UnityMMOAccount",
	-- 	user = "root",
	-- 	password = "123456",
	-- })
	
	skynet.exit()
end)