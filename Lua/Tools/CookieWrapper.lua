
CookieWrapper = {}
local CookieWrapper = CookieWrapper
CookieVersion = 1
CookieAccountInfo = "AccountInfo"

-- 层次类型
CookieLevelType = {
	Common = 1,
	Account = 2,
	Server = 3,
}

-- 时间类型
CookieTimeType = {
	TYPE_DAY = 1,
	TYPE_ALWAYS = 2,
}

CookieKey = {
	LastLoginInfo					= "CookieKey.LastLoginInfo",			--上次登录的信息
	LastSelectRoleID				= "CookieKey.LastSelectRoleID",			--上次登录的角色
}

function CookieWrapper:Init()
	if CookieWrapper.Instance ~= nil then
		return
	end
	CookieWrapper.Instance = self

	self.cookie_root_path = cookiesManager:GetCookiePath()
	self.account_info_path = self.cookie_root_path .. "account_info/"

	self:InitCommonValue()

	self.account_info_init = false
	self.account_name = nil

	self.is_dirty = false
	self.role_pos_is_dirty = false

end

function CookieWrapper:GetInstance()
	if CookieWrapper.Instance==nil then
		CookieWrapper:Init()
	end
	return CookieWrapper.Instance
end

function CookieWrapper:InitCommonValue()
	cookiesManager:LoadCookie("LoginInfo", nil)

	cookiesManager:LoadCookie("Server", nil)
	if not Cookies["Server"] then
		Cookies["Server"] = {}
	end

	cookiesManager:LoadCookie("Common", nil)
	if not Cookies["Common"] then
		Cookies["Common"] = {}
	end

	local now_version = Cookies["Common"]["__Version"]
	if not now_version or now_version ~= CookieVersion then
		Cookies["Common"] = {}
		Cookies["Common"]["__Version"] = CookieVersion
	end
	self:ClearOverdueItem(Cookies["Common"])

end

function CookieWrapper:InitAccountValue(account_name)
	if not account_name then
		return
	end

	self.account_name = tostring(account_name)
	cookiesManager:LoadCookie(self.account_name, self.account_info_path)

	if not Cookies[CookieAccountInfo] then
		Cookies[CookieAccountInfo] = {}
	end

	if not Cookies[CookieAccountInfo][self.account_name] then
		Cookies[CookieAccountInfo][self.account_name] = {}
	end

	local now_version = Cookies[CookieAccountInfo][self.account_name]["__Version"]
	if not now_version or now_version ~= CookieVersion then
		Cookies[CookieAccountInfo][self.account_name] = {}
		Cookies[CookieAccountInfo][self.account_name]["__Version"] = CookieVersion
	end

	if not Cookies[CookieAccountInfo][self.account_name]["Common"] then
		Cookies[CookieAccountInfo][self.account_name]["Common"] = {}
	end

	self:ClearOverdueItem(Cookies[CookieAccountInfo][self.account_name]["Common"])

	self.account_info_init = true

	GlobalEventSystem:Fire(EventName.COOKIE_INIT_FINISH)
end

function CookieWrapper:SaveCookie(level_type, cache_type, key, value)
	if not level_type or not cache_type or not key or value == nil then
		return
	end

	self.is_dirty = true

	local item = {}
	item.value = value
	item.time_type = cache_type
	item.server_time = Time:GetServerTime()

	if level_type == CookieLevelType.Common then
		Cookies["Common"][key] = item
		cookiesManager:WriteCookie("Common", "Common", "")--马上写入保存(画质设置没有立即保存所以加一下)
	elseif level_type == CookieLevelType.Server then
		Cookies["Server"][key] = item
	elseif level_type == CookieLevelType.Account then
		if self.account_info_init then
			Cookies[CookieAccountInfo][self.account_name]["Common"][key] = item
		end
	end
end

function CookieWrapper:ClearCookie( level_type, key)

	if level_type == CookieLevelType.Common then
		Cookies["Common"][key] = nil
	elseif level_type == CookieLevelType.Server then
		Cookies["Server"][key] = nil
	elseif level_type == CookieLevelType.Account then
		if self.account_info_init then
			Cookies[CookieAccountInfo][self.account_name]["Common"][key] = nil
		end
	end
end

function CookieWrapper:GetCookie(level_type, key)
	if not level_type or not key then
		return
	end

	local handle_item = nil
	if level_type == CookieLevelType.Common then
		handle_item = Cookies["Common"][key]
	elseif level_type == CookieLevelType.Server then
		handle_item = Cookies["Server"][key]
	elseif level_type == CookieLevelType.Account then
		if self.account_info_init then
			handle_item = Cookies[CookieAccountInfo][self.account_name]["Common"][key]
		end
	end
	if handle_item then
		return handle_item.value
	end
end

function CookieWrapper:ClearOverdueItem(handle_table)
	local del_arr = {}

	local server_time = Time:GetServerTime()
	for key, item in pairs(handle_table) do
		if type(item) == "table" and item.time_type ~= CookieTimeType.TYPE_ALWAYS and
				server_time - item.server_time > 3600 * 24 then
			table.insert(del_arr, key)
		end
	end

	for _, key in pairs(del_arr) do
		handle_table[key] = nil
	end
end

 function CookieWrapper:WriteAccount()
	if self.account_name ~= nil then
		cookiesManager:WriteCookie(self.account_name, CookieAccountInfo .. "." .. self.account_name, self.account_info_path)
	end
end

function CookieWrapper:WriteAll()
	local save_pos_state = false
	if self.role_pos_is_dirty then
		self.role_pos_is_dirty = false
		save_pos_state = true
		self:WriteAccount()
	end

	if not self.is_dirty then
		return
	end
	self.is_dirty = false

	cookiesManager:WriteCookie("LoginInfo", "LoginInfo", "")
	cookiesManager:WriteCookie("Common", "Common", "")
	cookiesManager:WriteCookie("Server", "Server", "")

	if not save_pos_state then
		self:WriteAccount()
	end
end

function CookieWrapper:OnDestroy()

	self:WriteAll()

	if self.timer_id ~= 0 then
		GlobalTimerQuest:CancelQuest(self.timer_id)
		self.timer_id = 0
	end
end

return CookieWrapper