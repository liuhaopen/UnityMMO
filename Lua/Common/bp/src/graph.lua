local oo = require("bp.common.lua_oo")
local bb_mgr = require("bp.src.bb_mgr")
local slot_mgr = require("bp.src.slot_mgr")

--管理一颗树的数据，包含用户自定义数据，黑板，孔的数据
local mt = {}
local class_template = {type="graph", __index=mt}
local graph = oo.class(class_template)

function mt:init( value )
	assert(value, "bp graph root node cannot is nil!")
	self.root = value
	self.root:start(self)
end

function mt:update( dt )
	return self.root:update(dt)
end

function mt:is_done(  )
	return self.root:is_done()
end

function mt:destroy(  )
	if self.on_destroy_cbs then
		for _,v in pairs(self.on_destroy_cbs) do
			v:on_destroy(self)
		end
		self.on_destroy_cbs = nil
	end
end

function mt:register_on_destroy( node )
	self.on_destroy_cbs = self.on_destroy_cbs or {}
	assert(node.on_destroy, "must has on_destroy method!")
	self.on_destroy_cbs[node] = node
end

function mt:get_root( )
	return self.root
end

function mt:set_data(data)
	self.data = data
end

function mt:get_data(data)
	return self.data
end

function mt:get_bb_mgr()
	if not self.bb_mgr then
		self.bb_mgr = bb_mgr{}
		self.bb_mgr:start(self)
	end
	return self.bb_mgr
end

local bb_func_list = {
	"set_bb", "get_bb"
}
for i,func_name in ipairs(bb_func_list) do
	mt[func_name] = function(self, ...)
		local mgr = self:get_bb_mgr()
		return mgr[func_name](mgr, ...)
	end
end

function mt:get_slot_mgr()
	if not self.slot_mgr then
		self.slot_mgr = slot_mgr{}
		self.slot_mgr:start(self)
	end
	return self.slot_mgr
end

local slot_func_list = {
	"get_slot_value", "set_slot_value", "register_slot", 
}
for i,func_name in ipairs(slot_func_list) do
	mt[func_name] = function(self, ...)
		local mgr = self:get_slot_mgr()
		return mgr[func_name](mgr, ...)
	end
end
return graph