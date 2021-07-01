return [[

.actor_ghost_info {
	scene_uid 0 : integer
	uid 1 : integer
	name 3 : string
}

.actor_looks_item {
	looks_id 0 : integer
	looks_value 1 : integer
}

.actor_full_static_info {
	career 0 : integer
	name 6 : string
	looks 0 : *actor_looks_item
}

.actor_scene_info {
	hp 7 : integer
	mp 7 : integer
	max_hp 8 : integer
    max_mp 10: integer
}

get_role_look_info 114 {
	request {
		uid 0 : *integer
	}
	response {
		result 0 : integer
		role_looks_info 1 : actor_scene_info
	}
}

on_actor_enter 102 {
	request {
	}
	response {
		actors 0 : *actor_ghost_info
	}
}
]]