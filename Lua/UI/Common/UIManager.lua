UIManager = UIManager or {}
UIMgr = UIManager

UIMgr.ShowType = {
	HideOther = 1,--显示时隐藏底下的界面
	Normal = 2,--显示到最前面,不管底下的界面

}

local SetViewVisible = function ( view, visible )
	view.gameObject:SetActive(visible)
end
function UIMgr:Init( canvas_names, default_canvas_name )
	self.canvas = {}
	for i,v in ipairs(canvas_names) do
		local canvas = GameObject.Find(v)
		local key = v
		--把/符号前的字符串去掉
		if key and find(key,"/") then
			key = string.gsub(key,".+/","")
		end
		self.canvas[key] = canvas.transform
	end
	self.default_canvas = default_canvas_name
	self.opened_views = {}
end

--界面堆栈的大小,当显示的界面数量超过此大小时,每打开一新界面都会移除最旧一个的界面
function UIMgr:SetViewMaxStackSize( max_stack )
	UIMgr.max_stack = max_stack
end

--清掉界面堆栈
function UIMgr:ClearViewStack( )

end

function UIMgr:Show( view )
	assert(view, "cannot open a nil view")
	assert(view.UIConfig, "cannot find UIConfig in view", view)
	assert(view.UIConfig.prefab, "cannot find UIConfig.prefab in view", view)
	assert(self.canvas and next(self.canvas)~=nil, "must register canvas before show view")

	view.UIConfig.canvas_name = view.UIConfig.canvas_name or self.default_canvas
	view.UIConfig.show_type = view.UIConfig.show_type or UIMgr.ShowType.HideOther
	if view.UIConfig.show_type == UIMgr.ShowType.HideOther then
		for k,v in pairs(self.opened_views) do
			SetViewVisible(v, false)
		end

	end
	table.insert(self.opened_views, view)

	local on_load_succeed = function ( gameObject )
		print('Cat:UIMgr.lua[on_load_succeed]')
		view.gameObject = gameObject
		view.transform = gameObject.transform
		view.gameObject.layer = LayerMask.NameToLayer("UI")
		view.transform:SetParent(self.canvas[view.UIConfig.canvas_name])
		view.transform.localScale = Vector3.one
        view.transform.localPosition = Vector3.zero
		if view.UIConfig.components then
			for i,v in ipairs(view.UIConfig.components) do
				if v.OnLoadOk then
					v.OnLoadOk(view)	
				end
			end
		end
		if view.OnLoadOk then
			view:OnLoadOk()
		end
	end
	PanelMgr:CreatePanel(view.UIConfig.prefab, on_load_succeed)
end

function UIMgr:Close( view )
	assert(view, "cannot close a nil view")
	SetViewVisible(view, false)

	--界面和其它组件处理OnClose时有可能把view.UIConfig.is_wait_destroy设为false,比如延迟销毁组件
	view.UIConfig.is_wait_destroy = true
	if view.OnClose then
		view:OnClose()
	end
	if view.UIConfig.components then
		for i,v in ipairs(view.UIConfig.components) do
			if v.OnClose then
				v.OnClose(view)
			end
		end
	end
	if view.UIConfig.is_wait_destroy then
		GameObject.Destroy(view.gameObject)
	end
	
	self.opened_views[#self.opened_views] = nil
	local last_view = self.opened_views[#self.opened_views]
	if last_view then
		SetViewVisible(last_view, true)
	end
	--恢复上个界面
	if view.UIConfig.show_type == UIMgr.ShowType.HideOther then

	end
end

function UIMgr:InitComponents( view )
	
end

return UIMgr