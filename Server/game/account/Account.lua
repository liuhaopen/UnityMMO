local skynet = require "skynet"
require "common.util"
local queue = require "skynet.queue"
local cs = queue()

local id_service
local gameDBServer
local account = {}

function account.account_get_role_list(user_info, req_data)
	--先不考虑性能吧,优先把前端搞定再优化
	gameDBServer = gameDBServer or skynet.localname(".GameDBServer")
	local is_succeed, role_list = skynet.call(gameDBServer, "lua", "select_by_key", "RoleList", "account_id", user_info.user_id)
	if is_succeed and role_list and #role_list > 0 then
		local sort_func = function ( a, b )
			return a.create_time < b.create_time
		end
		table.sort(role_list, sort_func)
		local ack_data = {role_list={}}
		for i,v in ipairs(role_list) do
			local role_id = v.role_id
			if role_id then
				local is_ok, role_info = skynet.call(gameDBServer, "lua", "select_one_by_key", "RoleBaseInfo", "role_id", role_id)
				if is_ok and role_info then
					local is_ok, looks_info = skynet.call(gameDBServer, "lua", "select_one_by_key", "RoleLooksInfo", "role_id", role_id)
					if is_ok then
						for k,v in pairs(looks_info) do
							role_info[k] = v
						end
					end
					table.insert(ack_data.role_list, role_info)
				end
			else
				skynet.error("account_get_role_list error : role id nil!")
			end
		end
		return ack_data
	end
	return {}
end

local gen_random_role_looks = function ( role_id )
	return {role_id=role_id, body=math.random(0,1), hair=math.random(0,1)}
end

local gen_role_base_attr = function ( role_id )
	return {role_id=role_id, att=100, hp=10000, def=100, hit=100, dodge=120, crit=100}
end

function account.account_create_role( user_info, req_data )
	local create_role = function (  )
		gameDBServer = gameDBServer or skynet.localname(".GameDBServer")
		local is_succeed, role_list = skynet.call(gameDBServer, "lua", "select_by_key", "RoleList", "account_id", user_info.user_id)
		local is_full_role = role_list and #role_list >= 3
		if is_full_role then
			return {result = ErrorCode.FullCreateRoleNum}
		end
		id_service = id_service or skynet.localname(".id_service")
		local new_role_id = skynet.call(id_service, "lua", "gen_uid", "role")
		assert(new_role_id, "gen role id failed!")
		local is_create_succeed = false
		if new_role_id then
			is_succeed = skynet.call(gameDBServer, "lua", "insert", "RoleList", {account_id=user_info.user_id, role_id=new_role_id, create_time=math.floor(skynet.time()*1000+0.5)})
			if is_succeed then
				local attr_info = gen_role_base_attr(new_role_id)
				is_succeed = skynet.call(gameDBServer, "lua", "insert", "RoleBaseInfo", {role_id=new_role_id, name=req_data.name, career=req_data.career, level=1, hp=attr_info.hp})
				is_succeed = is_succeed and skynet.call(gameDBServer, "lua", "insert", "RoleLooksInfo", gen_random_role_looks(new_role_id))
				is_succeed = is_succeed and skynet.call(gameDBServer, "lua", "insert", "AttrInfo", attr_info)
			end
			if is_succeed then
				is_create_succeed = true
			end
		end
		return {result=is_create_succeed and ErrorCode.Succeed or ErrorCode.Unknow, role_id=new_role_id}
	end
	--防止同时进入
	return cs(create_role)
end

function account.account_get_server_time( user_info, req_data )
	local cur_time = skynet.time()
	return {server_time = math.floor(cur_time*1000+0.5)}
end

function account.account_select_role_enter_game( user_info, req_data )
	if req_data and req_data.role_id and req_data.role_id ~= 0 then
		user_info.cur_role_id = req_data.role_id
		--角色进入游戏场景
		local world = skynet.uniqueservice ("world")
		local result_code = skynet.call(world, "lua", "role_enter_game", user_info, req_data.role_id)
		return {result = result_code}
	else
		skynet.error("wrong req_data for proto account_select_role_enter_game")
		return {result = ErrorCode.WrongRoleIDForEnterGame}
	end
end

return account