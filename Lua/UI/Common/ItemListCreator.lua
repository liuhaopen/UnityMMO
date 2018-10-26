--[[说明:
用于创建Item列表的组件,支持三种创建方式:
)传入Item类名:使用字段item_class
)传入prefab资源名:使用字段prefab_path
)对象池类型如:UIObjPool.UIType.AwardItem:使用字段obj_pool_type
各字段使用说明:
data_list:子节点的信息列表
create_frequency:分时加载,即每隔这么多秒加载一个子节点
item_con:装载子节点的容器节点
scroll_view:滚动容器节点(带ScrollRect组件的那个),如果不是滚动容器的话此值设为和item_con一样即可
item_width:单个子节点的宽度, item_height为高度
space_x:子节点间的水平间隔, space_y为垂直间隔
start_x:子节点的初始偏移,start_y同理
show_col:显示的列数, 暂不支持控制行数
on_update_item:子节点刷新事件的回调
reuse_item_num:复用子节点的数量,为nil或大于0时开启,默认是开启的,一般不需要手动指定,默认情况下将只创建可视区域里可见的最少数量节点,当子节点超出可视区域时会复用它填补到可视区域
alignment:对齐方式,目前只支持左上角,水平居中,垂直居中,当非左上角布局时优化选项无效(即reuse_item_num为0),有空再兼容处理吧
child_names:仅当prefab_path有值时才有效,指定子节点的子节点们的名字集
fitsize_scroll_view:是否把滚动容器的大小设为刚好容纳子节点们的大小
is_scroll_back_on_update:是否每次设置的时候重置滚动容器坐标
]]--
UI.ItemListCreator = UI.ItemListCreator or BaseClass(UI.UIComponent)
function UI.ItemListCreator:OnLoad()
	self.item_list = {}
	self.is_first_update = true
	self.last_unvisible_row = 0
end

local function InitInfo(self, info)
	if self.info and info.data_list and #info.data_list~=#self.info.data_list then
		--如果两次更新的子节点数量不一样就要重置一下
		self:Reset()
	else
		self:HideAllItems()
	end
	if not info or not info.data_list or #info.data_list <= 0 then return false end

	--item_width或item_height至少要有一个不为空
	assert(info.item_width~=nil or info.item_height~=nil, "item_height or item_width nil")
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
	-- if info.min_item_num and info.min_item_num > #info.data_list then
	--Cat_Todo : 设置最少多少个子节点
	-- end
	self.offset_x = info.item_width + info.space_x
	self.offset_y = info.item_height + info.space_y
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

	if info.item_width <= min_float then
		--你的滚动容器不是垂直滚动的,那应该就是水平滚动的吧,但是你没有配置item_width,显示肯定会有问题的
		assert(self.is_vertical_scroll or not info.is_scroll_view, "are you sure show items with vert layout in unvert scroll view?")
		--没配置节点宽度的肯定是垂直一行的排版
		self:UpdateItemsVert(info)
	elseif info.item_height <= min_float then
		--你的滚动容器不是水平滚动的,那应该就是垂直滚动的吧,但是你没有配置item_height,显示肯定会有问题的
		assert(self.is_horizon_scroll or not info.is_scroll_view, "are you sure show items with horizon layout in unhorizon scroll view?")
		--没配置节点高度的肯定是水平一行的排版
		self:UpdateItemsHori(info)
	else
		local visual_w = GetSizeDeltaX(info.scroll_view)
		info.show_col = round((visual_w+info.space_x) / self.offset_x)
		self:UpdateItemsGrid(info)
	end
end

--暂只支持垂直或水平居中和左上角,默认是左上角
local function GetAlignInfo( self, info, items_sum_width, items_sum_height )
	local item_align_x = 0--节点需要对齐的偏移
	local item_align_y = 0
	local con_align_x = 0--滚动容器(子节点的父节点)的对齐偏移
	local con_align_y = 0
	local new_con_size_w = items_sum_width
	local new_con_size_h = items_sum_height
	local alignment = info and info.alignment or CS.UnityEngine.TextAnchor.UpperLeft
	
	local visual_width, visual_height = GetSizeDeltaXY(info.scroll_view)
	local hori_align, vert_align = AlignTypeToStr(alignment)
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

local function UpdateScrollViewSize(self, info, items_sum_width, items_sum_height)
	local show_type = self.show_type or "Grid"
	if info.fitsize_scroll_view and info.is_scroll_view then
		if show_type == "Vert" then
			SetSizeDeltaY(info.scroll_view, items_sum_height)
		elseif show_type == "Hori" then
			SetSizeDeltaX(info.scroll_view, items_sum_width)
		else
			SetSizeDelta(info.scroll_view, items_sum_width, items_sum_height)
		end
	end
end

--刷新容器的大小坐标等,并返回一函数以供获取子节点的坐标
local function UpdateContainerQuads(self, info)
	local items_sum_width = info.start_x+self.offset_x*info.show_col-info.space_x
	local items_sum_height = -info.start_y+math.ceil(#info.data_list/info.show_col)*self.offset_y-info.space_y
	UpdateScrollViewSize(self, info, items_sum_width, items_sum_height)
	local item_align_x, item_align_y, con_align_x, con_align_y, new_con_size_w, new_con_size_h = GetAlignInfo(self, info, items_sum_width, items_sum_height)
	if info.is_scroll_view then
		if show_type == "Vert" then
			SetSizeDeltaY(info.item_con, new_con_size_h)
			if self.is_first_update or info.is_scroll_back_on_update then
				SetLocalPositionY(info.item_con, con_align_y)
			end
		elseif show_type == "Hori" then
			SetSizeDeltaX(info.item_con, new_con_size_w)
			if self.is_first_update or info.is_scroll_back_on_update then
				SetLocalPositionX(info.item_con, con_align_x)
			end
		else
			SetSizeDelta(info.item_con, new_con_size_w, new_con_size_h)
			if self.is_first_update or info.is_scroll_back_on_update then
				SetLocalPosition(info.item_con, con_align_x, con_align_y)
			end
		end
	end
	local get_item_pos_xy = function ( i )
		return item_align_x+info.start_x+self.offset_x*((i-1)%info.show_col), item_align_y+info.start_y-self.offset_y*math.floor((i-1)/info.show_col)
	end
	return get_item_pos_xy
end

local update_item_for_creator = function ( info, item )
	if info.on_update_item then
		local real_index = item._real_index_for_item_creator_
		local real_data = info.data_list[real_index]
		info.on_update_item(item, real_index, real_data)
	end
end

--支持三种子节点的创建方式
local function GetItemCreator(self, info)
	local creator = nil
	if info.item_class ~= nil then
		creator = function(i, v)
			local item = self.item_list[i]
			if not item then
				item = info.item_class.New(info.item_con)
				self.item_list[i] = item
				item._real_index_for_item_creator_ = i
			end
			item:SetVisible(true)
			item:SetPosition(self.get_item_pos_xy_func(item._real_index_for_item_creator_))
			update_item_for_creator(info, item)
		end
	elseif info.prefab_path then
		creator = function(i, v)
			local item = self.item_list[i]
			if not item then
				local on_load_ok = function ( item )
					item._real_index_for_item_creator_ = i
					self.item_list[i] = item
					GetChildren(item, info.child_names)
					SetParent(item.transform, info.item_con)
					item:SetPosition(self.get_item_pos_xy_func(item._real_index_for_item_creator_))
					update_item_for_creator(info, item)
				end
				ResMgr:LoadPrefab(self, info.prefab_path, on_load_ok)
			else
				item.gameObject:SetActive(true)
				item:SetPosition(self.get_item_pos_xy_func(item._real_index_for_item_creator_))
				if info.on_update_item then
					info.on_update_item(item, i, v)
				end
			end
		end
	elseif info.obj_pool_type then
		creator = function(i, v)
			local item = self.item_list[i]
			if not item then
				item = UIWidgetPool:CreateWidget(info.obj_pool_type)
				item.transform:SetParent(info.item_con)
				self.item_list[i] = item
				item._real_index_for_item_creator_ = i
			end
			item:SetVisible(true)
			item:SetPosition(self.get_item_pos_xy_func(item._real_index_for_item_creator_))
			if item.SetItemSize and info.obj_pool_type == UIObjPool.UIType.AwardItem then
				local item_size = info.item_width or info.item_height
				item:SetItemSize(item_size, item_size)
			end
			update_item_for_creator(info, item)
		end
		self.destroy_pool_type = info.obj_pool_type
	else
		--没有指定创建节点的方式
		assert(false, "has not specify create way!")
	end
	return creator
end

local function UpdateItemsRealIndex(self)
	
end

--滚动容器滚动时,根据滚动距离判断有几行节点越出边界了,然后把那几行节点往底部或上部移
local function HandleScrollChange(self)
	local info = self.info
	local cur_unvisible_row = 0
	local move_num_per_operate = 0--每次操作的个数
	if self.is_horizon_scroll then
		local con_x = -GetLocalPositionX(info.item_con)
		con_x = con_x - info.start_x
		local item_offset_x = info.item_width+info.space_x
		cur_unvisible_row = math.floor((con_x+info.space_x) / item_offset_x)
		--水平布局下是逐个移动的
		move_num_per_operate = 1
	else
		local con_y = GetLocalPositionY(info.item_con)
		con_y = con_y + info.start_y
		local item_offset_y = info.item_height+info.space_y
		cur_unvisible_row = math.floor((con_y+info.space_y) / item_offset_y)
		--垂直布局下是整行移动的
		move_num_per_operate = info.show_col
	end
	local last_unvisible_row = self.last_unvisible_row
	local new_unvisible_row = math.abs(last_unvisible_row-cur_unvisible_row)
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
				-- item.gameObject.name = new_real_index
				item:SetPosition(self.get_item_pos_xy_func(new_real_index))
				if info.on_update_item then
					info.on_update_item(item, new_real_index, info.data_list[new_real_index])
				end
			end
			--把节点移到队尾
			table.insert(self.item_list, item)
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
				-- item.gameObject.name = new_real_index
				item:SetPosition(self.get_item_pos_xy_func(new_real_index))
				if info.on_update_item then
					info.on_update_item(item, new_real_index, info.data_list[new_real_index])
				end
			end
			--把节点移到队头
			table.insert(self.item_list, 1, item)
		end
	end
end

--为滚动容器绑定滚动事件,在滚动时刷新子节点的坐标,把超过边界不可见的节点移到可见区域
local function BindScrollEvent( self )
	if not self.had_bind_scroll and self.info.scroll_view ~= nil then
		self.had_bind_scroll = true		
		local on_scroll_changed = function (  )
			HandleScrollChange(self)
		end
		self.scroll_view_scr.onValueChanged:AddListener(on_scroll_changed)
	end
end

function UI.ItemListCreator:HideAllItems()
	if self.step_create_item_id then
		--正在创建子节点中,创建完后会再次调用本函数的
		return
	end
	for i,v in ipairs(self.item_list) do
		if v.gameObject then
			v.gameObject:SetActive(false)
		end
	end
end

--Cat_Todo : jump to child
function UI.ItemListCreator:ScrollToItem(item_index, is_need_animte)

end

--水平加垂直布局,一般没特殊需求直接用UpdateItems这个接口就行了
function UI.ItemListCreator:UpdateItemsGrid(info)
	local is_init_ok = InitInfo(self, info)
	if not is_init_ok then return end
	
	if self.step_create_item_id then
		--有可能再次调用本类的刷新函数时还没创建完上次调用刷新的子节点们,所以需要等创建完后再刷新一次
		self.is_update_after_delay_create_items = true
		return
	end
	--先刷新容器的大小坐标等信息
	self.get_item_pos_xy_func = UpdateContainerQuads(self, info)
	--获取子节点的创建函数
	local creator = GetItemCreator(self, info)
	
	--可视区域内最多可以显示多少个节点,尽量只创建少点节点,因为就算在滚动容器不可见的地方,节点也要占drawcall的
	local max_visual_item_num = 0
	if self.is_reuse_items then
		max_visual_item_num = info.reuse_item_num or self:GetMaxVisualItemNum(info)
	else
		max_visual_item_num = #info.data_list
	end
	local min_item_num = math.min(#info.data_list, max_visual_item_num)

	if info.create_frequency ~= nil and self.is_first_update then
		self.is_first_update = false--首次刷新时才需要延时创建节点
		self.cur_create_index = 1
		self.max_create_num = min_item_num
		if self.scroll_view_scr then
			--延迟创建时不让滚动
			self.scroll_view_scr.enabled = false
		end
		local function step_create_item( )
			creator(self.cur_create_index, info.data_list[self.cur_create_index])
			self.cur_create_index = self.cur_create_index + 1
			if self.cur_create_index > self.max_create_num then
				if self.step_create_item_id then
					GlobalTimerQuest:CancelQuest(self.step_create_item_id)
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
					self:UpdateItemsGrid(info)
				end
			end
		end
		if not self.step_create_item_id then
			self.step_create_item_id = GlobalTimerQuest:AddPeriodQuest(step_create_item, info.create_frequency, -1)
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
	HandleScrollChange(self)
end

--获取可视区域内最多可显示多少个子节点 
function UI.ItemListCreator:GetMaxVisualItemNum(info)
	local show_type = self.show_type or "Grid"
	if show_type == "Vert" then
		local visual_h = GetSizeDeltaY(info.scroll_view)
		return round((visual_h+info.space_y) / self.offset_y)+1
	elseif show_type == "Hori" then
		local visual_w = GetSizeDeltaX(info.scroll_view)
		return round((visual_w+info.space_x) / self.offset_x)+1
	elseif show_type == "Grid" then
		local visual_h = GetSizeDeltaY(info.scroll_view)
		local max_row = round((visual_h+info.space_y) / self.offset_y)+1
		return info.show_col * max_row
	end
	assert(false, "scroll visual size too small for show items")
	return 0
end

function UI.ItemListCreator:Reset()
	self:OnDestroy()
end

--垂直布局,一般没特殊需求直接用UpdateItems这个接口就行了
function UI.ItemListCreator:UpdateItemsVert(info)
	local is_init_ok = InitInfo(self, info)
	if not is_init_ok then return end
	info.show_col = 1
	self.show_type = "Vert"
	self:UpdateItemsGrid(info)
end

--水平布局,一般没特殊需求直接用UpdateItems这个接口就行了
function UI.ItemListCreator:UpdateItemsHori(info)
	local is_init_ok = InitInfo(self, info)
	if not is_init_ok then return end
	info.show_col = info.data_list and #info.data_list or 0
	self.show_type = "Hori"
	self:UpdateItemsGrid(info)
end

--遍历所有子节点
function UI.ItemListCreator:IterateItems(func)
	if not func then return end
	
	for i=1,#self.item_list do
		func(self.item_list[i])
	end
end

--关闭界面时清除创建的节点
function UI.ItemListCreator:OnDestroy()
	self.had_bind_scroll = false
	self.is_first_update = true
	self.last_unvisible_row = 0
	
	if self.scroll_view_scr then
		self.scroll_view_scr:StopMovement()
	end
	if self.info and self.info.item_con then
		SetLocalPosition(self.info.item_con, 0, 0)
	end

	if self.destroy_pool_type then
		for i,item in pairs(self.item_list) do
			UIObjPool:PushItem(self.destroy_pool_type, item)
		end
		self.item_list = {} 
	else
		for k,v in pairs(self.item_list) do
			v:DeleteMe()
			v = nil
		end
		self.item_list = {}
	end
	if self.scroll_view_scr and self.had_bind_scroll then
		self.scroll_view_scr.onValueChanged:RemoveAllListeners()
		self.scroll_view_scr = nil
	end
	if self.step_create_item_id then
		GlobalTimerQuest:CancelQuest(self.step_create_item_id)
		self.step_create_item_id = nil
	end
end