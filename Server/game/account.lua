local skynet = require "skynet"
require "util.util"
local account = {}

function account.account_get_role_list(user_info, req_data)
	print("Cat:account [start:5] user_info:", user_info)
	PrintTable(user_info)
	print("Cat:account [end]")
	--先不考虑性能吧,优先把前端搞定再优化
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_succeed, result = skynet.call(gameDBServer, "lua", "select_by_key", "Account", "account_id", user)
	if is_succeed then
		print("Cat:account [start:12] result:", result)
		PrintTable(result)
		print("Cat:account [end]")
		local account_info = result and result[1]
		if account_info then

		else
			--未创建角色
			return {}
		end
	end
	-- local result = {role_list = {
	-- 	{role_id = 1, sex = 1, career = 1,name = "role_name_1"},
	-- 	{role_id = 2, sex = 2, career = 2,name = "role_name_2"}
	-- }}
	local result = {}
	return 	result
end

function account.account_create_role( user_info, req_data )
	print('Cat:account.lua[63] req_data.name, req_data.career', req_data.name, req_data.career)
	local gameDBServer = skynet.localname(".GameDBServer")
	--根据平台,帐号等生成唯一的角色id
	-- skynet.call(gameDBServer, "lua", "insert", "Account", {account_id=user_info.user_id, password="123"})

	return {result=1, role_id=1}
end

function account.account_get_server_time( user_info, req_data )
	return {server_time = 1531569879}
end

function account.account_select_role_enter_game( user_info, req_data )
	--角色进入游戏场景
	return {result = 1}
end

return account