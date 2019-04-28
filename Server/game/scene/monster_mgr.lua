local monster_cfg = require "Config.scene.config_monster"
local scene_helper = require "game.scene.scene_helper"
local scene_const = require "game.scene.scene_const"
local monster_const = require "game.scene.monster_const"
local BP = require("Blueprint")

local monster_mgr = {}

function monster_mgr:init( scene, cfg )
	self.scene_mgr = scene
	self.entity_mgr = scene.entity_mgr
	self.aoi = scene.aoi
	self.nest_cfg = cfg
	self.monster_entities = {}

	self:init_archetype()
	self:init_monster()
end

function monster_mgr:init_archetype(  )
	self.monster_archetype = self.entity_mgr:CreateArchetype({
		"umo.position", "umo.target_pos", "umo.uid", "umo.type_id", "umo.hp", "umo.scene_obj_type", "umo.monster_ai", "umo.monster_state"
	})
end

function monster_mgr:init_monster(  )
	for i,v in ipairs(self.nest_cfg) do
		for ii=1,v.monster_num do
			local random_pos_x = v.pos_x + math.random(-v.radius/2, v.radius/2)
			local random_pos_y = v.pos_y + math.random(-v.radius/2, v.radius/2)
			local random_pos_z = v.pos_z + math.random(-v.radius/2, v.radius/2)
			self:create_monster(v.monster_type_id, random_pos_x, random_pos_y, random_pos_z)
		end
	end
end

function monster_mgr:create_monster( type_id, pos_x, pos_y, pos_z )
	local cfg = monster_cfg[type_id]
	if not cfg then return end
	
	local monster = self.entity_mgr:CreateEntityByArcheType(self.monster_archetype)
	self.entity_mgr:SetComponentData(monster, "umo.position", {x=pos_x, y=pos_y, z=pos_z})
	self.entity_mgr:SetComponentData(monster, "umo.target_pos", {x=pos_x, y=pos_y, z=pos_z})
	local scene_uid = scene_helper:new_scene_uid(SceneObjectType.Monster)
	self.entity_mgr:SetComponentData(monster, "umo.uid", {value=scene_uid})
	self.entity_mgr:SetComponentData(monster, "umo.type_id", {value=type_id})
	self.entity_mgr:SetComponentData(monster, "umo.hp", {cur=cfg.max_hp, max=cfg.max_hp})
	self.entity_mgr:SetComponentData(monster, "umo.scene_obj_type", {value=SceneObjectType.Monster})
	self.entity_mgr:SetComponentData(monster, "umo.monster_ai", {ai_id=scene_uid})
	self.entity_mgr:SetComponentData(monster, "umo.monster_state", {state=monster_const.monster_state.patrol, sub_state=monster_const.monster_sub_state.enter})

	local handle = self.aoi:add()
	self.aoi:set_pos(handle, pos_x, pos_y, pos_z)

	-- print('Cat:monster_mgr.lua[53] scene_uid', scene_uid, handle)
	self.scene_mgr.aoi_handle_uid_map[handle] = scene_uid
	self.scene_mgr.uid_entity_map[scene_uid] = monster

	table.insert(self.monster_entities, monster)
	return monster
end

return monster_mgr