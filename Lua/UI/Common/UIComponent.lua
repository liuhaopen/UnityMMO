UIComponent = UIComponent or {}

--半透明黑色背景组件
UIComponent.Background = {}
function UIComponent.Background.OnLoad(view)
	print('Cat:UIComponent.lua[Background OnLoad]', view)
	local bg = UIWidgetPool:CreateWidget("Background")
	if view.UIConfig.background_alpha then
		bg.gameObject:GetComponent("RawImage").alpha = view.UIConfig.background_alpha
	end
	bg.transform:SetParent(view.transform)
	UIHelper.SetLocalPosition(bg.transform, 0, 0, 0)
	UIHelper.SetSizeDelta(bg.transform, 1280 ,720)
	bg.transform:SetAsFirstSibling()
	view.UIConfig.bg_widget = bg
end

function UIComponent.Background.OnClose(view)
	print('Cat:UIComponent.lua[Background OnClose]', view)
	if view.UIConfig.bg_widget then
		UIWidgetPool:RecycleWidget(view.UIConfig.bg_widget)
		view.UIConfig.bg_widget = nil
	end
end

--打开本界面时隐藏底下的界面
UIComponent.HideOtherView = {}
function UIComponent.HideOtherView.OnLoad(view)
	print('Cat:UIComponent.lua[HideOtherView OnLoad]', view)
	view.UIConfig.has_hide_other_view_component = true
	--当前已打开界面，从外向内逐个隐藏
	local opened_views = UIMgr:GetViewStack()
	print(" opened_views len :", #opened_views)
	for i=#opened_views,1,-1 do
		if view ~= opened_views[i] then
			UIMgr:SetViewVisible(opened_views[i], false)
			if opened_views[i].UIConfig.has_hide_other_view_component then
				--遇到有本组件界面就可以跳出循环了，因为再底下的界面已经被它隐藏了，我们不需要再隐藏多一次
				break
			end
		end
	end
end

function UIComponent.HideOtherView.OnClose(view)
	print('Cat:UIComponent.lua[HideOtherView OnClose]', view)
	local opened_views = UIMgr:GetViewStack()
	for i=#opened_views,1,-1 do
		if view ~= opened_views[i] then
			UIMgr:SetViewVisible(opened_views[i], true)
			if opened_views[i].UIConfig.has_hide_other_view_component then
				break
			end
		end
	end
end

--播放进入和关闭界面的音效
UIComponent.PlayOpenCloseSound = {}
function UIComponent.PlayOpenCloseSound.OnLoad(view)
	print('Cat:UIComponent.lua[play open sound]', view)
end

function UIComponent.PlayOpenCloseSound.OnClose(view)
	print('Cat:UIComponent.lua[play close sound]', view)
end

--延迟销毁界面
UIComponent.DelayDestroy = {}
function UIComponent.DelayDestroy.OnClose(view)
	print('Cat:UIComponent.lua[DelayDestroy]', view)
	view.UIConfig.is_destroyed = true
	local timer = Timer.New(function()
		GameObject.Destroy(view.gameObject)
	end, view.UIConfig.destroy_delay_time or 5)
	timer:Start()
end

--新手引导组件
UIComponent.NewGuide = {}
function UIComponent.NewGuide.OnLoad(view)
	--显示黑色遮罩，拦截点击事件
end	

--清除前面打开过的界面记录，这样关闭本界面后就不会回到上个界面了
UIComponent.ClearOpenedViewStack = {}
function UIComponent.ClearOpenedViewStack.OnLoad(view)
	UIMgr:ClearViewStack()
	UIMgr:PushOpenedView(view)
end	