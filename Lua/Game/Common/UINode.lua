local UINode = BaseClass()

function UINode:Constructor( parentTrans )
	self.parentTrans = parentTrans
	print('Cat:UINode.lua[5] self.parentTrans', self.parentTrans)
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
    	self.transform.anchoredPosition = Vector2.zero
        if self.__cacheLocalPos then
        	UI.SetLocalPositionXYZ(self.transform, self.__cacheLocalPos.x, self.__cacheLocalPos.y, self.__cacheLocalPos.z or 0)
        elseif self.__cachePos then
        	UI.SetPositionXYZ(self.transform, self.__cachePos.x, self.__cachePos.y, self.__cachePos.z or 0)
        else
        	local localPos = self.transform.localPosition
        	localPos.z = 0
       		self.transform.localPosition = localPos
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

function UINode:Destroy(  )
	GameObject.Destroy(self.gameObject)
end

function UINode:OnDestroy(  )
	--override me
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

return UINode