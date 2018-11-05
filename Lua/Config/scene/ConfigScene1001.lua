local config = {
	scene_id = 1001,
	name = "新手村",
	door_list = {
		{door_id=1, x=0, y=0, z=2, target_scene_id=1002, target_x=1, target_y=2, target_x=3}, 
	},
	npc_list = {
		{npc_type_id=1, x=0, y=0, z=2}
	},
	monster_list = {
		{monster_type_id=1, x=0, y=0, z=2},
		{monster_type_id=1, x=0, y=0, z=2, name="other_name"},
	},
}
return config