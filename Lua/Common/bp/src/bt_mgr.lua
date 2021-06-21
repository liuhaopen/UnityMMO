--提取简单的action管理器仅供参与，可以自己另外去定制更加丰富的版本，比如加上缓冲对象池功能什么的
local oo = require("bp.common.lua_oo")
local table_insert = table.insert
local table_remove = table.remove

local function update( self, deltaTimeMS )
	self.isUpdating = true
	local i = 1
	while i <= #self.actions do
		local action = self.actions[i]
		if not action.__removed__ then
			action:update(deltaTimeMS)
			if action:is_done() then
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
	for _,v in ipairs(self.waitForDelete) do
		self:remove(v)
	end
end

local function remove( self, action )
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

local bt_mgr = oo.class {
	type 	= "bt_mgr",
	__index = {
		init = function(self)
			self.actions = {}
			self.waitForDelete = {}
		end,
		set_done_cb = function(self, doneCallBack)
			self.doneCallBack = doneCallBack
		end,
		auto_update = function(self, action)
			table_insert(self.actions, action)
		end,
		update = update,
		remove = remove,
	},
}

return bt_mgr