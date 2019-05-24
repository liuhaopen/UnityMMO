local MainUITaskView = BaseClass()

function MainUITaskView:DefaultVar( )
	return { 
	UIConfig = {
		prefab_path = "Assets/AssetBundleRes/ui/mainui/MainUITaskView.prefab",
		canvas_name = "Normal",
		components = {
				{UI.HideOtherView},
				{UI.DelayDestroy, {delay_time=5}},
			},
		},
	}
end

function MainUITaskView:OnLoad(  )
	local names = {

	}
	UI.GetChildren(self, self.transform, names)
	
	self.model = TaskModel:GetInstance()
	self:AddEvents()
	self:UpdateView()
end

function MainUITaskView:AddEvents(  )
	local on_click = function ( click_btn )
		
	end
	-- UIHelper.BindClickEvent(self.return_btn, on_click)

	self.AckTaskList_ID = self.model:Bind(TaskConst.Events.AckTaskList, function()
		self:UpdateView()
	end)
	
	-- if self.AckTaskList_ID then
	-- 	self.model:UnBind(self.AckTaskList_ID)
	-- 	self.AckTaskList_ID = nil
	-- end
end

function MainUITaskView:UpdateView(  )
	print('Cat:MainUITaskView.lua[34] self.is_loaded', self.is_loaded)
	if not self.is_loaded then return end

	local taskList = self.model:GetTaskList()
end

return MainUITaskView