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
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600, arge = {}, desc = [[男角普攻1]],
			},
		},	
	},
	[110001] = {
		skill_id = 110001, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600, buff = {}, desc = [[男角普攻2]],
			},
		},	
	},
	[110002] = {
		skill_id = 110002, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600, buff = {}, desc = [[男角普攻3]], 
			},
		},	
	},
	[110003] = {
		skill_id = 110003, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600, buff = {}, desc = [[男角普攻4]], 
			},
		},	
	},
	[110010] = {
		skill_id = 110010, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 5000, attack_max_num = 2, damage_rate = 10000, area = 600, buff = {
					{1,}, desc = [[男角技能1，造成50点伤害]], 
				},
			},
		},	
	},
	[110011] = {
		skill_id = 110011, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 6000, attack_max_num = 2, damage_rate = 10000, area = 600, desc = [[男角技能2，造成100点伤害同时有20%的概率让对方防御减少100点]]
			},
		},	
	},
	[110012] = {
		skill_id = 120012, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 7000, attack_max_num = 2, damage_rate = 10000, area = 600, desc = [[男角技能3，造成150点伤害同时提升100%自己的技能伤害]]
			},
		},	
	},
	[110013] = {
		skill_id = 110013, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 1000, attack_max_num = 2, damage_rate = 10000, area = 600, desc = [[男角技能4，5秒内造成500点伤害同时有50%的概率让对方晕眩]]
			},
		},	
	},
	--女角色技能
	[120000] = {
		skill_id = 120000, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120001] = {
		skill_id = 120001, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120002] = {
		skill_id = 120002, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120003] = {
		skill_id = 120003, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 100, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120010] = {
		skill_id = 120010, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 5000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120011] = {
		skill_id = 120011, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 6000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120012] = {
		skill_id = 120012, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 7000, attack_max_num = 2, damage_rate = 10000, area = 600,
			},
		},	
	},
	[120013] = {
		skill_id = 120013, target_type = 1, shape = 1, duration = 1000, detail = {
			[1] = {
				condition = {{lv, 1}}, cd = 15000, attack_max_num = 2, damage_rate = 10000, area = 600,
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