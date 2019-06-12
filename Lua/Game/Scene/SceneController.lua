local SceneController = {}

function SceneController:Init(  )
	print('Cat:SceneController.lua[Init]')
    self.sceneNode = GameObject.Find("UICanvas/Scene")
	self.mainCamera = Camera.main
	self:InitEvents()

end

function SceneController:InitEvents(  )
    local on_down = function ( target, x, y )
    	print('Cat:SceneController.lua[down] x, y', target, x, y)
    	self.touch_begin_x = x
    	self.touch_begin_y = y
    end
	UIHelper.BindClickEvent(self.sceneNode, on_down)

	local on_up = function ( target, x, y )
    	print('Cat:SceneController.lua[up] x, y', target, x, y)
    	local wpos = self.mainCamera:ScreenToWorldPoint(Vector3(x, y, 0))
    	print('Cat:SceneController.lua[22] wpos.x, wpos.y, wpos.z', wpos.x, wpos.y, wpos.z)
    end
	UIHelper.BindClickEvent(self.sceneNode, on_up)


	print('Cat:SceneController.lua[27] CS.Unity.Entities.EntityManager', CS.Unity.Entities.EntityManager)
	local get_comp_generic = xlua.get_generic_method(CS.Unity.Entities.EntityManager, 'GetComponentData')
	print('Cat:SceneController.lua[28] get_comp_generic', get_comp_generic)
	local get_comp_fun = get_comp_generic(CS.UnityMMO.UID)
	print('Cat:SceneController.lua[31] get_comp_fun', get_comp_fun)
	-- get_comp_fun(SceneMgr.Instance.EntityManager, )

end

return SceneController