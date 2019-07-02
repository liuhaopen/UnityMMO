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

function ConfigMgr:GetMonsterPosInScene( sceneID, monsterID )
	local cfg = self:GetSceneCfg(sceneID)
	if cfg and cfg.monster_list then
		for k,v in pairs(cfg.monster_list) do
			if v.monster_type_id == monsterID then
				return {x=v.pos_x, y=v.pos_y, z=v.pos_z}
			end
		end
	end
	return nil
end

return ConfigMgr