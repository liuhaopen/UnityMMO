UI = UI or {}
--UI组件的基类
UI.UIComponent = UI.UIComponent or {}
function UI.UIComponent:OnAwake( view_owner )
	--组件被加载时触发
	self.view_owner = view_owner
end
function UI.UIComponent:OnLoad( )
	--界面加载成功后触发
end
function UI.UIComponent:OnClose( )
	--界面销毁后触发
end

--半透明黑色背景组件
UI.Background = BaseClass(UI.UIComponent)
function UI.Background.OnLoad(view)
	print('Cat:UI.lua[Background OnLoad]', view)
	local bg = UIWidgetPool:CreateWidget("Background")
	if view.UIConfig.background_alpha then
		bg.gameObject:GetComponent("RawImage").alpha = view.UIConfig.background_alpha
	end
	UIHelper.SetParent(bg.transform, view.transform)
	UIHelper.SetLocalPosition(bg.transform, 0, 0, 0)
	UIHelper.SetSizeDelta(bg.transform, 1280 ,720)
	UIHelper.SetLocalScale(bg.transform, 1.2 , 1.2, 1)
	bg.transform:SetAsFirstSibling()
	view.UIConfig.bg_widget = bg
end

function UI.Background:SetIsClickToClose(click_bg_to_close)
	self.click_bg_to_close = click_bg_to_close
end

function UI.Background:SetBgAlpha(bg_alpha)
	self.bg_alpha = bg_alpha
end

function UI.Background.OnClose(view)
	print('Cat:UI.lua[Background OnClose]', view)
	if view.UIConfig.bg_widget then
		UIWidgetPool:RecycleWidget(view.UIConfig.bg_widget)
		view.UIConfig.bg_widget = nil
	end
end

--打开本界面时隐藏底下的界面
UI.HideOtherView = BaseClass(UI)
function UI.HideOtherView.OnLoad(view)
	print('Cat:UI.lua[HideOtherView OnLoad]', view)
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

function UI.HideOtherView.OnClose(view)
	print('Cat:UI.lua[HideOtherView OnClose]', view)
	if UIMgr:IsClosingAllView() then return end
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
UI.PlayOpenCloseSound = BaseClass(UI)
function UI.PlayOpenCloseSound.OnLoad(view)
	print('Cat:UI.lua[play open sound]', view)
end

function UI.PlayOpenCloseSound.OnClose(view)
	print('Cat:UI.lua[play close sound]', view, UIMgr:IsClosingAllView())
end

--延迟销毁界面
UI.DelayDestroy = BaseClass(UI)
function UI.DelayDestroy.OnClose(view)
	print('Cat:UI.lua[DelayDestroy]', view)
	view.UIConfig.is_destroyed = true
	local timer = Timer.New(function()
		GameObject.Destroy(view.gameObject)
	end, view.UIConfig.destroy_delay_time or 5)
	timer:Start()
end

function UI.DelayDestroy:SetDelayTime(time)
	self.delay_time = time
end

--新手引导组件
UI.NewGuide = BaseClass(UI)
function UI.NewGuide.OnLoad(view)
	--显示黑色遮罩，拦截点击事件
end	

--清除前面打开过的界面记录，这样关闭本界面后就不会回到上个界面了
UI.ClearOpenedViewStack = BaseClass(UI)
function UI.ClearOpenedViewStack.OnLoad(view)
	UIMgr:ClearViewStack()
	UIMgr:PushOpenedView(view)
end	