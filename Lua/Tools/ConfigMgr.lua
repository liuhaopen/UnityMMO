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

return ConfigMgr