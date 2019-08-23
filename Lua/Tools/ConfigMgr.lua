local errorCode = require("Config.ConfigErrorCode")	
local ConfigMgr = {}

function ConfigMgr:GetTaskCfg( taskID )
	local cfg = require("Config.ConfigTask")
	return cfg[taskID]
end

function ConfigMgr:GetNPCName( npcID )
	local cfg = require("Config.ConfigNPC")[npcID]
	return cfg and cfg.name or "神秘NPC"
end

function ConfigMgr:GetMonsterName( monsterID )
	local cfg = require("Config.ConfigMonster")[monsterID]
	return cfg and cfg.name or "神秘怪物"
end

function ConfigMgr:GetErrorDesc( errorID )
	return errorCode[errorID]
end

function ConfigMgr:GetSceneCfg( sceneID )
	return require("Config.config_scene_"..sceneID)
end

function ConfigMgr:GetGoodsCfg( goodsTypeID )
	local cfg = require("Config.ConfigGoods")
	if cfg[goodsTypeID] then
		return cfg[goodsTypeID]
	else
		if not self.defaultGoodsCfg then
			self.defaultGoodsCfg = {
				type_id = 100000, name = [[默认道具]], icon = 1000000, type = 1, 
			}
		end
		return self.defaultGoodsCfg
	end
end

function ConfigMgr:GetMonsterPosInScene( sceneID, monsterID )
	local cfg = self:GetSceneCfg(sceneID)
	if cfg and cfg.monster_list then
		for k,v in pairs(cfg.monster_list) do
			if v.monster_type_id == monsterID then
				return {x=v.pos_x/GameConst.RealToLogic, y=v.pos_y/GameConst.RealToLogic, z=v.pos_z/GameConst.RealToLogic}
			end
		end
	end
	return nil
end

function ConfigMgr:GetNPCPosInScene( sceneID, npcID )
	local cfg = self:GetSceneCfg(sceneID)
	if cfg and cfg.npc_list then
		for k,v in pairs(cfg.npc_list) do
			if v.npc_id == npcID then
				return {x=v.pos_x/GameConst.RealToLogic, y=v.pos_y/GameConst.RealToLogic, z=v.pos_z/GameConst.RealToLogic}
			end
		end
	end
	return nil
end

return ConfigMgr