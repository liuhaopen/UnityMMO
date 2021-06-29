local scene_const = require("Game.Scene.scene_const")

local mt = {}

function mt:init()
	print('Cat:SceneController.lua[Init]')
    self.sceneNode = GameObject.Find("UICanvas/Scene")
    self.sceneNode:SetActive(false)
	self.mainCamera = Camera.main
	self:init_events()
end

function mt:init_events()
	local on_start_game = function (  )
        self.sceneNode:SetActive(true)
		ECS:Init(SceneMgr.Instance.EntityManager)
	end
    GlobalEventSystem:Bind(GlobalEvents.GameStart, on_start_game)

	local on_up = function ( target, x, y )
    	print('Cat:SceneController.lua[up] x, y', target, x, y, self.is_dragged)
        if self.is_dragged then
            self.is_dragged = false
            return
        end
        local hit = SceneHelper.GetClickSceneObject()
        print('Cat:SceneController.lua[34] hit.hit', hit.hit)
        if hit.hit then
            print('Cat:SceneController.lua[35] hit.entity, hit.point.x, hit.point.y', hit.entity, hit.point.x, hit.point.y, hit.point.z)
            -- print('Cat:SceneController.lua[30] ECS:HasComponent(hit.entity, CS.UnityMMO.Component.SceneObjectTypeData)', ECS:HasComponent(hit.entity, CS.UnityMMO.Component.SceneObjectTypeData))
            if hit.entity == Entity.Null then
                local goe = RoleMgr.GetInstance():GetMainRole()
                local moveQuery = goe:GetComponent(typeof(CS.UnityMMO.MoveQuery))
                local findInfo = {
                    destination = hit.point,
                    stoppingDistance = 0,
                }
                moveQuery:StartFindWay(findInfo)
                local fightAi = goe:GetComponent(typeof(CS.UnityMMO.AutoFight))
                if fightAi then
                    fightAi.enabled = false
                end
            elseif ECS:HasComponent(hit.entity, CS.UnityMMO.Component.SceneObjectTypeData) then
                local sceneObjType = ECS:GetComponentData(hit.entity, CS.UnityMMO.Component.SceneObjectTypeData)
                print('Cat:SceneController.lua[41] sceneObjType', sceneObjType.Value)
                if sceneObjType.Value == CS.UnityMMO.SceneObjectType.NPC then
                    local typeID = ECS:GetComponentData(hit.entity, CS.UnityMMO.Component.TypeID)
                    print('Cat:SceneController.lua[43] typeID.Value', typeID.Value, SceneMgr.Instance.CurSceneID)
                    TaskController:GetInstance():DoTalk({sceneID=SceneMgr.Instance.CurSceneID, npcID=typeID.Value})
                end
            end
        end
    end
	UI.BindClickEvent(self.sceneNode, on_up)

    local on_drag_begin = function ( obj, x, y )
        print('Cat:SceneController.lua[85]on_drag_begin obj, x, y', obj, x, y)
        self.is_dragged = true
        if not self.lastMousePos then
            self.lastMousePos = {x=x, y=y}
            self.freeLookCamera = SceneMgr.Instance.FreeLookCamera
            self.cameraRotateSpeed = {x=1280/3, y=720/250}
            self.screenSize = {x=CS.UnityEngine.Screen.width, y=CS.UnityEngine.Screen.height}
            -- self.cameraCtrl = CS.UnityMMO.CameraCtrl.Instance
            self.cameraCtrl = self.freeLookCamera:GetComponent("CameraCtrl")
            -- print('Cat:SceneController.lua[65] self.cameraCtrl', self.cameraCtrl, self.cameraCtrl)
        end
        self.lastMousePos.x = x
        self.lastMousePos.y = y
    end
    UI.BindDragBeginEvent(self.sceneNode, on_drag_begin)

    local on_drag = function ( obj, x, y )
        -- print('Cat:SceneController.lua[68] obj, x, y', obj, x, y)
        self.cameraCtrl:ApplyMove((x-self.lastMousePos.x)/self.screenSize.x*self.cameraRotateSpeed.x, 
            (y-self.lastMousePos.y)/self.screenSize.y*self.cameraRotateSpeed.y)
        self.lastMousePos.x = x
        self.lastMousePos.y = y
    end
    UI.BindDragEvent(self.sceneNode, on_drag)

    local MainRoleDie = function ( killerUID )
        print('Cat:SceneController.lua[58] self.reliveView', self.reliveView, killerUID)
        if not self.reliveView then
            self.reliveView = require("Game.Scene.ReliveView").New()
            self.reliveView:Load()
            self.reliveView:SetUnloadCallBack(function()
                self.reliveView = nil
            end)
        end
        self.reliveView:SetData(killerUID)
    end
    print('Cat:SceneController.lua[66] GlobalEvents.MainRoleDie', GlobalEvents.MainRoleDie, CSLuaBridge.GetInstance())
    print('Cat:SceneController.lua[67] CSLuaBridge.GetInstance:SetLuaFuncNum', CSLuaBridge.GetInstance().SetLuaFuncNum)
    CSLuaBridge.GetInstance():SetLuaFuncNum(GlobalEvents.MainRoleDie, MainRoleDie)
end

function mt:on_actor_enter()
    global.actor_mgr:add_actor(info)
end

return mt