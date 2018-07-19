local account = {}

function account.account_get_role_list(req_data)
	print('Cat:account.lua[57]account_get_role_list')
	-- local result = {role_list = {
	-- 	{role_id = 1, sex = 1, career = 1,},
	-- 	{role_id = 2, sex = 2, career = 2,}
	-- }}
	local result = {}
	return 	result
end

function account.account_create_role( req_data )
	print('Cat:account.lua[63] req_data.name, req_data.career', req_data.name, req_data.career)
	return {result=1, role_id=1}
end

function account.account_get_server_time( req_data )
	return {server_time = 1531569879}
end

return account