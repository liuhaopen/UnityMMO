local UILooksNode = BaseClass(UINode)

UILooksNode.ShowType = {
	Role = 1,
	Monster = 2,
	NPC = 3,
}
function UILooksNode:Constructor( parentTrans )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/common/UILooksNode.prefab",
		parentTrans = parentTrans,
	}
	self.cacheRole = {}
	self.cacheNPC = {}
	self:Load()
end

function UILooksNode:OnLoad(  )
	local names = {
		"container","camera"
	}
	UI.GetChildren(self, self.transform, names)

	self.camera = self.camera:GetComponent("Camera")
	UI.SetLocalPositionXYZ(self.transform, 9999, -9999, 12345)
	self:AddEvents()
end

function UILooksNode:AddEvents(  )
	if self.data and self.data.canRotate then

	end
end

function UILooksNode:SetData( data )
	self.lastData = self.data
	UINode.SetData(self, data)
end

function UILooksNode:OnUpdate(  )
	if not self.data then return end

	self:Reset()
	if self.data.showType == UILooksNode.ShowType.Role then
		self:LoadRole()
	elseif self.data.showType == UILooksNode.ShowType.NPC then
		self:LoadNPC()
	end
end

function UILooksNode:Reset(  )
	if not self.lastData then return end

	if self.lastData.showType == UILooksNode.ShowType.Role then
		if not IsNull(self.bodyTrans) then
			self.bodyTrans.gameObject:SetActive(false)
			self.cacheRole[self.curShowCareer..","..self.curShowBody..","..self.curShowHair] = self.bodyTrans
		end
	elseif self.lastData.showType == UILooksNode.ShowType.NPC then
		if not IsNull(self.npcTrans) then
			self.npcTrans.gameObject:SetActive(false)
			self.cacheNPC[self.curShowNPCID] = self.npcTrans
		end
	end
	self.curShowNPCID = nil
	self.curShowCareer = nil
	self.curShowBody = nil
	self.curShowHair = nil
end

function UILooksNode:LoadRole( )
	if (self.curShowCareer and self.curShowCareer==self.data.career) and (self.curShowBody and self.curShowBody == self.data.body) and (self.curShowHair and self.curShowHair == self.data.hair) then
		return
	end
	-- if not IsNull(self.bodyTrans) then
	-- 	self.bodyTrans.gameObject:SetActive(false)
	-- 	self.cacheRole[self.curShowCareer..","..self.curShowBody..","..self.curShowHair] = self.bodyTrans
	-- end
	self.curShowCareer = self.data.career
	local body = self.data.body
	local hair = self.data.hair
	self.curShowBody = body
	self.curShowHair = hair
	local cacheTrans = self.cacheRole[self.data.career..","..self.data.body..","..self.data.hair]
	if not IsNull(cacheTrans) then
		cacheTrans.gameObject:SetActive(true)
		self.bodyTrans = cacheTrans
		return
	end
	local bodyPath = ResPath.GetRoleBodyResPath(self.data.career, self.data.body)
    local on_load_body_succeed = function ( bodyObj )
    	if self.is_destroyed then return end
    	self.bodyTrans = bodyObj.transform
    	self.bodyTrans:SetParent(self.container)
        self.bodyTrans.localPosition = Vector3.zero
    	self.bodyTrans.localScale = Vector3.one
		self.bodyTrans.localRotation = Quaternion.identity
		UI.SetLayer(bodyObj, "UILooksNode", true)
    	self:UpdateRenderTexture()
    	self:LoadRoleHair(hair)
    end
    ResMgr:LoadPrefabGameObject(bodyPath, on_load_body_succeed)
end

function UILooksNode:LoadRoleHair( hair )
	local on_load_hair_succeed = function ( hairObj )
		if self.is_destroyed then return end
		local headBoneTrans = self.bodyTrans:Find("head")
		self.hairTrans = hairObj.transform
		self.hairTrans:SetParent(headBoneTrans)
        self.hairTrans.localPosition = Vector3.zero
    	self.hairTrans.localScale = Vector3.one
		self.hairTrans.localRotation = Quaternion.identity
		UI.SetLayer(hairObj, "UILooksNode", true)
	end
	local hairPath = ResPath.GetRoleHairResPath(self.curShowCareer, hair)
	ResMgr:LoadPrefabGameObject(hairPath, on_load_hair_succeed)
end

function UILooksNode:UpdateRenderTexture(  )
	if self.renderTexture then return end
	local imgWidth, imgHeight = UI.GetSizeDeltaXY(self.data.showRawImg.transform)
	local renderWidth = self.data.renderWidth or imgWidth
	local renderHeight = self.data.renderHeight or imgHeight
	renderWidth = math.floor(renderWidth)
	renderHeight = math.floor(renderHeight)
	self.renderTexture = CS.UnityEngine.RenderTexture(renderWidth, renderHeight, 24)
	self.camera.targetTexture = self.renderTexture
	self.data.showRawImg.texture = self.renderTexture
	UI.SetSizeDeltaXY(self.data.showRawImg.transform, renderWidth, renderHeight)
end

function UILooksNode:LoadNPC(  )
	if (self.curShowNPCID and self.curShowNPCID==self.data.npcID) then
		return
	end
	-- if not IsNull(self.npcTrans) then
	-- 	self.npcTrans.gameObject:SetActive(false)
	-- 	self.cacheNPC[self.curShowNPCID] = self.npcTrans
	-- end
	self.curShowNPCID = self.data.npcID
	local cacheTrans = self.cacheNPC[self.data.npcID]
	if not IsNull(cacheTrans) then
		cacheTrans.gameObject:SetActive(true)
		self.npcTrans = cacheTrans
		return
	end
	local looksPath = ResPath.GetNPCLooksPath(self.data.npcID)
    local on_load_body_succeed = function ( bodyObj )
    	if self.is_destroyed then return end
    	self.npcTrans = bodyObj.transform
    	self.npcTrans:SetParent(self.container)
        self.npcTrans.localPosition = Vector3.zero
    	self.npcTrans.localScale = Vector3.one
		self.npcTrans.localRotation = Quaternion.identity
		UI.SetLayer(bodyObj, "UILooksNode", true)
    	self:UpdateRenderTexture()
    end
    ResMgr:LoadPrefabGameObject(looksPath, on_load_body_succeed)
end

function UILooksNode:SetRenderObject( object )
end

function UILooksNode:OnDestroy(  )
	
end

return UILooksNode
