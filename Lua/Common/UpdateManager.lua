--[[
-- added by wsh @ 2017-12-18
-- 更新管理，负责Unity侧Update、LateUpdate、FixedUpdate对Lua脚本的调用
-- 注意：
-- 1、别直接用tolua的UpdateBeat、LateUpdateBeat、FixedUpdateBeat，所有需要以上三个更新函数的脚本，都从这里注册。
-- 2、tolua的event没有使用weak表，直接使用tolua的更新系统会导致脚本被event持有引用而无法释放---除非每次都记得手动去释放
--]]

local Messenger = require "Common.Messenger"
local UpdateManager = BaseClass()
local UpdateMsgName = "Update"
local LateUpdateMsgName = "LateUpdateMsgName"
local FixedUpdateMsgName = "FixedUpdateMsgName"

UpdateManager.Instance = nil
function UpdateManager:GetInstance()
	if UpdateManager.Instance == nil then
		UpdateManager.Instance = UpdateManager.New()
	end
	return UpdateManager.Instance
end

-- 构造函数
local function Constructor(self)
	-- 成员变量
	-- 消息中心
	self.ui_message_center = Messenger.New()
	-- Update
	self.__update_handle = nil
	-- LateUpdate
	self.__lateupdate_handle = nil
	-- FixedUpdate
	self.__fixedupdate_handle = nil

end

-- Update回调
local function UpdateHandle(self)
	self.ui_message_center:Fire(UpdateMsgName)
end

-- LateUpdate回调
local function LateUpdateHandle(self)
	self.ui_message_center:Fire(LateUpdateMsgName)
end

-- FixedUpdate回调
local function FixedUpdateHandle(self)
	self.ui_message_center:Fire(FixedUpdateMsgName)
end

-- 启动
local function Startup(self)
	print("update manager start up")
	self:Dispose()
	self.__update_handle = UpdateBeat:CreateListener(UpdateHandle, UpdateManager:GetInstance())
	self.__lateupdate_handle = LateUpdateBeat:CreateListener(LateUpdateHandle, UpdateManager:GetInstance())
	self.__fixedupdate_handle = FixedUpdateBeat:CreateListener(FixedUpdateHandle, UpdateManager:GetInstance())
	UpdateBeat:AddListener(self.__update_handle)
	LateUpdateBeat:AddListener(self.__lateupdate_handle)
	FixedUpdateBeat:AddListener(self.__fixedupdate_handle)
end

-- 释放
local function Dispose(self)
	if self.__update_handle ~= nil then
		UpdateBeat:RemoveListener(self.__update_handle)
		self.__update_handle = nil
	end
	if self.__lateupdate_handle ~= nil then
		LateUpdateBeat:RemoveListener(self.__lateupdate_handle)
		self.__lateupdate_handle = nil
	end
	if self.__fixedupdate_handle ~= nil then
		FixedUpdateBeat:RemoveListener(self.__fixedupdate_handle)
		self.__fixedupdate_handle = nil
	end
end

-- 清理：消息系统不需要强行清理
local function Cleanup(self)
end

-- 添加Update更新
local function AddUpdate(self, e_listener)
	self.ui_message_center:AddListener(UpdateMsgName, e_listener)
end

-- 添加LateUpdate更新
local function AddLateUpdate(self, e_listener)
	self.ui_message_center:AddListener(LateUpdateMsgName, e_listener)
end

-- 添加FixedUpdate更新
local function AddFixedUpdate(self, e_listener)
	self.ui_message_center:AddListener(FixedUpdateMsgName, e_listener)
end

-- 移除Update更新
local function RemoveUpdate(self, e_listener)
	self.ui_message_center:RemoveListener(UpdateMsgName, e_listener)
end

-- 移除LateUpdate更新
local function RemoveLateUpdate(self, e_listener)
	self.ui_message_center:RemoveListener(LateUpdateMsgName, e_listener)
end

-- 移除FixedUpdate更新
local function RemoveFixedUpdate(self, e_listener)
	self.ui_message_center:RemoveListener(FixedUpdateMsgName, e_listener)
end

-- 析构函数
local function __delete(self)
	self:Cleanup()
	self.ui_message_center = nil
end

UpdateManager.Constructor = Constructor
UpdateManager.Startup = Startup
UpdateManager.Dispose = Dispose
UpdateManager.Cleanup = Cleanup
UpdateManager.AddUpdate = AddUpdate
UpdateManager.AddLateUpdate = AddLateUpdate
UpdateManager.AddFixedUpdate = AddFixedUpdate
UpdateManager.RemoveUpdate = RemoveUpdate
UpdateManager.RemoveLateUpdate = RemoveLateUpdate
UpdateManager.RemoveFixedUpdate = RemoveFixedUpdate
UpdateManager.__delete = __delete
return UpdateManager;