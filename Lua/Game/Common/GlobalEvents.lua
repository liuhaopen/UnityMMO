--lua端的全局事件号以10000起始，c#的是1~9999
local GlobalEvents = BaseClass(CS.UnityMMO.GlobalEvents)
GlobalEvents.GameStart = 10000
GlobalEvents.SetMainUIVisible = 10001

return GlobalEvents