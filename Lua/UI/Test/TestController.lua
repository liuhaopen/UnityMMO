local TestController = {}
local this = TestController

function TestController.Init(  )
	if RuntimePlatform then
		if Application.platform == RuntimePlatform.Android then
			this.enable = false
		elseif Application.platform == RuntimePlatform.IPhonePlayer then
			this.enable = false
		else
			this.enable = true
		end
	end
	this.enable = true
	print('Cat:TestController.lua[14] this.enable', this.enable)
	if this.enable then
		UpdateBeat:Add(TestController.Update)	
	end
end

function TestController.Update(  )
	-- print('Cat:TestController.lua[Update]')
	if UnityEngine.Input.GetKeyUp(UnityEngine.KeyCode.F) then 
		print('Cat:TestController.lua[25] f up')
		-- local testView = require("UI/Test/TestView")
		-- UIMgr:Show(testView.New())
		this.test_flag = this.test_flag or 0
		this.test_flag = this.test_flag + 1
		Message:Show("hahahaha : "..this.test_flag)
	else
	end
end

return TestController