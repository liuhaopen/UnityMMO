local monster_cfg = require "game.config.scene.config_monster"
local SceneHelper = require "game.scene.SceneHelper"
local SceneConst = require "game.scene.SceneConst"
local MonsterFSM = require "game.scene.ai.MonsterFSM"
local BP = require("Blueprint")
local MonsterMgr = BaseClass()
local test_info = {
	create_num = 1,--只创建1只怪物，方便调试
}

function MonsterMgr:Init( sceneMgr, cfg )
	self.sceneMgr = sceneMgr
	self.entityMgr = sceneMgr.entityMgr
	self.aoi = sceneMgr.aoi
	self.nest_cfg = cfg
	self.monster_entities = {}
	self.graphs_owners = {}
	self.fsms = {}

	self:InitArchetype()
	self:InitMonster()
end

function MonsterMgr:InitArchetype(  )
	self.monster_archetype = self.entityMgr:CreateArchetype({
		"UMO.Position", "UMO.TargetPos", "UMO.UID", "UMO.TypeID", "UMO.HP", "UMO.SceneObjType", "UMO.MonsterAI", "UMO.PatrolInfo", "UMO.MoveSpeed", "UMO.AOIHandle", "UMO.DeadState", "UMO.DamageEvents"
	})
end

function MonsterMgr:InitMonster(  )
	local create_num = 0
	for i,v in ipairs(self.nest_cfg) do
		local patrolInfo = {x=v.pos_x, y=v.pos_y, z=v.pos_z, radius=v.radius}
		for ii=1,v.monster_num do
			self:CreateMonster(v.monster_type_id, patrolInfo, v)
			create_num = create_num + 1
			if test_info.create_num and create_num >= test_info.create_num then
				return
			end
		end
	end
end

function MonsterMgr:CreateMonster( type_id, patrolInfo )
	local cfg = monster_cfg[type_id]
	if not cfg then return end
	
	local radius = patrolInfo.radius/2
	local pos_x = patrolInfo.x + math.random(-radius, radius)
	local pos_y = patrolInfo.y + math.random(-radius, radius)
	local pos_z = patrolInfo.z + math.random(-radius, radius)

	local monster = self.entityMgr:CreateEntityByArcheType(self.monster_archetype)
	self.entityMgr:SetComponentData(monster, "UMO.Position", {x=pos_x, y=pos_y, z=pos_z})
	self.entityMgr:SetComponentData(monster, "UMO.TargetPos", {x=pos_x, y=pos_y, z=pos_z})
	local scene_uid = SceneHelper:NewSceneUID(SceneConst.ObjectType.Monster)
	self.entityMgr:SetComponentData(monster, "UMO.UID", scene_uid)
	self.entityMgr:SetComponentData(monster, "UMO.TypeID", {value=type_id})
	self.entityMgr:SetComponentData(monster, "UMO.HP", {cur=cfg.max_hp, max=cfg.max_hp})
	self.entityMgr:SetComponentData(monster, "UMO.SceneObjType", {value=SceneConst.ObjectType.Monster})
	self.entityMgr:SetComponentData(monster, "UMO.MonsterAI", scene_uid)
	self.entityMgr:SetComponentData(monster, "UMO.PatrolInfo", patrolInfo)
	self.entityMgr:SetComponentData(monster, "UMO.MoveSpeed", {value=cfg.move_speed})

	local handle = self.aoi:add()
	self.aoi:set_user_data(handle, "uid", scene_uid)
	-- self.aoi:set_user_data(handle, "entity", monster)
	self.aoi:set_pos(handle, pos_x, pos_y, pos_z)
	self.entityMgr:SetComponentData(monster, "UMO.AOIHandle", {value=handle})

	-- print('Cat:MonsterMgr.lua[53] scene_uid', scene_uid, handle)
	-- self.sceneMgr.aoi_handle_uid_map[handle] = scene_uid
	self.sceneMgr:SetEntity(scene_uid, monster)

    self:InitGraphsForMon(scene_uid, monster, self.entityMgr, handle, self.aoi, cfg)

	table.insert(self.monster_entities, monster)
	return monster
end

function MonsterMgr:InitGraphsForMon( scene_uid, entity, entityMgr, aoi_handle, aoi, cfg )
	--此graph会在monster_ai_system.lua里update
	local owner = BP.GraphsOwner.Create()
	self.graphs_owners[scene_uid] = owner
	local graph = BP.FSM.FSMGraph.Create(MonsterFSM)
	owner:AddGraph(graph)
	self.fsms[scene_uid] = graph

	local blackboard = owner:GetBlackboard()
	blackboard:SetVariable("entity", entity)
	blackboard:SetVariable("aoi_handle", aoi_handle)
	blackboard:SetVariable("sceneMgr", self.sceneMgr)
	blackboard:SetVariable("cfg", cfg)
	owner:Start()
end

function MonsterMgr:GetGraphsOwner( uid )
	return self.graphs_owners[uid]
end

function MonsterMgr:GetFSM( uid )
	return self.fsms[uid]
end

function MonsterMgr:TriggerState( uid, stateName )
	local fsm = self.fsms[uid]
	if not fsm then return end
	fsm:TriggerState(stateName)
end

function MonsterMgr:ChangeTargetPos( entity, pos )
	self.entityMgr:SetComponentData(entity, "UMO.TargetPos", pos)
	local uid = self.entityMgr:GetComponentData(entity, "UMO.UID")
	local change_target_pos_event_info = {key=SceneConst.InfoKey.TargetPos, value=math.floor(pos.x)..","..math.floor(pos.z), time=Time.timeMS}
	self.sceneMgr.eventMgr:AddSceneEvent(uid, change_target_pos_event_info)
end

return MonsterMgr