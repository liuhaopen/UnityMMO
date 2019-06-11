local skynet = require "skynet"
local SceneConst = require "game.scene.SceneConst"
local SceneHelper = require "game.scene.SceneHelper"

local RoleMgr = BaseClass()

function RoleMgr:Init( sceneMgr )
	self.sceneMgr = sceneMgr
	self.entityMgr = sceneMgr.entityMgr
	self.aoi = sceneMgr.aoi
	self.roleList = {}
	self:InitArchetype()
end

function RoleMgr:GetRole( roleID )
	return self.roleList[roleID]
end

function RoleMgr:InitArchetype(  )
	self.role_archetype = self.entityMgr:CreateArchetype({
		"UMO.Position", "UMO.TargetPos", "UMO.UID", "UMO.TypeID", "UMO.HP", "UMO.SceneObjType", "UMO.MoveSpeed", "UMO.AOIHandle", "UMO.Beatable", "UMO.DamageEvents"
	})
end

function RoleMgr:CreateRole( uid, roleID, pos_x, pos_y, pos_z, aoi_handle )
	local role = self.entityMgr:CreateEntityByArcheType(self.role_archetype)
	self.entityMgr:SetComponentData(role, "UMO.Position", {x=pos_x, y=pos_y, z=pos_z})
	self.entityMgr:SetComponentData(role, "UMO.TargetPos", {x=pos_x, y=pos_y, z=pos_z})
	self.entityMgr:SetComponentData(role, "UMO.UID", uid)
	self.entityMgr:SetComponentData(role, "UMO.TypeID", {value=roleID})
	self.entityMgr:SetComponentData(role, "UMO.HP", {cur=1000, max=1000})
	self.entityMgr:SetComponentData(role, "UMO.SceneObjType", {value=SceneConst.ObjectType.Role})
	self.entityMgr:SetComponentData(role, "UMO.MoveSpeed", {value=100})
	self.entityMgr:SetComponentData(role, "UMO.AOIHandle", {value=aoi_handle})
	return role
end

function RoleMgr:GetBaseInfoByRoleID( roleID )
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_ok, role_info = skynet.call(gameDBServer, "lua", "select_by_key", "RoleBaseInfo", "role_id", roleID)
	if is_ok and role_info and role_info[1] then
		return role_info[1]
	end
	return nil
end

function RoleMgr:GetLooksInfoByRoleID( roleID )
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_ok, looks_info = skynet.call(gameDBServer, "lua", "select_by_key", "RoleLooksInfo", "role_id", roleID)
	if is_ok and looks_info and looks_info[1] then
		return looks_info[1]
	end
	return nil
end

function RoleMgr:InitPosInfo( baseInfo )
	if not baseInfo then return end
	local is_need_reset_pos = false
	if not baseInfo.scene_id or self.sceneMgr.cur_scene_id ~= baseInfo.scene_id or not baseInfo.pos_x then
		is_need_reset_pos = true
	end
	if is_need_reset_pos then
		local born_list = self.sceneMgr.scene_cfg.born_list
		local door_num = born_list and #born_list or 0
		if door_num > 0 then
			local random_index = math.random(1, door_num)
			local born_info = born_list[random_index]
			baseInfo.pos_x = born_info.pos_x
			baseInfo.pos_y = born_info.pos_y
			baseInfo.pos_z = born_info.pos_z
		end
	end
end

function RoleMgr:RoleEnter( roleID )
	if not self.roleList[roleID] then
		local scene_uid = SceneHelper:NewSceneUID(SceneConst.ObjectType.Role)
		local base_info = self:GetBaseInfoByRoleID(roleID)
		local looks_info = self:GetLooksInfoByRoleID(roleID)
		self:InitPosInfo(base_info)

		local handle = self.sceneMgr.aoi:add()
		self.sceneMgr.aoi:set_user_data(handle, "uid", scene_uid)
		self.roleList[roleID] = {scene_uid=scene_uid, base_info=base_info, looks_info=looks_info, aoi_handle=handle, around_objs={}, radius_short=5000, radius_long=6000, fight_events_in_around={}}
		-- self.sceneMgr:SetSceneObj(scene_uid, self.roleList[roleID])
		-- self.aoi_handle_uid_map[handle] = scene_uid
		self.sceneMgr.aoi:set_pos(handle, base_info.pos_x, base_info.pos_y, base_info.pos_z)

		local entity = self:CreateRole(scene_uid, roleID, base_info.pos_x, base_info.pos_y, base_info.pos_z, handle)
		self.sceneMgr:SetEntity(scene_uid, entity)
		-- self.sceneMgr.aoi:set_user_data(handle, "entity", entity)
	end
end

local save_role_pos = function ( role_id, pos_x, pos_y, pos_z, scene_id )
	local gameDBServer = skynet.localname(".GameDBServer")
	is_succeed = skynet.call(gameDBServer, "lua", "update", "RoleBaseInfo", "role_id", role_id, {pos_x=pos_x, pos_y=pos_y, pos_z=pos_z, scene_id=scene_id})
end

function RoleMgr:RoleLeave( roleID )
	local role_info = self.roleList[roleID]
	print('Cat:scene.lua[role_leave_scene] roleID', roleID, role_info)
	if not role_info then return end
	
	self.sceneMgr.aoi:remove(role_info.aoi_handle)
	-- self.sceneMgr.aoi_handle_uid_map[role_info.aoi_handle] = nil --角色离开后还需要通过aoi_handle获取ta的uid
	save_role_pos(roleID, role_info.base_info.pos_x, role_info.base_info.pos_y, role_info.base_info.pos_z, self.sceneMgr.cur_scene_id)	

	if role_info.ack_scene_get_objs_info_change then
		role_info.ack_scene_get_objs_info_change(true, {})
	end
	self.roleList[roleID] = nil
end

function RoleMgr:GetMainRoleInfo( roleID )
	local role_info = self.roleList[roleID]
	role_info = role_info and role_info.base_info or nil
	if role_info then
		local entity = self.sceneMgr:GetEntity(self.roleList[roleID].scene_uid)
		local hpData = self.sceneMgr.entityMgr:GetComponentData(entity, "UMO.HP")
		local result =  {
			role_info={
				scene_uid=self.roleList[roleID].scene_uid,
				role_id=roleID,
				career=role_info.career,
				name=role_info.name,
				scene_id = self.sceneMgr.cur_scene_id,
				pos_x = role_info.pos_x,
				pos_y = role_info.pos_y,
				pos_z = role_info.pos_z,
				cur_hp = hpData.cur,
				max_hp = hpData.max,
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
				scene_uid=self.roleList[roleID].scene_uid,
				role_id=roleID,
				career=2,
				name="unknow_role_name",
				scene_id = self.sceneMgr.cur_scene_id,
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

function RoleMgr:GetRoleLooksInfo( uid )
	local looks_info
	local entity = self.sceneMgr:GetEntity(uid)
	if entity then
		local hpData = self.sceneMgr.entityMgr:GetComponentData(entity, "UMO.HP")
		local role_id = self.sceneMgr.entityMgr:GetComponentData(entity, "UMO.TypeID")
		local role_info = self:GetRole(role_id.value)
		if not role_info then
			looks_info = {result=1}
		end
		looks_info = {
			result = 0,
			role_looks_info = {
				career = role_info.base_info.career or 1,
				name = role_info.name,
				hp = hpData.cur,
				max_hp = hpData.max,
				body = role_info.looks_info.body or 0,
				hair = role_info.looks_info.hair or 0,
				-- body = 0,
				-- hair = 0,
				weapon = role_info.looks_info.weapon or 0,
				wing = role_info.looks_info.wing or 0,
				horse = 0,
			}
		}
	else
		looks_info = {result=1}
	end
	return looks_info
end

function RoleMgr:RoleWalk( roldID, req_data )
	local role_info = self.roleList[roldID]
	if role_info and role_info.base_info then
		-- role_info.base_info.pos = {x=req_data.start_x, y=req_data.start_y, z=req_data.start_z}
		role_info.base_info.pos_x = req_data.start_x
		role_info.base_info.pos_y = req_data.start_y
		role_info.base_info.pos_z = req_data.start_z
		local entity = self.sceneMgr:GetEntity(role_info.scene_uid)
		if entity then
			self.sceneMgr.entityMgr:SetComponentData(entity, "UMO.Position", {x=req_data.start_x, y=req_data.start_y, z=req_data.start_z})
			self.sceneMgr.entityMgr:SetComponentData(entity, "UMO.TargetPos", {x=req_data.end_x, y=req_data.start_y, z=req_data.end_z})
		end
		self.sceneMgr.aoi:set_pos(role_info.aoi_handle, role_info.base_info.pos_x, role_info.base_info.pos_y, role_info.base_info.pos_z)
		local pos_info = role_info.base_info.pos_x..","..role_info.base_info.pos_y..","..role_info.base_info.pos_z
		local target_pos_info = req_data.end_x..","..req_data.end_z
		local cur_time = Time.timeMS
		-- local change_pos_event_info = {key=SceneConst.InfoKey.PosChange, value=pos_info, time= cur_time}
		local change_target_pos_event_info = {key=SceneConst.InfoKey.TargetPos, value=target_pos_info, time=cur_time}
		self.sceneMgr.eventMgr:AddSceneEvent(role_info.scene_uid, change_target_pos_event_info)
		if req_data.jump_state ~= 0 then
			-- table.insert(self.sceneMgr.event_list[role_info.scene_uid], {key=SceneConst.InfoKey.JumpState, value="1", time=cur_time})
			self.sceneMgr.eventMgr:AddSceneEvent(role_info.scene_uid, {key=SceneConst.InfoKey.JumpState, value="1", time=cur_time})
		end
	end
	return {}
end
return RoleMgr