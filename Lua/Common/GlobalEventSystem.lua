local EventSystem = require "Common.EventSystem"
GlobalEventSystem = EventSystem.New()

function GlobalEventSystem.Init(  )
	print('Cat:GlobalEventSystem.lua[Init]')
	--为了性能考虑，一些c#侧的事件在lua侧只监听一次，然后在lua内部转发，减少c#与lua的交互
	local SceneChanged = function ( )
		GlobalEventSystem:Fire(GlobalEvents.SceneChanged)
	end
	CSLuaBridge.GetInstance():SetLuaFunc(GlobalEvents.SceneChanged, SceneChanged)

	local MessageShow = function ( content )
		Message:Show(content)
	end
	CSLuaBridge.GetInstance():SetLuaFuncStr(GlobalEvents.MessageShow, MessageShow)

	local AlertShow = function ( showData )
		UI.AlertView.Show(showData)
	end
	CSLuaBridge.GetInstance():SetLuaFuncStr(GlobalEvents.AlertShow, AlertShow)
	
	local ExpChanged = function ( newExp, isUpgrade )
		print("Cat:GlobalEventSystem [start:23] ", newExp, isUpgrade)
		GlobalEventSystem:Fire(GlobalEvents.ExpChanged, newExp, isUpgrade)
	end
	CSLuaBridge.GetInstance():SetLuaFunc2Num(GlobalEvents.ExpChanged, ExpChanged)
end