local skynet = require "skynet"
local SceneConst = require "game.scene.SceneConst"

local RoleMgr = BaseClass()

function RoleMgr:Init( sceneMgr )
	self.sceneMgr = sceneMgr
	self.entityMgr = sceneMgr.entityMgr
	self.aoi = sceneMgr.aoi
	self.roleList = {}
	self:InitArchetype()
end

function RoleMgr:InitArchetype(  )
	self.role_archetype = self.entityMgr:CreateArchetype({
		"UMO.Position", "UMO.TargetPos", "UMO.UID", "UMO.TypeID", "UMO.HP", "UMO.SceneObjType", "UMO.MoveSpeed", "UMO.AOIHandle", 
	})
end

function RoleMgr:CreateRole( uid, roleID, pos_x, pos_y, pos_z, aoi_handle )
	local role = self.entityMgr:CreateEntityByArcheType(self.role_archetype)
	self.entityMgr:SetComponentData(role, "UMO.Position", {x=pos_x, y=pos_y, z=pos_z})
	self.entityMgr:SetComponentData(role, "UMO.TargetPos", {x=pos_x, y=pos_y, z=pos_z})
	self.entityMgr:SetComponentData(role, "UMO.UID", {value=uid})
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

function RoleMgr:GetLooksInfoByRoleID(  )
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


return RoleMgr