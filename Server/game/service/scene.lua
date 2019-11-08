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

function CMD.scene_enter_to( user_info, req_data )
	local world = skynet.uniqueservice("world")
	local result = skynet.call(world, "lua", "change_to_scene", user_info, req_data.scene_id, req_data.door_id)
	return {result=result}
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
	-- print('Cat:scene.lua[scene_get_objs_info_change] user_info, role_id', user_info, user_info.cur_role_id, sceneMgr.curSceneID)
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
		local result_code, cd_end_time, fight_event = sceneMgr.fightMgr:CastSkill(role.scene_uid, req_data)
		-- print("Cat:scene [start:355] fight_event, result_code:", fight_event, result_code)
		-- PrintTable(fight_event)
		-- print("Cat:scene [end]")
		return {result=result_code, cd_end_time=cd_end_time, skill_id=req_data.skill_id}
	else
		skynet.error("error : cannot find role by id : "..(user_info.role_id or " nil"))
		return {result=1}
	end
end

function CMD.scene_listen_skill_event(user_info, req_data)
	local role_info = sceneMgr.roleMgr:GetRole(user_info.cur_role_id)
	if role_info and not role_info.ack_scene_listen_skill_event then
		--synch info at fixed time
		role_info.ack_scene_listen_skill_event = skynet.response()
		return NORET
	end
	return {}
end

function CMD.scene_listen_hurt_event( user_info, req_data )
	local role_info = sceneMgr.roleMgr:GetRole(user_info.cur_role_id)
	if role_info and not role_info.ack_scene_listen_hurt_event then
		--synch info at fixed time
		role_info.ack_scene_listen_hurt_event = skynet.response()
		return NORET
	end
	return {}
end

function CMD.scene_relive( user_info, req_data )
	local ret = sceneMgr.roleMgr:Relive(user_info.cur_role_id, req_data.relive_type)
	return {result=ret, relive_type=req_data.relive_type}
end

function CMD.change_attr( role_id, attr_list )
	sceneMgr.roleMgr:ChangeAttr(user_info.cur_role_id, attr_list)
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