--[[
move_spped：移动速度
max_hp：血量
skill_list：技能列表
attr_list：属性列表
ai.fight_back：是否会反击
ai.reborn_time：复活时间（毫秒）
ai.remove_after_dead：死后隔多久清理尸体（毫秒）
ai.attack_area：攻击范围，max_distance为最远攻击距离，min_distance用于一些弓箭手或魔法师，要离敌人最少一定距离才攻击
ai.patrol.type：巡逻类型，1为随机在活动范围里找个点然后走过去；2是路点巡逻，读取way_points字段
ai.patrol.hunt_radius：追捕半径，为0时就不主动找人打
ai.patrol.way_points：巡逻路点,如{{1,2},{3,5}}
ai.patrol.idle_min和ai.patrol.idle_max：发呆的时间范围，怪物走一会就会发呆一会
--]]
local config = {
	[2000] = {
		type_id = 2000, name = "小灰狼", max_hp=1000, 
		attr_list = {[1]=100,[2]=100}, move_speed=500, ai={
			fight_back = true, reborn_time = 1000, remove_after_dead = 500, patrol={type=1, idle_min=2000, idle_max=5000, hunt_radius=0}, attack_area = {min_distance=0, max_distance=100}, skill_list = {
				-- {skill_id=200001}
			},
		},
	},
	[2001] = {
		type_id = 2001, name = "大灰", max_hp=1000, 
		attr_list = {[1]=100,[2]=100}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 1000, remove_after_dead = 500, patrol={type=1, idle_min=2000, idle_max=5000, hunt_radius=0}, attack_area = {min_distance=0, max_distance=100}, skill_list = {
				{skill_id=200001, probability=5000}
			},
		},
	},
	[2002] = {
		type_id = 2002, name = "金钱猫",max_hp=1000, 
		attr_list = {[1]=100,[2]=100}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 1000, remove_after_dead = 500, patrol={type=1, idle_min=2000, idle_max=5000, hunt_radius=0}, attack_area = {min_distance=0, max_distance=100}, skill_list = {
				-- {skill_id=200001}
			},
		},
	},
	[2003] = {
		type_id = 2003, name = "红袍妖女", max_hp=1000, 
		attr_list = {[1]=100,[2]=100}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 1000, remove_after_dead = 500, patrol={type=1, idle_min=2000, idle_max=5000, hunt_radius=0}, attack_area = {min_distance=0, max_distance=100}, skill_list = {
				-- {skill_id=200001}
			},
		},
	},
	[2004] = {
		type_id = 2004, name = "断头鬼", max_hp=1000, 
		attr_list = {[1]=100,[2]=100}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 1000, remove_after_dead = 500, patrol={type=1, idle_min=2000, idle_max=5000, hunt_radius=0}, attack_area = {min_distance=0, max_distance=100}, skill_list = {
				-- {skill_id=200001}
			},
		},
	},
	[2005] = {
		type_id = 2005, name = "唱大戏的", max_hp=1000, 
		attr_list = {[1]=100,[2]=100}, move_speed=500, skill_list={}, ai={
			fight_back = true, reborn_time = 1000, remove_after_dead = 500, patrol={type=1, idle_min=2000, idle_max=5000, hunt_radius=0}, attack_area = {min_distance=0, max_distance=100}, skill_list = {
				-- {skill_id=200001}
			},
		},
	},
}
return config