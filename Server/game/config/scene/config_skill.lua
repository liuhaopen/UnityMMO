--[[
skill_id：技能id
target_type：1敌方，2自己，3我方
shape：攻击范围形状--1圆形，2直线，3扇形
duration：施放时间（毫秒）
detail：每级的具体属性
detail.condition：学习条件--{lv,1}角色等级，{money,100}货币
detail.cd：冷却时间（毫秒）
detail.attack_max_num：攻击最大数量
detail.damage_rate：伤害比率
detail.area：攻击范围--shape为圆形时即半径，直线时即为距离
detail.arge：具体的技能参数，针对不同技能有不同参数，见SkillActions.lua说明
暂不用detail.buff：buff效果，类型为数组，元素结构：{BuffType(详见后端的SceneConst),触发概率(万份之几),作用方(1自己2已方3敌方),数值,持续时间,作用次数}
]]--
local config = {
	--男角色技能
	[110000] = {
		skill_id = 110000, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 2, damage_rate = 10000, area = 600, 
				desc = [[男角普攻1]],
			},
		},	
	},
	[110001] = {
		skill_id = 110001, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 3, damage_rate = 10000, area = 600, 
				desc = [[男角普攻2]],
			},
		},	
	},
	[110002] = {
		skill_id = 110002, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 4, damage_rate = 10000, area = 600, 
				desc = [[男角普攻3]], 
			},
		},	
	},
	[110003] = {
		skill_id = 110003, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 5, damage_rate = 10000, area = 600, 
				desc = [[男角普攻4]], 
			},
		},	
	},
	[110010] = {
		skill_id = 110010, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 5000, attack_max_num = 5, damage_rate = 15000, area = 600, 
				desc = [[男角技能1，造成50点伤害，并有10%概率减对方50%防御]], 
				buff = {probability=10000,duration=5000,attr_id=3,value=-5000,is_percent=true}
			},
			[2] = {
				condition = {{lv, 2}}, cd = 5000, attack_max_num = 3, damage_rate = 15000, area = 600, 
				desc = [[男角技能1，造成50点伤害，并有15%概率减对方60%防御]], 
				buff = {probability=1500,duration=5000,attr_id=3,value=-6000,is_percent=true}
			},
		},	
	},
	[110011] = {
		skill_id = 110011, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 8000, attack_max_num = 5, damage_rate = 20000, area = 600, 
				desc = [[男角技能2，造成600点伤害同时有50%的概率让对方晕眩3秒]],
				buff = {probability=50000, duration=8000}
			},
			[2] = {
				condition = {{lv, 2}}, cd = 8000, attack_max_num = 5, damage_rate = 22000, area = 700, 
				desc = [[男角技能2，造成700点伤害同时有20%的概率让对方晕眩4秒]],
				buff = {probability=2000, duration=4000}
			},
		},	
	},
	[110012] = {
		skill_id = 120012, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 12000, attack_max_num = 6, damage_rate = 200000, area = 600, 
				desc = [[男角技能3，造成250点伤害同时有20%的概率让对方冰冻3秒]],
				buff = {probability=2000, duration=4000}
			},
		},	
	},
	[110013] = {
		skill_id = 110013, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 15000, attack_max_num = 5, damage_rate = 30000, area = 600, 
				desc = [[男主大招，5秒内造成5次范围伤害，每次有100%的概率从对方吸收5%血量]],
				buff = {probability=10000,suck_interval=0,suck_count=1,value=500,is_percent=true}
			},
		},	
	},
	--女角色技能
	[120000] = {
		skill_id = 120000, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120001] = {
		skill_id = 120001, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 3, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120002] = {
		skill_id = 120002, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 4, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120003] = {
		skill_id = 120003, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 200, attack_max_num = 5, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120010] = {
		skill_id = 120010, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 5000, attack_max_num = 5, damage_rate = 10000, area = 600,
				desc = [[女角技能1，造成50点伤害，并有10%概率减对方50%防御]], 
				buff = {probability=10000,duration=5000,attr_id=3,value=-5000,is_percent=true}
			},
		},	
	},
	[120011] = {
		skill_id = 120011, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 8000, attack_max_num = 15, damage_rate = 10000, area = 600,
				desc = [[女角技能2，造成600点伤害同时有50%的概率让对方晕眩3秒]],
				buff = {probability=50000, duration=8000}
			},
		},	
	},
	[120012] = {
		skill_id = 120012, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 12000, attack_max_num = 6, damage_rate = 200000, area = 600,
			},
		},	
	},
	[120013] = {
		skill_id = 120013, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 15000, attack_max_num = 5, damage_rate = 10000, area = 600,
				desc = [[女主大招，5秒内造成5次范围伤害，每次有100%的概率从对方吸收5%血量]],
				buff = {probability=10000,suck_interval=0,suck_count=1,value=500,is_percent=true}
			},
		},	
	},
	--怪物技能
	[200000] = {
		skill_id = 200000, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200001] = {
		skill_id = 200001, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200100] = {
		skill_id = 200100, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200101] = {
		skill_id = 200101, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200200] = {
		skill_id = 200200, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200201] = {
		skill_id = 200201, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200300] = {
		skill_id = 200300, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200301] = {
		skill_id = 200301, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200400] = {
		skill_id = 200400, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200401] = {
		skill_id = 200401, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200500] = {
		skill_id = 200500, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[200501] = {
		skill_id = 200501, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
}

return config