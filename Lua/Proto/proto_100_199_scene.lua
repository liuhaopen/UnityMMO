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
	base_info 8 : scene_role_base_info
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
.scene_role_base_info {
	level 0 : integer
	career 1 : integer
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

.scene_fight_defender_info {
	uid 0 : integer
	hp 1 : integer
	hurt 2 : integer
}
.scene_fight_event_info {
	attacker_uid 0 : integer
	skill_id 1 : integer
	skill_lv 2 : integer
	attacker_pos_x 3 : integer
	attacker_pos_y 4 : integer
	attacker_pos_z 5 : integer
	target_pos_x 6 : integer
	target_pos_y 7 : integer
	target_pos_z 8 : integer
	time 9 : integer
	defenders 10 : *scene_fight_defender_info
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
		start_x 0 : integer
		start_y 1 : integer
		start_z 2 : integer
		end_x 3 : integer
		end_y 4 : integer
		end_z 5 : integer
		time  6 : integer
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

scene_use_skill 105 {
	request {
		skill_id 0 : integer
		target_pos_x 1 : integer
		target_pos_y 2 : integer
		target_pos_z 3 : integer
	}
	response {
		result 0 : integer
	}
}

scene_listen_fight_event 106 {
	request {
	}
	response {
		fight_events 0 : *scene_fight_event_info
	}
}

scene_get_monster_detail 107 {
	request {
		uid 0 : integer
	}
	response {
		monster_info 0 : scene_monster_info
	}
}

]]

