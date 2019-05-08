local monster_cfg = require "game.config.scene.config_monster"
local scene_helper = require "game.scene.scene_helper"
local scene_const = require "game.scene.scene_const"
local monster_const = require "game.scene.monster_const"
local monster_fsm_cfg = require "game.scene.ai.monster_fsm_cfg"
local BP = require("Blueprint")
local monster_mgr = {}
local test_info = {
	-- create_num = 1,--只创建1只怪物，方便调试
}

function monster_mgr:init( scene, cfg )
	self.scene_mgr = scene
	self.entity_mgr = scene.entity_mgr
	self.aoi = scene.aoi
	self.nest_cfg = cfg
	self.monster_entities = {}
	self.graphs_owners = {}

	self:init_archetype()
	self:init_monster()
end

function monster_mgr:getInstance()
	return monster_mgr
end

function monster_mgr:init_archetype(  )
	self.monster_archetype = self.entity_mgr:CreateArchetype({
		"umo.position", "umo.target_pos", "umo.uid", "umo.type_id", "umo.hp", "umo.scene_obj_type", "umo.monster_ai", "umo.monster_state", "umo.patrol_info", "umo.move_speed", "umo.aoi_handle", 
	})
end

function monster_mgr:init_monster(  )
	local create_num = 0
	for i,v in ipairs(self.nest_cfg) do
		local patrolInfo = {x=v.pos_x, y=v.pos_y, z=v.pos_z, radius=v.radius}
		for ii=1,v.monster_num do
			self:create_monster(v.monster_type_id, patrolInfo, v)
			create_num = create_num + 1
			if test_info.create_num and create_num >= test_info.create_num then
				return
			end
		end
	end
end

function monster_mgr:create_monster( type_id, patrolInfo )
	local cfg = monster_cfg[type_id]
	if not cfg then return end
	
	local radius = patrolInfo.radius/2
	local pos_x = patrolInfo.x + math.random(-radius, radius)
	local pos_y = patrolInfo.y + math.random(-radius, radius)
	local pos_z = patrolInfo.z + math.random(-radius, radius)

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
	self.entity_mgr:SetComponentData(monster, "umo.patrol_info", patrolInfo)
	self.entity_mgr:SetComponentData(monster, "umo.move_speed", {value=cfg.move_speed})

	local handle = self.aoi:add()
	self.aoi:set_pos(handle, pos_x, pos_y, pos_z)
	self.entity_mgr:SetComponentData(monster, "umo.aoi_handle", {value=handle})

	-- print('Cat:monster_mgr.lua[53] scene_uid', scene_uid, handle)
	self.scene_mgr.aoi_handle_uid_map[handle] = scene_uid
	self.scene_mgr.uid_entity_map[scene_uid] = monster

    self:init_graphs_for_mon(scene_uid, monster, self.entity_mgr, handle, self.aoi)

	table.insert(self.monster_entities, monster)
	return monster
end

function monster_mgr:init_graphs_for_mon( scene_uid, entity, entityMgr, aoi_handle, aoi )
	--此graph会在monster_ai_system.lua里update
	local owner = BP.GraphsOwner.Create()
	self.graphs_owners[scene_uid] = owner
	local graph = BP.FSM.FSMGraph.Create(monster_fsm_cfg)
	owner:AddGraph(graph)

	local blackboard = owner:GetBlackboard()
	blackboard:SetVariable("entity", entity)
	blackboard:SetVariable("entityMgr", entityMgr)
	blackboard:SetVariable("aoi_handle", aoi_handle)
	blackboard:SetVariable("aoi", aoi)
	blackboard:SetVariable("aoi_area", 250)
	blackboard:SetVariable("monsterMgr", self)
	owner:Start()
end

function monster_mgr:get_graphs_owner( uid )
	return self.graphs_owners[uid]
end

function monster_mgr:change_target_pos( entity, pos )
	self.entity_mgr:SetComponentData(entity, "umo.target_pos", pos)
	local uid = self.entity_mgr:GetComponentData(entity, "umo.uid")
	local change_target_pos_event_info = {key=SceneInfoKey.TargetPos, value=pos.x..","..pos.z, time=Time.timeMS}
	self.scene_mgr.event_mgr:AddSceneEvent(uid.value, change_target_pos_event_info)
end

return monster_mgr