local skynet = require "skynet"

local CMD = {}
local scene_services = {}
local online_role = {}
local scene_ids = {1001, 2001}

function CMD.role_enter_game( user_info, role_id )
	if online_role[role_id] ~= nil then
		skynet.error(string.format("multiple login detected, role_id %d", role_id))
		CMD.kick(role_id)
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
		scene = scene_services[scene_ids[1]]
	end
	user_info.scene_service = scene
	skynet.call(scene, "lua", "role_enter_scene", role_id, user_info.agent)
	return 1
end

function CMD.role_leave_game(role_id)
	-- print('Cat:world.lua[44] role_id_in_account', role_id_in_account, online_role[role_id_in_account])
	if role_id and online_role[role_id] then
		skynet.call(online_role[role_id].scene_service, "lua", "role_leave_scene", role_id)
	 	online_role[role_id] = nil
	end
end

function CMD.get_role_scene_service(role_id)
	return online_role[role_id] and online_role[role_id].scene_service
end

function CMD.change_to_scene( user_info, scene_id, door_id )
	local role_id = user_info.cur_role_id
	local result = 1
	local lastScene = role_id and online_role[role_id] and online_role[role_id].scene_service
	local newScene = scene_services[scene_id]
	if lastScene == newScene then
		result = 3
	else
		if newScene then
			user_info.scene_service = newScene
			online_role[role_id] = user_info
			result = 0
		else
			result = 2
		end
		if lastScene then
			skynet.call(lastScene, "lua", "role_leave_scene", role_id)
		end
		if newScene then
			skynet.call(newScene, "lua", "role_enter_scene", role_id, user_info.agent)
		else
		end
	end
	return result
end

-- function CMD.dispatch( proto_name, user_info, req_data )
-- 	--先取到角色所在场景的服务id
-- 	local scene_service = CMD.get_role_scene_service(user_info.cur_role_id)
-- 	assert(scene_service, "cannot find scene service! req proto name:"..proto_name)
-- 	response = skynet.call(scene_service, "lua", proto_name, user_info, req_data)
-- 	return response
-- end

skynet.start (function ()
	local self = skynet.self()

	--init all scene service
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
