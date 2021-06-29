local global = require "global"

local mt = {}
--[[
uuid：后端那边发过来的唯一 id，只有玩家才有
cuuid：全称 client_uuid，即前端那边的唯一 id，适用于所有场景对象，包括玩家，怪物，npc，陷阱之类的
--]]
function mt:init()
	self.role_archetype = global.entity_mgr:get_archetype("move_info", "aoi", "visual", "actor_info")
	self.actors = {}
	self.cuuid = 0
end

function mt:add_actor(info)
	local e = global.entity_mgr:create_entity_by_archetype(self.role_archetype)
	self.cuuid = self.cuuid + 1
	info.cuuid = self.cuuid
	global.entity_mgr:set_component(e, "actor_info", info)
	self.actors[self.cuuid] = e
end

global.actor_mgr = mt
return mt