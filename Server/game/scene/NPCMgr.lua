local npc_cfg = require "game.config.scene.config_npc"
local SceneHelper = require "game.scene.SceneHelper"
local SceneConst = require "game.scene.SceneConst"
-- local BP = require("Blueprint")
local NPCMgr = BaseClass()
local test_info = {
}

function NPCMgr:Init( sceneMgr, npcCfgList )
	self.sceneMgr = sceneMgr
	self.entityMgr = sceneMgr.entityMgr
	self.aoi = sceneMgr.aoi
	self.npcCfgList = npcCfgList

	self:InitArchetype()
	self:InitNPC()
end

function NPCMgr:InitArchetype(  )
	self.npc_archetype = self.entityMgr:CreateArchetype({
		"UMO.Position", "UMO.UID", "UMO.TypeID", "UMO.SceneObjType", "UMO.AOIHandle", 
	})
end

function NPCMgr:InitNPC(  )
	for i,v in ipairs(self.npcCfgList) do
		self:CreateNPC(v.npc_id, v.pos_x, v.pos_y, v.pos_z)
	end
end

function NPCMgr:CreateNPC( type_id, pos_x, pos_y, pos_z )
	local cfg = npc_cfg[type_id]
	if not cfg then return end
	
	local entity = self.entityMgr:CreateEntityByArcheType(self.npc_archetype)
	self.entityMgr:SetComponentData(entity, "UMO.Position", {x=pos_x, y=pos_y, z=pos_z})
	local scene_uid = SceneHelper:NewSceneUID(SceneConst.ObjectType.NPC)
	self.entityMgr:SetComponentData(entity, "UMO.UID", scene_uid)
	self.entityMgr:SetComponentData(entity, "UMO.TypeID", type_id)
	self.entityMgr:SetComponentData(entity, "UMO.SceneObjType", {value=SceneConst.ObjectType.NPC})

	local handle = self.aoi:add()
	self.aoi:set_user_data(handle, "uid", scene_uid)
	-- self.aoi:set_user_data(handle, "entity", entity)
	self.sceneMgr:SetAOI(handle, scene_uid)
	self.aoi:set_pos(handle, pos_x, pos_y, pos_z)
	self.entityMgr:SetComponentData(entity, "UMO.AOIHandle", {value=handle})

	self.sceneMgr:SetEntity(scene_uid, entity)

	return entity
end

return NPCMgr