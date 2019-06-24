require("Game/MainUI/MainUIConst")
require("Game/MainUI/MainUIModel")

MainUIController = {}

function MainUIController:Init(  )
    GlobalEventSystem:Bind(GlobalEvents.GameStart, MainUIController.InitMainUIViews, self)
    
end		

function MainUIController:InitMainUIViews(  )
    print('Cat:MainUIController.lua[InitMainUIViews]')
    local view = require("Game/MainUI/MainUIJoystickView").New()
    UIMgr:Show(view)

    local view = require("Game/MainUI/MainUISkillBtnView").New()
    UIMgr:Show(view)

    local view = require("Game/MainUI/MainUIRoleHeadView").New()
    UIMgr:Show(view)
    
    local view = require("Game/MainUI/MainUITaskTeamBaseView").New()
    view:Load()
end

return MainUIController