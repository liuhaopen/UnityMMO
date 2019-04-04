local monster_cfg = require "Config.scene.config_monster"
local scene_helper = require "game.scene.scene_helper"
local scene_const = require "game.scene.scene_const"

local monster_mgr = {}

function monster_mgr:init( entity_mgr, cfg )
	self.entity_mgr = entity_mgr
	self.nest_cfg = cfg

	self:init_archetype()
	self:init_monster()
end

function monster_mgr:init_archetype(  )
	self.monster_archetype = self.entity_mgr:CreateArchetype({
		"umo.position", "umo.uid", "umo.type_id", "umo.hp"
	})
	-- self.monster_nest_archetype = self.entity_mgr:CreateArchetype({
	-- 	"umo.position", "umo.monster_nest",
	-- })
end

function monster_mgr:init_monster(  )
	self.monster_entities = {}
	for i,v in ipairs(self.nest_cfg) do
		for ii=1,v.monster_num do
			local random_pos_x, random_pos_y, random_pos_z = v.pos_x, v.pos_y, v.pos_z
			local monster = self:create_monster(v.monster_type_id, random_pos_x, random_pos_y, random_pos_z)
			table.insert(self.monster_entities, monster)
		end
	end
end

function monster_mgr:create_monster( type_id, pos_x, pos_y, pos_z )
	local cfg = monster_cfg[type_id]
	if not cfg then return end
	
	local monster = self.entity_mgr:CreateEntityByArcheType(self.monster_archetype)
	self.entity_mgr:SetComponentData(monster, "umo.position", {x=pos_x, y=pos_y, z=pos_z})
	self.entity_mgr:SetComponentData(monster, "umo.uid", {value=scene_helper:new_scene_uid(SceneObjectType.Monster)})
	self.entity_mgr:SetComponentData(monster, "umo.type_id", {value=type_id})
	self.entity_mgr:SetComponentData(monster, "umo.hp", {cur=cfg.max_hp, max=cfg.max_hp})

	return monster
end

return monster_mgr