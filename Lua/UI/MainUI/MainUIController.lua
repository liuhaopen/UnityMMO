require("UI/MainUI/MainUIConst")
require("UI/MainUI/MainUIModel")

MainUIController = {}

function MainUIController:Init(  )
    self.login_succeed_handler = GlobalEventSystem:Bind(MainUIConst.Event.InitMainUIViews, MainUIController.InitMainUIViews, self)
    
end		

function MainUIController:InitMainUIViews(  )
    print('Cat:MainUIController.lua[InitMainUIViews]')
    local view = require("UI/MainUI/MainUIJoystickView").New()
    UIMgr:Show(view)
end

return MainUIController