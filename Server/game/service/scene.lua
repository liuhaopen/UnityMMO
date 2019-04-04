local skynet = require "skynet"
require "Common.Util.util"
require "ECS.ECS"
require "common.helper"
local scene_helper = require "game.scene.scene_helper"
RequireAllLuaFileInFolder("./game/scene/system")

local NORET = {}
local CMD = {}
local this = {
	cur_scene_id = 0,
	scene_uid = 0,
	role_list = {},
	npc_list = {},
	monster_list = {},
	object_list = {},--the scene object includes the role monster npc
	event_list = {},
	ecs_world = false,
	entity_mgr = false,
	monster_mgr = require "game.scene.monster_mgr",
	ecs_system_mgr = require "game.scene.ecs_system_mgr",
	aoi = require "game.scene.aoi",
}

local get_cur_time = function (  )
	return math.floor(skynet.time()*1000+0.5)
end

--enter_radius should be smaller than leave_radius
local get_around_roles = function ( role_id, enter_radius, leave_radius )
	return this.role_list
end

local init_npc = function (  )
	if not this.scene_cfg or not this.scene_cfg.npc_list then return end
	
	for k,v in pairs(this.scene_cfg.npc_list) do
		local npc = {}
		npc.id = v.npc_id
		npc.uid = scene_helper:new_scene_uid(SceneObjectType.NPC)
		npc.pos_x = v.pos_x
		npc.pos_y = v.pos_y
		npc.pos_z = v.pos_z
		local npc_entity = this.entity_mgr:CreateEntityByArcheType(this.npc_archetype)
		table.insert(this.npc_list, npc)
	end
end

local fork_loop_ecs = function (  )
	Time = {deltaTime=0}
	lastUpdateTime = get_cur_time()
	skynet.fork(function()
		while true do
			local curTime = get_cur_time()
			Time.deltaTime = (curTime-lastUpdateTime)/1000
			lastUpdateTime = curTime
			this.ecs_system_mgr:update()
			skynet.sleep(5)
		end
	end)
end

--更新角色可视区域的感兴趣节点集合（玩家，怪物，NPC)
local update_interest_objs = function ( role_info )
	local objs = this.aoi:get_around_offset(role_info.aoi_handle, role_info.radius_short, role_info.radius_long)
	local cur_time = get_cur_time()
	for k,v in pairs(objs) do
		print('Cat:scene.lua[66] v.is_enter', v.is_enter)
		if v.is_enter then
			role_info.interest_objs[v.uid] = v.uid
			local info = this.object_list[v.uid]
			print('Cat:scene.lua[69] info', info)
			if info then
				role_info.change_obj_infos = scene_helper.add_info_item(role_info.change_obj_infos, v.uid, {key=SceneInfoKey.EnterView, value=info.scene_type..","..info.pos_x..","..info.pos_y..","..info.pos_z, time=cur_time})
			end
		else
			role_info.interest_objs[v.uid] = nil
			role_info.change_obj_infos = scene_helper.add_info_item(role_info.change_obj_infos, v.uid, {key=SceneInfoKey.LeaveView, value="", time=cur_time})
		end
	end
end

local collect_events = function (  )
	for _,role_info in pairs(this.role_list) do
		update_interest_objs(role_info)
		for _,interest_uid in pairs(role_info.interest_objs) do
			local event_list = this.event_list[interest_uid]
			if event_list then
				for i,event_info in ipairs(event_list) do
					role_info.change_obj_infos = scene_helper.add_info_item(role_info.change_obj_infos, interest_uid, event_info)
				end
			end
		end
	end
	this.event_list = {}
end

local fork_loop_scene_info_change = function (  )
	skynet.fork(function()
		while true do
			collect_events()
			--synch info at fixed time
			for _,role_info in pairs(this.role_list) do
				if role_info.change_obj_infos and role_info.ack_scene_get_objs_info_change then
					role_info.ack_scene_get_objs_info_change(true, role_info.change_obj_infos)
					role_info.change_obj_infos = nil
					role_info.ack_scene_get_objs_info_change = nil
				end
			end
			skynet.sleep(5)
		end
	end)
end

local fork_loop_fight_event = function (  )
	skynet.fork(function()
		while true do 
			for k,role_info in pairs(this.role_list) do
				if role_info.fight_events and role_info.ack_scene_listen_fight_event then
					role_info.ack_scene_listen_fight_event(true, role_info.fight_events)
					role_info.fight_events = nil
					role_info.ack_scene_listen_fight_event = nil
				end
			end
			skynet.sleep(5)
		end
	end)
end

function CMD.init(scene_id)
	this.ecs_world = ECS.InitWorld("scene_world")
	this.entity_mgr = ECS.World.Active:GetOrCreateManager(ECS.EntityManager.Name)
	this.npc_archetype = this.entity_mgr:CreateArchetype({"umo.position", "umo.uid", "umo.type_id"})
	this.scene_cfg = require("Config.scene.config_scene_"..scene_id)
	this.cur_scene_id = scene_id
	init_npc()
	this.monster_mgr:init(this.entity_mgr, this.scene_cfg.monster_list)
	this.ecs_system_mgr:init(this.ecs_world)

	fork_loop_ecs()
	fork_loop_scene_info_change()
	fork_loop_fight_event()
end

local get_base_info_by_roleid = function ( role_id )
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_ok, role_info = skynet.call(gameDBServer, "lua", "select_by_key", "RoleBaseInfo", "role_id", role_id)
	if is_ok and role_info and role_info[1] then
		return role_info[1]
	end
	return nil
end

local get_looks_info_by_roleid = function ( role_id )
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_ok, looks_info = skynet.call(gameDBServer, "lua", "select_by_key", "RoleLooksInfo", "role_id", role_id)
	if is_ok and looks_info and looks_info[1] then
		return looks_info[1]
	end
	return nil
end

local init_pos_info = function ( base_info )
	if not base_info then return end
	local is_need_reset_pos = false
	if not base_info.scene_id or this.cur_scene_id ~= base_info.scene_id or not base_info.pos_x then
		is_need_reset_pos = true
	end
	if is_need_reset_pos then
		local born_list = this.scene_cfg.born_list
		local door_num = born_list and #born_list or 0
		-- print('Cat:scene.lua[136] door_num', door_num)
		if door_num > 0 then
			local random_index = math.random(1, door_num)
			local born_info = born_list[random_index]
			base_info.pos_x = born_info.pos_x
			base_info.pos_y = born_info.pos_y
			base_info.pos_z = born_info.pos_z
		end
	end
end

function CMD.role_enter_scene(role_id)
	print('Cat:scene.lua[role_enter_scene] role_id', role_id)
	if not this.role_list[role_id] then
		local scene_uid = scene_helper:new_scene_uid(SceneObjectType.Role)
		local base_info = get_base_info_by_roleid(role_id)
		local looks_info = get_looks_info_by_roleid(role_id)
		init_pos_info(base_info)
		this.role_list[role_id] = {scene_uid=scene_uid, base_info=base_info, looks_info=looks_info}
		this.object_list[scene_uid] = this.role_list[role_id]

		local cur_time = get_cur_time()
		--tell the new guy who are here
		for k,v in pairs(this.role_list) do
			if v.scene_uid ~= scene_uid then
				this.role_list[role_id].change_obj_infos = scene_helper.add_info_item(this.role_list[role_id].change_obj_infos, v.scene_uid, {key=SceneInfoKey.EnterView, value=SceneObjectType.Role, time=cur_time})
				local pos_info_str = v.base_info.pos_x..","..v.base_info.pos_y..","..v.base_info.pos_z
				this.role_list[role_id].change_obj_infos = scene_helper.add_info_item(this.role_list[role_id].change_obj_infos, v.scene_uid, {key=SceneInfoKey.PosChange, value=pos_info_str, time=cur_time})
			end
		end
		--tell every one a new role enter scene
		local pos_info_str = base_info.pos_x..","..base_info.pos_y..","..base_info.pos_z
		local enter_event_info = {key=SceneInfoKey.EnterView, value=SceneObjectType.Role, time=cur_time}
		local pos_change_event_info = {key=SceneInfoKey.PosChange, value=pos_info_str, time=cur_time}
		for k,v in pairs(this.role_list) do
			if v.scene_uid ~= scene_uid then
				v.change_obj_infos = scene_helper.add_info_item(v.change_obj_infos, scene_uid, enter_event_info)
				v.change_obj_infos = scene_helper.add_info_item(v.change_obj_infos, scene_uid, pos_change_event_info)
			end
		end
		-- for k,v in pairs(this.npc_list) do
		-- 	this.role_list[role_id].change_obj_infos = scene_helper.add_info_item(this.role_list[role_id].change_obj_infos, v.scene_uid, {key=SceneInfoKey.EnterView, value=SceneObjectType.NPC, time=0})
		-- end
	end
end

local save_role_pos = function ( role_id, pos_x, pos_y, pos_z, scene_id )
	local gameDBServer = skynet.localname(".GameDBServer")
	-- print('Cat:scene.lua[191] role_id, pos_x, pos_y, pos_z, scene_id', role_id, pos_x, pos_y, pos_z, scene_id)
	is_succeed = skynet.call(gameDBServer, "lua", "update", "RoleBaseInfo", "role_id", role_id, {pos_x=pos_x, pos_y=pos_y, pos_z=pos_z, scene_id=scene_id})
end

function CMD.role_leave_scene(role_id)
	local role_info = this.role_list[role_id]
	-- print('Cat:scene.lua[role_leave_scene] role_id', role_id, role_info)
	if not role_info then return end
	
	save_role_pos(role_id, role_info.base_info.pos_x, role_info.base_info.pos_y, role_info.base_info.pos_z, this.cur_scene_id)	

	local cur_time = get_cur_time()
	--tell every one this role leave scene
	for k,v in pairs(this.role_list) do
		local cur_role_id = k
		if v.cur_role_id ~= role_id then
			v.change_obj_infos = scene_helper.add_info_item(v.change_obj_infos, role_info.scene_uid, {key=SceneInfoKey.LeaveView, value=SceneObjectType.Role, time=cur_time})
		end
	end
	if role_info.ack_scene_get_objs_info_change then
		role_info.ack_scene_get_objs_info_change(true, {})
	end
	this.role_list[role_id] = nil
end

function CMD.scene_get_role_look_info( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_role_look_info] user_info, req_data', user_info, user_info.cur_role_id)
	local role_info = this.object_list[req_data.uid]
	-- print('Cat:scene.lua[211] role_info', role_info, req_data.uid, this.object_list[req_data.uid])
	local looks_info
	if role_info then
		looks_info = {
			result = 0,
			role_looks_info = {
				career = role_info.base_info.career or 1,
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
	local role_info = this.role_list[user_info.cur_role_id]
	role_info = role_info and role_info.base_info or nil
	if role_info then
		local result =  {
			role_info={
				scene_uid=this.role_list[user_info.cur_role_id].scene_uid,
				role_id=user_info.cur_role_id,
				career=role_info.career,
				name=role_info.name,
				scene_id = this.cur_scene_id,
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
				scene_uid=this.role_list[user_info.cur_role_id].scene_uid,
				role_id=user_info.cur_role_id,
				career=2,
				name="unknow_role_name",
				scene_id = this.cur_scene_id,
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
	local role_info = this.role_list[user_info.cur_role_id]
	if role_info and role_info.base_info then
		-- role_info.base_info.pos = {x=req_data.start_x, y=req_data.start_y, z=req_data.start_z}
		role_info.base_info.pos_x = req_data.start_x
		role_info.base_info.pos_y = req_data.start_y
		role_info.base_info.pos_z = req_data.start_z
		local pos_info = role_info.base_info.pos_x..","..role_info.base_info.pos_y..","..role_info.base_info.pos_z
		local target_pos_info = req_data.end_x..","..req_data.end_z
		-- print('Cat:scene.lua[116] pos_info', pos_info, role_info.scene_uid)
		local cur_time = get_cur_time()
		-- local change_pos_event_info = {key=SceneInfoKey.PosChange, value=pos_info, time= cur_time}
		local change_target_pos_event_info = {key=SceneInfoKey.TargetPos, value=target_pos_info, time=cur_time}
		this.event_list[role_info.scene_uid] = this.event_list[role_info.scene_uid] or {}
		table.insert(this.event_list[role_info.scene_uid], change_target_pos_event_info)
		--for test 
		-- for k,v in pairs(this.role_list) do
		-- 	local role_id = k
		-- 	if role_id ~= user_info.cur_role_id then
		-- 		-- v.change_obj_infos = scene_helper.add_info_item(v.change_obj_infos, role_info.scene_uid, change_pos_event_info)
		-- 		v.change_obj_infos = scene_helper.add_info_item(v.change_obj_infos, role_info.scene_uid, change_target_pos_event_info)
		-- 	end
		-- end
	end
	return {}
end

function CMD.scene_get_objs_info_change( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_objs_info_change] user_info, role_id', user_info, user_info.cur_role_id)
	local role_info = this.role_list[user_info.cur_role_id]
	if role_info and not role_info.ack_scene_get_objs_info_change then
		--synch info at fixed time
		role_info.ack_scene_get_objs_info_change = skynet.response()
		return NORET
	end
	return {}
end

function CMD.scene_cast_skill(user_info, req_data)
	
end

function CMD.scene_listen_fight_event(user_info, req_data)
	local role_info = this.role_list[user_info.cur_role_id]
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