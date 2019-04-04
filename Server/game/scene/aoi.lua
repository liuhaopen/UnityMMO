local aoi = {}
--以后有空优化时再改成c实现吧

function aoi:init( width, height )
	self.width = width
	self.height = height

	self.counter = 0
	self.nodes = {}
end

function aoi:add( mode )
	self.counter = self.counter + 1
	local node = {mode=mode}
	self.nodes[self.counter] = node
	return self.counter
end

function aoi:remove( handle )
	self.nodes[handle] = nil
end

function aoi:set_pos( handle, pos )
	self.nodes[handle] = self.nodes[handle] or {}
	self.nodes[handle].pos = pos
end

function aoi:get_pos( handle )
	return self.nodes[handle].pos
end

function aoi:is_near( pos1, pos2, distance )
	return math.distance(pos1, pos2) <= distance
end

function aoi:get_around_offset( handle, radius_short, radius_long )
	local node = self.nodes[handle]
	if not node then
		return
	end
	local result = {}
	for k,v in pairs(node.around_list) do
		local is_near = self:is_near(v.pos, node.pos, radius_long)
		if not is_near then
			table.insert(result, {is_enter=false, handle=v.handle})
		end
	end
	return result
end

return aoi