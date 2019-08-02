--deprecated,UINode is better
UIManager = UIManager or {}
UIMgr = UIManager
function UIMgr:Init( canvas_names, default_canvas_name )
	self.canvas = {}
	for i,v in ipairs(canvas_names) do
		local canvas = GameObject.Find(v)
		local key = v
		--把/符号前的字符串去掉
		if key and string.find(key,"/") then
			key = string.gsub(key,".+/","")
		end
		self.canvas[key] = canvas.transform
	end
	self.default_canvas = default_canvas_name
	self.opened_views = {}
	self.before_show_func = {}
end

function UIMgr:AddBeforeShowFunc( func )
	table.insert(self.before_show_func, func)
end

--界面堆栈的大小,当显示的界面数量超过此大小时,每打开一新界面都会移除最旧一个的界面
function UIMgr:SetViewMaxStackSize( max_stack )
	--Cat_Todo : 优化:界面堆栈大过某大小时自动清理旧界面
	self.max_stack = max_stack
end

function UIMgr:GetViewStack(  )
	return self.opened_views
end

function UIMgr:AddUIComponent( view, component, arge )
	-- print('Cat:UIManager.lua[35] view, component, arge', view, component, tostring(arge))
	assert(view~=nil, "cannot add component for a nil view")
	assert(component~=nil, "cannot add nil component for view")
	local new_comp = component.New()
	new_comp:OnAwake(view, arge)
	if view.is_loaded then
		new_comp:OnLoad()
	end
	view._components_for_uimgr_ = view._components_for_uimgr_ or {}
	table.insert(view._components_for_uimgr_, new_comp)
	-- print("Cat:UIManager [start:45] new_comp:", new_comp, new_comp.OnLoad)
	return new_comp
end

--清掉界面堆栈
function UIMgr:ClearViewStack( )
	self.opened_views = {}
end

function UIMgr:PushOpenedView( view )
	table.insert(self.opened_views, view)
end

function UIMgr:SetViewVisible(view, visible)
	local last_visible = view.gameObject.activeSelf
	view.gameObject:SetActive(visible)
	if last_visible ~= visible and view.OnVisibleChanged then
		view.OnVisibleChanged(visible)
	end
end

function UIMgr:AssignBaseFunc( view )
	if not view then return end
	
	assert(not view.SetPosition, tostring(view).." already has SetPosition function")
	assert(not view.SetLocalPosition, tostring(view).." already has SetLocalPosition function")
	assert(not view.SetParent, tostring(view).." already has SetParent function")
	assert(not view.GetPosition, tostring(view).." already has GetPosition function")
	assert(not view.GetLocalPosition, tostring(view).." already has GetLocalPosition function")

	view.SetPositionXYZ = function(view, x, y, z)
		if view.is_loaded then
			UI.SetPositionXYZ(view.transform, x, y, z)
		else
			view.__cachePos = {x,y,z}
		end
	end
	view.SetLocalPositionXYZ = function(view, x, y, z)
		if view.is_loaded then
			UI.SetLocalPositionXYZ(view.transform, x, y, z)
		else
			view.__cacheLocalPos = {x,y,z}
		end
	end
	view.GetPositionXYZ = function(view)
		if view.is_loaded then
			return UI.GetPositionXYZ(view.transform)
		else
			return view.__cachePos or Vector3.zero
		end
	end
	view.GetLocalPositionXYZ = function(view)
		if view.is_loaded then
			return UI.GetLocalPositionXYZ(view.transform)
		else
			return view.__cacheLocalPos or Vector3.zero
		end
	end
end

function UIManager:AddToCanvas( node, canvasName )
	if canvasName and self.canvas[canvasName] then
		node.transform:SetParent(self.canvas[canvasName])
		-- self:PushOpenedView(view)
	end
end

function UIMgr:Load( view )
	assert(view, "cannot open a nil view")
	assert(view.UIConfig, "cannot find UIConfig in view")
	assert(view.UIConfig.prefab_path, "cannot find UIConfig.prefab_path in view")
	assert(self.canvas and next(self.canvas)~=nil, "must register canvas before show view")

	for i,v in ipairs(self.before_show_func) do
		v(view)
	end
	self:DressUpNode(view)
	local on_load_succeed = function ( gameObject )
		view.gameObject = gameObject
		view.transform = gameObject.transform
		view.gameObject.layer = CS.UnityEngine.LayerMask.NameToLayer("UI")
		local canvas_name = view.UIConfig.canvas_name
		if canvas_name and self.canvas[canvas_name] then
			view.transform:SetParent(self.canvas[canvas_name])
			self:PushOpenedView(view)
		end
		view.transform.localScale = Vector3.one
        view.transform.anchoredPosition = Vector3.zero
        local localPos = view.transform.localPosition
        localPos.z = 0
        view.transform.localPosition = localPos
		if view.UIConfig.components then
			for i,v in ipairs(view.UIConfig.components) do
				self:AddUIComponent(view, v[1], v[2])
			end
		end
		if view._components_for_uimgr_ then
			for i,v in ipairs(view._components_for_uimgr_) do
				v:OnLoad()
			end
		end
		view.is_loaded = true
		if view.OnLoad then
			view:OnLoad()
		end
	end
	ResMgr:LoadPrefabGameObject(view.UIConfig.prefab_path, on_load_succeed)
	return view
end

function UIMgr:Show( view )
	assert(view, "cannot open a nil view")
	assert(view.UIConfig, "cannot find UIConfig in view", view)
	assert(view.UIConfig.prefab_path, "cannot find UIConfig.prefab_path in view", view)
	assert(self.canvas and next(self.canvas)~=nil, "must register canvas before show view")

	for i,v in ipairs(self.before_show_func) do
		v(view)
	end

	local on_load_succeed = function ( gameObject )
		view.gameObject = gameObject
		view.transform = gameObject.transform
		view.gameObject.layer = CS.UnityEngine.LayerMask.NameToLayer("UI")
		local canvas_name = view.UIConfig.canvas_name
		if canvas_name and self.canvas[canvas_name] then
			view.transform:SetParent(self.canvas[canvas_name])
			self:PushOpenedView(view)
		end
		view.transform.localScale = Vector3.one
        view.transform.anchoredPosition = Vector3.zero
        local localPos = view.transform.localPosition
        localPos.z = 0
        view.transform.localPosition = localPos
		if view.UIConfig.components then
			for i,v in ipairs(view.UIConfig.components) do
				self:AddUIComponent(view, v[1], v[2])
			end
		end
		if view._components_for_uimgr_ then
			for i,v in ipairs(view._components_for_uimgr_) do
				v:OnLoad()
			end
		end
		view.is_loaded = true
		if view.OnLoad then
			view:OnLoad()
		end
	end
	ResMgr:LoadPrefabGameObject(view.UIConfig.prefab_path, on_load_succeed)
end

function UIMgr:RemoveAllComponents( view )
	if view._components_for_uimgr_ then
		for i,v in ipairs(view._components_for_uimgr_) do
			v:OnClose()
		end
		view._components_for_uimgr_ = nil
	end
end

--Cat_Todo : 如果传入的view不是最前的界面要怎么处理?应该在组件里处理就行了:UI.HideOtherView
function UIMgr:Close( view )
	assert(view, "cannot close a nil view")

	self:SetViewVisible(view, false)

	if view.OnClose then
		view:OnClose()
	end
	self:RemoveAllComponents(view)
	if not view.is_destroyed then
		GameObject.Destroy(view.gameObject)
	end
	self.opened_views[#self.opened_views] = nil
end

function UIMgr:CloseAllView(  )
	self.is_closing_all_view = true
	for i=#self.opened_views,1,-1 do
		self:Close(self.opened_views[i])
	end
	self.is_closing_all_view = false
end

function UIMgr:IsClosingAllView(  )
	return self.is_closing_all_view
end

return UIMgr