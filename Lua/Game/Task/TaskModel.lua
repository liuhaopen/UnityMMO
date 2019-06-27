TaskModel = BaseClass(EventDispatcher)

function TaskModel:Constructor(  )
	self:Reset()
end

function TaskModel:GetInstance(  )
	if not TaskModel.Instance then
		TaskModel.Instance = TaskModel.New()
	end
	return TaskModel.Instance
end

function TaskModel:Reset(  )
	self.taskInfo = nil
end

function TaskModel:GetTaskInfos( )
	return self.taskInfo
end

function TaskModel:SetTaskInfos( value )
	self.taskInfo = value
	if self.taskInfo and self.taskInfo.taskList then
		for i,v in ipairs(self.taskInfo.taskList) do
			self:CompleteTaskInfo(v)
		end
	end
end

--completing some data that server side were not sent but the client needed to use
function TaskModel:CompleteTaskInfo( taskInfo )
	if not taskInfo or not taskInfo.subTypeID then return end
	
	if not self.completeFuncs then
		self.completeFuncs = {
			[TaskConst.SubType.Talk] = TaskModel.CompleteTaskInfoNPC,
			[TaskConst.SubType.KillMonster] = TaskModel.CompleteTaskInfoKillMonster,
		}
	end
	local func = self.completeFuncs[taskInfo.subTypeID]
	if func then
		local cfg = ConfigMgr:GetTaskCfg(taskInfo.typeID)
		local subCfg = cfg and cfg.subTasks and cfg.subTasks[taskInfo.subTaskIndex]
		if cfg and subCfg then 
			local isNeedShowNum = func(self, taskInfo, cfg, subCfg)
			if isNeedShowNum then
				local numStr = string.format("(%s/%s)", taskInfo.curProgress, taskInfo.maxProgress)
				taskInfo.desc = taskInfo.desc..numStr
			end
		else
			print('Cat:TaskModel.lua cannot find cfg for : '..taskInfo.taskID.." subTaskIndex:"..taskInfo.subTaskIndex)
		end
	else
		print("Cat:TaskModel.lua has not find complete func for : "..taskInfo.taskID.." subTaskIndex:"..taskInfo.subTaskIndex)
	end
end

function TaskModel:CompleteTaskInfoNPC( taskInfo, cfg, subCfg )
	taskInfo.npcID = subCfg.contentID
	local npcName = ConfigMgr:GetNPCName(taskInfo.npcID)
	taskInfo.desc = string.format(TaskConst.Desc[TaskConst.SubType.Talk], npcName)
end

function TaskModel:CompleteTaskInfoKillMonster( taskInfo )
	taskInfo.monsterID = subCfg.contentID
	local monsterName = ConfigMgr:GetMonsterName(taskInfo.monsterID)
	taskInfo.desc = string.format(TaskConst.Desc[TaskConst.SubType.KillMonster], monsterName)
	return true
end

function TaskModel:UpdateTaskInfo( taskInfo )
	self:CompleteTaskInfo(taskInfo)
	local isIn = false
	if self.taskInfo and self.taskInfo.taskList then
		for i,v in ipairs(self.taskInfo.taskList) do
			if v.typeID == taskInfo.typeID then
				self.taskInfo.taskList[i] = taskInfo
				isIn = true
				break
			end
		end
	end
	if not isIn then
		self.taskInfo = self.taskInfo or {}
		self.taskInfo.taskList = self.taskInfo.taskList or {}
		table.insert(self.taskInfo.taskList, taskInfo)
	end
end

return TaskModel