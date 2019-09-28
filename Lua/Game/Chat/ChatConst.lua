local ChatConst = {
    Event = {
        UpdateChatList = "ChatConst.Event.UpdateChatList",
        SetChatViewVisible = "ChatConst.Event.SetChatViewVisible",
        SendChat = "ChatConst.Event.SendChat",
    },
    MaxHistoryNum = 50,
    -- ChatConst.Channel.World
    Channel = {
        World = 1,--世界频道
        Notify = 2,--系统通知
        Private = 3,--私人
        Team = 4,--队伍
        CS = 5,--跨服
    },
}
return ChatConst