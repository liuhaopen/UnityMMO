require "game.scene.Global"
local skynet = require "skynet"
local ECS = require "ECS"
local BP = require "Blueprint"
local SceneConst = require "game.scene.SceneConst"
local SceneHelper = require "game.scene.SceneHelper"
RequireAllLuaFileInFolder("./game/scene/System")


local SceneMgr = BaseClass()

function SceneMgr:Constructor(  )
	self.scene_uid = 0
	self.role_list = {}
	self.npc_list = {}
	self.object_list = {}--the scene object includes the role monster npc
	self.ecs_world = false
	self.uid_entity_map = {}
	
end

local fork_loop_ecs = function ( sceneMgr )
	skynet.fork(function()
		while true do
			sceneMgr.ecsSystemMgr:update()
			skynet.sleep(3)
		end
	end)
end

local fork_loop_time = function (  )
	Time:update()
	skynet.fork(function()
		while true do
			Time:update()
			BP.Time:Update(Time.time)
			skynet.sleep(1)
		end
	end)
end

--更新角色可视区域的感兴趣节点集合（玩家，怪物，NPC)
local update_around_objs = function ( sceneMgr, role_info )
	local objs = sceneMgr.aoi:get_around_offset(role_info.aoi_handle, role_info.radius_short, role_info.radius_long)
	local cur_time = Time.timeMS
	for aoi_handle, flag in pairs(objs) do
		-- local scene_uid = sceneMgr.aoi_handle_uid_map[aoi_handle]
		local scene_uid = sceneMgr.aoi:get_user_data(aoi_handle, "uid")
		print('Cat:scene.lua[76] scene_uid', scene_uid)
		local is_enter = flag==1
		-- print('Cat:scene.lua[67] flag, ', flag, aoi_handle, role_info.aoi_handle, scene_uid)
		if is_enter then
			role_info.around_objs[scene_uid] = scene_uid
			local entity = sceneMgr.uid_entity_map[scene_uid]
			if entity then
				local pos = sceneMgr.entityMgr:GetComponentData(entity, "UMO.Position")
				local target_pos = sceneMgr.entityMgr:GetComponentData(entity, "UMO.TargetPos")
				local scene_obj_type = sceneMgr.entityMgr:GetComponentData(entity, "UMO.SceneObjType")
				local type_id = sceneMgr.entityMgr:GetComponentData(entity, "UMO.TypeID")
				role_info.change_obj_infos = SceneHelper.add_info_item(role_info.change_obj_infos, scene_uid, {key=SceneConst.InfoKey.EnterView, value=scene_obj_type.value..","..type_id.value..","..pos.x..","..pos.y..","..pos.z..","..target_pos.x..","..target_pos.y..","..target_pos.z, time=cur_time})
			end
		else
			if scene_uid then
				role_info.around_objs[scene_uid] = nil
			end
			role_info.change_obj_infos = SceneHelper.add_info_item(role_info.change_obj_infos, scene_uid, {key=SceneConst.InfoKey.LeaveView, value="", time=cur_time})
		end
	end
end

local collect_events = function ( sceneMgr )
	for _,role_info in pairs(sceneMgr.role_list) do
		for _,interest_uid in pairs(role_info.around_objs) do
			-- local event_list = sceneMgr.event_list[interest_uid]
			local event_list = sceneMgr.eventMgr:GetSceneEvent(interest_uid)
			if event_list then
				for i,event_info in ipairs(event_list) do
					role_info.change_obj_infos = SceneHelper.add_info_item(role_info.change_obj_infos, interest_uid, event_info)
				end
			end
		end
	end
	-- sceneMgr.event_list = {}
	sceneMgr.eventMgr:ClearAllSceneEvents()
end

local fork_loop_scene_info_change = function ( sceneMgr )
	skynet.fork(function()
		while true do
			collect_events(sceneMgr)
			--synch info at fixed time
			for _,role_info in pairs(sceneMgr.role_list) do
				if role_info.change_obj_infos and role_info.ack_scene_get_objs_info_change then
					role_info.ack_scene_get_objs_info_change(true, role_info.change_obj_infos)
					role_info.change_obj_infos = nil
					role_info.ack_scene_get_objs_info_change = nil
				end
			end
			skynet.sleep(5)
		end
	end)
end

local collect_fight_events = function ( sceneMgr )
	for _,role_info in pairs(sceneMgr.role_list) do
		for _,interest_uid in pairs(role_info.around_objs) do
			-- local event_list = sceneMgr.fight_events[interest_uid]
			local event_list = sceneMgr.eventMgr:GetFightEvent(interest_uid)
			if event_list then
				for i,event_info in ipairs(event_list) do
					table.insert(role_info.fight_events_in_around, event_info)
				end
			end
		end
	end
	sceneMgr.eventMgr:ClearAllFightEvents()
	-- sceneMgr.fight_events = {}
end

--定时合批发送战斗事件
local fork_loop_fight_event = function ( sceneMgr )
	skynet.fork(function()
		while true do 
			collect_fight_events(sceneMgr)
			for k,role_info in pairs(sceneMgr.role_list) do
				if role_info.fight_events_in_around and #role_info.fight_events_in_around>0 and role_info.ack_scene_listen_fight_event then
					print("Cat:scene [start:138] role_info.fight_events_in_around:", role_info.fight_events_in_around)
					PrintTable(role_info.fight_events_in_around)
					print("Cat:scene [end]")
					role_info.ack_scene_listen_fight_event(true, {fight_events=role_info.fight_events_in_around})
					role_info.fight_events_in_around = {}
					role_info.ack_scene_listen_fight_event = nil
				end
			end
			skynet.sleep(5)
		end
	end)
end

--定时更新每个玩家的感兴趣列表
local fork_loop_update_around = function ( sceneMgr )
	skynet.fork(function()
		while true do 
			for _,role_info in pairs(sceneMgr.role_list) do
				update_around_objs(sceneMgr, role_info)
			end
			skynet.sleep(10)
		end
	end)
end

function SceneMgr:Init( scene_id )
	fork_loop_time()

	self.roleMgr = require("game.scene.RoleMgr").New()
	self.monsterMgr = require("game.scene.MonsterMgr").New()
	self.fightMgr = require("game.scene.FightMgr").New()

	--管理所有的ECS System
	self.ecsSystemMgr = require("game.scene.ECSSystemMgr").New()

	--场景里的节点状态变更事件及战斗事件管理器
	self.eventMgr = require("game.scene.EventMgr").New()

	--全称area of interest
	self.aoi = require "game.scene.aoi"

	--注册所有的蓝图类
	local BlueprintRegister = require "game.scene.ai.BlueprintRegister"
	BlueprintRegister:register_all()

	self.aoi:init()
	self.ecs_world = ECS.InitWorld("scene_world")
	self.entityMgr = ECS.World.Active:GetOrCreateManager(ECS.EntityManager.Name)
	self.scene_cfg = require("Config.scene.config_scene_"..scene_id)
	self.cur_scene_id = scene_id
	self.roleMgr:Init(self)
	self.monsterMgr:Init(self, self.scene_cfg.monster_list)
	self.fightMgr:Init(self)
	self.ecsSystemMgr:Init(self.ecs_world, self)
	self.eventMgr:Init(self)

	fork_loop_ecs(self)
	fork_loop_scene_info_change(self)
	fork_loop_fight_event(self)
	fork_loop_update_around(self)
end

function SceneMgr:AddRole( roleID )
	if not self.role_list[roleID] then
		local scene_uid = SceneHelper:new_scene_uid(SceneConst.ObjectType.Role)
		local base_info = self.roleMgr:GetBaseInfoByRoleID(roleID)
		local looks_info = self.roleMgr:GetLooksInfoByRoleID(roleID)
		self.roleMgr:InitPosInfo(base_info)

		local handle = self.aoi:add()
		self.aoi:set_user_data(handle, "uid", scene_uid)
		self.role_list[roleID] = {scene_uid=scene_uid, base_info=base_info, looks_info=looks_info, aoi_handle=handle, around_objs={}, radius_short=5000, radius_long=6000, fight_events_in_around={}}
		self.object_list[scene_uid] = self.role_list[roleID]
		-- self.aoi_handle_uid_map[handle] = scene_uid
		self.aoi:set_pos(handle, base_info.pos_x, base_info.pos_y, base_info.pos_z)

		local entity = self.roleMgr:CreateRole(scene_uid, roleID, base_info.pos_x, base_info.pos_y, base_info.pos_z, handle)
		self.uid_entity_map[scene_uid] = entity
		self.aoi:set_user_data(handle, "entity", entity)
	end
end

local init_npc = function (  )
	if not sceneMgr.scene_cfg or not sceneMgr.scene_cfg.npc_list then return end
	
	for k,v in pairs(sceneMgr.scene_cfg.npc_list) do
		local npc = {}
		npc.id = v.npc_id
		npc.uid = SceneHelper:new_scene_uid(SceneConst.ObjectType.NPC)
		npc.pos_x = v.pos_x
		npc.pos_y = v.pos_y
		npc.pos_z = v.pos_z
		local npc_entity = sceneMgr.entityMgr:CreateEntityByArcheType(sceneMgr.npc_archetype)
		table.insert(sceneMgr.npc_list, npc)
	end
end
return SceneMgr