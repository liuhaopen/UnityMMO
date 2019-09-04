local monster_cfg = require "game.config.scene.config_monster"
local SceneHelper = require "game.scene.SceneHelper"
local SceneConst = require "game.scene.SceneConst"
local MonsterFSM = require "game.scene.ai.MonsterFSM"
local BP = require("Blueprint")
local SpeedData = require("game.scene.com.SpeedData")
local MonsterMgr = BaseClass()
local test_info = {
	-- create_num = 1,--只创建1只怪物，方便调试
	-- create_mon_types = {2000}
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
		"UMO.Position", "UMO.TargetPos", "UMO.UID", "UMO.TypeID", "UMO.HP", "UMO.SceneObjType", "UMO.MonsterAI", "UMO.PatrolInfo", "UMO.MoveSpeed", "UMO.AOIHandle", "UMO.Beatable", "UMO.DamageEvents", "UMO.Buff", 
		"UMO.BaseAttr", "UMO.FightAttr", "UMO.Ability"
	})
end

function MonsterMgr:InitMonster(  )
	local create_num = 0
	for i,v in ipairs(self.nest_cfg) do
		local mons_type_ok = true
		if test_info.create_mon_types then
			mons_type_ok = false
			create_num = 0
			for _,test_mons_type in ipairs(test_info.create_mon_types) do
				if v.monster_type_id == test_mons_type then
					mons_type_ok = true 
					break
				end
			end
		elseif test_info.create_num and create_num >= test_info.create_num then
			break
		end
		if mons_type_ok then
			local patrolInfo = {x=v.pos_x, y=v.pos_y, z=v.pos_z, radius=v.radius}
			for ii=1,v.monster_num do
				self:CreateMonster(v.monster_type_id, patrolInfo, v)
				create_num = create_num + 1
				if test_info.create_num and create_num >= test_info.create_num then
					break
				end
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
	self.entityMgr:SetComponentData(monster, "UMO.TypeID", type_id)
	self.entityMgr:SetComponentData(monster, "UMO.SceneObjType", {value=SceneConst.ObjectType.Monster})
	self.entityMgr:SetComponentData(monster, "UMO.MonsterAI", scene_uid)
	self.entityMgr:SetComponentData(monster, "UMO.PatrolInfo", patrolInfo)
	-- self.entityMgr:SetComponentData(monster, "UMO.MoveSpeed", {value=cfg.move_speed})
	self.entityMgr:SetComponentData(monster, "UMO.MoveSpeed", SpeedData.New(cfg.move_speed))
	self.entityMgr:SetComponentData(monster, "UMO.BaseAttr", cfg.attr_list)
	self.entityMgr:SetComponentData(monster, "UMO.FightAttr", table.deep_copy(cfg.attr_list))
	local maxHp = cfg.attr_list[SceneConst.Attr.HP] or 0
	if maxHp <= 0 then
		skynet.error("monster max hp is 0, please check the configh_monster.lua.mon id : "..cfg.type_id)
	end
	self.entityMgr:SetComponentData(monster, "UMO.HP", {cur=maxHp, max=maxHp})

	local handle = self.aoi:add()
	self.aoi:set_user_data(handle, "uid", scene_uid)
	-- self.aoi:set_user_data(handle, "entity", monster)
	self.aoi:set_pos(handle, pos_x, pos_y, pos_z)
	self.entityMgr:SetComponentData(monster, "UMO.AOIHandle", {value=handle})

	self.sceneMgr:SetAOI(handle, scene_uid)
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

return MonsterMgr