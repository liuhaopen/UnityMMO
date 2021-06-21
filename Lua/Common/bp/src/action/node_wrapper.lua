local mt = {}

--因为不同项目的节点接口不同，比如get_pos，set_pos之类的，所以就用这个中间层粘合，请传入你们项目的wrapper，其实现了具体的一系列接口
function mt:init_wrapper( wrapper )
	self.wrapper = wrapper
end

function mt:get_wrapper(  )
	assert(self.wrapper, "has not init wrapper!")
	return self.wrapper
end

--[[
例如：
local mt = {}
function mt.get_anchored_pos(node)
	if node.get_anchoredPosition then
		return node:get_anchoredPosition()
	elseif node.GetAnchoredPos then
		return node:GetAnchoredPos()
	end
end

function mt.set_anchored_pos(node, x, y)
	if node.set_anchoredPosition then
		return node:set_anchoredPosition(x, y)
	elseif node.SetAnchoredPos then
		return node:SetAnchoredPos(x, y)
	end
end

function mt.get_local_pos(node)
	if node.get_local_position then
		return node:get_local_position()
	elseif node.GetLocalPos then
		return node:GetLocalPos()
	end
end

function mt.set_local_pos(node, x, y, z)
	if node.set_local_position then
		return node:set_local_position(x, y, z)
	elseif node.SetLocalPos then
		return node:SetLocalPos(x, y, z)
	end
end

function mt.get_abs_pos(node)
	if node.get_position then
		return node:get_position()
	elseif node.GetPos then
		return node:GetPos()
	end
end

function mt.set_local_pos(node, x, y, z)
	if node.set_position then
		return node:set_position(x, y, z)
	elseif node.SetPos then
		return node:SetPos(x, y, z)
	end
end

local node_wrapper = require "bp.src.action.node_wrapper"
node_wrapper:init_wrapper(mt)
--]]

return mt