local MainUIMenuView = BaseClass(UINode)

MainUIMenuView.IconWidth = 74
MainUIMenuView.IconHeight = 74
MainUIMenuView.IconSpaceX = 10
MainUIMenuView.IconSpaceY = 10
function MainUIMenuView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUIMenuView.prefab",
		canvasName = "MainUI",
	}
end

function MainUIMenuView:OnLoad(  )
	local names = {
		"bag:obj","swith_btn:obj","icon_con","swith_red:obj","bag_red:obj","swith_ring",
	}
	UI.GetChildren(self, self.transform, names)

	self.icon_start_pos_x = self.icon_con.sizeDelta.x - MainUIMenuView.IconWidth - 20
    self.icon_start_pos_y = self.icon_con.sizeDelta.y - MainUIMenuView.IconHeight

	self:AddEvents()
	self:OnUpdate()
end

function MainUIMenuView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.bag_obj == click_obj then
			local view = require("Game.Bag.BagMainView").New()
    		view:Load()
		-- elseif self.main_city_obj == click_obj then
			-- SceneMgr.Instance:ReqEnterScene(1001, 0)
		end
	end
	-- UI.BindClickEvent(self.main_city_obj, on_click)
	UI.BindClickEvent(self.bag_obj, on_click)
	
end

function MainUIMenuView:OnUpdate(  )
	self:UpdateIconsLogicPos()
    if not self.is_loaded then return end

    self:UpdateIcons()
    self:UpdateRed()
    self:UpdateSwithIconRed()
end

function MainUIMenuView:UpdateIcons(  )
    for k,v in pairs(self.item_list) do
        v:SetVisible(false)
    end
    for i,v in ipairs(self.icon_infos) do
        local item = self.item_list[i]
        if not item then
            item = MainUIMenuViewIcon.New(self.icon_con)
            self.item_list[i] = item
        end
        local posX, posY = self:CalculateIconPos(v.visual_index)
        item:SetPosition(posX, posY)
    end
end

function MainUIMenuView:UpdateRed(  )
end

function MainUIMenuView:UpdateSwithIconRed(  )
end

--获取所有符合条件的图标数据
function MainUIMenuView:GetOpenFunctionIcon()
    local list = {}
    local roleVo = RoleManager:getInstance():GetMainRoleVo()
    if not roleVo then return end
    
    local myLv = roleVo.level
    local openDay = ServerTimeModel:getInstance():GetOpenServerDay()
    for k, v in pairs(Config.ConfigFunctionIcon.LeftBottom) do
        local openLv = v.open_lv <= myLv
        local openTaskOk = not v.open_task or TaskModel:getInstance():IsTaskFinished(v.open_task)
        local openDayOK = (v.open_day or 0) <= openDay
        if openLv and openTaskOk and openDayOK then
            list[v.icon_type] = v
        end
    end
    return list
end

--因为开启系统图标时，新手指导系统需要先拿到图标的坐标，所以图标的最终坐标需要先单独计算，不需要拿真实节点的，因为有可能图标还没创建成功呢
function MainUIMenuView:UpdateIconsLogicPos(  )
    local openIcons = self:GetOpenFunctionIcon()
    self.icon_infos = {}
    for k,v in pairs(openIcons) do
        table.insert(self.icon_infos, v)
    end
    local sort_func = function ( a, b )
        return a.pos_index < b.pos_index
    end
    table.sort(self.icon_infos, sort_func)
    local visual_counter = 1
    for i,v in ipairs(self.icon_infos) do
        local visual_index = visual_counter
        v.visual_index = visual_index
        visual_counter = visual_counter + 1
    end
end

function MainUIMenuView:CalculateIconPos( i )
    local startPosX = self.icon_start_pos_x
    local startPoxY = self.icon_start_pos_y
    local newX = startPosX
    if i > 3 then
        newX = newX - (MainUIMenuView.IconWidth+MainUIMenuView.IconSpaceX)*(i - 3) 
    end
    local newY = startPoxY
    if i > 3 then
        newY = newY - (MainUIMenuView.IconHeight+MainUIMenuView.IconSpaceY)*(2)
    else
        newY = newY - (MainUIMenuView.IconHeight+MainUIMenuView.IconSpaceY)*(i - 1)
    end
    return newX, newY
end

return MainUIMenuView