local global = require "global"

local mt = {}
--[[
uid：后端那边发过来的唯一角色 id，只有玩家才有
actor：场景的角色，包括玩家，怪物，npc，陷阱等各种各样的东西
aid：即 actor_id，即前端这边的唯一 id，适用于所有 actor
actor_ghost：场景里只显示名字的 actor，用于玩家太多时减少显示
--]]
function mt:init()
	self.actor_archetype = global.entity_mgr:get_archetype("move_info", "aoi", "visual", "actor_info")
	self.ghost_archetype = global.entity_mgr:get_archetype("move_info", "aoi", "actor_ghost_info")
	self.actors = {}
	self.aid = 0
end

function mt:add_actor_entity(e)
	self.aid = self.aid + 1
	self.actors[self.aid] = e
end

function mt:add_actor(info)
	local e = global.entity_mgr:create_entity_by_archetype(self.actor_archetype)
	global.entity_mgr:set_component(e, "actor_info", info)
	info.aid = self.aid
	self:add_actor_entity(e)
end

function mt:add_actor_ghost(info)
	local e = global.entity_mgr:create_entity_by_archetype(self.ghost_archetype)
	self:add_actor_entity(e)
end

global.actor_mgr = mt
return mt