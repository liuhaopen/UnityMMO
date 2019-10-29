local TaskDialogView = BaseClass(UINode)

function TaskDialogView:Constructor(  )
	self.viewCfg = {
		prefabPath = "Assets/AssetBundleRes/ui/task/TaskDialogView.prefab",
		canvasName = "Normal",
	}
	self.model = TaskModel:GetInstance()
	self:Load()
end

function TaskDialogView:OnLoad(  )
	local names = {
		"skip_con/skip_btn:obj","bottom/npc:raw","bottom/btn:obj","bottom/npc_name:txt","bottom/chat:txt","click_bg:obj","bottom/btn/btn_label:txt","bottom/left_time:txt",
	}
	UI.GetChildren(self, self.transform, names)
	self.transform.sizeDelta = Vector2.zero

	self:AddEvents()
end

function TaskDialogView:HandleBtnClick(  )
	print('Cat:TaskDialogView.lua[26] self.curShowData.clickCallBack', self.curShowData.clickCallBack)
	if self.curShowData.clickCallBack then
		self.curShowData.clickCallBack()
	else
		self:Unload()
	end
end

function TaskDialogView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.btn_obj == click_obj then
			self:HandleBtnClick()
		elseif self.skip_btn_obj == click_obj then
			self:Unload()
		elseif self.click_bg_obj == click_obj then
			self:Unload()
		end
	end
	UI.BindClickEvent(self.btn_obj, on_click)
	UI.BindClickEvent(self.skip_btn_obj, on_click)
	UI.BindClickEvent(self.click_bg_obj, on_click)
	
end

function TaskDialogView:ShowNextTalk(  )
    self.curShowTalkNum = self.curShowTalkNum + 1
	self:OnUpdate()
end

function TaskDialogView:ReqTakeTask(  )
	local ackTakeTask = function ( ackData )
		if ackData.result == NoError then
        	self:Unload()
        else
        	Message:Show(ConfigMgr:GetErrorDesc(ackData.result))
        end
    end
    NetDispatcher:SendMessage("Task_TakeTask", {taskID=self.curShowData.taskID}, ackTakeTask)
end

function TaskDialogView:ReqDoTask(  )
	local ackDoTask = function ( ackData )
		if ackData.result == NoError then
        	self:Unload()
        else
        	Message:Show(ConfigMgr:GetErrorDesc(ackData.result))
        end
    end
    NetDispatcher:SendMessage("Task_DoTask", {taskID=self.curShowData.taskID}, ackDoTask)
end

function TaskDialogView:ClickOk(  )
	self:Unload()
end

function TaskDialogView:ProcessBtnNameAndCallBack( flag )
	if not self.flagMap then
		self.flagMap = {
			[TaskConst.DialogBtnFlag.Continue] = {name="继续", func=TaskDialogView.ShowNextTalk},
			[TaskConst.DialogBtnFlag.TakeTask] = {name="接取", func=TaskDialogView.ReqTakeTask},
			[TaskConst.DialogBtnFlag.DoTask]   = {name="完成", func=TaskDialogView.ReqDoTask},
			[TaskConst.DialogBtnFlag.Ok] 	   = {name="确定", func=TaskDialogView.ClickOk},
		}
	end
	local flagInfo = self.flagMap[flag]
	if not flagInfo then return end
	
	self.curShowData.btnName = flagInfo.name
    self.curShowData.clickCallBack = function()
    	flagInfo.func(self)
	end
	self.countdown = self.countdown or self:AddUIComponent(UI.Countdown)
  	self.countdown:CountdownByLeftTime(8, function(leftTime)
  		if leftTime > 0 then
  			self.left_time_txt.text = string.format("%s秒后自动", math.floor(leftTime/1000))
  		else
			self:HandleBtnClick()
		end
	end, 0.2)
end

function TaskDialogView:ProcessTaskInfo(  )
	if not self.data then return end

	local taskNum = self.data.taskIDList and #self.data.taskIDList or 0
	if taskNum == 1 then
		local taskInfo = self.model:GetTaskInfo(self.data.taskIDList[1])
        self.curShowData = table.deep_copy(taskInfo)
        local taskCfg = ConfigMgr:GetTaskCfg(self.curShowData.taskID)
        local subTaskCfg = taskCfg.subTasks[self.curShowData.subTaskIndex]
        if not taskCfg or not subTaskCfg or not subTaskCfg.content or self.curShowData.subType ~= TaskConst.SubType.Talk then
        	self.curShowData = nil
        end
        self.curShowTalkNum = self.curShowTalkNum or 1
        local talkCfg = subTaskCfg.content[self.curShowTalkNum]
        if talkCfg then
	        self.curShowData.content = talkCfg.chat
	        self.curShowData.who = talkCfg.who
	        self:ProcessBtnNameAndCallBack(talkCfg.flag)
	    end
    elseif taskNum > 1 then
		--Cat_Todo : multi task in npc
    else
        --show default conversation
        self.curShowData = {}
        self.curShowData.content = "哈哈,你猜我是谁?"
        self.curShowData.who = self.data.npcID
        self:ProcessBtnNameAndCallBack(TaskConst.DialogBtnFlag.Ok)
    end
end

function TaskDialogView:OnUpdate(  )
	self:ProcessTaskInfo()
	if not self.curShowData then return end

	self:UpdateContent()
	self:UpdateLooks()
end

function TaskDialogView:UpdateContent(  )
	self.npc_name_txt.text = ConfigMgr:GetNPCName(self.curShowData.npcID)
	self.chat_txt.text = self.curShowData.content
	self.btn_label_txt.text = self.curShowData.btnName
end

function TaskDialogView:UpdateLooks( )
	local show_data
	if self.curShowData.who == 0 then
		local mainRoleLooksInfo = LRoleMgr:GetMainRoleLooksInfo()
		if not mainRoleLooksInfo then return end
		show_data = {
			showType = UILooksNode.ShowType.Role,
			showRawImg = self.npc_raw,
			body = mainRoleLooksInfo.body,
			hair = mainRoleLooksInfo.hair,
			career = mainRoleLooksInfo.career,
			canRotate = true,
		}
	else
		show_data = {
			showType = UILooksNode.ShowType.NPC,
			showRawImg = self.npc_raw,
			npcID = self.curShowData.who,
			canRotate = true,
		}
	end
	self.looksNode = self.looksNode or UILooksNode.New(self.npc)
	self.looksNode:SetData(show_data)
end

return TaskDialogView