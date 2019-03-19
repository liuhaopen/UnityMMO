require("Game/MainUI/MainUIConst")
require("Game/MainUI/MainUIModel")

MainUIController = {}

function MainUIController:Init(  )
    self.login_succeed_handler = GlobalEventSystem:Bind(MainUIConst.Event.InitMainUIViews, MainUIController.InitMainUIViews, self)
    
end		

function MainUIController:InitMainUIViews(  )
    print('Cat:MainUIController.lua[InitMainUIViews]')
    local view = require("Game/MainUI/MainUIJoystickView").New()
    UIMgr:Show(view)

    local view = require("Game/MainUI/MainUISkillBtnView").New()
    UIMgr:Show(view)
end

return MainUIController