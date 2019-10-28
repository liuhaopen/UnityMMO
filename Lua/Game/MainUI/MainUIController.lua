require("Game.MainUI.MainUIConst")
require("Game.MainUI.MainUIModel")

MainUIController = {}

function MainUIController:Init(  )
    self.model = MainUIModel:GetInstance()

    self:AddEvents()
end		

function MainUIController:AddEvents(  )
    GlobalEventSystem:Bind(GlobalEvents.GameStart, MainUIController.InitMainUIViews, self)
    
    GlobalEventSystem:Bind(GlobalEvents.SetMainUIVisible, MainUIController.SetMainUIVisible, self)
end

function MainUIController:InitMainUIViews(  )
    print('Cat:MainUIController.lua[InitMainUIViews]')
    self.views = {}
    local view = require("Game.MainUI.MainUIJoystickView").New()
    UIMgr:Show(view)
    self.views[MainUIConst.View.Joystick] = view

    local view = require("Game.MainUI.MainUISkillBtnView").New()
    view:Load()
    self.views[MainUIConst.View.SkillBtn] = view

    local view = require("Game.MainUI.MainUIRoleHeadView").New()
    UIMgr:Show(view)
    self.views[MainUIConst.View.RoleHead] = view
    
    local view = require("Game.MainUI.MainUITaskTeamBaseView").New()
    view:Load()
    self.views[MainUIConst.View.Task] = view

    local view = require("Game.MainUI.MainUIActIconView").New()
    view:Load()
    self.views[MainUIConst.View.ActIcon] = view
    
    self.mainMenu = require("Game.MainUI.MainUIMenuView").New()
    self.mainMenu:Load()
    self.views[MainUIConst.View.MainMenu] = self.mainMenu
end

function MainUIController:SetMainUIVisible( viewType, visible, reson )
    local view = self.views[viewType]
    -- print('Cat:MainUIController.lua[51] viewType, reson, visible', viewType, reson, visible, view)
    if not view then
        print("Cat:MainUIController [SetMainUIVisible] unknow view type:", viewType)
        return
    end
    UI.UpdateVisibleByJury(view, visible, reson)
end

return MainUIController