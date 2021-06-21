local class = require "common.class"

local mt = class("entity_mgr")

function mt:on_new()
	if mt.active == nil then
		mt.active = self
	end
end

function mt:add_system()
	return nil
end

return mt