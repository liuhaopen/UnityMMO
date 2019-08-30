require "game.scene.Global"
require "game.scene.com.Components"
local skynet = require "skynet"
local Ac = require "Action"
local ECS = require "ECS"
local BP = require "Blueprint"
local SceneConst = require "game.scene.SceneConst"
local SceneHelper = require "game.scene.SceneHelper"
RequireAllLuaFileInFolder("./game/scene/system")


local SceneMgr = BaseClass()

function SceneMgr:Constructor(  )
	self.uid_entity_map = {}
	self.aoi_uid_map = {}
end

function SceneMgr:GetEntity( uid )
	return self.uid_entity_map[uid]
end

function SceneMgr:SetEntity( uid, entity )
	self.uid_entity_map[uid] = entity
end

function SceneMgr:GetUIDByAOI( aoi )
	return self.aoi_uid_map[aoi]
end

function SceneMgr:SetAOI( aoi, uid )
	self.aoi_uid_map[aoi] = uid
end

local fork_loop_logic = function ( sceneMgr )
	Time:update()
	skynet.fork(function()
		while true do
			Time:update()
			BP.Time:Update(Time.time)
			sceneMgr.ecsSystemMgr:Update()
			sceneMgr.actionMgr:Update(Time.deltaTimeMS)
			skynet.sleep(1)
		end
	end)
end

--更新角色可视区域的感兴趣节点集合（玩家，怪物，NPC)
local update_around_objs = function ( sceneMgr, role_info )
	local objs = sceneMgr.aoi:get_around_offset(role_info.aoi_handle, role_info.radius_short, role_info.radius_long)
	local cur_time = Time.timeMS
	for aoi_handle, flag in pairs(objs) do
		local scene_uid = sceneMgr:GetUIDByAOI(aoi_handle)
		-- local scene_uid = sceneMgr.aoi:get_user_data(aoi_handle, "uid")
		local is_enter = flag==1
		-- print('Cat:SceneMgr.lua[57] flag, scene_uid, aoi_handle', is_enter, flag, role_info.scene_uid, role_info.aoi_handle, scene_uid, aoi_handle)
		if is_enter and scene_uid then
			role_info.around_objs[scene_uid] = scene_uid
			local entity = sceneMgr.uid_entity_map[scene_uid]
			if entity then
				local pos = sceneMgr.entityMgr:GetComponentData(entity, "UMO.Position")
				local scene_obj_type = sceneMgr.entityMgr:GetComponentData(entity, "UMO.SceneObjType")
				local target_pos = pos 
				if sceneMgr.entityMgr:HasComponent(entity, "UMO.TargetPos") then
					target_pos = sceneMgr.entityMgr:GetComponentData(entity, "UMO.TargetPos")
				end
				local type_id = sceneMgr.entityMgr:GetComponentData(entity, "UMO.TypeID")
				local eventStr = scene_obj_type.value..","..type_id..","..math.floor(pos.x)..","..math.floor(pos.y)..","..math.floor(pos.z)..","..math.floor(target_pos.x)..","..math.floor(target_pos.y)..","..math.floor(target_pos.z)
				if sceneMgr.entityMgr:HasComponent(entity, "UMO.HP") then
					local hp = sceneMgr.entityMgr:GetComponentData(entity, "UMO.HP")
					eventStr = eventStr..","..math.floor(hp.cur)..","..math.floor(hp.max)
				end
				role_info.change_obj_infos = SceneHelper.AddInfoItem(role_info.change_obj_infos, scene_uid, {key=SceneConst.InfoKey.EnterView, value=eventStr, time=cur_time})
			end
		elseif scene_uid then
			role_info.around_objs[scene_uid] = nil
			role_info.change_obj_infos = SceneHelper.AddInfoItem(role_info.change_obj_infos, scene_uid, {key=SceneConst.InfoKey.LeaveView, value="", time=cur_time})
		end
	end
end

local collect_events = function ( sceneMgr )
	for _,role_info in pairs(sceneMgr.roleMgr.roleList) do
		for _,interest_uid in pairs(role_info.around_objs) do
			local event_list = sceneMgr.eventMgr:GetSceneEvent(interest_uid)
			if event_list then
				for i,event_info in ipairs(event_list) do
					if not event_info.is_private then
						role_info.change_obj_infos = SceneHelper.AddInfoItem(role_info.change_obj_infos, interest_uid, event_info)
					end
				end
			end
		end
		--有些就算是自己的事件也要转发给其它玩家，比如自己复活
		local event_list = sceneMgr.eventMgr:GetSceneEvent(role_info.scene_uid)
		if event_list then
			for i,event_info in ipairs(event_list) do
				if SceneConst.InterestSelfEvent[event_info.key] then
					role_info.change_obj_infos = SceneHelper.AddInfoItem(role_info.change_obj_infos, role_info.scene_uid, event_info)
				end
			end
		end
	end
	sceneMgr.eventMgr:ClearAllSceneEvents()
end

--synch info at fixed time
local fork_loop_scene_info_change = function ( sceneMgr )
	skynet.fork(function()
		while true do
			collect_events(sceneMgr)
			for _,role_info in pairs(sceneMgr.roleMgr.roleList) do
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
	for _,role_info in pairs(sceneMgr.roleMgr.roleList) do
		for _,interest_uid in pairs(role_info.around_objs) do
			local event_list = sceneMgr.eventMgr:GetSkillEvent(interest_uid)
			if event_list and #event_list > 0 then
				for i,event_info in ipairs(event_list) do
					table.insert(role_info.skill_events_in_around, event_info)
				end
			end
			--Cat_Todo : 受击者是关注对象时也要发，因为有可能施法者离自己较远，不在关注列表
			local event_list = sceneMgr.eventMgr:GetHurtEvent(interest_uid)
			if event_list and #event_list > 0 then
				for i,event_info in ipairs(event_list) do
					table.insert(role_info.hurt_events_in_around, event_info)
				end
			end
		end
		--自己发的技能伤害事件列表也要告诉自己
		local event_list = sceneMgr.eventMgr:GetHurtEvent(role_info.scene_uid)
		if event_list and #event_list > 0 then
			for i,event_info in ipairs(event_list) do
				table.insert(role_info.hurt_events_in_around, event_info)
			end
		end
		local sort_func = function ( a, b )
			return a.time < b.time
		end
		table.sort(role_info.hurt_events_in_around, sort_func)
	end
	sceneMgr.eventMgr:ClearAllSkillEvents()
	sceneMgr.eventMgr:ClearAllHurtEvents()
end

--Notify combat events at fixed times
local fork_loop_fight_event = function ( sceneMgr )
	skynet.fork(function()
		while true do 
			collect_fight_events(sceneMgr)
			for k,role_info in pairs(sceneMgr.roleMgr.roleList) do
				if role_info.skill_events_in_around and #role_info.skill_events_in_around>0 and role_info.ack_scene_listen_skill_event then
					role_info.ack_scene_listen_skill_event(true, {events=role_info.skill_events_in_around})
					role_info.skill_events_in_around = {}
					role_info.ack_scene_listen_skill_event = nil
				end
				if role_info.hurt_events_in_around and #role_info.hurt_events_in_around>0 and role_info.ack_scene_listen_hurt_event then
					role_info.ack_scene_listen_hurt_event(true, {events=role_info.hurt_events_in_around})
					role_info.hurt_events_in_around = {}
					role_info.ack_scene_listen_hurt_event = nil
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
			for _,role_info in pairs(sceneMgr.roleMgr.roleList) do
				update_around_objs(sceneMgr, role_info)
			end
			skynet.sleep(10)
		end
	end)
end

function SceneMgr:Init( scene_id )
	-- fork_loop_time()

	self.roleMgr = require("game.scene.RoleMgr").New()
	self.monsterMgr = require("game.scene.MonsterMgr").New()
	self.npcMgr = require("game.scene.NPCMgr").New()
	self.fightMgr = require("game.scene.FightMgr").New()
	self.actionMgr = Ac.ActionMgr{}
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
	self.scene_cfg = require("config.scene.config_scene_"..scene_id)
	self.curSceneID = scene_id
	self.actionMgr:Init()
	self.roleMgr:Init(self)
	self.monsterMgr:Init(self, self.scene_cfg.monster_list)
	self.npcMgr:Init(self, self.scene_cfg.npc_list)
	self.fightMgr:Init(self)
	self.ecsSystemMgr:Init(self.ecs_world, self)
	self.eventMgr:Init(self)
	local FightHelper = require("game.scene.FightHelper")
	FightHelper:Init(self)
	--开始游戏循环
	fork_loop_logic(self)
	fork_loop_scene_info_change(self)
	fork_loop_fight_event(self)
	fork_loop_update_around(self)
end

function SceneMgr:GetCurSceneID(  )
	return self.curSceneID
end

return SceneMgr