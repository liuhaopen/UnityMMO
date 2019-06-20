local MeshNode = BaseClass(UINode)

MeshNode.ShowType = {
	Role = 1,
	Monster = 2,
	NPC = 3,
}
local parentTrans = GameObject.Find("UICanvas/MeshNodeCon").transform
function MeshNode:Constructor(  )
	self.prefabPath = "Assets/AssetBundleRes/ui/common/MeshNode.prefab"
	self.parentTrans = parentTrans
	self:Load()
end

function MeshNode:OnLoad(  )
	local names = {
		"container","camera"
	}
	UI.GetChildren(self, self.transform, names)

	self.camera = self.camera:GetComponent("Camera")
	print('Cat:MeshNode.lua[20] self.camera', self.camera)

	self:AddEvents()
end

function MeshNode:AddEvents(  )
	if self.data and self.data.canRotate then

	end
end

function MeshNode:OnUpdate(  )
	if not self.data then return end

	if self.data.showType == MeshNode.ShowType.Role then
		self:LoadRole()
	end
end

function MeshNode:LoadRole( )
	local bodyPath = ResPath.GetRoleBodyResPath(self.data.career, self.data.bodyID)
    local on_load_body_succeed = function ( bodyObj )
    	if self.is_destroyed then return end
    	local bodyTrans = bodyObj.transform
    	bodyTrans:SetParent(self.container)
        bodyTrans.localPosition = Vector3.zero
    	bodyTrans.localScale = Vector3.one
		bodyTrans.localRotation = Quaternion.identity
		UI.SetLayer(bodyObj, "MeshNode", true)
    	self:UpdateRenderTexture()
    	local on_load_hair_succeed = function ( hairObj )
    		if self.is_destroyed then return end
    		local headBoneTrans = bodyTrans:Find("head")
    		local hairTrans = hairObj.transform
    		hairTrans:SetParent(headBoneTrans)
	        hairTrans.localPosition = Vector3.zero
	    	hairTrans.localScale = Vector3.one
			hairTrans.localRotation = Quaternion.identity
			UI.SetLayer(hairObj, "MeshNode", true)
    	end
    	local hairPath = ResPath.GetRoleHairResPath(self.data.career, self.data.hairID)
    	ResMgr:LoadPrefabGameObject(hairPath, on_load_hair_succeed)
    end
    ResMgr:LoadPrefabGameObject(bodyPath, on_load_body_succeed)
end

function MeshNode:UpdateRenderTexture(  )
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

function MeshNode:SetRenderObject( object )
end

function MeshNode:OnDestroy(  )
	
end

return MeshNode
