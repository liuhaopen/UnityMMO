local SceneController = {}

function SceneController:Init(  )
	print('Cat:SceneController.lua[Init]')
    self.sceneNode = GameObject.Find("UICanvas/Scene")
	self.mainCamera = Camera.main
	self:InitEvents()

end

function SceneController:InitEvents(  )
	local on_start_game = function (  )
		print('Cat:SceneController.lua[13]')
		ECS:Init(SceneMgr.Instance.EntityManager)
	end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, on_start_game)

    local on_down = function ( target, x, y )
    	print('Cat:SceneController.lua[down] x, y', target, x, y)
    	self.touch_begin_x = x
    	self.touch_begin_y = y
    end
	UIHelper.BindClickEvent(self.sceneNode, on_down)

	local on_up = function ( target, x, y )
    	-- print('Cat:SceneController.lua[up] x, y', target, x, y)
    	-- local wpos = self.mainCamera:ScreenToWorldPoint(Vector3(x, y, 0))
    	-- print('Cat:SceneController.lua[22] wpos.x, wpos.y, wpos.z', wpos.x, wpos.y, wpos.z)
        SceneMgr.Instance:GetSceneObjectByPos(Vector3.zero)
    end
	UIHelper.BindClickEvent(self.sceneNode, on_up)

end

return SceneController