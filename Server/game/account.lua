local skynet = require "skynet"
require "Common.Util.util"
local queue = require "skynet.queue"
local cs = queue()

local account = {}

function account.account_get_role_list(user_info, req_data)
	--先不考虑性能吧,优先把前端搞定再优化
	local gameDBServer = skynet.localname(".GameDBServer")
	local is_succeed, result = skynet.call(gameDBServer, "lua", "select_by_key", "Account", "account_id", user_info.user_id)
	if is_succeed then
		local account_info = result and result[1]
		if account_info then
			user_info.role_list = {account_info.role_id_1, account_info.role_id_2, account_info.role_id_3}
			local ack_data = {role_list={}}
			for i=1,3 do
				local role_id = account_info["role_id_"..i]
				if role_id then
					local is_ok, role_info = skynet.call(gameDBServer, "lua", "select_by_key", "RoleBaseInfo", "role_id", role_id)
					if is_ok and role_info and role_info[1] then
						table.insert(ack_data.role_list, role_info[1])
					end
				end
			end
			return ack_data
		else
			--未创建角色
			return {}
		end
	end
	local result = {}
	return 	result
end

function account.account_create_role( user_info, req_data )
	local create_role = function (  )
		local gameDBServer = skynet.localname(".GameDBServer")
		--根据平台,服id,等生成唯一的角色id
		local new_role_id
		--各服间不共用数据库的话下面这样就够用了,如果想提高并发效率的话可以改成snowflake算法生成,这样就不需要锁数据库了
		local is_succeed, result = skynet.call(gameDBServer, "lua", "query", "select max(role_id) from RoleBaseInfo")
		if is_succeed then
			local max_id = result and result[1]
			max_id = max_id and max_id["max(role_id)"]
			if max_id then
				new_role_id = max_id + 1
			else
				--角色id为50位,前10位为平台id,11~20位为服id,其它位递增
				local role_bit = 30
				local server_bit = 10
				local platform_bit = 10
				local left_shift_for_server = role_bit
				local left_shift_for_platform = role_bit+server_bit
				new_role_id = (user_info.platform<<left_shift_for_platform)|(user_info.server_id<<left_shift_for_server)
			end
			print('Cat:account.lua[gen new_role_id] :', new_role_id)
		end
		assert(new_role_id, "gen role id failed!")
		local is_create_succeed = false
		if new_role_id then
			local already_has_role = user_info.role_list and #user_info.role_list or 0
			local is_ok, find_account = skynet.call(gameDBServer, "lua", "select_by_key", "Account", "account_id", user_info.user_id)
			local has_account_record = is_ok and find_account and find_account[1]
			local is_succeed = false
			if has_account_record then
				is_succeed = skynet.call(gameDBServer, "lua", "update", "Account", "account_id", user_info.user_id, {["role_id_"..(already_has_role+1)]=new_role_id})
			else
				is_succeed = skynet.call(gameDBServer, "lua", "insert", "Account", {account_id=user_info.user_id, role_id_1=new_role_id})
			end
			if is_succeed then
				is_succeed = skynet.call(gameDBServer, "lua", "insert", "RoleBaseInfo", {role_id=new_role_id, name=req_data.name, career=req_data.career, level=0})
			end
			if is_succeed then
				is_create_succeed = true
			end
		end
		return {result=is_create_succeed and 1 or 0, role_id=new_role_id}
	end
	--防止同时进入
	return cs(create_role)
end

function account.account_get_server_time( user_info, req_data )
	local cur_time = skynet.time()
	return {server_time = math.floor(cur_time*1000+0.5)}
end

function account.account_select_role_enter_game( user_info, req_data )
	if req_data and req_data.role_id then
		user_info.cur_role_id = req_data.role_id
		--角色进入游戏场景
		local world = skynet.uniqueservice ("world")
		local result_code = skynet.call(world, "lua", "role_enter_game", user_info, req_data.role_id)
		return {result = result_code}
	else
		skynet.error("wrong req_data for proto account_select_role_enter_game")
		return {result = 2}
	end
end

return account