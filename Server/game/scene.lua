local skynet = require "skynet"
require "Common.Util.util"

local NORET = {}
local CMD = {}
local this = {
	--the scene object includes the role monster npc
	scene_uid = 0,
	role_lists = {},
}
local SceneObjectType={
	Role=1,Monster=2,NPC=3,
}
local SceneInfoKey = {
	EnterScene=1,
    LeaveScene=2,
    PosChange=3,
}

local new_scene_uid = function (  )
	this.scene_uid = this.scene_uid + 1
	return this.scene_uid
end

--enter_radius should be smaller than leave_radius
local get_around_roles = function ( role_id, enter_radius, leave_radius )
	return this.role_lists
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

function CMD.init(scene_id)
	print('Cat:scene.lua[init] scene_id', scene_id)
	skynet.fork(function()
		while true do
			--synch info at fixed time
			for k,role_info in pairs(this.role_lists) do
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

function CMD.role_enter_scene(role_id)
	print('Cat:scene.lua[role_enter_scene] role_id', role_id)
	do 
		--tell every one a new role enter scene
		for k,v in pairs(this.role_lists) do
			v.change_obj_infos = add_info_item(v.change_obj_infos, v.scene_uid, {key=SceneInfoKey.EnterScene, value=SceneObjectType.Role, time=os.time()})
		end
	end
	if not this.role_lists[role_id] then
		local scene_uid = new_scene_uid()
		this.role_lists[role_id] = {scene_uid=scene_uid}
		--tell the new guy
		for k,v in pairs(this.role_lists) do
			if v.scene_uid ~= scene_uid then
				this.role_lists[role_id].change_obj_infos = add_info_item(this.role_lists[role_id].change_obj_infos, v.scene_uid, {key=SceneInfoKey.EnterScene, value=SceneObjectType.Role, time=os.time()})
			end
		end
	end
end

function CMD.role_leave_scene(role_id)
	local role_info = this.role_lists[role_id]
	print('Cat:scene.lua[role_leave_scene] role_id', role_id, role_info)
	if not role_info then return end
	
	--tell every one this role leave scene
	for k,v in pairs(this.role_lists) do
		local cur_role_id = k
		if v.cur_role_id ~= role_id then
			v.change_obj_infos = add_info_item(v.change_obj_infos, role_info.scene_uid, {key=SceneInfoKey.LeaveScene, value=SceneObjectType.Role, time=os.time()})
		end
	end
	if role_info.ack_scene_get_objs_info_change then
		role_info.ack_scene_get_objs_info_change(true, {})
	end
	this.role_lists[role_id] = nil
end

function CMD.scene_get_main_role_info( user_info, req_data )
	print('Cat:scene.lua[scene_get_main_role_info] user_info, req_data', user_info, user_info.cur_role_id)
	-- local gameDBServer = skynet.localname(".GameDBServer")
	-- local is_succeed, result = skynet.call(gameDBServer, "lua", "select_by_key", "RoleBaseInfo", "role_id", user_info.cur_role_id)
	return {
		role_info={
			scene_uid=this.role_lists[user_info.cur_role_id].scene_uid,
			role_id=user_info.cur_role_id,
			career=2,name="haha"
			}
		}
end

function CMD.scene_walk( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_main_role_info] user_info, req_data', user_info, user_info.cur_role_id)
	local role_info = this.role_lists[user_info.cur_role_id]
	if role_info then
		role_info.pos = {x=req_data.pos_x, y=req_data.pos_y, z=req_data.pos_z}
		local pos_info = role_info.pos.x..","..role_info.pos.y..","..role_info.pos.z
		-- print('Cat:scene.lua[116] pos_info', pos_info, role_info.scene_uid)
		--for test 
		for k,v in pairs(this.role_lists) do
			local role_id = k
			-- print('Cat:scene.lua[101] role_id, user_info.cur_role_id', role_id, user_info.cur_role_id, v.scene_uid, role_info.scene_uid)
			if role_id ~= user_info.cur_role_id then
				v.change_obj_infos = add_info_item(v.change_obj_infos, role_info.scene_uid, {key=SceneInfoKey.PosChange, value=pos_info, time=os.time()})
			end
		end
	end
	return {}
end

function CMD.scene_get_objs_info_change( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_objs_info_change] user_info, role_id', user_info, user_info.cur_role_id)
	local role_info = this.role_lists[user_info.cur_role_id]
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