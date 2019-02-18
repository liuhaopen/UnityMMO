return [[

.scene_main_role_info {
	scene_uid 0 : integer
	role_id 1 : integer
	career 2 : integer
	name 3 : string
	scene_id 4 : integer
	pos_x 5 : integer
	pos_y 6 : integer
	pos_z 7 : integer
}
.info_item {
	key 0 : integer
	value 1 : string
	time 2 : integer
}
.scene_obj_info {
	scene_obj_uid 0 : integer
	info_list 1 : *info_item
}
.scene_role_info {
	role_id 0 : integer
	career 1 : integer
	name 2 : string
}
.scene_role_looks_info {
	career 0 : integer
	body 1 : integer
	hair 2 : integer
	weapon 3 : integer
	wing 4 : integer
	horse 5 : integer
}
.scene_monster_info {
	monster_id 0 : integer
	monster_type_id 1 : integer
	hp 2 : integer
	maxhp 3 : integer
}
.scene_npc_info {
	npc_id 0 : integer
	npc_type_id 1 : integer
}

scene_get_main_role_info 100 {
	response {
		role_info 0 : scene_main_role_info
	}
}

scene_walk 101 {
	request {
		pos_x 0 : integer
		pos_y 1 : integer
		pos_z 2 : integer
		time  3 : integer
	}
	response {
	}
}

scene_get_objs_info_change 102 {
	request {
	}
	response {
		obj_infos 0 : *scene_obj_info
	}
}

scene_change_aoi_radius 103 {
	request {
		radius 0 : integer
	}
}

scene_get_role_look_info 104 {
	request {
		uid 0 : integer
	}
	response {
		result 0 : integer
		role_looks_info 1 : scene_role_looks_info
	}
}

scene_get_monster_detail 105 {
	request {
		uid 0 : integer
	}
	response {
		monster_info 0 : scene_monster_info
	}
}

]]

