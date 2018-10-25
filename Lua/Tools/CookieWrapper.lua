
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
	-- LastLoginRoleID					= "CookieKey.LastLoginRoleID",			--上次登录的角色
	-- LastLoginAccount				= "CookieKey.LastLoginAccount",			--上次登录的用户名
	-- LastLoginPassword				= "CookieKey.LastLoginPassword",		--上次登录的密码
}

function CookieWrapper:Init()
	if CookieWrapper.Instance ~= nil then
		return
	end
	CookieWrapper.Instance = self

	self.cookie_root_path = cookiesManager:GetCookiePath()
	self.account_info_path = self.cookie_root_path .. "account_info/"
	self.chat_histroy_path = self.cookie_root_path .. "chat_histroy/"

	self:InitCommonValue()

	self.chat_histroy_id = nil

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

	self.chat_histroy_id = nil

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

	if key == CookieKey.SCENE_X_Y then
		self.role_pos_is_dirty = true
	else
		self.is_dirty = true
	end

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

	--添加保存聊天记录
	self:WriteChatHistroy()
	if self.chat_timer_id ~= 0 then
		GlobalTimerQuest:CancelQuest(self.chat_timer_id)
		self.chat_timer_id = 0
	end

end

function CookieWrapper:initChatValue(playerid)
	--这里层次结构为[str][自身id][密聊玩家id]
	--以cookiekey为第一层，没使用cookietype
	if not self.account_name or not playerid then return end
	playerid = tostring(playerid)
	local path = self.chat_histroy_path..self.account_name.."/"
	cookiesManager:LoadCookie(tostring(playerid), path)
	if not Cookies[CookieKey.CHAT_HISTROY] then
		Cookies[CookieKey.CHAT_HISTROY] = {}
	end
	if not Cookies[CookieKey.CHAT_HISTROY][self.account_name] then
		Cookies[CookieKey.CHAT_HISTROY][self.account_name] = {}
	end
	if not Cookies[CookieKey.CHAT_HISTROY][self.account_name][playerid] then
		Cookies[CookieKey.CHAT_HISTROY][self.account_name][playerid] = {}
	end
end

function CookieWrapper:WriteChatHistroy()
	--保存场景为，每一分钟/关闭chatview/密聊返回btn/switchbar
	-- if not self.chat_histroy_id or not self.account_name then return end
	-- if IsTableEmpty(Cookies[CookieKey.CHAT_HISTROY][self.account_name][self.chat_histroy_id]) then return end
	-- local table_name = CookieKey.CHAT_HISTROY.."."..self.account_name.."."..self.chat_histroy_id
	-- local path = self.chat_histroy_path..self.account_name.."/"
	-- cookiesManager:WriteCookie(tostring(self.chat_histroy_id), table_name, path)
	if Cookies[CookieKey.CHAT_HISTROY] and Cookies[CookieKey.CHAT_HISTROY][self.account_name] then
		for k,v in pairs(Cookies[CookieKey.CHAT_HISTROY][self.account_name]) do
			local table_name = CookieKey.CHAT_HISTROY.."."..self.account_name.."."..k
			local path = self.chat_histroy_path..self.account_name.."/"
			cookiesManager:WriteCookie(k, table_name, path)
		end
	end
end

function CookieWrapper:SaveChatHistroy(playerid,cache_type,value)
	--这里不做增删操作，value就是整个table --value需要经过简单处理
	--修改：value传的只是增操作，退出窗口私聊内容的列表就清空了...
	if not playerid or not cache_type or not value then return end
	playerid = tostring(playerid)
	self:GetChatHistroy(playerid)
	local item = value
	Cookies[CookieKey.CHAT_HISTROY][self.account_name][playerid] = item
end

function CookieWrapper:GetChatHistroy(playerid,all)
	if not playerid then return end
	playerid = tostring(playerid)

	--全部玩家手动删除一次聊天cookie  已经解决cookie数据错误的问题了， 这段代码去掉
	local tb = CookieWrapper.Instance:GetCookie(CookieLevelType.Account,CookieKey.DEL_CHAT_COOKIE) or {}
	if not tb[playerid] then
		local file_path = Util.DataPath..CookieWrapper.Instance.chat_histroy_path..CookieWrapper.Instance.account_name.."/"..playerid..".cfg"
		local result,log = os.remove(file_path)
		tb[playerid] = true
		CookieWrapper.Instance:SaveCookie(CookieLevelType.Account,CookieTimeType.TYPE_ALWAYS, CookieKey.DEL_CHAT_COOKIE, tb)
	end

	-- if self.chat_histroy_id ~= playerid then
	-- 	self:initChatValue(playerid)
	-- 	self.chat_histroy_id = playerid
	-- end
	if not Cookies[CookieKey.CHAT_HISTROY] or not Cookies[CookieKey.CHAT_HISTROY][self.account_name] or not Cookies[CookieKey.CHAT_HISTROY][self.account_name][playerid] then
		self:initChatValue(playerid)
	end
	if all then
		return Cookies[CookieKey.CHAT_HISTROY][self.account_name]
	else
		return Cookies[CookieKey.CHAT_HISTROY][self.account_name][playerid]
	end
end

function CookieWrapper:DeleteChatHistroyFile(playerid)
	if not playerid then return end
	local file_path = Util.DataPath..self.chat_histroy_path..self.account_name.."/"..playerid..".cfg"
	local result,log = os.remove(file_path)
	CookieWrapper.Instance:SaveChatHistroy(playerid,CookieTimeType.TYPE_ALWAYS,{})
	self:WriteAll()
end

return CookieWrapper