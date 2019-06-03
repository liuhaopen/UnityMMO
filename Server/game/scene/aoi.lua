local aoi = {}

local aoi_state = {
	original = 1, inited = 2, removed = 3,
}
--先用三维度的十字链表法，以后有空再优化吧
function aoi:init( )
	self.counter = 0
	self.nodes = {}
	self.link_heads = {}--三维度，即三个链表指针
end

function aoi:add(  )
	--Cat_Todo : 需要做成原子操作
	self.counter = self.counter + 1
	local node = {
		handle = self.counter,
		next = {},--三维度，即三个链表指针
		last = {},--三维度，即三个链表指针
		pos = {},
		around_list = {},
		state = aoi_state.original,
	}
	self.nodes[self.counter] = node
	return self.counter
end

function aoi:delete_from_link( node, dimension )
	if not node or node.state==aoi_state.original then return end
	
	local next_node = node.next[dimension]
	local last_node = node.last[dimension]
	if last_node then
		last_node.next[dimension] = next_node
	else
		self.link_heads[dimension] = next_node
	end
	if next_node then
		next_node.last[dimension] = last_node
	end
	node.next[dimension] = nil
	node.last[dimension] = nil
end

function aoi:remove( handle )
	local node = self.nodes[handle]
	if not node then return end
	node.state = aoi_state.removed
	for dimension=1,3 do
		self:delete_from_link(node, dimension)
	end
	self.nodes[handle] = nil
end

function aoi:insert_to_front( insert_node, last_node, dimension )
	if not insert_node then return end
	
	if last_node ~= nil then
		insert_node.last[dimension] = last_node
		insert_node.next[dimension] = last_node.next[dimension]
		if last_node.next[dimension] ~= nil then
			last_node.next[dimension].last[dimension] = insert_node
		end
		last_node.next[dimension] = insert_node
	else
		if self.link_heads[dimension] ~= nil then
			self.link_heads[dimension].last[dimension] = insert_node
		end
		insert_node.last[dimension] = nil
		insert_node.next[dimension] = self.link_heads[dimension]
		self.link_heads[dimension] = insert_node
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
			local start_node = node
			--新坐标比旧的大就往前找插入点
			local is_find_in_front
			if is_first_set_pos then
				--新坐标比首个节点的还大就往前
				is_find_in_front = new_pos[dimension] > self.link_heads[dimension].pos[dimension]
				start_node = self.link_heads[dimension]
			else
				--新坐标比旧坐标大也往前寻找节点
				is_find_in_front = new_pos[dimension] > node.pos[dimension]
			end
			local test_node = start_node
			if is_find_in_front then
				while true do
					if not test_node or not test_node.next[dimension] or test_node.next[dimension].pos[dimension] > new_pos[dimension] then
						break
					end
					test_node = test_node.next[dimension]
				end
			else
				while true do
					if not test_node or test_node.pos[dimension] < new_pos[dimension] then
						break
					end
					test_node = test_node.last[dimension]
				end
			end
			if (node.last[dimension] ~= test_node and node ~= test_node) or (test_node==nil and node~=self.link_heads[dimension]) then
				self:delete_from_link(node, dimension)
				self:insert_to_front(node, test_node, dimension)
			end
		else
			self.link_heads[dimension] = node
		end
	end
	node.pos = new_pos
	node.state = aoi_state.inited
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

function aoi:find_around_in_link( node, distance )
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
			end
			next_node = next_node.next[1]
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
			end
			last_node = last_node.last[1]
		else
			break
		end
	end
	return result
end

function aoi:set_user_data( handle, key, value )
	local node = self.nodes[handle]
	if not node then return end
	node.user_data = node.user_data or {}
	node.user_data[key] = value
end

function aoi:get_user_data( handle, key )
	local node = self.nodes[handle]
	if not node then return end
	node.user_data = node.user_data or {}
	return node.user_data[key]
end

--获取附近的节点列表，注意是只读的，别修改此列表
function aoi:get_around( handle, radius_short, radius_long )
	local node = self.nodes[handle]
	if not node then return {} end
	self:get_around_offset(handle, radius_short, radius_long)
	return node.around_list
end

--获取附近的节点,但每次只返回和上次的差异集合，即新进入和新离开的节点集合。进入条件为：小于等于radius_short半径的节点；离开条件为：大于等于radius_long的节点
function aoi:get_around_offset( handle, radius_short, radius_long )
	local node = self.nodes[handle]
	if not node then return {} end
	
	radius_long = radius_long or radius_short
	local new_around_maps = self:find_around_in_link(node, radius_short)
	local result = {}
	for _,test_node in pairs(node.around_list) do
		if not new_around_maps[test_node.handle] then
			--为了防止颠陂，离开可视区域的距离要更大
			if test_node.state == aoi_state.removed or not self:is_near(node.pos, test_node.pos, radius_long) then
				--之前的可视区域节点不见了
				node.around_list[test_node.handle] = nil
				result[test_node.handle] = 2
			end
		end
	end
	for _,test_node in pairs(new_around_maps) do
		if not node.around_list[test_node.handle] then
			--新加入可视区域的节点
			node.around_list[test_node.handle] = test_node
			result[test_node.handle] = 1
		end
	end
	return result
end

return aoi