local LoginSceneBgView = {}

function LoginSceneBgView:SetActive( isActive )
	self.isActive = isActive
	print('Cat:LoginSceneBgView.lua[5] self.is_inited', self.is_inited)
	if not self.is_inited then
		self:Init()
	end
end

function LoginSceneBgView:Init(  )
	self.main_camera = GameObject.Find("MainCamera")
	-- self.main_camera_cam = self.main_camera:GetComponent("Camera")
	self:LoadSceneView()
end

function LoginSceneBgView:SwitchLoginCamera( is_login_camera )
	if is_login_camera then
		self.main_camera.gameObject:SetActive(false)
		self.login_camera.gameObject:SetActive(true)
	else
		self.main_camera.gameObject:SetActive(true)
		self.login_camera.gameObject:SetActive(false)
		-- self.main_camera_cam.orthographic = true
		-- self.main_camera_cam.transform.rotation = Vector3(0, 0, 0)
	end
end

function LoginSceneBgView:LoadSceneView()
	local camera_prefab_name = "Assets/AssetBundleRes/scene/login/objs/other_effect/chuangjue/camera_for_create_role.prefab"
	local on_load_camera_ok = function ( go )
		print('Cat:LoginSceneBgView.lua[32] go', go)
		-- if not objs or not objs[0] then
		-- 	logWarn("cannot find camera prefab name : "..camera_prefab_name)
		-- 	return
		-- end
		-- local go = newObject(objs[0])
		self.login_camera = go
		self.camera_animator = go:GetComponent("Animator")
		self.camera = go.transform
		self:SwitchLoginCamera(true)
	end
	ResMgr:LoadPrefabGameObject(camera_prefab_name, on_load_camera_ok)
	
	-- local load_scene_res = function (  )
		local scene_prefab_name = "Assets/AssetBundleRes/scene/login/objs/other_effect/chuangjue/scene_for_login.prefab"
		local on_load_scene_ok = function ( go )
			print('Cat:LoginSceneBgView.lua[47] go', go)
			self.scene_obj = go
		end
		ResMgr:LoadPrefabGameObject(scene_prefab_name, on_load_scene_ok)
	-- end
	-- self:PreloadRes(load_scene_res)
end

function LoginSceneBgView:Hide(  )
	
end

return LoginSceneBgView