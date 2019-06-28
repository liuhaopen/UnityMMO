local UINode = BaseClass()

function UINode:Constructor( parentTrans )
	self.parentTrans = parentTrans
	self.bindEventInfos = {}
end

function UINode:Load(  )
	assert(self.prefabPath, "cannot find prefabPath field")
	local on_load_succeed = function ( gameObject )
		self.gameObject = gameObject
		self.transform = gameObject.transform
		self.gameObject.layer = CS.UnityEngine.LayerMask.NameToLayer("UI")
		if self.parentTrans then
			self.transform:SetParent(self.parentTrans)
		else
			self.canvasName = self.canvasName or "Normal"
			UIMgr:AddToCanvas(self, self.canvasName)
		end
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
    	if self.__cacheVisible ~= nil then
    		self.gameObject:SetActive(self.__cacheVisible)
    		self.__cacheVisible = nil
    	end
    	if self._components_ then
			for i,v in ipairs(self._components_) do
				v:OnLoad()
			end
		end
    	self.isLoaded = true
		self:OnLoad()
		if self.isNeedUpdateOnLoad then
			self:OnUpdate()
		end
	end
	ResMgr:LoadPrefabGameObject(self.prefabPath, on_load_succeed)
end

function UINode:OnLoad(  )
	--override me
end

function UINode:OnDestroy(  )
	GameObject.Destroy(self.gameObject)
	self.isLoaded = nil
	for k,v in pairs(self.bindEventInfos) do
		if v[1] and v[2] then
			v[1]:UnBind(v[2])
		end
	end
	if self._components_ then
		for i,v in ipairs(self._components_) do
			v:OnClose()
		end
		self._components_ = nil
	end
	self.bindEventInfos = nil
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

function UINode:BindEvent( eventName, handleFunc, bindDispather )
	assert(bindDispather~=nil, "bindDispather must be not nil!")
	local hadBind = self.bindEventInfos[eventName..tostring(bindDispather)]
	if hadBind then 
		print('Cat:UINode.lua had bind event name : ', eventName)
		return
	end
	print('Cat:UINode.lua[69] eventName..tostring(bindDispather)', eventName..tostring(bindDispather))
	local eventID = bindDispather:Bind(eventName, handleFunc)
	self.bindEventInfos[eventName..tostring(bindDispather)] = {bindDispather, eventID}
	return eventID
end

function UINode:SetPositionXYZ( x, y, z )
	if self.isLoaded then
		UI.SetPositionXYZ(self.transform, x, y, z)
	else
		self.__cachePos = {x=x,y=y,z}
	end
end

function UINode:SetLocalPositionXYZ( x, y, z )
	if self.isLoaded then
		UI.SetLocalPositionXYZ(self.transform, x, y, z)
	else
		self.__cacheLocalPos = {x,y,z}
	end
end

function UINode:SetParent( parent )
	self.parentTrans = parent
	if self.isLoaded then
		self.transform:SetParent(parent)
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

function UINode:SetVisible( visible )
	if self.isLoaded then
		self.gameObject:SetActive(visible)
	else
		self.__cacheVisible = visible
	end
end

function UINode:JustShowMe( subNode )
	if self.lastShowSubNode ~= nil and self.lastShowSubNode ~= subNode then
 		self.lastShowSubNode:SetVisible(false)
 	end
 	self.lastShowSubNode = subNode
 	self.lastShowSubNode:SetVisible(true)
end

function UINode:AddUIComponent( component, arge )
	print('Cat:UINode.lua[AddUIComponent] component, arge', component, tostring(arge))
	assert(component~=nil, "cannot add nil component for view")
	local new_comp = component.New()
	new_comp:OnAwake(view, arge)
	if self.isLoaded then
		new_comp:OnLoad()
	end
	self._components_ = self._components_ or {}
	table.insert(self._components_, new_comp)
	return new_comp
end

return UINode