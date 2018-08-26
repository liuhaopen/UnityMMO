local skynet = require "skynet"
require "Common.Util.util"

local CMD = {}
local empty_table = {}

function CMD.init(scene_id)
	print('Cat:scene.lua[init] scene_id', scene_id)
end

function CMD.role_enter_scene(role_id)
	print('Cat:scene.lua[role_enter_scene] role_id', role_id)
end

function CMD.scene_get_main_role_info( user_info, req_data )
	print('Cat:scene.lua[scene_get_main_role_info] user_info, req_data', user_info, user_info.cur_role_id)
	local gameDBServer = skynet.localname(".GameDBServer")
	-- local is_succeed, result = skynet.call(gameDBServer, "lua", "select_by_key", "RoleBaseInfo", "role_id", user_info.cur_role_id)
end

function CMD.scene_walk( user_info, req_data )
	print('Cat:scene.lua[scene_get_main_role_info] user_info, req_data', user_info, user_info.cur_role_id)
	print("Cat:scene [start:22] req_data:", req_data)
	PrintTable(req_data)
	print("Cat:scene [end]")
	return empty_table
end

skynet.start(function()
	skynet.dispatch("lua", function(session, source, command, ...)
		local f = assert(CMD[command])
		skynet.ret(skynet.pack(f(...)))
	end)
end)