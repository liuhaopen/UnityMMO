return [[
.role_figure {
	role_id 0 : integer
	career 1 : integer
	name 2 : string
	body 3 : integer
	hair 4 : integer
}

account_get_server_time 1 {
	response {
		server_time 0 : integer
	}
}

account_get_role_list 2 {
	response {
		role_list 0 : *role_figure
	}
}

account_create_role 3 {
	request {
		career 0 : integer
		name 1 : string
	}
	response {
		result 0 : integer
		role_id 1 : integer
	}
}

account_delete_role 4 {
	request {
		role_id 0 : integer
	}
	response {
		result 0 : integer
	}
}

account_select_role_enter_game 5 {
	request {
		role_id 0 : integer
	}
	response {
		result 0 : integer
	}
}

]]

