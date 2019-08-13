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
	cur_hp 8 : integer
	max_hp 9 : integer
	base_info 10 : scene_role_base_info
	coin 11 : integer
	diamond 12 : integer
}

#key 对应前端SceneInfoKey.cs里的SceneInfoKey或后端SceneConst.lua里的SceneConst.InfoKey
#1:EnterView即有场景节点（角色、怪物或NPC）进入视角，value为scene_uid,type_id,pos_x,pos_y,pos_z
#2:LeaveView场景节点离开视角
#3:PosChange场景节点的坐标变更
#4:TargetPos场景节点的目标坐标变更
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
	name 6 : string
	hp 7 : integer
	max_hp 8 : integer
}
.scene_monster_info {
	monster_id 0 : integer
	monster_type_id 1 : integer
	hp 2 : integer
	maxhp 3 : integer
}

#flag：1时技能被打断
.scene_skill_event_info {
	attacker_uid 0 : integer
	skill_id 1 : integer
	skill_lv 2 : integer
	attacker_pos_x 3 : integer
	attacker_pos_y 4 : integer
	attacker_pos_z 5 : integer
	target_pos_x 6 : integer
	target_pos_y 7 : integer
	target_pos_z 8 : integer
	direction 9 : integer
	time 10 : integer
	flag 11 : integer
}

#flag: 0普通扣血 1暴击 2Miss 3穿刺 4死亡
.scene_hurt_defender_info {
	uid 0 : integer
	change_num 1 : integer
	cur_hp 2 : integer
	flag 3 : integer 
}

.scene_hurt_event_info {
	attacker_uid 0 : integer
	time 1 : integer
	defenders 2 : *scene_hurt_defender_info
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

#走路协议
scene_walk 101 {
	request {
		start_x 0 : integer
		start_y 1 : integer
		start_z 2 : integer
		end_x 3 : integer
		end_z 4 : integer
		time  5 : integer
		jump_state 6 : integer
	}
	response {
	}
}

#通用状态变更协议，具体内容见上面的scene_obj_info.info_item结构体注释
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

scene_cast_skill 105 {
	request {
		skill_id 0 : integer
		cur_pos_x 1 : integer
		cur_pos_y 2 : integer
		cur_pos_z 3 : integer
		target_pos_x 4 : integer
		target_pos_y 5 : integer
		target_pos_z 6 : integer
		direction 7 : integer
	}
	response {
		result 0 : integer
		skill_id 1 : integer
		cd_end_time 2 : integer
	}
}

#一有别的玩家或怪物出招，自己就会收到此事件
scene_listen_skill_event 106 {
	request {
	}
	response {
		events 0 : *scene_skill_event_info
	}
}

#监听扣血伤害相关的事件
scene_listen_hurt_event 107 {
	request {
	}
	response {
		events 0 : *scene_hurt_event_info
	}
}

scene_enter_to 108 {
	request {
		scene_id 0 : integer
		door_id 1 : integer
	}
	response {
		result 0 : integer
	}
}

scene_relive 109 {
	request {
		relive_type 0 : integer
	}
	response {
		result 0 : integer
		relive_type 1 : integer
	}
}

]]

