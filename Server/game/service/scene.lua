local skynet = require "skynet"
local SceneMgr = require "game.scene.SceneMgr"

local NORET = {}
local CMD = {}
local sceneMgr = SceneMgr.New()

function CMD.init(scene_id)
	sceneMgr:Init(scene_id)
end

function CMD.role_enter_scene(roleID)
	sceneMgr:AddRole(roleID)
end

local save_role_pos = function ( role_id, pos_x, pos_y, pos_z, scene_id )
	local gameDBServer = skynet.localname(".GameDBServer")
	is_succeed = skynet.call(gameDBServer, "lua", "update", "RoleBaseInfo", "role_id", role_id, {pos_x=pos_x, pos_y=pos_y, pos_z=pos_z, scene_id=scene_id})
end

function CMD.role_leave_scene(role_id)
	local role_info = sceneMgr.role_list[role_id]
	print('Cat:scene.lua[role_leave_scene] role_id', role_id, role_info)
	if not role_info then return end
	
	sceneMgr.aoi:remove(role_info.aoi_handle)
	-- sceneMgr.aoi_handle_uid_map[role_info.aoi_handle] = nil --角色离开后还需要通过aoi_handle获取ta的uid
	save_role_pos(role_id, role_info.base_info.pos_x, role_info.base_info.pos_y, role_info.base_info.pos_z, sceneMgr.cur_scene_id)	

	if role_info.ack_scene_get_objs_info_change then
		role_info.ack_scene_get_objs_info_change(true, {})
	end
	sceneMgr.role_list[role_id] = nil
end

function CMD.scene_get_role_look_info( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_role_look_info] user_info, req_data', user_info, user_info.cur_role_id)
	local role_info = sceneMgr.object_list[req_data.uid]
	-- print('Cat:scene.lua[211] role_info', role_info, req_data.uid, sceneMgr.object_list[req_data.uid])
	local looks_info
	local entity = sceneMgr.uid_entity_map[req_data.uid]
	if role_info and entity then
		local hpData = sceneMgr.entityMgr:GetComponentData(entity, "UMO.HP")
		looks_info = {
			result = 0,
			role_looks_info = {
				career = role_info.base_info.career or 1,
				name = role_info.name,
				hp = hpData.cur,
				max_hp = hpData.max,
				body = 0,
				hair = 0,
				weapon = 0,
				wing = 0,
				horse = 0,
			}
		}
	else
		looks_info = {result=1}
	end
	return looks_info
end

function CMD.scene_get_main_role_info( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_main_role_info] user_info, req_data', user_info, user_info.cur_role_id)
	local role_info = sceneMgr.role_list[user_info.cur_role_id]
	role_info = role_info and role_info.base_info or nil
	if role_info then
		local result =  {
			role_info={
				scene_uid=sceneMgr.role_list[user_info.cur_role_id].scene_uid,
				role_id=user_info.cur_role_id,
				career=role_info.career,
				name=role_info.name,
				scene_id = sceneMgr.cur_scene_id,
				pos_x = role_info.pos_x,
				pos_y = role_info.pos_y,
				pos_z = role_info.pos_z,
				base_info = {
					level = 0,
				},
			}
		}
		return result
	else
		--cannot find main role?
		return {
			role_info={
				scene_uid=sceneMgr.role_list[user_info.cur_role_id].scene_uid,
				role_id=user_info.cur_role_id,
				career=2,
				name="unknow_role_name",
				scene_id = sceneMgr.cur_scene_id,
				pos_x = 0,
				pos_y = 0,
				pos_z = 0,
				base_info = {
					level = 0,
				},
			}
		}
	end
end

function CMD.scene_walk( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_main_role_info] user_info, req_data', user_info, user_info.cur_role_id)
	local role_info = sceneMgr.role_list[user_info.cur_role_id]
	if role_info and role_info.base_info then
		-- role_info.base_info.pos = {x=req_data.start_x, y=req_data.start_y, z=req_data.start_z}
		role_info.base_info.pos_x = req_data.start_x
		role_info.base_info.pos_y = req_data.start_y
		role_info.base_info.pos_z = req_data.start_z
		local entity = sceneMgr.uid_entity_map[role_info.scene_uid]
		if entity then
			sceneMgr.entityMgr:SetComponentData(entity, "UMO.Position", {x=req_data.start_x, y=req_data.start_y, z=req_data.start_z})
			sceneMgr.entityMgr:SetComponentData(entity, "UMO.TargetPos", {x=req_data.end_x, y=req_data.start_y, z=req_data.end_z})
		end
		sceneMgr.aoi:set_pos(role_info.aoi_handle, role_info.base_info.pos_x, role_info.base_info.pos_y, role_info.base_info.pos_z)
		local pos_info = role_info.base_info.pos_x..","..role_info.base_info.pos_y..","..role_info.base_info.pos_z
		local target_pos_info = req_data.end_x..","..req_data.end_z
		local cur_time = Time.timeMS
		-- local change_pos_event_info = {key=SceneConst.InfoKey.PosChange, value=pos_info, time= cur_time}
		local change_target_pos_event_info = {key=SceneConst.InfoKey.TargetPos, value=target_pos_info, time=cur_time}
		sceneMgr.eventMgr:AddSceneEvent(role_info.scene_uid, change_target_pos_event_info)
		if req_data.jump_state ~= 0 then
			-- table.insert(sceneMgr.event_list[role_info.scene_uid], {key=SceneConst.InfoKey.JumpState, value="1", time=cur_time})
			sceneMgr.eventMgr:AddSceneEvent(role_info.scene_uid, {key=SceneConst.InfoKey.JumpState, value="1", time=cur_time})
		end
	end
	return {}
end

function CMD.scene_get_objs_info_change( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_objs_info_change] user_info, role_id', user_info, user_info.cur_role_id)
	local role_info = sceneMgr.role_list[user_info.cur_role_id]
	if role_info and not role_info.ack_scene_get_objs_info_change then
		--synch info at fixed time
		role_info.ack_scene_get_objs_info_change = skynet.response()
		return NORET
	end
	return {}
end

function CMD.scene_cast_skill(user_info, req_data)
	local result_code, fight_event = sceneMgr.fightMgr:cast_skill(user_info, req_data)
	print("Cat:scene [start:355] fight_event, result_code:", fight_event, result_code)
	PrintTable(fight_event)
	print("Cat:scene [end]")
	-- do
		--test
		-- table.insert(sceneMgr.role_list[user_info.cur_role_id].fight_events_in_around, fight_event)
	-- end
	return {result=result_code, fight_event=fight_event}
end

function CMD.scene_listen_fight_event(user_info, req_data)
	local role_info = sceneMgr.role_list[user_info.cur_role_id]
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