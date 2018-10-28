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
		UpdateManager:GetInstance():AddUpdate(TestController.Update, self)	
	end
end

function TestController:Update(  )
	if CS.UnityEngine.Input.GetKeyUp(CS.UnityEngine.KeyCode.F) then 
		print('Cat:TestController.lua[25] f up')
		local testView = require("Game/Test/TestView")
		UIMgr:Show(testView.New())
		-- self.test_flag = self.test_flag or 0
		-- self.test_flag = self.test_flag + 1
		-- Message:Show("hahahaha : "..self.test_flag)
	else
	end
end

return TestController