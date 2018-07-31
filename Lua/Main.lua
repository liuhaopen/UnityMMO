require "Common/define"
require("util.util")

--主入口函数。从这里开始lua逻辑
function Main()					
	print("logic start")	 
	-- UpdateBeat:Add(Update, self)	
end

function Update( )
	-- print("sssssss", Time.deltaTime, Time.unscaledDeltaTime)
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function OnApplicationQuit()
end