--提取简单的action管理器仅供参与，可以自己另外去定制更加丰富的版本，比如加上缓冲对象池功能什么的
local table_insert = table.insert
local table_remove = table.remove

local function Update( self, deltaTimeMS )
	self.isUpdating = true
	local i = 1
	while i <= #self.actions do
		local action = self.actions[i]
		if not action.__removed__ then
			action:Update(deltaTimeMS)
			if action:IsDone() then
				table_remove(self.actions, i)
				if self.doneCallBack then
					self.doneCallBack(action)
				end
			else
				i = i + 1
			end
		else
			i = i + 1
		end
	end
	self.isUpdating = false
	for i,v in ipairs(self.waitForDelete) do
		self:RemoveAction(v)
	end
end

local function RemoveAction( self, action )
	if self.isUpdating then
		action.__removed__ = true
		table_insert(self.waitForDelete, action)
		return
	end
	for i,v in ipairs(self.actions) do
		if v == action then
			table_remove(self.actions, i)
			if self.doneCallBack then
				self.doneCallBack(action)
			end
		end
	end
end

local ActionMgr = Ac.OO.Class {
	type 	= "ActionMgr",
	__index = {
		Init = function(self)
			self.actions = {}
			self.waitForDelete = {}
		end,
		SetDoneCallBack = function(self, doneCallBack)
			self.doneCallBack = doneCallBack
		end,
		AutoUpdate = function(self, action)
			table_insert(self.actions, action)
		end,
		Update = Update,
		RemoveAction = RemoveAction,
	},
}

return ActionMgr