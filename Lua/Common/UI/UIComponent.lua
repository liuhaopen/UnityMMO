UI = UI or {}
--UI组件的基类
UI.UIComponent = UI.UIComponent or {}
function UI.UIComponent:OnAwake( view_owner, arges )
	--组件被加载时触发
	self.view_owner = view_owner
	if arges then
		for k,v in pairs(arges) do
			self[k] = v
		end
	end
end
function UI.UIComponent:OnLoad( )
	--界面加载成功后触发
end
function UI.UIComponent:OnClose( )
	--界面销毁后触发
end

--半透明黑色背景组件
UI.Background = BaseClass(UI.UIComponent)
function UI.Background:OnLoad()
	-- print('Cat:UIComponent.lua[23] self.alpha, self.is_click_to_close', self.alpha, self.is_click_to_close)
	local bg = PrefabPool:Get("Background")
	self.bg_widget = bg
	self.bg_img = bg.gameObject:GetComponent("RawImage")
	self.bg_obj = bg.gameObject
	if self.alpha then
		local color = self.bg_img.color
		color.a = self.alpha
		self.bg_img.color = color
	end
	UIHelper.SetParent(bg.transform, self.view_owner.transform)
	UIHelper.SetLocalPosition(bg.transform, 0, 0, 0)
	UIHelper.SetSizeDelta(bg.transform, 1280 ,720)
	UIHelper.SetLocalScale(bg.transform, 1.2 , 1.2, 1)
	bg.transform:SetAsFirstSibling()
	if self.is_click_to_close then
		self.bg_img.raycastTarget = true
		local on_click = function ( )
			print('Cat:UIComponent.lua[41] self.view_owner.Unload', self.view_owner, self.view_owner.Unload)
			if self.view_owner.Unload then
				self.view_owner:Unload()
			else
				UIMgr:Close(self.view_owner)
			end
		end
		UI.BindClickEvent(self.bg_obj, on_click)
	end
end

function UI.Background:SetIsClickToClose(is_click_to_close)
	self.is_click_to_close = is_click_to_close
end

function UI.Background:SetAlpha(alpha)
	self.alpha = alpha
end

function UI.Background:OnClose()
	print('Cat:UI.lua[Background OnClose]')
	if self.bg_widget then
		PrefabPool:Recycle(self.bg_widget)
		self.bg_widget = nil
	end
end

--打开本界面时隐藏底下的界面
UI.HideOtherView = BaseClass(UI.UIComponent)
function UI.HideOtherView:OnLoad()
	print('Cat:UI.lua[HideOtherView OnLoad]')
	self.view_owner.has_hide_other_view_component = true
	--当前已打开界面，从外向内逐个隐藏
	local opened_views = UIMgr:GetViewStack()
	print(" opened_views len :", #opened_views)
	for i=#opened_views,1,-1 do
		if self.view_owner ~= opened_views[i] then
			UIMgr:SetViewVisible(opened_views[i], false)
			if opened_views[i].has_hide_other_view_component then
				--遇到有本组件界面就可以跳出循环了，因为再底下的界面已经被它隐藏了，我们不需要再隐藏多一次
				break
			end
		end
	end
end

function UI.HideOtherView:OnClose()
	-- print('Cat:UI.lua[HideOtherView OnClose]')
	if UIMgr:IsClosingAllView() then return end
	local opened_views = UIMgr:GetViewStack()
	-- print('Cat:UIComponent.lua[87] opened_views', opened_views)
	for i=#opened_views,1,-1 do
		if self.view_owner ~= opened_views[i] then
			UIMgr:SetViewVisible(opened_views[i], true)
			if opened_views[i].has_hide_other_view_component then
				break
			end
		end
	end
end

--播放进入和关闭界面的音效
UI.PlayOpenCloseSound = BaseClass(UI.UIComponent)
function UI.PlayOpenCloseSound:OnLoad()
	print('Cat:UI.lua[play open sound]')
end

function UI.PlayOpenCloseSound:OnClose()
	print('Cat:UI.lua[play close sound]', UIMgr:IsClosingAllView())
end

--延迟销毁界面
UI.DelayDestroy = BaseClass(UI.UIComponent)
function UI.DelayDestroy:OnClose()
	-- print('Cat:UI.lua[DelayDestroy]')
	self.view_owner.is_destroyed = true
	local timer = Timer.New(function()
		-- print('Cat:UIComponent.lua[Destroy]')
		GameObject.Destroy(self.view_owner.gameObject)
	end, self.delay_time or 5)
	timer:Start()
end

function UI.DelayDestroy:SetDelayTime(time)
	self.delay_time = time
end

--新手引导组件
UI.NewGuide = BaseClass(UI.UIComponent)
function UI.NewGuide:OnLoad()
	--显示黑色遮罩，拦截点击事件
end	

--清除前面打开过的界面记录，这样关闭本界面后就不会回到上个界面了
UI.ClearOpenedViewStack = BaseClass(UI.UIComponent)
function UI.ClearOpenedViewStack:OnLoad()
	UIMgr:ClearViewStack()
	UIMgr:PushOpenedView(view)
end	
