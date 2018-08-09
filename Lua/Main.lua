require("Logic.Game")

--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")	 
	UpdateManager:GetInstance():Startup()
	-- TimerManager:GetInstance():Startup()

	Game.OnInitOK()
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
	UpdateManager:GetInstance():Dispose()
	-- TimerManager:GetInstance():Startup()

end