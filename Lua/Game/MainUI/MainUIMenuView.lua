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
    self.model = MainUIModel:GetInstance()
    self.red_infos = {}
    self.item_list = {}
    self.callback_afr_move = self:AddUIComponent(UI.Countdown)
end

function MainUIMenuView:OnLoad(  )
	local names = {
		"bag:obj","swith_btn:obj","icon_con",
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
		elseif self.swith_btn_obj == click_obj then
            self:ShowMenuList(not self.show_menu)
		end
	end
	UI.BindClickEvent(self.swith_btn_obj, on_click)
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
        item:SetPosition(posX, posY)
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
    local roleVo = RoleManager:GetInstance():GetMainRoleVo()
    if not roleVo then return end
    
    local myLv = roleVo.level
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

function MainUIMenuView:PlayActionForSwitchBtn(  )
    local runner = self.swith_btn_obj.AddComponent(typeof(Cocos.ActionRunner))
    local moveAction = Cocos.MoveBy.CreateLocal(1, Vector3(50,30,40))
    local action = Cocos.Sequence.Create(moveAction, Cocos.DelayTime.Create(1), Cocos.FadeIn.Create(0.5))
    runner:PlayAction(action)   
end

--先收缩 后伸展
function MainUIMenuView:ShowMenuList(show_menu, is_force)
    if self.show_menu == show_menu and not is_force then return end
    self.callback_afr_move:Stop()
    self.show_menu = show_menu
    local animate_time = 0.3
    if show_menu then
        GlobalEventSystem:Fire(GlobalEvents.SetMainUIVisible, MainUIConst.View.SkillBtn, true, "ForMainMenu")
        -- GlobalEventSystem:Fire(EventName.HIDE_MAIN_CHAT_VIEW, true, MainUIModel.OTHER_MODE, true)
        -- GlobalEventSystem:Fire(EventName.HIDE_RIGHT_BOTTOM_VIEW, true, MainUIModel.OTHER_MODE, true)
        
        self:PlayActionForSwitchBtn(-360, animate_time)
        -- TweenLite.to(self, self.swith_btn_img, TweenLite.UiAnimationType.ROTATION, -360, animate_time)
        self.icon_con_obj:SetActive(true)

        for k,item in pairs(self.item_list) do
            item:SetPosition(self.icon_start_pos_x, self.icon_start_pos_y)
            local posX, posY = self:CalculateIconPos(item:GetVisualIndex())
            item:MoveToPos(Vector2(posX, posY), animate_time)
        end
    else
        GlobalEventSystem:Fire(GlobalEvents.SetMainUIVisible, MainUIConst.View.SkillBtn, false, "ForMainMenu")
        -- GlobalEventSystem:Fire(EventName.HIDE_MAIN_CHAT_VIEW, false, MainUIModel.OTHER_MODE, true)
        -- GlobalEventSystem:Fire(EventName.HIDE_RIGHT_BOTTOM_VIEW, false, MainUIModel.OTHER_MODE, true)

        self:PlayActionForSwitchBtn(360, animate_time)
        -- TweenLite.to(self, self.swith_btn_img, TweenLite.UiAnimationType.ROTATION, 360, animate_time)
        
        for k,item in pairs(self.item_list) do
            item:MoveToPos(Vector2(self.icon_start_pos_x, self.icon_start_pos_y), animate_time)
        end

        self.callback_afr_move:DelayCallByLeftTime(animate_time, function()
            self.icon_con_obj:SetActive(false)
        end, 0.1)
    end
    self:UpdateSwithIconRed()
    GlobalEventSystem:Fire(EventName.BROADCAST_SWITCH_BTN_STATE, self.show_menu)
end

return MainUIMenuView