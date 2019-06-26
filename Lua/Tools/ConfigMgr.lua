local ConfigMgr = {}

function ConfigMgr:GetTaskCfg( taskID )
	local cfg = require("Config.ConfigTask")
	return cfg[taskID]
end

function ConfigMgr:GetNPCName( npcID )
	local cfg = require("Config.ConfigNPC")[npcID]
	return cfg and cfg.name or "神秘NPC"
end

return ConfigMgr