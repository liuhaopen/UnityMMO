local TaskModel = BaseClass(EventDispatcher)

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

function TaskModel:GetTaskInfo( taskID )
	if self.taskInfo and self.taskInfo.taskList then
		for i,v in ipairs(self.taskInfo.taskList) do
			if v.taskID == taskID then
				return v
			end
		end
	end
	return nil
end

--completing some data that server side were not sent but the client needed to use
function TaskModel:CompleteTaskInfo( taskInfo )
	if not taskInfo or not taskInfo.subType then return end
	
	if not self.completeFuncs then
		self.completeFuncs = {
			[TaskConst.SubType.Talk] = TaskModel.CompleteTalk,
			[TaskConst.SubType.KillMonster] = TaskModel.CompleteKillMonster,
			[TaskConst.SubType.Collect] = TaskModel.CompleteCollect,
		}
	end
	local func = self.completeFuncs[taskInfo.subType]
	print('Cat:TaskModel.lua[53] func, taskInfo.subType', func, taskInfo.subType)
	if func then
		local cfg = ConfigMgr:GetTaskCfg(taskInfo.taskID)
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

function TaskModel:CompleteTalk( taskInfo, cfg, subCfg )
	taskInfo.npcID = subCfg.contentID
	taskInfo.sceneID = subCfg.sceneID
	local npcName = ConfigMgr:GetNPCName(taskInfo.npcID)
	taskInfo.desc = string.format(TaskConst.Desc[TaskConst.SubType.Talk], npcName)
end

function TaskModel:CompleteKillMonster( taskInfo, cfg, subCfg )
	taskInfo.monsterID = subCfg.contentID
	taskInfo.sceneID = subCfg.sceneID
	local monsterName = ConfigMgr:GetMonsterName(taskInfo.monsterID)
	taskInfo.desc = string.format(TaskConst.Desc[TaskConst.SubType.KillMonster], monsterName)
	return true
end

function TaskModel:CompleteCollect( taskInfo, cfg, subCfg )
	taskInfo.collectID = subCfg.contentID
	local monsterName = ConfigMgr:GetMonsterName(taskInfo.collectID)
	taskInfo.desc = string.format(TaskConst.Desc[TaskConst.SubType.Collect], monsterName)
	return subCfg.maxProgress > 1
end

function TaskModel:UpdateTaskInfo( taskInfo )
	self:CompleteTaskInfo(taskInfo)
	local isIn = false
	if self.taskInfo and self.taskInfo.taskList then
		for i,v in ipairs(self.taskInfo.taskList) do
			if v.taskID == taskInfo.taskID then
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

function TaskModel:GetTaskType( taskID )
	local cfg = ConfigMgr:GetTaskCfg(taskID)
	return cfg and cfg.taskType or TaskConst.Type.Unknow
end

function TaskModel:GetTaskInfoByType( taskType )
	if self.taskInfo and self.taskInfo.taskList then
		for i,v in ipairs(self.taskInfo.taskList) do
			if self:GetTaskType(v.taskID) == taskType then
				return v
			end
		end
	end
	return nil
end

function TaskModel:IsTaskFinished( taskID )
	return true
end

return TaskModel