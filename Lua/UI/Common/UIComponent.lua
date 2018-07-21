UIComponent = UIComponent or {}

--播放进入和关闭界面的音效
UIComponent.PlayOpenCloseSound = {}
function UIComponent.PlayOpenCloseSound.OnLoadOk(view)
	print('Cat:UIComponent.lua[play open sound]', view)
end

function UIComponent.PlayOpenCloseSound.OnClose(view)
	print('Cat:UIComponent.lua[play close sound]', view)
end

--延迟销毁界面
UIComponent.DelayDestroy = {}
function UIComponent.DelayDestroy.OnClose(view)
	print('Cat:UIComponent.lua[DelayDestroy]', view)
	if view.UIConfig.destroy_delay_time and view.UIConfig.destroy_delay_time > 0 then
		view.UIConfig.is_wait_destroy = false
		local timer = Timer.New(function()
			GameObject.Destroy(view.gameObject)
		end, view.UIConfig.destroy_delay_time or 5)
		timer:Start()
	else
		GameObject.Destroy(view.gameObject)
	end
end