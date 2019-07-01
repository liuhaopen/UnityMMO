local skynet = require "skynet"
local SceneMgr = require "game.scene.SceneMgr"

local NORET = {}
local CMD = {}
local sceneMgr = SceneMgr.New()

function CMD.init(scene_id)
	sceneMgr:Init(scene_id)
end

function CMD.role_enter_scene(roleID, agent)
	sceneMgr.roleMgr:RoleEnter(roleID, agent)
end

function CMD.role_leave_scene(roleID)
	sceneMgr.roleMgr:RoleLeave(roleID)
end

function CMD.scene_get_role_look_info( user_info, req_data )
	return sceneMgr.roleMgr:GetRoleLooksInfo(req_data.uid)
end

function CMD.scene_get_main_role_info( user_info, req_data )
	-- local result = skynet.call(user_info.agent, "lua", "execute", "Task", "NPCHasTask", 11, 22)
	-- print('Cat:scene.lua[29] result', result)
	return sceneMgr.roleMgr:GetMainRoleInfo(user_info.cur_role_id)
end

function CMD.scene_walk( user_info, req_data )
	return sceneMgr.roleMgr:RoleWalk(user_info.cur_role_id, req_data)
end

function CMD.scene_get_objs_info_change( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_objs_info_change] user_info, role_id', user_info, user_info.cur_role_id)
	local role_info = sceneMgr.roleMgr.roleList[user_info.cur_role_id]
	if role_info and not role_info.ack_scene_get_objs_info_change then
		--synch info at fixed time
		role_info.ack_scene_get_objs_info_change = skynet.response()
		return NORET
	end
	return {}
end

function CMD.scene_cast_skill(user_info, req_data)
	local role = sceneMgr.roleMgr:GetRole(user_info.cur_role_id)
	if role ~= nil then
		local result_code, fight_event = sceneMgr.fightMgr:CastSkill(role.scene_uid, req_data)
		-- print("Cat:scene [start:355] fight_event, result_code:", fight_event, result_code)
		-- PrintTable(fight_event)
		-- print("Cat:scene [end]")
		-- do
			--test : 给自己发送自己的技能协议
			-- table.insert(sceneMgr.roleMgr.roleList[user_info.cur_role_id].fight_events_in_around, fight_event)
		-- end
		return {result=result_code, fight_event=fight_event}
	else
		skynet.error("error : cannot find role by id : "..(user_info.role_id or " nil"))
		return {result=1}
	end
end

function CMD.scene_listen_fight_event(user_info, req_data)
	local role_info = sceneMgr.roleMgr:GetRole(user_info.cur_role_id)
	if role_info and not role_info.ack_scene_listen_fight_event then
		--synch info at fixed time
		role_info.ack_scene_listen_fight_event = skynet.response()
		return NORET
	end
	return {}
end

skynet.start(function()
	skynet.dispatch("lua", function(session, source, command, ...)
		local f = assert(CMD[command])
		local r = f(...)
		if r ~= NORET then
			skynet.ret(skynet.pack(r))
		end
	end)
end)