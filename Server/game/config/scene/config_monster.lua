--[[
move_spped：移动速度
skill_list：技能列表
attr_list：属性列表
ai.fight_back：是否会反击
ai.reborn_time：复活时间（毫秒）
ai.remove_after_dead：死后隔多久清理尸体（毫秒）
ai.attack_area：攻击范围，max_distance为最远攻击距离，min_distance用于一些弓箭手或魔法师，要离敌人最少一定距离才攻击
ai.patrol.type：巡逻类型，1为随机在活动范围里找个点然后走过去；2是路点巡逻，读取way_points字段
ai.patrol.auto_attack_radius：主动攻击半径，为nil或0时就不主动找人打
ai.patrol.way_points：巡逻路点,如{{1,2},{3,5}}
ai.patrol.idle_min和ai.patrol.idle_max：发呆的时间范围，怪物走一会就会发呆一会
ai.hunt_radius：追捕半径，超过此半径就不再追了，切换到巡逻状态
--]]
local config = {
	[2000] = {
		type_id = 2000, name = "小灰狼",
		attr_list = {[1]=110,[2]=1000,[3]=20,[4]=20}, move_speed=500, ai={
			fight_back = true, reborn_time = 2000, remove_after_dead = 500, patrol={type=1, idle_min=3000, idle_max=7000, auto_attack_radius=1000}, attack_area = {min_distance=70, max_distance=150}, hunt_radius=1000, skill_list = {
				{skill_id=200000, random=60}, {skill_id=200001, random=40}
			},
		},
	},
	[2001] = {
		type_id = 2001, name = "大灰", attr_list = {[1]=150,[2]=3000,[3]=50,[4]=100}, move_speed=500, skill_list={}, 
		ai={
			fight_back = true, reborn_time = 2000, remove_after_dead = 500, patrol={type=1, idle_min=3000, idle_max=7000, auto_attack_radius=1000}, attack_area = {min_distance=140, max_distance=300}, hunt_radius=1000, skill_list = {
				{skill_id=200100, random=50}, {skill_id=200101, random=50}
			},
		},
	},
	[2002] = {
		type_id = 2002, name = "金钱猫",
		attr_list = {[1]=150,[2]=1000,[3]=20,[4]=30}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 3000, remove_after_dead = 500, patrol={type=1, idle_min=3000, idle_max=7000, auto_attack_radius=1000}, attack_area = {min_distance=60, max_distance=150}, hunt_radius=1000, skill_list = {
				{skill_id=200200, random=50}, {skill_id=200201, random=50}
			},
		},
	},
	[2003] = {
		type_id = 2003, name = "红袍妖女", 
		attr_list = {[1]=150,[2]=3000,[3]=20,[4]=120}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 3000, remove_after_dead = 500, patrol={type=1, idle_min=3000, idle_max=7000, auto_attack_radius=1000}, attack_area = {min_distance=60, max_distance=200}, hunt_radius=1000, skill_list = {
				{skill_id=200300, random=50}, {skill_id=200301, random=50}
			},
		},
	},
	[2004] = {
		type_id = 2004, name = "断头鬼", 
		attr_list = {[1]=150,[2]=1000,[3]=20,[4]=30}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 3000, remove_after_dead = 500, patrol={type=1, idle_min=3000, idle_max=7000, auto_attack_radius=1000}, attack_area = {min_distance=120, max_distance=200}, hunt_radius=1000, skill_list = {
				{skill_id=200400, random=50}, {skill_id=200401, random=50}
			},
		},
	},
	[2005] = {
		type_id = 2005, name = "唱大戏的", 
		attr_list = {[1]=150,[2]=3000,[3]=20,[4]=150}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 3000, remove_after_dead = 500, patrol={type=1, idle_min=3000, idle_max=7000, auto_attack_radius=1000}, attack_area = {min_distance=80, max_distance=300}, hunt_radius=1000, skill_list = {
				{skill_id=200500, random=50}, {skill_id=200501, random=50}
			},
		},
	},
}
return config