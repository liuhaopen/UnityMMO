local MainUIMenuView = BaseClass(UINode)
local MainUIMenuIcon = require("Game.MainUI.MainUIMenuIcon")

MainUIMenuView.IconWidth = 74
MainUIMenuView.IconHeight = 74
MainUIMenuView.IconSpaceX = 10
MainUIMenuView.IconSpaceY = 10
function MainUIMenuView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUIMenuView.prefab",
		canvasName = "MainUI",
	}
    self.model = MainUIModel:GetInstance()
    self.red_infos = {}
    self.item_list = {}
    self.callback_afr_move = self:AddUIComponent(UI.Countdown)
end

function MainUIMenuView:OnLoad(  )
	local names = {
		"bag:obj","swith_btn:obj","icon_con:obj",
	}
	UI.GetChildren(self, self.transform, names)

	self.icon_start_pos_x = self.icon_con.sizeDelta.x - MainUIMenuView.IconWidth - 24
    self.icon_start_pos_y = -15

	self:AddEvents()
    self:OnUpdate()
    self:SetActive(false)
end

function MainUIMenuView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.bag_obj == click_obj then
			local view = require("Game.Bag.BagMainView").New()
    		view:Load()
		elseif self.swith_btn_obj == click_obj then
            -- Message:Show("尚未开放其它系统")
            self:SetActive(not self.show_menu)
		end
	end
	UI.BindClickEvent(self.swith_btn_obj, on_click)
	UI.BindClickEvent(self.bag_obj, on_click)
	
end

function MainUIMenuView:OnUpdate(  )
    self:UpdateIconsLogicPos()
    if not self.isLoaded then return end

    self:UpdateIcons()
    self:UpdateRed()
    self:UpdateSwithIconRed()
end

function MainUIMenuView:UpdateIcons(  )
    for k,v in pairs(self.item_list) do
        v:SetActive(false)
    end
    for i,v in ipairs(self.icon_infos) do
        local item = self.item_list[i]
        if not item then
            item = MainUIMenuIcon.New(self.icon_con)
            self.item_list[i] = item
            item:Load()
        end
        item:SetData(v)
        item:SetActive(true)
        local posX, posY = self:CalculateIconPos(v.visual_index)
        item:SetLocalPositionXYZ(posX, posY, 0)
    end
end

function MainUIMenuView:UpdateRed(  )
    for i,v in ipairs(self.item_list) do
        v:UpdateRedDot()
    end
end

function MainUIMenuView:UpdateSwithIconRed(  )
end

--获取所有符合条件的图标数据
function MainUIMenuView:GetOpenFunctionIcon()
    local list = {}

    local myLv = MainRole:GetInstance():GetLv()
    for k, v in pairs(MainUIConst.MainIcons) do
        local openLv = v.open_lv <= myLv
        local openTaskOk = not v.open_task or TaskModel:GetInstance():IsTaskFinished(v.open_task)
        if openLv and openTaskOk then
            list[v.id] = v
        end
    end
    return list
end

--因为开启系统图标时，新手指导系统需要先拿到图标的坐标，所以图标的最终坐标需要先单独计算，不需要拿真实节点的，因为有可能图标还没创建成功呢
function MainUIMenuView:UpdateIconsLogicPos(  )
    local openIcons = self:GetOpenFunctionIcon()
    print("Cat:MainUIMenuView [start:99] openIcons: ", openIcons)
    PrintTable(openIcons)
    print("Cat:MainUIMenuView [end]")
    self.icon_infos = {}
    for k,v in pairs(openIcons) do
        table.insert(self.icon_infos, v)
    end
    local sort_func = function ( a, b )
        return a.order < b.order
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

function MainUIMenuView:PlayActionForSwitchBtn(  )
    local runner = Cocos.ActionRunner.GetOrCreate(self.swith_btn_obj)
    runner:Stop()
    local moveAction = Cocos.RotateBy.CreateLocal(0.3, Vector3(0,0,360))
    runner:PlayAction(moveAction)   
end

--先收缩 后伸展
function MainUIMenuView:SetActive(show_menu, is_force)
    if self.show_menu == show_menu and not is_force then return end
    self.callback_afr_move:Stop()
    self.show_menu = show_menu
    local animate_time = 0.3
    if show_menu then
        GlobalEventSystem:Fire(GlobalEvents.SetMainUIVisible, MainUIConst.View.SkillBtn, false, "ForMainMenu")
        GlobalEventSystem:Fire(GlobalEvents.SetMainUIVisible, MainUIConst.View.SmallChat, false, "ForMainMenu")
        
        self:PlayActionForSwitchBtn(-360, animate_time)
        self.icon_con_obj:SetActive(true)

        for k,item in pairs(self.item_list) do
            item:SetLocalPositionXYZ(self.icon_start_pos_x, self.icon_start_pos_y, 0)
            local posX, posY = self:CalculateIconPos(item:GetVisualIndex())
            item:MoveToPos({x=posX, y=posY}, animate_time)
        end
    else
        GlobalEventSystem:Fire(GlobalEvents.SetMainUIVisible, MainUIConst.View.SkillBtn, true, "ForMainMenu")
        GlobalEventSystem:Fire(GlobalEvents.SetMainUIVisible, MainUIConst.View.SmallChat, true, "ForMainMenu")

        self:PlayActionForSwitchBtn(360, animate_time)
        
        for k,item in pairs(self.item_list) do
            item:MoveToPos({x=self.icon_start_pos_x, y=self.icon_start_pos_y+20}, animate_time)
        end

        self.callback_afr_move:DelayCallByLeftTime(animate_time, function()
            self.icon_con_obj:SetActive(false)
        end, 0.1)
    end
    self:UpdateSwithIconRed()
end

return MainUIMenuView