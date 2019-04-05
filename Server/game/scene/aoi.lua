local aoi = {}
--以后有空优化时再改成c实现吧

function aoi:init( )
	self.counter = 0
	self.nodes = {}
	self.link_heads = {}
end

function aoi:add(  )
	self.counter = self.counter + 1
	local node = {
		handle = self.counter
	}
	self.nodes[self.counter] = node
	return self.counter
end

function aoi:remove( handle )
	self.nodes[handle] = nil
end

function aoi:set_pos( handle, pos )
	local node = self.nodes[handle]
	if not node then
		return
	end
	node.pos = pos
	if not node.next and not node.last then

	end
end

function aoi:get_pos( handle )
	return self.nodes[handle].pos
end

function aoi:is_near( pos1, pos2, distance )
	return math.distance(pos1, pos2) <= distance
end

local get_distance = function ( pos1, pos2 )
	
end

function aoi:get_around_offset( handle, radius_short, radius_long )
	local node = self.nodes[handle]
	if not node then
		return
	end
	local result = {}
	local next_node = node.next
	while next_node ~= nil do
		local is_near = self:is_near(next_node.pos, node.pos, radius_short)
		if is_near  then
			if not node.around_list[next_node.handle] then
				node.around_list[next_node.handle] = next_node
				table.insert(result, {true, next_node.handle})
			end
		elseif node.around_list[next_node.handle] ~= nil then
			local is_near_long = self:is_near(next_node.pos, node.pos, radius_long)
			if not is_near_long or not self.nodes[next_node.handle] then
				--它已经离我远去
				node.around_list[next_node.handle] = nil
				table.insert(result, {true, next_node.handle})
			end
		end
		next_node = next_node.next
	end
	-- for k,v in pairs(node.around_list) do
	-- 	local is_near = self:is_near(v.pos, node.pos, radius_long)
	-- 	if not is_near then
	-- 		table.insert(result, {is_enter=false, handle=v.handle})
	-- 	end
	-- end
	return result
end

return aoi