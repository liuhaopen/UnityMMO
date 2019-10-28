local UINode = BaseClass()

function UINode:Constructor( )
end

function UINode.Create( prefabPath, parentTransOrCanvasName, canvasName )
	local node = UINode.New()
	node.viewCfg = {
		prefabPath = prefabPath,
	}
	if type(parentTransOrCanvasName)=="string" then
		node.viewCfg.canvasName = parentTransOrCanvasName
	else
		node.viewCfg.parentTrans = parentTransOrCanvasName
	end
	return node
end

function UINode:Load(  )
	if self.isLoaded then 
		if self.viewCfg.parentTrans then
			self.transform:SetParent(self.viewCfg.parentTrans)
		elseif self.viewCfg and self.viewCfg.canvasName then
			UIMgr:AddToCanvas(self, self.viewCfg.canvasName)
		end
		return 
	end
	assert(self.viewCfg, "has no assign viewCfg before load")
	assert(self.viewCfg.prefabPath or self.viewCfg.prefabPoolName, "has no assign prefabPath or prefabPoolName field")
	if self.viewCfg.prefabPath then
		local on_load_succeed = function ( gameObject )
			self:Init(gameObject)
		end
		ResMgr:LoadPrefabGameObject(self.viewCfg.prefabPath, on_load_succeed)
	elseif self.viewCfg.prefabPoolName then
		local widget = PrefabPool:Get(self.viewCfg.prefabPoolName)
		self.viewCfg.prefabPoolObj = widget
		self:Init(widget.gameObject)
	end
end

function UINode:UpdateTransform(  )
	self.transform.localScale = Vector3.one
	self.transform.localRotation = Quaternion.identity
	if self.transform.anchoredPosition then
		self.transform.anchoredPosition = Vector2.zero
	end
    if self.__cacheLocalPos then
    	UI.SetLocalPositionXYZ(self.transform, self.__cacheLocalPos.x, self.__cacheLocalPos.y, self.__cacheLocalPos.z or 0)
    	self.__cacheLocalPos = nil
    elseif self.__cachePos then
    	UI.SetPositionXYZ(self.transform, self.__cachePos.x, self.__cachePos.y, self.__cachePos.z or 0)
    	self.__cachePos = nil
    else
    	local localPos = self.transform.localPosition
    	localPos.z = 0
   		self.transform.localPosition = localPos
	end
end

function UINode:Init( gameObject )
	self.gameObject = gameObject
	if self.viewCfg and self.viewCfg.name then
		gameObject.name = self.viewCfg.name
	end
	self.transform = gameObject.transform
	self.gameObject.layer = CS.UnityEngine.LayerMask.NameToLayer("UI")
	if self.viewCfg and self.viewCfg.parentTrans then
		self.transform:SetParent(self.viewCfg.parentTrans)
	elseif self.viewCfg and self.viewCfg.canvasName then
		UIMgr:AddToCanvas(self, self.viewCfg.canvasName)
	end
	self:UpdateTransform()
	if self.__cacheVisible ~= nil then
		self.gameObject:SetActive(self.__cacheVisible)
		self.__cacheVisible = nil
	end
	self:InitComponents()--这句一定要在self.isLoaded = true之前，不然会重复调用组件的OnLoad方法
	self.isLoaded = true
	if self.OnLoad then
		self:OnLoad()
	end
	if self.isNeedUpdateOnLoad then
		self:OnUpdate()
	end
end

function UINode:Unload(  )
	self:Destroy()
end

function UINode:OnDestroy(  )
	self.isLoaded = nil
	self.destroyed = true
	if self.attachNode then
		self:Detach()
	end
	if self.autoDestroyNodes then
		for i,v in ipairs(self.autoDestroyNodes) do
			if v.Unload then
				v:Unload()
			else
				v:Destroy()
			end
		end
		self.autoDestroyNodes = nil
	end
	if self.bindEventInfos then
		for k,v in pairs(self.bindEventInfos) do
			if v[1] and v[2] then
				v[1]:UnBind(v[2])
			end
		end
		self.bindEventInfos = nil
	end
	if self.viewCfg and self.viewCfg.loadedComponents then
		for i,v in ipairs(self.viewCfg.loadedComponents) do
			v:OnClose()
		end
		self.viewCfg.loadedComponents = nil
	end
	if self.action_nodes then
		for k,v in pairs(self.action_nodes) do
			cc.ActionManager:getInstance():removeAllActionsFromTarget(v)
		end
		self.action_nodes = nil
	end
	GameObject.Destroy(self.gameObject)
	if self.unloadCallBack then
		self.unloadCallBack()
	end
end

function UINode:OnUpdate(  )
	--override me
end

function UINode:SetData( data )
	self.data = data
	if self.isLoaded then
		self:OnUpdate()
	else
		self.isNeedUpdateOnLoad = true
	end
end

function UINode:BindEvent( bindDispather, eventName, handleFunc )
	assert(bindDispather~=nil, "bindDispather must be not nil!")
	self.bindEventInfos = self.bindEventInfos or {}
	local hadBind = self.bindEventInfos[eventName..tostring(bindDispather)]
	if hadBind then 
		print('Cat:UINode.lua had bind event name : ', eventName)
		return
	end
	-- print('Cat:UINode.lua[69] eventName..tostring(bindDispather)', eventName..tostring(bindDispather))
	local eventID = bindDispather:Bind(eventName, handleFunc)
	self.bindEventInfos[eventName..tostring(bindDispather)] = {bindDispather, eventID}
	return eventID
end

function UINode:SetPositionXYZ( x, y, z )
	if self.isLoaded then
		UI.SetPositionXYZ(self.transform, x, y, z or 0)
	else
		self.__cachePos = {x=x,y=y,z=z}
	end
end

function UINode:SetLocalPositionXYZ( x, y, z )
	if self.isLoaded then
		UI.SetLocalPositionXYZ(self.transform, x, y, z)
	else
		self.__cacheLocalPos = {x=x,y=y,z=z}
	end
end

function UINode:SetParent( parent )
	self.viewCfg = self.viewCfg or {}
	self.viewCfg.parentTrans = parent
	if self.isLoaded then
		self.transform:SetParent(parent)
		self:UpdateTransform()
	end
end

function UINode:SetCanvas( canvasName )
	self.viewCfg.canvasName = canvasName
	if self.isLoaded then
		UIMgr:AddToCanvas(self, self.viewCfg.canvasName)
	end
end

function UINode:GetPositionXYZ(  )
	if self.isLoaded then
		return UI.GetPositionXYZ(self.transform)
	else
		return self.__cachePos or Vector3.zero
	end
end

function UINode:GetLocalPositionXYZ(  )
	if self.isLoaded then
		return UI.GetLocalPositionXYZ(self.transform)
	else
		return self.__cacheLocalPos or Vector3.zero
	end
end

function UINode:SetActive( visible )
	if self.isLoaded then
		self.gameObject:SetActive(visible)
	else
		self.__cacheVisible = visible
	end
end

function UINode:JustShowMe( subNode )
	if self.lastShowSubNode ~= nil and self.lastShowSubNode ~= subNode then
 		self.lastShowSubNode:SetActive(false)
 	end
 	self.lastShowSubNode = subNode
 	self.lastShowSubNode:SetActive(true)
end

--关闭界面时销毁该节点
function UINode:AutoDestroy( deleteNode )
	self.autoDestroyNodes = self.autoDestroyNodes or {}
	table.insert(self.autoDestroyNodes, deleteNode)
end

function UINode:AddUIComponent( component, arge )
	-- print('Cat:UINode.lua[AddUIComponent] component, arge', component, tostring(arge), self)
	assert(component~=nil, "cannot add nil component for view")
	local new_comp = component.New()
	new_comp:OnAwake(self, arge)
	if self.isLoaded then
		new_comp:OnLoad()
	end
	self.viewCfg.loadedComponents = self.viewCfg.loadedComponents or {}
	table.insert(self.viewCfg.loadedComponents, new_comp)
	return new_comp
end

function UINode:InitComponents(  )
	if not self.viewCfg then return end
	
	if self.viewCfg.isShowBackground then
		self:AddUIComponent(UI.Background)
	end
	if self.viewCfg.components then
		for i,v in ipairs(self.viewCfg.components) do
			self:AddUIComponent(v[1], v[2])
		end
	end
	if self.viewCfg and self.viewCfg.loadedComponents then
		for i,v in ipairs(self.viewCfg.loadedComponents) do
			v:OnLoad()
		end
	end
end

--有些时候我们只想扩展一个旧的UINode而不想另外新创建一个prefab，这时可以新建一个类，继承UINode并Attach(旧的UINode)
function UINode:Attach( uiNode )
	self.transform = uiNode.transform
	self.gameObject = uiNode.gameObject
	self.attachNode = uiNode
	self:InitComponents()
	self.isLoaded = true
	if self.OnLoad then
		self:OnLoad()
	end
	if self.isNeedUpdateOnLoad then
		self:OnUpdate()
	end
end

function UINode:Detach(  )
	self.transform = nil
	self.gameObject = nil
	self.attachNode = nil
end

function UINode:Hide(  )
	self.transform:SetParent(PrefabPool.hide_container)
end

function UINode:SetUnloadCallBack( callBack )
	self.unloadCallBack = callBack
end

function UINode:AddAction( action, node )
	self.action_nodes = self.action_nodes or {}
	self.action_nodes[node] = node
	-- table.insert(self.action_nodes, node)
	cc.ActionManager:getInstance():addAction(action, node)
end

return UINode