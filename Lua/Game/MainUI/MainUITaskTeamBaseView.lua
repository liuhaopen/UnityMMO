local MainUITaskTeamBaseView = BaseClass(UINode)

function MainUITaskTeamBaseView:Constructor( )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/mainui/MainUITaskTeamBaseView.prefab",
		canvasName = "MainUI",
		curTabID = nil,
	}
	-- print('Cat:MainUITaskTeamBaseView.lua[9] TaskModel', TaskModel)
	self.model = TaskModel:GetInstance()
end

function MainUITaskTeamBaseView:OnLoad(  )
	local names = {
		"sub_con","team_btn:obj","task_btn:obj","task_label","team_label",
	}
	UI.GetChildren(self, self.transform, names)

	self:AddEvents()
	self:SwitchTabView(self.curTabID or MainUIConst.TaskTeamTabID.Task, true)
end

function MainUITaskTeamBaseView:AddEvents(  )
	local on_click = function ( click_btn )
		if click_btn == self.task_btn_obj then
			self:SwitchTabView(MainUIConst.TaskTeamTabID.Task)
		elseif click_btn == self.team_btn_obj then
			self:SwitchTabView(MainUIConst.TaskTeamTabID.Team)
		end
	end
	UIHelper.BindClickEvent(self.task_btn_obj, on_click)
	UIHelper.BindClickEvent(self.team_btn_obj, on_click)

end

function MainUITaskTeamBaseView:SwitchTabView( tabID, force )
	if self.curTabID == tabID and not force then 
		return
	end
	print('Cat:MainUITaskTeamBaseView.lua[37] tabID', tabID, self.sub_con)
	if tabID == MainUIConst.TaskTeamTabID.Task then
		self.taskSubView = self.taskSubView or require("Game/MainUI/MainUITaskView").New(self.sub_con)
		print('Cat:MainUITaskTeamBaseView.lua[40] self.taskSubView', self.taskSubView)
		self:JustShowMe(self.taskSubView)
	elseif tabID == MainUIConst.TaskTeamTabID.Team then
		Message:Show("功能未开启")
		return
	end
	self.curTabID = tabID
end

return MainUITaskTeamBaseView