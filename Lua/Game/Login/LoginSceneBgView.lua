local LoginSceneBgView = {}

function LoginSceneBgView:SetActive( isActive )
	self.isActive = isActive
	if not self.is_inited then
		self.is_inited = true
		self:Init()
	end
	if self.scene_obj then
		self.scene_obj:SetActive(self.isActive)
	end
	if self.login_camera then
		self:SwitchLoginCamera(self.isActive)
	end
	if self.delayDestroyTimer then
		self.delayDestroyTimer:Stop()
		self.delayDestroyTimer = nil
	end
	if not self.isActive then
		--隐藏5秒后销毁本场景资源
		self.delayDestroyTimer = self.delayDestroyTimer or Timer.New(function()
        	GameObject.Destroy(self.scene_obj)
        	GameObject.Destroy(self.login_camera)
		end, 5)
		self.delayDestroyTimer:Start()
	end
end

function LoginSceneBgView:Init(  )
	self.main_camera = GameObject.Find("MainCamera")
	self:LoadSceneView()
end

function LoginSceneBgView:SwitchLoginCamera( is_login_camera )
	if is_login_camera then
		self.main_camera.gameObject:SetActive(false)
		self.login_camera.gameObject:SetActive(true)
	else
		self.main_camera.gameObject:SetActive(true)
		self.login_camera.gameObject:SetActive(false)
	end
end

function LoginSceneBgView:LoadSceneView()
	local camera_prefab_name = "Assets/AssetBundleRes/scene/login/objs/other_effect/chuangjue/camera_for_create_role.prefab"
	local on_load_camera_ok = function ( go )
		self.login_camera = go
		self.camera_animator = go:GetComponent("Animator")
		self.camera = go.transform
		self:SwitchLoginCamera(true)
	end
	ResMgr:LoadPrefabGameObject(camera_prefab_name, on_load_camera_ok)
	
	local scene_prefab_name = "Assets/AssetBundleRes/scene/login/objs/other_effect/chuangjue/scene_for_login.prefab"
	local on_load_scene_ok = function ( go )
		self.scene_obj = go
	end
	ResMgr:LoadPrefabGameObject(scene_prefab_name, on_load_scene_ok)
end

return LoginSceneBgView