local skynet = require "skynet"

local CMD = {}
local scene_services = {}
local online_role = {}
local scene_ids = {1001, 2001}

function CMD.role_enter_game( user_info, role_id )
	if online_role[role_id] ~= nil then
		skynet.error(string.format("multiple login detected, role_id %d", role_id))
		-- CMD.kick(role_id)
	end
	online_role[role_id] = user_info
	skynet.error(string.format("role_id(%d) enter world", role_id))

	--查数据库该角色上次在线时的场景id
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_ok, role_info = skynet.call(gameDBServer, "lua", "select_by_key", "RoleBaseInfo", "role_id", role_id)
	role_info = role_info and role_info[1]
	local scene_id
	if is_ok and role_info then
		scene_id = role_info.scene_id
	end
		
	local scene = scene_services[scene_id]
	if not scene then
		--该场景不存在,默认进入新手村场景
		scene = scene_services[scene_ids[2]]
	end
	user_info.scene_service = scene
	skynet.call(scene, "lua", "role_enter_scene", role_id, user_info.agent)
	return 1
end

function CMD.role_leave_game(user_info)
	local role_id_in_account 
	--wait for optimization
	for role_id,v in pairs(online_role) do
		if v.user_id == user_info.user_id then
			role_id_in_account = role_id
			break
		end
	end
	-- print('Cat:world.lua[44] role_id_in_account', role_id_in_account, online_role[role_id_in_account])
	if role_id_in_account and online_role[role_id_in_account] then
		skynet.call(online_role[role_id_in_account].scene_service, "lua", "role_leave_scene", role_id_in_account)
	 	online_role[role_id_in_account] = nil
	 end
end

function CMD.get_role_scene_service(role_id)
	return online_role[role_id] and online_role[role_id].scene_service
end

function CMD.change_to_scene( role_id, scene_id )
	
end

skynet.start (function ()
	local self = skynet.self()

	--初始化所有场景服务
	for i=1,2 do
		local scene_service = skynet.newservice("scene", self)
		skynet.call(scene_service, "lua", "init", scene_ids[i])
		scene_services[scene_ids[i]] = scene_service
	end
	
	skynet.dispatch ("lua", function (_, _, command, ...)
		local f = assert(CMD[command])
		skynet.retpack(f(...))
	end)
end)
