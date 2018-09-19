local skynet = require "skynet"
require "Common.Util.util"

local NORET = {}
local CMD = {}
local this = {
	--the scene object includes the role monster npc
	scene_uid = 0,
	role_lists = {},
}
-- local scene_uid = 0
-- local role_lists = {}

local new_scene_uid = function (  )
	this.scene_uid = this.scene_uid + 1
	return this.scene_uid
end

--enter_radius should be smaller than leave_radius
local get_around_roles = function ( role_id, enter_radius, leave_radius )
	return this.role_lists
end

local add_info_item = function ( obj_infos, scene_uid, info_item )
	obj_infos = obj_infos or {}
	local cur_info = nil
	for i,v in ipairs(obj_infos) do
		if v.scene_obj_uid == scene_uid then
			cur_info = v
		end
	end
	if not cur_info then
		cur_info = {scene_obj_uid=scene_uid, info_list={}}
		table.insert(obj_infos, cur_info)
	end
	table.insert(cur_info.info_list, info_item)
	return obj_infos
end

function CMD.init(scene_id)
	print('Cat:scene.lua[init] scene_id', scene_id)
	skynet.fork(function()
		while true do
			--synch scene info 
			for k,role_info in pairs(this.role_lists) do
				if role_info.change_obj_infos and role_info.ack_scene_get_objs_info_change then
					print("Cat:scene [start:41] role_info.change_obj_infos:", role_info.change_obj_infos)
					PrintTable(role_info.change_obj_infos)
					print("Cat:scene [end]")
					role_info.ack_scene_get_objs_info_change(true, role_info.change_obj_infos)
					role_info.change_obj_infos = nil
					role_info.ack_scene_get_objs_info_change = nil
				end
			end
			skynet.sleep(100)
		end
	end)
end

function CMD.role_enter_scene(role_id)
	print('Cat:scene.lua[role_enter_scene] role_id', role_id)
	do 
		--for test 
		for k,v in pairs(this.role_lists) do
			v.change_obj_infos = add_info_item(v.change_obj_infos, v.scene_uid, {key=1, value=1, time=os.time()})
		end
	end
	if not this.role_lists[role_id] then
		local scene_uid = new_scene_uid()
		this.role_lists[role_id] = {scene_uid=scene_uid}
		for k,v in pairs(this.role_lists) do
			if v.scene_uid ~= scene_uid then
				this.role_lists[role_id].change_obj_infos = add_info_item(this.role_lists[role_id].change_obj_infos, scene_uid, {key=1, value="1", time=os.time()})
			end
		end
	end
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
	--test skynet.response
	-- skynet.timeout(200, function()
	-- 	print('Cat:scene.lua[32] response', response)
	-- 	if response then
	-- 		response(true, {role_info={role_id=1,career=2,name="haha"}})
	-- 	end
	-- end)
	-- response = skynet.response()
	-- print('Cat:scene.lua[38] response', response)
	-- return NORET
end

function CMD.scene_walk( user_info, req_data )
	-- print('Cat:scene.lua[scene_get_main_role_info] user_info, req_data', user_info, user_info.cur_role_id)
	-- print("Cat:scene [start:22] req_data:", req_data)
	-- PrintTable(req_data)
	-- print("Cat:scene [end]")
	-- print("Cat:scene [start:49] user_info:", user_info)
	-- PrintTable(user_info)
	-- print("Cat:scene [end]")
	local role_info = this.role_lists[user_info.cur_role_id]
	if role_info then
		role_info.pos = {x=req_data.pos_x, y=req_data.pos_y, z=req_data.pos_z}
	end
	return {}
end

function CMD.scene_get_objs_info_change( user_info, req_data )
	print('Cat:scene.lua[scene_get_objs_info_change] user_info, role_id', user_info, user_info.cur_role_id)
	local role_info = this.role_lists[user_info.cur_role_id]
	if role_info and not role_info.ack_scene_get_objs_info_change then
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