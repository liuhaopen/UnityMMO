local TestController = {}

function TestController:Init(  )
	if RuntimePlatform then
		if Application.platform == RuntimePlatform.Android then
			self.enable = false
		elseif Application.platform == RuntimePlatform.IPhonePlayer then
			self.enable = false
		else
			self.enable = true
		end
	end
	self.enable = true
	print('Cat:TestController.lua[14] self.enable', self.enable)
	if self.enable then
		self.__update_handle = BindCallback(self, TestController.Update)
		UpdateManager:GetInstance():AddUpdate(self.__update_handle)	
	end
end

function TestController:Update(  )
	if CS.UnityEngine.Input.GetKeyUp(CS.UnityEngine.KeyCode.F) then 
		print('Cat:TestController.lua[25] f up')
		if self.view_f then
			self.view_f:Destroy()
			self.view_f = nil
		end
        self.view_f = require("Game/Test/TestGoodsItem").New()
		-- local testView = require("Game/Test/TestView")
		-- UIMgr:Show(testView.New())

		-- self.test_flag = self.test_flag or 0
		-- self.test_flag = self.test_flag + 1
		-- Message:Show("hahahaha : "..self.test_flag)
	elseif CS.UnityEngine.Input.GetKeyUp(CS.UnityEngine.KeyCode.H) then 
		print('Cat:TestController.lua[h click]', self)
		-- self:TestGenericMethod()
		GMController:GetInstance():ReqExcuteGM([[goods,100000,3]])
	end
end

function TestController:TestGenericMethod(  )
	-- print('Cat:SceneController.lua[27] CS.Unity.Entities.EntityManager', CS.Unity.Entities.EntityManager)
	-- local get_comp_generic = xlua.get_generic_method(CS.Unity.Entities.EntityManager, 'GetComponentData')
	-- print('Cat:SceneController.lua[28] get_comp_generic', get_comp_generic)
	-- local get_comp_fun = get_comp_generic(CS.UnityMMO.UID)
	-- print('Cat:SceneController.lua[31] get_comp_fun', get_comp_fun)
    local mainRole = CS.UnityMMO.RoleMgr.GetInstance():GetMainRole()
 --    print("Cat:TestController [start:42] mainRole:", mainRole, mainRole.Entity)
 --    PrintTable(mainRole)
 --    print("Cat:TestController [end]")
 --    local uidData = get_comp_fun(SceneMgr.Instance.EntityManager, mainRole.Entity, CS.UnityMMO.UID)
 --    print("Cat:TestController [start:45] uidData:", uidData, uidData.Value)
 --    PrintTable(uidData)
 --    print("Cat:TestController [end]")

	-- local uidData = ECS:GetComponentData(mainRole.Entity, CS.UnityMMO.UID)
	-- print('Cat:TestController.lua[51] uidData.Value', uidData.Value)

	-- ECS:SetComponentData(mainRole.Entity, CS.UnityMMO.UID, {Value=123})
	-- local uidData = ECS:GetComponentData(mainRole.Entity, CS.UnityMMO.UID)
	-- print('Cat:TestController.lua[5111] uidData.Value', uidData.Value)

	print('Cat:TestController.lua[57] CS.UnityEngine.Transform', CS.UnityEngine.Transform)
	local trans = ECS:GetComponentObject(mainRole.Entity, CS.UnityEngine.Transform)
	print('Cat:TestController.lua[59] trans', trans)
end

return TestController