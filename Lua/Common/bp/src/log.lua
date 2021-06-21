local oo = require("bp.common.lua_oo")
local bt_helper = require 'bp.src.bt_helper'

local mt = {}
local class_template = {type="log", __index=mt}
local log = oo.class(class_template)

function mt:start(graph)
	self.graph = graph
end

function mt:is_done(  )
	return true
end

function mt:update()
	-- slog.cat(self.str)
	local content = {}
	for i,v in ipairs(self) do
		content[#content + 1] = bt_helper.get_input(self, i, self[i])
	end
	slog.ailog(table.concat(content))
end

return log