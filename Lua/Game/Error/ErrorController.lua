local ErrorController = {}

local this = ErrorController

function ErrorController.Init()
	this.InitEvents()
end

function ErrorController.InitEvents() 
	print('Cat:ErrorController.lua[14]InitEvents')
	local func = function(msg, stackTrace,msg_type)
		if msg_type == CS.UnityEngine.LogType.Warning or msg_type == CS.UnityEngine.LogType.Exception or msg_type == CS.UnityEngine.LogType.Assert or msg_type == CS.UnityEngine.LogType.Error then
			--显示报错界面
			-- local view = require("Game/GM/ErrorView")
			-- view:Open()
		end
	end
	CS.UnityEngine.Application.logMessageReceived = CS.UnityEngine.Application.logMessageReceived + func

end


return ErrorController