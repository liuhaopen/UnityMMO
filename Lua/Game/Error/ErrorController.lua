local ErrorController = {}

local this = ErrorController

function ErrorController.Init()
	this.InitEvents()
end

function ErrorController.InitEvents() 
	print('Cat:ErrorController.lua[14]InitEvents')
	local func = function(msg, stackTrace,msg_type)
		if msg_type == UnityEngine.LogType.Warning or msg_type == UnityEngine.LogType.Exception or msg_type == UnityEngine.LogType.Assert or msg_type == UnityEngine.LogType.Error then
			--显示报错界面
			-- local view = require("Game/Error/ErrorView")
			-- view:Open()
		end
	end
	UnityEngine.Application.logMessageReceived = UnityEngine.Application.logMessageReceived + func

end


return ErrorController