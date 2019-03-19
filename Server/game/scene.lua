local skynet = require "skynet"
require "Common.Util.util"
require "ECS.ECS"
require "common.helper"
RequireAllLuaFileInFolder("./game/System")

local NORET = {}
local CMD = {}
local this = {
	--the scene object includes the role monster npc
	cur_scene_id = 0,
	scene_uid = 0,
	role_list = {},
	npc_list = {},
	monster_list = {},
	object_list = {},
	entity_mgr = false,
}
local SceneObjectType={
	Role=1,Monster=2,NPC=3,
}
local SceneInfoKey = {
	EnterScene=1,
    LeaveScene=2,
    PosChange=3,
    TargetPos=4,
}

local get_cur_time = function (  )
	return math.floor(skynet.time()*1000+0.5)
end

local new_scene_uid = function ( scene_obj_type )
	this.scene_uid = scene_obj_type*10000000000+this.scene_uid + 1
	return this.scene_uid
end

--enter_radius should be smaller than leave_radius
local get_around_roles = function ( role_id, enter_radius, leave_radius )
	return this.role_list
end

local add_info_item = function ( change_obj_infos, scene_uid, info_item )
	change_obj_infos = change_obj_infos or {obj_infos={}}
	local cur_info = nil
	for i,v in ipairs(change_obj_infos.obj_infos) do
		if v.scene_obj_uid == scene_uid then
			cur_info = v
		end
	end
	if not cur_info then
		cur_info = {scene_obj_uid=scene_uid, info_list={}}
		table.insert(change_obj_infos.obj_infos, cur_info)
	end
	table.insert(cur_info.info_list, info_item)
	return change_obj_infos
end

local init_npc = function (  )
	if not this.scene_cfg or not this.scene_cfg.npc_list then return end
	
	for k,v in pairs(this.scene_cfg.npc_list) do
		local npc = {}
		npc.id = v.npc_id
		npc.uid = new_scene_uid(SceneObjectType.NPC)
		npc.pos_x = v.pos_x
		npc.pos_y = v.pos_y
		npc.pos_z = v.pos_z
		-- local npc_entity = this.entity_mgr:CreateEntityByArcheType(this.npc_archetype)
		table.insert(this.npc_list, npc)
	end
end

local init_monster = function (  )
	if not this.scene_cfg or not this.scene_cfg.monster_list then return end
	for k,v in pairs(this.scene_cfg.monster_list) do
		local monster = this.entity_mgr:CreateEntityByArcheType(this.monster_archetype)
		this.entity_mgr:SetComponentData(monster, "UMO.Position", {x=v.pos_x, y=v.pos_y, z=v.pos_z})
		this.entity_mgr:SetComponentData(monster, "UMO.UID", {value=new_scene_uid(SceneObjectType.Monster)})
		this.entity_mgr:SetComponentData(monster, "UMO.TypeID", {value=v.monster_id})
	end
end

function CMD.init(scene_id)
	ECS.InitWorld("scene_world")
	this.entity_mgr = ECS.World.Active:GetOrCreateManager(ECS.EntityManager.Name)
	this.monster_archetype = this.entity_mgr:CreateArchetype({"UMO.Position", "UMO.UID", "UMO.TypeID"})
	this.npc_archetype = this.entity_mgr:CreateArchetype({"UMO.Position", "UMO.UID", "UMO.TypeID"})
	this.scene_cfg = require("Config.scene.config_scene_"..scene_id)
	this.cur_scene_id = scene_id
	init_npc()
	init_monster()

	Time = {deltaTime=0}
	lastUpdateTime = os.time()
	skynet.fork(function()
		while true do
			local curTime = os.time()
			Time.deltaTime = curTime-lastUpdateTime
			lastUpdateTime = curTime
			ECS.Update()
			skynet.sleep(10)
		end
	end)
	skynet.fork(function()
		while true do
			--synch info at fixed time
			for k,role_info in pairs(this.role_list) do
				-- print("Cat:scene [start:46] role_info.change_obj_infos:", role_info.change_obj_infos)
				-- PrintTable(role_info.change_obj_infos)
				-- print("Cat:scene [end]")
				if role_info.change_obj_infos and role_info.ack_scene_get_objs_info_change then
					role_info.ack_scene_get_objs_info_change(true, role_info.change_obj_infos)
					role_info.change_obj_infos = nil
					role_info.ack_scene_get_objs_info_change = nil
				end
			end
			skynet.sleep(10)
		end
	end)
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
	local cur_time = get_cur_time()
	do 
		--tell every one a new role enter scene
		for k,v in pairs(this.role_list) do
			v.change_obj_infos = add_info_item(v.change_obj_infos, v.scene_uid, {key=SceneInfoKey.EnterScene, value=SceneObjectType.Role, time=cur_time})
		end
	end
	if not this.role_list[role_id] then
		local scene_uid = new_scene_uid(SceneObjectType.Role)
		local base_info = get_base_info_by_roleid(role_id)
		local looks_info = get_looks_info_by_roleid(role_id)
		init_pos_info(base_info)
		this.role_list[role_id] = {scene_uid=scene_uid, base_info=base_info, looks_info=looks_info}
		this.object_list[scene_uid] = this.role_list[role_id]
		--tell the new guy who are here
		for k,v in pairs(this.role_list) do
			if v.scene_uid ~= scene_uid then
				this.role_list[role_id].change_obj_infos = add_info_item(this.role_list[role_id].change_obj_infos, v.scene_uid, {key=SceneInfoKey.EnterScene, value=SceneObjectType.Role, time=cur_time})
			end
		end
		for k,v in pairs(this.npc_list) do
			this.role_list[role_id].change_obj_infos = add_info_item(this.role_list[role_id].change_obj_infos, v.scene_uid, {key=SceneInfoKey.EnterScene, value=SceneObjectType.NPC, time=0})
		end
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
			v.change_obj_infos = add_info_item(v.change_obj_infos, role_info.scene_uid, {key=SceneInfoKey.LeaveScene, value=SceneObjectType.Role, time=cur_time})
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
		local target_pos_info = req_data.end_x..","..req_data.end_y..","..req_data.end_z
		-- print('Cat:scene.lua[116] pos_info', pos_info, role_info.scene_uid)
		local cur_time = get_cur_time()
		--for test 
		for k,v in pairs(this.role_list) do
			local role_id = k
			-- print('Cat:scene.lua[101] role_id, user_info.cur_role_id', role_id, user_info.cur_role_id, v.scene_uid, role_info.scene_uid)
			if role_id ~= user_info.cur_role_id then
				v.change_obj_infos = add_info_item(v.change_obj_infos, role_info.scene_uid, {key=SceneInfoKey.PosChange, value=pos_info, time= cur_time})
				v.change_obj_infos = add_info_item(v.change_obj_infos, role_info.scene_uid, {key=SceneInfoKey.TargetPos, value=target_pos_info, time=cur_time})
			end
		end
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

skynet.start(function()
	skynet.dispatch("lua", function(session, source, command, ...)
		local f = assert(CMD[command])
		local r = f(...)
		if r ~= NORET then
			skynet.ret(skynet.pack(r))
		end
	end)
end)