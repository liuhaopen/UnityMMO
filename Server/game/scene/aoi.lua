local aoi = {}
--先用三维度的十字链表法，以后有空再优化吧
function aoi:init( )
	self.counter = 0
	self.nodes = {}
	self.link_heads = {}--三维度，即三个链表指针
end

function aoi:add(  )
	self.counter = self.counter + 1
	local node = {
		handle = self.counter,
		next = {},--三维度，即三个链表指针
		last = {},--三维度，即三个链表指针
		pos = {},
		around_list = {},
	}
	self.nodes[self.counter] = node
	return self.counter
end

function aoi:remove( handle )
	local node = self.nodes[handle]
	if not node then return end
	
	for i=1,3 do
		local last_node = node.last[i]
		local next_node = node.next[i]
		if last_node then
			last_node.next[i] = next_node
		end
		if next_node then
			next_node.last[i] = last_node
		end
	end
	self.nodes[handle] = nil
end

local insert_to_front = function ( insert_node, next_node, dimension )
	if not insert_node then return end
	
	local last_node = next_node and next_node.last[dimension]
	insert_node.next[dimension] = last_node
	insert_node.last[dimension] = next_node and next_node.next[dimension]
	if next_node then
		next_node.last[dimension] = insert_node
	end
	if last_node then
		last_node.next[dimension] = insert_node
	end
end

function aoi:set_pos( handle, pos_x, pos_y, pos_z )
	local node = self.nodes[handle]
	if not node then
		return
	end
	local new_pos = {pos_x, pos_y, pos_z}
	for dimension=1,3 do
		if self.link_heads[dimension] then
			local is_first_set_pos = not node.pos[dimension]
			--新坐标比旧的大就往前找插入点
			if is_first_set_pos or new_pos[dimension] > node.pos[dimension] then
				--如果是第一次插入的话就从链表第一个节点开始找起
				local start_node = node.next[dimension]
				if is_first_set_pos then
					start_node = self.link_heads[dimension]
				end
				local test_node = start_node
				while test_node ~= nil do
					if test_node.pos[dimension] > new_pos[dimension] then
						--遇到强者就要认输了
						break
					end
					test_node = test_node.next[dimension]
				end
				insert_to_front(node, test_node, dimension)
			else
				local test_node = node.last[dimension]
				while test_node ~= nil do
					if test_node.pos[dimension] < new_pos[dimension] then
						--遇到强者就要认输了
						break
					end
					test_node = test_node.next[dimension]
				end
				insert_to_front(node, test_node, dimension)
			end
		else
			self.link_heads[dimension] = node
		end
	end
	node.pos = new_pos
end

function aoi:get_pos( handle )
	return self.nodes[handle].pos
end

function aoi:is_near( pos1, pos2, distance )
	local is_ok = true
	for i=1,3 do
		if math.abs(pos1[i]-pos2[i]) > distance then
			is_ok = false
			break
		end
	end
	return is_ok
end

local find_around_in_link = function ( node, distance )
	local result = {}
	local next_node = node.next[1]
	while next_node ~= nil do
		if math.abs(node.pos[1] - next_node.pos[1]) <= distance then
			local is_ok = true
			for i=2,3 do
				if math.abs(next_node.pos[i] - node.pos[i]) > distance then
					is_ok = false
					break
				end
			end
			if is_ok then
				result[next_node.handle] = next_node
				next_node = next_node.next[1]
			end
		else
			break
		end
	end
	local last_node = node.last[1]
	while last_node ~= nil do 
		if math.abs(node.pos[1] - last_node.pos[1]) <= distance then
			local is_ok = true
			for i=2,3 do
				if math.abs(last_node.pos[i] - node.pos[i]) > distance then
					is_ok = false
					break
				end
			end
			if is_ok then
				result[last_node.handle] = last_node
				last_node = last_node.last[1]
			end
		else
			break
		end
	end
	return result
end

function aoi:get_around_offset( handle, radius_short, radius_long )
	local node = self.nodes[handle]
	if not node then return end
	
	local new_around_maps = find_around_in_link(node, radius_short)
	local result = {}
	for _,test_node in pairs(node.around_list) do
		if not new_around_maps[test_node.handle] then
			--为了防止颠陂，离开可视区域的距离要更大
			local is_near_long = self:is_near(node.pos, test_node.pos, radius_long)
			if not is_near_long then
				--之前的可视区域节点不见了
				node.around_list[test_node.handle] = nil
				result[test_node.handle] = {false, test_node.handle}
			end
		end
	end
	for _,test_node in pairs(new_around_maps) do
		if not node.around_list[test_node.handle] then
			--新加入可视区域的节点
			node.around_list[test_node.handle] = test_node
			result[test_node.handle] = {true, test_node.handle}
		end
	end
	return result
end

return aoi