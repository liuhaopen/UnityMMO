--[[说明:
用于创建Item列表的组件,支持三种创建方式:
)传入Item类名:使用字段item_class
)传入prefab资源名:使用字段prefab_path
)对象池类型如:GoodsItem:使用字段lua_pool_name
)单个控件如：字段prefab_pool_name
各字段使用说明:
data_list:子节点的信息列表
create_frequency:分时加载,即每隔这么多秒加载一个子节点
item_con:装载子节点的容器节点
scroll_view:滚动容器节点(带ScrollRect组件的那个),如果不是滚动容器的话此值设为和item_con一样即可
item_width:单个子节点的宽度, item_height为高度
get_item_width:如果你的子节点们不是同一大小，这时就需要传入该回调用于查询每个子节点的大小,get_item_height同理，设定后还需要指定reuse_item_num字段，因为子节点是动态的了，我帮不了你预先计算可视区域里最多可显示多少个子节点
space_x:子节点间的水平间隔, space_y为垂直间隔
start_x:子节点的初始偏移,start_y同理
show_col:显示的列数, 暂不支持控制行数
on_update_item:子节点刷新事件的回调
reuse_item_num:复用子节点的数量,为nil或大于0时开启,默认是开启的,一般不需要手动指定（除非你的子节点大小是动态的）,默认情况下将只创建可视区域里可见的最少数量节点,当子节点超出可视区域时会复用它填补到可视区域
alignment:对齐方式,目前只支持左上角,水平居中,垂直居中,当非左上角布局时优化选项无效(即reuse_item_num为0),有空再兼容处理吧
child_names:仅当prefab_ab_name和prefab_res_name有值时才有效,指定子节点的子节点们的名字集
fitsize_scroll_view:是否把滚动容器的大小设为刚好容纳子节点们的大小
is_scroll_back_on_update:是否每次设置的时候重置滚动容器坐标
]]--
UI = UI or {}
UI.ItemListCreator = UI.ItemListCreator or BaseClass(UI.UIComponent)
local math_ceil = math.ceil
local math_min = math.min
local math_floor = math.floor
local math_abs = math.abs
--先声明私有函数
local GetUnVisibleRowColBySize, GetCurUnvisibleRowOrCol, UpdateContainerQuads, GetItemCreator
, BindScrollEvent, HandleScrollChange, GetAlignInfo, UpdateScrollViewSize, InitInfo
, UpdateItemsGrid, ResetItemsRealIndex, ResizeItemList

-- local PrintItems = function ( self )
-- 	local str = ""
-- 	for i=1,#self.item_list do
-- 		local item = self.item_list[i]
-- 		str = str..tostring(item._real_index_for_item_creator_)..", "
-- 	end
-- 	print('Cat:ItemListCreator.lua[PrintItems] :', str)
-- end

function UI.ItemListCreator:OnLoad()
	self.item_list = {}
	self.is_first_update = true
	self.last_unvisible_row = 0
end

local InitInfo = function ( self, info )
	self:HideAllItems()
	if not info or not info.data_list or #info.data_list <= 0 then return false end

	--item_width或item_height至少要有一个不为空
	assert(info.item_width~=nil or info.item_height~=nil or info.get_item_width~=nil or info.get_item_height, "item_height or item_width or get_item_width or get_item_height is nil")
	--item_con就是装子节点的,scroll_view就是带ScrollRect组件的transform组件
	assert(info.item_con~=nil and info.scroll_view~=nil, "item_con and scroll_view cannot be nil")
	info.is_scroll_view = info.item_con~=info.scroll_view

	info.item_width = info.item_width or 0
	info.item_height = info.item_height or 0
	info.space_x = info.space_x or 0
	info.space_y = info.space_y or 0
	info.show_col = info.show_col or 0
	info.start_x = info.start_x or 0
	info.start_y = info.start_y or 0
	
	self.offset_x = info.item_width + info.space_x
	self.offset_y = info.item_height + info.space_y
	local sizeDelta = info.scroll_view.sizeDelta
	local visual_width, visual_height = sizeDelta.x, sizeDelta.y
	self.visual_width = info.visual_width or visual_width
	self.visual_height = info.visual_height or visual_height
	if info.reuse_item_num == nil or info.reuse_item_num > 0 then
		--默认开启优化选项
		self.is_reuse_items = true
	end

	if not self.scroll_view_scr then
		self.scroll_view_scr = info.scroll_view:GetComponent("ScrollRect")
		if self.scroll_view_scr then
			self.is_horizon_scroll = self.scroll_view_scr.horizontal
			self.is_vertical_scroll = self.scroll_view_scr.vertical
		end
	end
	self.info = info
	return true
end

--根据传入的配置和滚动容器大小判断显示的方式:垂直,水平,格子
function UI.ItemListCreator:UpdateItems(info)
	local is_init_ok = InitInfo(self, info)
	if not is_init_ok then return end
	
	local min_float = 0.0001

	if info.item_width <= min_float and info.get_item_width==nil then
		--你的滚动容器不是垂直滚动的,那应该就是水平滚动的吧,但是你没有配置item_width,显示肯定会有问题的
		assert(self.is_vertical_scroll or not info.is_scroll_view, "are you sure show items with vert layout in unvert scroll view?")
		--没配置节点宽度的肯定是垂直一行的排版
		self.info.show_col = 1
		self.show_type = "Vert"
		UpdateItemsGrid(self)
	elseif info.item_height <= min_float and info.get_item_height==nil then
		--你的滚动容器不是水平滚动的,那应该就是垂直滚动的吧,但是你没有配置item_height,显示肯定会有问题的
		assert(self.is_horizon_scroll or not info.is_scroll_view, "are you sure show items with horizon layout in unhorizon scroll view?")
		--没配置节点高度的肯定是水平一行的排版
		self.info.show_col = self.info.data_list and #self.info.data_list or 0
		self.show_type = "Hori"
		UpdateItemsGrid(self)
	else
		-- local visual_w = GetSizeDeltaX(info.scroll_view)
		info.show_col = math.floor((self.visual_width+info.space_x) / self.offset_x)
		UpdateItemsGrid(self)
	end
end

--水平加垂直布局,一般没特殊需求直接用UpdateItems这个接口就行了
UpdateItemsGrid = function ( self )
	local info = self.info
	if self.step_create_item_id then
		--有可能再次调用本类的刷新函数时还没创建完上次调用刷新的子节点们,所以需要等创建完后再刷新一次
		self.is_update_after_delay_create_items = true
		return
	end
	--先刷新容器的大小坐标等信息
	self.get_item_pos_xy_func = UpdateContainerQuads(self)
	--获取子节点的创建函数
	local creator = GetItemCreator(self)
	
	--可视区域内最多可以显示多少个节点,尽量只创建少点节点,因为就算在滚动容器不可见的地方,节点也要占drawcall的
	local max_visual_item_num = 0
	if self.is_reuse_items then
		max_visual_item_num = info.reuse_item_num or self:GetMaxVisualItemNum(info)
	else
		max_visual_item_num = #info.data_list
	end
	local min_item_num = math_min(#info.data_list, max_visual_item_num)
	ResizeItemList(self, min_item_num)
	ResetItemsRealIndex(self)
	if info.create_frequency ~= nil and self.is_first_update then
		self.cur_create_index = 1
		self.max_create_num = min_item_num
		if self.scroll_view_scr then
			--延迟创建时不让滚动
			self.scroll_view_scr.enabled = false
		end
		local step_create_item = function (  )
			creator(self.cur_create_index, info.data_list[self.cur_create_index])
			self.cur_create_index = self.cur_create_index + 1
			if self.cur_create_index > self.max_create_num then
				if self.step_create_item_id then
					self.step_create_item_id:Stop()
					self.step_create_item_id = nil
				end
				if self.scroll_view_scr then
					self.scroll_view_scr.enabled = true
				end
				if min_item_num < #info.data_list then
					BindScrollEvent(self)
				end
				if self.is_update_after_delay_create_items then
					self.is_update_after_delay_create_items = false
					UpdateItemsGrid(self)
				end
				if self.cache_scroll_to_item_info then
					self:ScrollToItem(unpack(self.cache_scroll_to_item_info))
					self.cache_scroll_to_item_info = nil
				end
			end
		end
		if not self.step_create_item_id then
			if info.create_num_per_time then
				local origin_func = step_create_item
				step_create_item = function()
					for i=1,info.create_num_per_time do
						if self.cur_create_index > self.max_create_num then
							break
						end
						origin_func()
					end
				end
			end
			self.step_create_item_id = Timer.New(step_create_item, info.create_frequency, -1)
			self.step_create_item_id:Start()
		end
		step_create_item()
	else
		for i=1,min_item_num do
			creator(i, info.data_list[i])
		end
		if min_item_num < #info.data_list then
			BindScrollEvent(self)
		end
	end
	self.is_first_update = false--首次刷新时才需要延时创建节点
end

--暂只支持垂直或水平居中和左上角,默认是左上角
GetAlignInfo = function ( self, items_sum_width, items_sum_height )
	local item_align_x = 0--节点需要对齐的偏移
	local item_align_y = 0
	local con_align_x = 0--滚动容器(子节点的父节点)的对齐偏移
	local con_align_y = 0
	local new_con_size_w = items_sum_width
	local new_con_size_h = items_sum_height
	local alignment = self.info and self.info.alignment or TextAnchor.UpperLeft
	
	-- local visual_width, visual_height = GetSizeDeltaXY(self.info.scroll_view)
	local visual_width, visual_height = self.visual_width, self.visual_height
	local hori_align, vert_align = UI.AlignTypeToStr(alignment)
	if vert_align == "Upper" then
		--不需要做什么
	elseif vert_align == "Middle" then
		--垂直居中
		local half_y = -(visual_height-items_sum_height)/2
		if new_con_size_h < visual_height then
			--所有子节点的高度小于容器可视化高度时,子节点的父节点的高度要设为容器的可视化高度,同时所有子节点要往右移
			new_con_size_h = visual_height
			item_align_y = half_y
		else
			--子节点的父节点往上移就行了
			con_align_y = half_y
		end
		--非左上角布局暂不支持优化
		self.is_reuse_items = false
	elseif vert_align == "Lower" then
		--左下对齐
		local align_y = -(visual_height-items_sum_height)
		if new_con_size_h < visual_height then
			--所有子节点的高度小于容器可视化高度时,子节点的父节点的高度要设为容器的可视化高度,同时所有子节点要往右移
			new_con_size_h = visual_height
			item_align_y = align_y
		else
			--子节点的父节点往上移就行了
			con_align_y = align_y
		end
		--非左上角布局暂不支持优化
		self.is_reuse_items = false
	end
	if hori_align == "Left" then
		new_con_size_w = items_sum_width-visual_width
	elseif hori_align == "Center" then
		--水平居中
		local half_x = (visual_width-items_sum_width)/2
		if new_con_size_w < visual_width then
			--所有子节点的宽度小于容器可视化宽度时,子节点的父节点的宽度要设为容器的可视化宽度,同时所有子节点要往右移
			new_con_size_w = 0--因为item_con是水平拉伸的,所以sizeDelta设为0就是其父节点的宽度了
			item_align_x = half_x
		else
			--如果子节点们可以填满父节点,那就不需要动了,留着下面的注释
			-- con_align_x = half_x
			--之所以要减去visual_width是因为滚动容器父节点的anchor是水平拉伸的
			new_con_size_w = items_sum_width-visual_width
		end
		--非左上角布局暂不支持优化
		self.is_reuse_items = false
	elseif hori_align == "Right" then
		--右对齐
		local align_x = (visual_width-items_sum_width)
		if new_con_size_w < visual_width then
			--所有子节点的宽度小于容器可视化宽度时,子节点的父节点的宽度要设为容器的可视化宽度,同时所有子节点要往右移
			new_con_size_w = 0--因为item_con是水平拉伸的,所以sizeDelta设为0就是其父节点的宽度了
			item_align_x = align_x
		else
			--子节点的父节点往左移就行了
			con_align_x = align_x
			--之所以要减去visual_width是因为滚动容器父节点的anchor是水平拉伸的
			new_con_size_w = items_sum_width-visual_width
		end
		--非左上角布局暂不支持优化
		self.is_reuse_items = false
	end
	return item_align_x, item_align_y, con_align_x, con_align_y, new_con_size_w, new_con_size_h
end

UpdateScrollViewSize = function ( self, items_sum_width, items_sum_height )
	local show_type = self.show_type or "Grid"
	if self.info.fitsize_scroll_view and self.info.is_scroll_view then
		if show_type == "Vert" then
			UIHelper.SetSizeDeltaY(self.info.scroll_view, items_sum_height)
		elseif show_type == "Hori" then
			UIHelper.SetSizeDeltaX(self.info.scroll_view, items_sum_width)
		else
			UIHelper.SetSizeDelta(self.info.scroll_view, items_sum_width, items_sum_height)
		end
	end
end
--刷新容器的大小坐标等,并返回一函数以供获取子节点的坐标
UpdateContainerQuads = function ( self )
	local info = self.info
	local items_sum_width = info.start_x+self.offset_x*info.show_col-info.space_x
	local items_sum_height = -info.start_y+math_ceil(#info.data_list/info.show_col)*self.offset_y-info.space_y
	if self.is_horizon_scroll and info.get_item_width then		
		items_sum_width = 0
		for i=1,#info.data_list do
			local w = info.get_item_width(i)
			items_sum_width = items_sum_width + w + self.info.space_x
		end
		items_sum_width = info.start_x + items_sum_width - self.info.space_x
	elseif info.get_item_height then
		items_sum_height = 0
		for i=1,#info.data_list do
			local h = info.get_item_height(i)
			items_sum_height = items_sum_height + h + self.info.space_y
		end
		items_sum_height = -info.start_y + items_sum_height - self.info.space_y
	end
	UpdateScrollViewSize(self, items_sum_width, items_sum_height)
	local item_align_x, item_align_y, con_align_x, con_align_y, new_con_size_w, new_con_size_h = GetAlignInfo(self, items_sum_width, items_sum_height)
	if info.is_scroll_view then
		local show_type = self.show_type or "Grid"
		if show_type == "Vert" then
			UIHelper.SetSizeDeltaY(info.item_con, new_con_size_h)
			if self.is_first_update or info.is_scroll_back_on_update then
				UIHelper.SetLocalPositionY(info.item_con, con_align_y)
			end
		elseif show_type == "Hori" then
			UIHelper.SetSizeDeltaX(info.item_con, new_con_size_w)
			if self.is_first_update or info.is_scroll_back_on_update then
				UIHelper.SetLocalPositionX(info.item_con, con_align_x)
			end
		else
			UIHelper.SetSizeDelta(info.item_con, new_con_size_w, new_con_size_h)
			if self.is_first_update or info.is_scroll_back_on_update then
				UIHelper.SetLocalPosition(info.item_con, con_align_x, con_align_y)
			end
		end
	end
	local get_item_pos_xy = function ( i )
		local new_x = 0
		local new_y = 0
		if not info.get_item_width then
			new_x = item_align_x+info.start_x+self.offset_x*((i-1)%info.show_col)
		else
			for tmp_i=1,i-1 do
				local w = info.get_item_width(tmp_i)
				new_x = new_x + w + self.info.space_x
			end
			if i>1 then
				new_x = new_x - self.info.space_x
			end
			new_x = new_x + item_align_x + info.start_x
		end
		if not info.get_item_height then
			new_y = item_align_y+info.start_y-self.offset_y*math_floor((i-1)/info.show_col)
		else
			for tmp_i=1,i-1 do
				local h = info.get_item_height(tmp_i)
				new_y = new_y - h - self.info.space_y
			end
			if i>1 then
				new_y = new_y + self.info.space_y
			end
			new_y = new_y + item_align_y + info.start_y
		end
		return new_x, new_y
	end
	return get_item_pos_xy
end

local update_item_for_creator = function ( self, item )
	if self.info.on_update_item then
		local real_index = item._real_index_for_item_creator_
		local real_data = self.info.data_list[real_index]
		self.info.on_update_item(item, real_index, real_data)
	end
end

local on_update_prefab_item = function ( item, i, self )
	update_item_for_creator(self, item)
end

--支持三种子节点的创建方式
GetItemCreator = function ( self )
	local info = self.info
	local creator = nil
	if info.item_class ~= nil then
		creator = function(i, v)
			local item = self.item_list[i]
			if not item then
				item = info.item_class.New()
				item:SetParent(info.item_con)
				self.item_list[i] = item
				item._real_index_for_item_creator_ = i
			end
			item:SetActive(true)
			item:SetLocalPositionXYZ(self.get_item_pos_xy_func(item._real_index_for_item_creator_))
			update_item_for_creator(self, item)
		end
	elseif info.prefab_path or info.prefab_pool_name then
		creator = function(i, v)
			local item = self.item_list[i]
			if not item then
				local on_load_ok = function ( item )
					UI.GetChildren(item, item.transform, info.child_names)
					if item.___data_for_itmector___ then
						item.___data_for_itmector___ = nil
						update_item_for_creator(self, item)
					end
				end
				item = UINode.New()
				if info.prefab_path then
					item.viewCfg = {
						prefabPath = info.prefab_path,
					}
				elseif info.prefab_pool_name then
					item.viewCfg = {
						prefabPoolName = info.prefab_pool_name,
					}
				end
				item.viewCfg.parentTrans = info.item_con
				item.SetData = function(item, i, v)
					if item.isLoaded and item.OnUpdate then
						update_item_for_creator(self, item)
					else
						item.___data_for_itmector___ = {index=i, data=v}
					end
				end
				item.OnLoad = on_load_ok
				self.item_list[i] = item
				self.item_list[i]._real_index_for_item_creator_ = i
				item:Load()
			end
			item:SetActive(true)
			item:SetLocalPositionXYZ(self.get_item_pos_xy_func(item._real_index_for_item_creator_))
			item:SetData(i, v)
		end
	elseif info.lua_pool_name then
		creator = function(i, v)
			local item = self.item_list[i]
			if not item then
				item = LuaPool:Get(info.lua_pool_name)
				item:Load()
				item:SetParent(info.item_con)
				self.item_list[i] = item
				item._real_index_for_item_creator_ = i
			end
			item:SetActive(true)
			item:SetLocalPositionXYZ(self.get_item_pos_xy_func(item._real_index_for_item_creator_))
			-- if item.SetItemSize and info.lua_pool_name == "GoodsItem" then
			-- 	local item_size = info.item_width or info.item_height
			-- 	item:SetItemSize(item_size, item_size)
			-- end
			update_item_for_creator(self, item)
		end
		self.destroy_pool_type = info.lua_pool_name
	else
		--没有指定创建节点的方式
		assert(false, "has not specify create way!")
	end
	return creator
end

--获取unvisible_size的区域里有几个单位（滚动容器为垂直时就是行，水平滚动时就是列）子节点不可见
GetUnVisibleRowColBySize = function( self, unvisible_size )
	local item_num = #self.info.data_list
	local result = 0
	if self.is_horizon_scroll then
		if not self.info.get_item_width then
			result = math_floor(unvisible_size / self.offset_x)
		else
			for i=1,item_num do
				local w = self.info.get_item_width(i)
				unvisible_size = unvisible_size - w - self.info.space_x
				if unvisible_size < 0 then
					break
				else
					result = result + 1
				end
			end
		end
	else
		if not self.info.get_item_height then
			result = math_floor(unvisible_size / self.offset_y)
		else
			for i=1,item_num do
				local h = self.info.get_item_height(i)
				unvisible_size = unvisible_size - h - self.info.space_y
				if unvisible_size < 0 then
					break
				else
					result = result + 1
				end
			end
		end
	end
	return result
end

GetCurUnvisibleRowOrCol = function ( self )
	local info = self.info
	local cur_unvisible_row = 0
	local move_num_per_operate = 0--每次操作的个数
	if self.is_horizon_scroll then
		local con_x = -UI.GetLocalPositionX(info.item_con)
		con_x = con_x - info.start_x
		cur_unvisible_row = GetUnVisibleRowColBySize(self, con_x+info.space_x)
		--水平布局下是逐个移动的
		move_num_per_operate = 1
	else
		local con_y = UI.GetLocalPositionY(info.item_con)
		con_y = con_y + info.start_y
		cur_unvisible_row = GetUnVisibleRowColBySize(self, con_y+info.space_y)
		--垂直布局下是整行移动的
		move_num_per_operate = info.show_col
	end
	if cur_unvisible_row < 0 then
		cur_unvisible_row = 0
	end
	return cur_unvisible_row, move_num_per_operate
end

ResetItemsRealIndex = function ( self )
	if not self.info then return end
	local cur_unvisible_row, move_num_per_operate = GetCurUnvisibleRowOrCol(self)
	self.last_unvisible_row = cur_unvisible_row
	local last_index = cur_unvisible_row*move_num_per_operate
	for i=1, #self.item_list do
		local item = self.item_list[i]
		local new_real_index = last_index + i
		if new_real_index > #self.info.data_list then
			last_index = -i+1
			new_real_index = 1
		end
		item._real_index_for_item_creator_ = new_real_index
		-- item.gameObject.name = "item_list_creator_"..new_real_index
	end
end

--滚动容器滚动时,根据滚动距离判断有几行节点越出边界了,然后把那几行节点往底部或上部移
HandleScrollChange = function ( self )
	local info = self.info
	if not info.data_list then return end
	local cur_unvisible_row, move_num_per_operate = GetCurUnvisibleRowOrCol(self)
	local last_unvisible_row = self.last_unvisible_row
	local new_unvisible_row = math_abs(last_unvisible_row-cur_unvisible_row)
	self.last_unvisible_row = cur_unvisible_row
	local all_data_num = #info.data_list
	if cur_unvisible_row > last_unvisible_row then
		local last_item = self.item_list[#self.item_list]
		if not last_item then return end
		
		local last_index = last_item._real_index_for_item_creator_
		for i=1,new_unvisible_row*move_num_per_operate do
			local item = table.remove(self.item_list, 1)
			if item then
				local new_real_index = last_index + i
				if new_real_index > all_data_num then
					last_index = -i+1
					new_real_index = 1
				end
				item._real_index_for_item_creator_ = new_real_index
				-- item.gameObject.name = "item_list_creator_"..new_real_index
				item:SetLocalPositionXYZ(self.get_item_pos_xy_func(new_real_index))
				if info.prefab_ab_name and info.prefab_res_name then
					item:SetData(nil, self)
				elseif info.on_update_item then
					info.on_update_item(item, new_real_index, info.data_list[new_real_index])
				end
				-- 把节点移到队尾
				table.insert(self.item_list, item)
			end
		end
	else
		local first_item = self.item_list[1]
		if not first_item then return end
		
		local first_index = first_item._real_index_for_item_creator_
		for i=1,new_unvisible_row*move_num_per_operate do
			local item = table.remove(self.item_list, #self.item_list)
			if item then
				local new_real_index = first_index - i
				if new_real_index <= 0 then
					new_real_index = all_data_num
					first_index = new_real_index+i-1
				end
				item._real_index_for_item_creator_ = new_real_index
				-- item.gameObject.name = "item_list_creator_"..new_real_index
				item:SetLocalPositionXYZ(self.get_item_pos_xy_func(new_real_index))
				if info.prefab_ab_name and info.prefab_res_name then
					item:SetData(nil, self)
				elseif info.on_update_item then
					info.on_update_item(item, new_real_index, info.data_list[new_real_index])
				end
			end
			--把节点移到队头
			table.insert(self.item_list, 1, item)
		end
	end
	-- PrintItems(self)
end

--为滚动容器绑定滚动事件,在滚动时刷新子节点的坐标,把超过边界不可见的节点移到可见区域
BindScrollEvent = function ( self )
	if not self.had_bind_scroll and self.info.scroll_view ~= nil then
		self.had_bind_scroll = true		
		local on_scroll_changed = function (  )
			HandleScrollChange(self)
		end
		self.scroll_view_scr.onValueChanged:AddListener(on_scroll_changed)
	end
	HandleScrollChange(self)
end

function UI.ItemListCreator:HideAllItems()
	if self.step_create_item_id then
		--正在创建子节点中,创建完后会再次调用本函数的
		return
	end
	for i,v in ipairs(self.item_list) do
		v:SetActive(false)
	end
end

--滚动到指定子节点的位置上，默认在最左或最上，可以配合pos_offset设置偏移，默认使用动画，不想动画的话就传入animte_info为false
function UI.ItemListCreator:ScrollToItem(item_index, pos_offset, animte_info)
	if not self.info or not self.info.item_con then return end
	if self.step_create_item_id then
		--还在延迟创建节点中，等创建完毕后再滚过去
		self.cache_scroll_to_item_info = {item_index, pos_offset, animte_info}
		return
	end
	pos_offset = pos_offset or 0

	cc.ActionManager:getInstance():removeAllActionsFromTarget(self.info.item_con)
	local duration = 0.3
	if animte_info then
		duration = animte_info.duration
	end
	if self.is_horizon_scroll then
		local target_pos_x, _ = self.get_item_pos_xy_func(item_index)
		target_pos_x = -target_pos_x+pos_offset
		if target_pos_x > 0 then
			target_pos_x = 0
		end
		local con_width = GetSizeDeltaX(self.info.item_con)
		local max_move = math.min(0, -con_width)
		if target_pos_x < max_move then
			target_pos_x = max_move
		end
		if animte_info or animte_info==nil then
			local _, origin_pos_y, origin_pos_z = UI.GetLocalPositionXYZ(self.info.item_con)
			local action = cc.MoveTo.createLocalType(duration, target_pos_x, origin_pos_y, origin_pos_z)
			cc.ActionManager:getInstance():addAction(action, self.info.item_con)
		else
			UIHelper.SetLocalPositionX(self.info.item_con, target_pos_x)
		end
	else
		local _, target_pos_y = self.get_item_pos_xy_func(item_index)
		target_pos_y = -target_pos_y+pos_offset
		if target_pos_y < 0 then
			target_pos_y = 0
		end
		local con_height = GetSizeDeltaY(self.info.item_con)
		-- local visual_height = GetSizeDeltaY(self.info.scroll_view)
		local max_move = math.max(0, con_height-self.visual_height)
		if target_pos_y > max_move then
			target_pos_y = max_move
		end
		if animte_info or animte_info==nil then
			local origin_pos_x, _, origin_pos_z = UI.GetLocalPositionXYZ(self.info.item_con)
			local action = cc.MoveTo.createLocalType(duration, origin_pos_x, target_pos_y, origin_pos_z)
			cc.ActionManager:getInstance():addAction(action, self.info.item_con)
		else
			UIHelper.SetLocalPositionY(self.info.item_con, target_pos_y)
		end
	end
end

--获取可视区域内最多可显示多少个子节点 
function UI.ItemListCreator:GetMaxVisualItemNum(info)
	local show_type = self.show_type or "Grid"
	-- print('Cat:ItemListCreator.lua[520] show_type', show_type)
	if show_type == "Vert" then
		-- local visual_h = GetSizeDeltaY(info.scroll_view)
		return Round((self.visual_height+info.space_y) / self.offset_y)+1
	elseif show_type == "Hori" then
		-- local visual_w = GetSizeDeltaX(info.scroll_view)
		return Round((self.visual_width+info.space_x) / self.offset_x)+1
	elseif show_type == "Grid" then
		-- local visual_h = GetSizeDeltaY(info.scroll_view)
		local max_row = Round((self.visual_height+info.space_y) / self.offset_y)+1
		return info.show_col * max_row
	end
	assert(false, "scroll visual size too small for show items")
	return 0
end

function UI.ItemListCreator:Reset()
	self:OnDestroy()
end

--遍历所有创建的节点
function UI.ItemListCreator:IterateItems(func)
	if not func then return end
	
	for i=1,#self.item_list do
		local real_index = self.item_list[i]._real_index_for_item_creator_
		local is_break = func(self.item_list[i], real_index, self.info and self.info.data_list and self.info.data_list[real_index])
		if is_break then
			break
		end
	end
end

--获取第几节点的坐标
function UI.ItemListCreator:GetItemPosXY(index)
	if self.get_item_pos_xy_func then
		return self.get_item_pos_xy_func(index)
	end
	print('Cat:ItemListCreator.lua[get wrong item pos!!!get_item_pos_xy_func is nil!!!]')
	return nil
end

ResizeItemList = function( self, min_item_num )
	if #self.item_list > min_item_num then
		for i=min_item_num+1, #self.item_list do
			if self.destroy_pool_type then
				LuaPool:Recycle(self.destroy_pool_type, self.item_list[i])
			elseif self.item_list[i].Unload then
				self.item_list[i]:Unload()
			else
				self.item_list[i]:Destroy()
			end
			self.item_list[i] = nil
		end
	end
end

--关闭界面时清除创建的节点
function UI.ItemListCreator:OnClose()
	self.had_bind_scroll = false
	self.is_first_update = true
	self.last_unvisible_row = 0
	
	if self.scroll_view_scr then
		self.scroll_view_scr:StopMovement()
	end
	if self.info and self.info.item_con then
		cc.ActionManager:getInstance():removeAllActionsFromTarget(self.info.item_con)
		UIHelper.SetLocalPosition(self.info.item_con, 0, 0)
	end

	if self.destroy_pool_type then
		for i,item in pairs(self.item_list) do
			LuaPool:Recycle(self.destroy_pool_type, item)
		end
		self.item_list = {} 
	else
		for k,v in pairs(self.item_list) do
			if v.Unload then
				v:Unload()
			else
				v:Destroy()
			end
		end
		self.item_list = {}
	end
	if self.scroll_view_scr and self.had_bind_scroll then
		self.scroll_view_scr.onValueChanged:RemoveAllListeners()
		self.scroll_view_scr = nil
	end
	if self.step_create_item_id then
		self.step_create_item_id:Stop()
		self.step_create_item_id = nil
	end
end