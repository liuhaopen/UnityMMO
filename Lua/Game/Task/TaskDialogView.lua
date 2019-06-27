local TaskDialogView = BaseClass(UINode)

function TaskDialogView:Constructor(  )
	self.prefabPath = "Assets/AssetBundleRes/ui/task/TaskDialogView.prefab"
	self:Load()
end

function TaskDialogView:OnLoad(  )
	local names = {
		"skip_con/skip_btn:obj","bottom/npc:raw","bottom/btn:obj","bottom/npc_name:txt","bottom/chat:txt","click_bg:obj","btn/btn_label:txt"
	}
	UI.GetChildren(self, self.transform, names)
	self.transform.sizeDelta = Vector2.zero

	self:AddEvents()
end

-- function TaskDialogView:SetData( data )
-- 	UINode.SetData(self, data)
-- end

function TaskDialogView:AddEvents(  )
	local on_click = function ( click_obj )
		if self.btn_obj == click_obj then
			if self.curShowData.clickCallBack then
				self.curShowData.clickCallBack()
			else
				self:Destroy()
			end
		elseif self.skip_btn_obj == click_obj then
			self:Destroy()
		elseif self.click_bg_obj == click_obj then
			self:Destroy()
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

function TaskDialogView:ProcessTaskInfo(  )
	if not self.data then return end

	local taskNum = self.data.taskList and #self.data.taskList or 0
	if taskNum == 1 then
        self.curShowData = table.deep_copy(self.data.taskList[1])
        local taskCfg = ConfigMgr:GetTaskCfg(self.curShowData.taskID)
        local subTaskCfg = taskCfg.subTasks[self.curShowData.subTaskIndex]
        if not taskCfg or not subTaskCfg or not subTaskCfg.content or self.curShowData.subTypeID ~= TaskConst.SubType.Talk then
        	self.curShowData = nil
        end
        self.curShowTalkNum = self.curShowTalkNum or 1
        self.curShowData.content = subTaskCfg.content[self.curShowTalkNum].chat
        self.curShowData.who = subTaskCfg.content[self.curShowTalkNum].who
        self.curShowData.clickCallBack = function()
        	self:ShowNextTalk()
    	end
    elseif taskNum > 1 then
		--Cat_Todo : multi task in npc
    else
        --show default conversation
        local view = require("Game/Task/TaskDialogView").New()
        local data = {
            {
                npcID = npcID,
                content = "哈哈，你猜我是谁？",
                btnName = "继续",
            },
            {
            },
        }
        view:SetData(data)
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
end

function TaskDialogView:UpdateLooks( )
	local show_data = {
		showType = UILooksNode.ShowType.NPC,
		showRawImg = self.npc_raw,
		npcID = self.curShowData.who,
		canRotate = true,
	}
	self.looksNode = self.looksNode or UILooksNode.New(self.npc)
	self.looksNode:SetData(show_data)
end

return TaskDialogView