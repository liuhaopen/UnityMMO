
local skynet = require "skynet"

local CMD = {}

function CMD.get_handlers()
    local handlers = {}
    for k,v in pairs(CMD) do
        table.insert(handlers, k)
    end
    return handlers
end

function CMD.Chat_GetHistory( user_info, req_data )
    print("Cat:chat [start:11] ", user_info, req_data)
end

function CMD.Chat_Send( user_info, req_data )

end

function CMD.Chat_GetNew( user_info, req_data )

end

skynet.start (function ()
	skynet.dispatch ("lua", function (_, _, command, ...)
		local f = assert(CMD[command])
		skynet.retpack(f(...))
	end)
end)
