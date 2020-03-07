local skynet = require "skynet"
local SceneConst = require "game.scene.SceneConst"
local SceneHelper = require "game.scene.SceneHelper"
local SpeedData = require("game.scene.com.SpeedData")

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
		"UMO.Position", "UMO.TargetPos", "UMO.UID", "UMO.TypeID", "UMO.HP", "UMO.SceneObjType", "UMO.MoveSpeed", "UMO.AOIHandle", "UMO.Beatable", "UMO.DamageEvents", "UMO.MsgAgent", "UMO.CD", "UMO.Buff", "UMO.BaseAttr",
		"UMO.FightAttr", "UMO.Ability"
	})
end

function RoleMgr:CreateRole( uid, roleID, pos_x, pos_y, pos_z, aoi_handle, agent )
	local role = self.entityMgr:CreateEntityByArcheType(self.role_archetype)
	self.entityMgr:SetComponentData(role, "UMO.Position", {x=pos_x, y=pos_y, z=pos_z})
	self.entityMgr:SetComponentData(role, "UMO.TargetPos", {x=pos_x, y=pos_y, z=pos_z})
	self.entityMgr:SetComponentData(role, "UMO.UID", uid)
	self.entityMgr:SetComponentData(role, "UMO.TypeID", roleID)
	self.entityMgr:SetComponentData(role, "UMO.SceneObjType", {value=SceneConst.ObjectType.Role})
	self.entityMgr:SetComponentData(role, "UMO.MoveSpeed", SpeedData.New(100))
	self.entityMgr:SetComponentData(role, "UMO.AOIHandle", {value=aoi_handle})
	self.entityMgr:SetComponentData(role, "UMO.MsgAgent", agent)
	self.entityMgr:SetComponentData(role, "UMO.CD", {})
	return role
end

function RoleMgr:GetBaseInfoByRoleID( roleID )
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_ok, role_info = skynet.call(gameDBServer, "lua", "select_one_by_key", "RoleBaseInfo", "role_id", roleID)
	if is_ok then
		local is_ok, attr_info = skynet.call(gameDBServer, "lua", "select_one_by_key", "AttrInfo", "role_id", roleID)
		if is_ok then
			role_info.attr_info = attr_info
		end
		return role_info
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

function RoleMgr:GetBirthPos(  )
	local born_list = self.sceneMgr.scene_cfg.born_list
	local door_num = born_list and #born_list or 0
	if door_num > 0 then
		local random_index = math.random(1, door_num)
		local born_info = born_list[random_index]
		return born_info
	end
	return {pos_x=0, pos_y=0, pos_z=0}
end

function RoleMgr:InitPosInfo( baseInfo, targetDoor )
	if not baseInfo then return end
	local is_need_reset_pos = false
	if not baseInfo.scene_id or self.sceneMgr.curSceneID ~= baseInfo.scene_id or not baseInfo.pos_x or baseInfo.hp<=0 then
		is_need_reset_pos = true
		--relive
		if baseInfo.hp <= 0 then
			baseInfo.hp = baseInfo.attr_info and baseInfo.attr_info.hp or 0
		end
	end
	if is_need_reset_pos then
		-- local born_list = self.sceneMgr.scene_cfg.born_list
		-- local door_num = born_list and #born_list or 0
		-- if door_num > 0 then
		-- 	local random_index = math.random(1, door_num)
		-- 	local born_info = born_list[random_index]
		local born_info = self:GetBirthPos()
		baseInfo.pos_x = born_info.pos_x
		baseInfo.pos_y = born_info.pos_y
		baseInfo.pos_z = born_info.pos_z
		-- end
	end
end

function RoleMgr:RoleEnter( roleID, agent )
	if not self.roleList[roleID] then
		local scene_uid = SceneHelper:NewSceneUID(SceneConst.ObjectType.Role)
		local base_info = self:GetBaseInfoByRoleID(roleID)
		local looks_info = self:GetLooksInfoByRoleID(roleID)
		self:InitPosInfo(base_info)

		local handle = self.sceneMgr.aoi:add()
		print('Cat:RoleMgr.lua[enter] scene_uid, handle', scene_uid, handle)
		self.sceneMgr.aoi:set_user_data(handle, "uid", scene_uid)
		self.roleList[roleID] = {scene_uid=scene_uid, base_info=base_info, looks_info=looks_info, aoi_handle=handle, around_objs={}, radius_short=5000, radius_long=6000, skill_events_in_around={}, hurt_events_in_around={}}
		-- self.sceneMgr:SetSceneObj(scene_uid, self.roleList[roleID])
		-- self.aoi_handle_uid_map[handle] = scene_uid
		self.sceneMgr:SetAOI(handle, scene_uid)
		self.sceneMgr.aoi:set_pos(handle, base_info.pos_x, base_info.pos_y, base_info.pos_z)

		local entity = self:CreateRole(scene_uid, roleID, base_info.pos_x, base_info.pos_y, base_info.pos_z, handle, agent)
		self.sceneMgr:SetEntity(scene_uid, entity)
		self.entityMgr:SetComponentData(entity, "UMO.HP", {cur=base_info.hp, max=base_info.attr_info and base_info.attr_info.hp or 10000})
		local baseAttr = {}
		for k,v in pairs(base_info.attr_info) do
			local attrIndex = SceneConst.AttrStrMap[k]
			if attrIndex then
				baseAttr[attrIndex] = v
			end
		end
		self.entityMgr:SetComponentData(entity, "UMO.BaseAttr", baseAttr)
		self.entityMgr:SetComponentData(entity, "UMO.FightAttr", table.deep_copy(baseAttr))
		-- self.sceneMgr.aoi:set_user_data(handle, "entity", entity)
		local changeSceneStr = string.format("%s,%s,%s,%s,%s", self.sceneMgr.curSceneID, scene_uid, base_info.pos_x, base_info.pos_y, base_info.pos_z)
		-- local changeSceneStr = self.sceneMgr.curSceneID..","..","..base_info.pos_x..","..base_info.pos_y..","..base_info.pos_z
		local change_scene_event_info = {key=SceneConst.InfoKey.SceneChange, value=changeSceneStr}
		change_scene_event_info.is_private = true
		-- self.sceneMgr.eventMgr:AddSceneEvent(scene_uid, change_scene_event_info)
		self.roleList[roleID].change_obj_infos = SceneHelper.AddInfoItem(self.roleList[roleID].change_obj_infos, scene_uid, change_scene_event_info)
	end
end

local save_role_base_info = function ( role_id, base_info )
	local gameDBServer = skynet.localname(".GameDBServer")
	is_succeed = skynet.call(gameDBServer, "lua", "update", "RoleBaseInfo", "role_id", role_id, base_info)
end

function RoleMgr:RoleLeave( roleID )
	local role_info = self.roleList[roleID]
	-- print('Cat:RoleMgr.lua[role_leave_scene] roleID', roleID, role_info, self.sceneMgr.curSceneID)
	if not role_info then return end
	
	print('Cat:RoleMgr.lua[role_leave] role_info.scene_uid', role_info.scene_uid, role_info.aoi_handle)
	self.sceneMgr.aoi:remove(role_info.aoi_handle)
	-- self.sceneMgr.aoi_handle_uid_map[role_info.aoi_handle] = nil --角色离开后还需要通过aoi_handle获取ta的uid
	local entity = self.sceneMgr:GetEntity(role_info.scene_uid)
	role_info.base_info.attr_info = nil--需要存数据库了，属性字段要忽略掉
	local hp = self.sceneMgr.entityMgr:GetComponentData(entity, "UMO.HP")
	role_info.base_info.hp = hp.cur
	save_role_base_info(roleID, role_info.base_info)
	if role_info.ack_scene_get_objs_info_change then
		role_info.ack_scene_get_objs_info_change(true, {})
		role_info.ack_scene_get_objs_info_change = nil
	end
	if role_info.ack_scene_listen_skill_event then
		role_info.ack_scene_listen_skill_event(true, {})
		role_info.ack_scene_listen_skill_event = nil
	end
	if role_info.ack_scene_listen_hurt_event then
		role_info.ack_scene_listen_hurt_event(true, {})
		role_info.ack_scene_listen_hurt_event = nil
	end
	self.entityMgr:DestroyEntity(entity)
	self.sceneMgr:SetEntity(role_info.scene_uid, nil)
	self.roleList[roleID] = nil
end

function RoleMgr:GetMainRoleInfo( roleID )
	local role_info = self.roleList[roleID]
	local base_info = role_info and role_info.base_info or nil
	if base_info then
		local entity = self.sceneMgr:GetEntity(self.roleList[roleID].scene_uid)
		local hpData = self.sceneMgr.entityMgr:GetComponentData(entity, "UMO.HP")
		local result =  {
			role_info={
				scene_uid = self.roleList[roleID].scene_uid,
				role_id   = roleID,
				career    = base_info.career,
				name      = base_info.name,
				scene_id  = self.sceneMgr.curSceneID,
				pos_x     = base_info.pos_x,
				pos_y     = base_info.pos_y,
				pos_z     = base_info.pos_z,
				cur_hp    = hpData.cur,
				max_hp    = hpData.max,
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
				scene_id = self.sceneMgr.curSceneID,
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
		local role_info = self:GetRole(role_id)
		if not role_info then
			looks_info = {result=1}
		end
		looks_info = {
			result = 0,
			role_looks_info = {
				career   = role_info.base_info.career or 1,
				name     = role_info.base_info.name,
				hp 		 = hpData.cur,
				max_hp   = hpData.max,
				body 	 = role_info.looks_info.body or 0,
				hair 	 = role_info.looks_info.hair or 0,
				weapon   = role_info.looks_info.weapon or 0,
				wing 	 = role_info.looks_info.wing or 0,
				horse 	 = 0,
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

function RoleMgr:Relive( roleID, reliveType )
	local role_info = self.roleList[roleID]
	local baseInfo = role_info and role_info.base_info or nil
	local ret = ErrorCode.Unknow
	if baseInfo then
		local entity = self.sceneMgr:GetEntity(role_info.scene_uid)
		local hpData = self.sceneMgr.entityMgr:GetComponentData(entity, "UMO.HP")
		if hpData.cur > 0 then
			skynet.error("try to relive when hp greater than 0, role id : "..roleID.." hp:"..hpData.cur)
		end
		if baseInfo.attr_info and baseInfo.attr_info.hp then
			hpData.cur = baseInfo.attr_info.hp
			local change_target_pos_event_info = {key=SceneConst.InfoKey.HPChange, value=math.floor(baseInfo.attr_info.hp)..",relive", time=Time.timeMS}
			self.sceneMgr.eventMgr:AddSceneEvent(role_info.scene_uid, change_target_pos_event_info)
			ret = ErrorCode.Succeed
		else
			skynet.error("cannot find attr info when relive, role id : "..roleID)
		end
		if reliveType == SceneConst.ReliveType.SafeArea then
			local safePos = self:GetBirthPos()
			baseInfo.pos_x = safePos.pos_x
			baseInfo.pos_y = safePos.pos_y
			baseInfo.pos_z = safePos.pos_z
			if entity then
				self.sceneMgr.entityMgr:SetComponentData(entity, "UMO.Position", {x=baseInfo.pos_x, y=baseInfo.pos_y, z=baseInfo.pos_z})
				self.sceneMgr.entityMgr:SetComponentData(entity, "UMO.TargetPos", {x=baseInfo.pos_x, y=baseInfo.pos_y, z=baseInfo.pos_z})
			end
			self.sceneMgr.aoi:set_pos(role_info.aoi_handle, baseInfo.pos_x, baseInfo.pos_y, baseInfo.pos_z)
			local pos_info = baseInfo.pos_x..","..baseInfo.pos_y..","..baseInfo.pos_z
			local cur_time = Time.timeMS
			local change_pos_event_info = {key=SceneConst.InfoKey.PosChange, value=pos_info, time=cur_time}
			self.sceneMgr.eventMgr:AddSceneEvent(role_info.scene_uid, change_pos_event_info)
			ret = ErrorCode.Succeed
		end
	else
		skynet.error("cannot find role info when relive, role id : "..roleID)
	end
	return ret
end

function RoleMgr:ChangeAttr(roldID, attrList)
	local role_info = self.roleList[roldID]
	if not role_info then
		return
	end
	local entity = self.sceneMgr:GetEntity(role_info.scene_uid)
	local fightAttr = self.entityMgr:GetComponentData(entity, "UMO.FightAttr")
	for i, v in ipairs(attrList) do
		fightAttr[v[1]] = tonumber(v[2])
	end
	print("Role:ChangeAttr fightAttr start : ", fightAttr)
	PrintTable(fightAttr)
	print("Role:ChangeAttr fightAttr end")
end

return RoleMgr